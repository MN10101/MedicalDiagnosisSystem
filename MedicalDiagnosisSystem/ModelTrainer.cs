using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MedicalDiagnosisSystem
{
    public class ModelTrainer
    {
        private readonly MLContext _mlContext;
        private readonly string _dataPath = @"C:\Users\mnde\Desktop\C#\MedicalDiagnosisSystem\MedicalDiagnosisSystem\New_Diseases_and_Symptoms.csv";
        private readonly string _modelPath = @"C:\Users\mnde\Desktop\C#\MedicalDiagnosisSystem\MedicalDiagnosisSystem\Model.zip";
        private string[] _symptomValues;
        private string[] _uniqueDiseases;
        public static string[] SymptomValues => Instance._symptomValues;
        public static string[] UniqueDiseases => Instance._uniqueDiseases;
        public static ModelTrainer Instance { get; } = new ModelTrainer();

        private ModelTrainer()
        {
            _mlContext = new MLContext(seed: 42);
            _symptomValues = Array.Empty<string>();
            _uniqueDiseases = Array.Empty<string>();
        }

        public string[] GetSymptomValues() => _symptomValues ?? Array.Empty<string>();
        public string[] GetUniqueDiseases() => _uniqueDiseases ?? Array.Empty<string>();

        public void TrainAndSaveModel(Action<string>? logCallback = null)
        {
            try
            {
                logCallback?.Invoke("Starting training process...\n");

                // Validate CSV file
                if (!File.Exists(_dataPath))
                {
                    logCallback?.Invoke($"Dataset file not found at: {_dataPath}. Skipping training.\n");
                    return;
                }

                // Read headers
                string[] headers;
                using (var reader = new StreamReader(_dataPath))
                {
                    var headerLine = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(headerLine))
                    {
                        logCallback?.Invoke("CSV file is empty or has no header.\n");
                        return;
                    }
                    headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();
                    const int expectedColumnCount = 132;
                    if (headers.Length != expectedColumnCount)
                    {
                        logCallback?.Invoke($"Header has {headers.Length} columns; expected {expectedColumnCount}. Aborting.\n");
                        return;
                    }
                }

                // Define schema
                var trainingColumns = new TextLoader.Column[headers.Length];
                trainingColumns[0] = new TextLoader.Column("Disease", DataKind.String, 0);
                for (int i = 1; i < headers.Length; i++)
                {
                    trainingColumns[i] = new TextLoader.Column(headers[i], DataKind.Single, i);
                }

                var textLoaderOptions = new TextLoader.Options
                {
                    Separators = new[] { ',' },
                    HasHeader = true,
                    Columns = trainingColumns,
                    AllowQuoting = true,
                    AllowSparse = false
                };

                // Load data
                logCallback?.Invoke("Loading data...\n");
                var dataView = _mlContext.Data.LoadFromTextFile(_dataPath, textLoaderOptions);
                logCallback?.Invoke("Data loaded successfully.\n");

                // Validate data
                var rowCount = _mlContext.Data.CreateEnumerable<SymptomDiagnosis>(dataView, reuseRowObject: false).Count();
                if (rowCount < 2)
                {
                    logCallback?.Invoke($"Not enough data to train the model. Found {rowCount} rows; need at least 2.\n");
                    return;
                }

                // Populate symptom values and unique diseases
                _symptomValues = headers.Skip(1).ToArray();
                _uniqueDiseases = _mlContext.Data.CreateEnumerable<SymptomDiagnosis>(dataView, reuseRowObject: false)
                    .Select(x => x.Disease)
                    .Where(x => x != null) 
                    .Select(x => x!) 
                    .Distinct()
                    .ToArray(); 

                if (_uniqueDiseases.Length < 2)
                {
                    logCallback?.Invoke($"Not enough unique diseases to train. Found {_uniqueDiseases.Length} unique diseases; need at least 2.\n");
                    return;
                }

                logCallback?.Invoke($"Dataset: {rowCount} rows, {_uniqueDiseases.Length} unique diseases.\n");

                // Simple balancing: Take a subset of data to ensure balance
                var diseaseCounts = _mlContext.Data.CreateEnumerable<SymptomDiagnosis>(dataView, reuseRowObject: false)
                    .GroupBy(x => x.Disease)
                    .Select(g => new { Disease = g.Key, Count = g.Count() })
                    .ToList();
                var minCount = diseaseCounts.Min(dc => dc.Count);
                if (minCount < 5) minCount = 5;
                var maxCount = diseaseCounts.Max(dc => dc.Count);
                if (maxCount > minCount * 1.5)
                {
                    logCallback?.Invoke($"Balancing dataset (min count: {minCount})...\n");
                    var balancedData = new List<SymptomDiagnosis>();
                    foreach (var group in diseaseCounts)
                    {
                        var rows = _mlContext.Data.CreateEnumerable<SymptomDiagnosis>(dataView, reuseRowObject: false)
                            .Where(x => x.Disease == group.Disease)
                            .Take(minCount)
                            .ToList();
                        balancedData.AddRange(rows);
                    }
                    dataView = _mlContext.Data.LoadFromEnumerable(balancedData);
                    logCallback?.Invoke($"Balanced dataset to {balancedData.Count} rows.\n");
                }

                // Train the model
                logCallback?.Invoke("Training the model...\n");
                var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", "Disease")
                    .Append(_mlContext.Transforms.Concatenate("Features", _symptomValues))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                        new SdcaMaximumEntropyMulticlassTrainer.Options
                        {
                            LabelColumnName = "Label",
                            FeatureColumnName = "Features",
                            MaximumNumberOfIterations = 20,
                            L2Regularization = 0.1f,
                            L1Regularization = 0.1f
                        }))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                var model = pipeline.Fit(dataView);
                logCallback?.Invoke("Model training completed.\n");

                // Save the model
                logCallback?.Invoke("Saving the model...\n");
                Directory.CreateDirectory(Path.GetDirectoryName(_modelPath) ?? throw new InvalidOperationException("Invalid model path directory."));
                using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    _mlContext.Model.Save(model, dataView.Schema, fileStream);
                }
                logCallback?.Invoke($"Model saved to: {_modelPath}\n");
            }
            catch (Exception ex)
            {
                logCallback?.Invoke($"Error during training: {ex.Message}\n{ex.StackTrace}\n");
                throw;
            }
        }

        public ITransformer? LoadModel(Action<string>? logCallback = null)
        {
            try
            {
                if (!File.Exists(_modelPath))
                {
                    logCallback?.Invoke($"Model file not found at: {_modelPath}. Using default behavior.\n");
                    return null;
                }

                using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var model = _mlContext.Model.Load(stream, out var schema);
                    logCallback?.Invoke("Model loaded successfully.\n");
                    PopulateSymptomValuesAndDiseases(logCallback);
                    return model;
                }
            }
            catch (Exception ex)
            {
                logCallback?.Invoke($"Error loading model: {ex.Message}\n{ex.StackTrace}\n");
                return null;
            }
        }

        private void PopulateSymptomValuesAndDiseases(Action<string>? logCallback = null)
        {
            try
            {
                if (_symptomValues.Any() && _uniqueDiseases.Any())
                {
                    return;
                }

                if (!File.Exists(_dataPath))
                {
                    logCallback?.Invoke($"Dataset file not found at: {_dataPath}. Cannot populate symptom values and diseases.\n");
                    return;
                }

                string[] headers;
                using (var reader = new StreamReader(_dataPath))
                {
                    var headerLine = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(headerLine))
                    {
                        logCallback?.Invoke("CSV file is empty or has no header.\n");
                        return;
                    }
                    headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();
                }

                const int expectedColumnCount = 132;
                if (headers.Length != expectedColumnCount)
                {
                    logCallback?.Invoke($"CSV header has {headers.Length} columns; expected {expectedColumnCount}.\n");
                    return;
                }

                var columns = new TextLoader.Column[headers.Length];
                columns[0] = new TextLoader.Column("Disease", DataKind.String, 0);
                for (int i = 1; i < headers.Length; i++)
                {
                    columns[i] = new TextLoader.Column(headers[i], DataKind.Single, i);
                }

                var textLoaderOptions = new TextLoader.Options
                {
                    Separators = new[] { ',' },
                    HasHeader = true,
                    Columns = columns,
                    AllowQuoting = true,
                    AllowSparse = false
                };

                var dataView = _mlContext.Data.LoadFromTextFile(_dataPath, textLoaderOptions);
                _symptomValues = headers.Skip(1).ToArray();
                _uniqueDiseases = _mlContext.Data.CreateEnumerable<SymptomDiagnosis>(dataView, reuseRowObject: false)
                    .Select(x => x.Disease)
                    .Where(x => x != null) 
                    .Select(x => x!) 
                    .Distinct()
                    .ToArray(); 
            }
            catch (Exception ex)
            {
                logCallback?.Invoke($"Error populating symptom values and diseases: {ex.Message}\n{ex.StackTrace}\n");
            }
        }
    }
}