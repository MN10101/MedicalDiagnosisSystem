using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;


namespace MedicalDiagnosisSystem
{
    public partial class MainWindow : Window
    {
        private readonly MLContext _mlContext = new MLContext();
        private ITransformer? _model;
        private readonly string _modelPath = @"C:\Users\mnde\Desktop\C#\MedicalDiagnosisSystem\MedicalDiagnosisSystem\Model.zip";
        private string[] _symptomValues;
        private string[] _uniqueDiseases;

        public MainWindow()
        {
            InitializeComponent();
            if (SuggestionText == null || DiagnosisLabel == null || ConfidenceProgress == null || TensorFlowDiagnosisLabel == null || SymptomDescriptionBox == null)
            {
                throw new InvalidOperationException("One or more XAML elements failed to initialize.");
            }
            _symptomValues = Array.Empty<string>();
            _uniqueDiseases = Array.Empty<string>();
        }

        private void InitializeModelButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeModel();
        }

        private void LogToSuggestionText(string message)
        {
            Dispatcher.Invoke(() =>
            {
                SuggestionText.Text += message + "\n";
                var scrollViewer = FindName("SuggestionScrollViewer") as System.Windows.Controls.ScrollViewer;
                scrollViewer?.ScrollToEnd();
            });
        }

        private void InitializeModel()
        {
            try
            {
                LogToSuggestionText("Starting model initialization...\n");

                if (!File.Exists(_modelPath))
                {
                    DiagnosisLabel.Content = "Model not found. Training model...";
                    ModelTrainer.Instance.TrainAndSaveModel(LogToSuggestionText);
                }

                _model = ModelTrainer.Instance.LoadModel(LogToSuggestionText);
                if (_model == null)
                {
                    DiagnosisLabel.Content = "Model load failed.";
                    return;
                }

                _symptomValues = ModelTrainer.Instance.GetSymptomValues();
                _uniqueDiseases = ModelTrainer.Instance.GetUniqueDiseases();
                DiagnosisLabel.Content = "Model loaded successfully.";
                LogToSuggestionText("Model loaded successfully.\n");
            }
            catch (Exception ex)
            {
                DiagnosisLabel.Content = $"Error initializing model: {ex.Message}";
                LogToSuggestionText($"Exception during initialization: {ex.Message}\n{ex.StackTrace}\n");
            }
        }

        private PredictionInput ParseSymptomDescription(string description)
        {
            var input = new PredictionInput();
            foreach (var symptom in _symptomValues)
            {
                input.Symptoms[symptom] = 0f;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                LogToSuggestionText("No symptoms provided.\n");
                return input;
            }

            string lowerDescription = description.ToLower().Trim();
            lowerDescription = Regex.Replace(lowerDescription, @"\s+", " ");

            foreach (var symptom in _symptomValues)
            {
                string symptomText = symptom.Replace("_", " ").ToLower();
                if (symptomText.Contains(" "))
                {
                    if (lowerDescription.Contains(symptomText))
                    {
                        input.Symptoms[symptom] = 1f;
                    }
                }
                else
                {
                    var regex = new Regex($@"\b{symptomText}\b");
                    if (regex.IsMatch(lowerDescription))
                    {
                        input.Symptoms[symptom] = 1f;
                    }
                }
            }

            return input;
        }

        private void DiagnoseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string description = SymptomDescriptionBox.Text.Trim();
                if (string.IsNullOrEmpty(description))
                {
                    LogToSuggestionText("Please enter symptoms to diagnose.\n");
                    return;
                }

                if (_model == null)
                {
                    LogToSuggestionText("Model not initialized. Please click 'Initialize Model' first.\n");
                    return;
                }

                var input = ParseSymptomDescription(description);
                if (!input.Symptoms.Any(s => s.Value == 1))
                {
                    LogToSuggestionText("No symptoms detected. Cannot make a diagnosis.\n");
                    DiagnosisLabel.Content = "Diagnosis: N/A";
                    ConfidenceProgress.Value = 0;
                    return;
                }

                var symptomData = new SymptomDiagnosis();
                // Avoid reflection by using a dictionary to map symptom names to actions
                var symptomSetters = new Dictionary<string, Action<float>>
                {
                    { "itching", v => symptomData.Itching = v },
                    { "skin_rash", v => symptomData.SkinRash = v },
                    { "nodal_skin_eruptions", v => symptomData.NodalSkinEruptions = v },
                    { "continuous_sneezing", v => symptomData.ContinuousSneezing = v },
                    { "shivering", v => symptomData.Shivering = v },
                    { "chills", v => symptomData.Chills = v },
                    { "joint_pain", v => symptomData.JointPain = v },
                    { "stomach_pain", v => symptomData.StomachPain = v },
                    { "acidity", v => symptomData.Acidity = v },
                    { "ulcers_on_tongue", v => symptomData.UlcersOnTongue = v },
                    { "muscle_wasting", v => symptomData.MuscleWasting = v },
                    { "vomiting", v => symptomData.Vomiting = v },
                    { "burning_micturition", v => symptomData.BurningMicturition = v },
                    { "spotting_urination", v => symptomData.SpottingUrination = v },
                    { "fatigue", v => symptomData.Fatigue = v },
                    { "weight_gain", v => symptomData.WeightGain = v },
                    { "anxiety", v => symptomData.Anxiety = v },
                    { "cold_hands_and_feets", v => symptomData.ColdHandsAndFeets = v },
                    { "mood_swings", v => symptomData.MoodSwings = v },
                    { "weight_loss", v => symptomData.WeightLoss = v },
                    { "restlessness", v => symptomData.Restlessness = v },
                    { "lethargy", v => symptomData.Lethargy = v },
                    { "patches_in_throat", v => symptomData.PatchesInThroat = v },
                    { "irregular_sugar_level", v => symptomData.IrregularSugarLevel = v },
                    { "cough", v => symptomData.Cough = v },
                    { "high_fever", v => symptomData.HighFever = v },
                    { "sunken_eyes", v => symptomData.SunkenEyes = v },
                    { "breathlessness", v => symptomData.Breathlessness = v },
                    { "sweating", v => symptomData.Sweating = v },
                    { "dehydration", v => symptomData.Dehydration = v },
                    { "indigestion", v => symptomData.Indigestion = v },
                    { "headache", v => symptomData.Headache = v },
                    { "yellowish_skin", v => symptomData.YellowishSkin = v },
                    { "dark_urine", v => symptomData.DarkUrine = v },
                    { "nausea", v => symptomData.Nausea = v },
                    { "loss_of_appetite", v => symptomData.LossOfAppetite = v },
                    { "pain_behind_the_eyes", v => symptomData.PainBehindTheEyes = v },
                    { "back_pain", v => symptomData.BackPain = v },
                    { "constipation", v => symptomData.Constipation = v },
                    { "abdominal_pain", v => symptomData.AbdominalPain = v },
                    { "diarrhoea", v => symptomData.Diarrhoea = v },
                    { "mild_fever", v => symptomData.MildFever = v },
                    { "yellow_urine", v => symptomData.YellowUrine = v },
                    { "yellowing_of_eyes", v => symptomData.YellowingOfEyes = v },
                    { "acute_liver_failure", v => symptomData.AcuteLiverFailure = v },
                    { "fluid_overload", v => symptomData.FluidOverload = v },
                    { "swelling_of_stomach", v => symptomData.SwellingOfStomach = v },
                    { "swelled_lymph_nodes", v => symptomData.SwelledLymphNodes = v },
                    { "malaise", v => symptomData.Malaise = v },
                    { "blurred_and_distorted_vision", v => symptomData.BlurredAndDistortedVision = v },
                    { "phlegm", v => symptomData.Phlegm = v },
                    { "throat_irritation", v => symptomData.ThroatIrritation = v },
                    { "redness_of_eyes", v => symptomData.RednessOfEyes = v },
                    { "sinus_pressure", v => symptomData.SinusPressure = v },
                    { "runny_nose", v => symptomData.RunnyNose = v },
                    { "congestion", v => symptomData.Congestion = v },
                    { "chest_pain", v => symptomData.ChestPain = v },
                    { "weakness_in_limbs", v => symptomData.WeaknessInLimbs = v },
                    { "fast_heart_rate", v => symptomData.FastHeartRate = v },
                    { "pain_during_bowel_movements", v => symptomData.PainDuringBowelMovements = v },
                    { "pain_in_anal_region", v => symptomData.PainInAnalRegion = v },
                    { "bloody_stool", v => symptomData.BloodyStool = v },
                    { "irritation_in_anus", v => symptomData.IrritationInAnus = v },
                    { "neck_pain", v => symptomData.NeckPain = v },
                    { "dizziness", v => symptomData.Dizziness = v },
                    { "cramps", v => symptomData.Cramps = v },
                    { "bruising", v => symptomData.Bruising = v },
                    { "obesity", v => symptomData.Obesity = v },
                    { "swollen_legs", v => symptomData.SwollenLegs = v },
                    { "swollen_blood_vessels", v => symptomData.SwollenBloodVessels = v },
                    { "puffy_face_and_eyes", v => symptomData.PuffyFaceAndEyes = v },
                    { "enlarged_thyroid", v => symptomData.EnlargedThyroid = v },
                    { "brittle_nails", v => symptomData.BrittleNails = v },
                    { "swollen_extremeties", v => symptomData.SwollenExtremeties = v },
                    { "excessive_hunger", v => symptomData.ExcessiveHunger = v },
                    { "extra_marital_contacts", v => symptomData.ExtraMaritalContacts = v },
                    { "drying_and_tingling_lips", v => symptomData.DryingAndTinglingLips = v },
                    { "slurred_speech", v => symptomData.SlurredSpeech = v },
                    { "knee_pain", v => symptomData.KneePain = v },
                    { "hip_joint_pain", v => symptomData.HipJointPain = v },
                    { "muscle_weakness", v => symptomData.MuscleWeakness = v },
                    { "stiff_neck", v => symptomData.StiffNeck = v },
                    { "swelling_joints", v => symptomData.SwellingJoints = v },
                    { "movement_stiffness", v => symptomData.MovementStiffness = v },
                    { "spinning_movements", v => symptomData.SpinningMovements = v },
                    { "loss_of_balance", v => symptomData.LossOfBalance = v },
                    { "unsteadiness", v => symptomData.Unsteadiness = v },
                    { "weakness_of_one_body_side", v => symptomData.WeaknessOfOneBodySide = v },
                    { "loss_of_smell", v => symptomData.LossOfSmell = v },
                    { "bladder_discomfort", v => symptomData.BladderDiscomfort = v },
                    { "foul_smell_of_urine", v => symptomData.FoulSmellOfUrine = v },
                    { "continuous_feel_of_urine", v => symptomData.ContinuousFeelOfUrine = v },
                    { "passage_of_gases", v => symptomData.PassageOfGases = v },
                    { "internal_itching", v => symptomData.InternalItching = v },
                    { "toxic_look_typhos", v => symptomData.ToxicLookTyphos = v },
                    { "depression", v => symptomData.Depression = v },
                    { "irritability", v => symptomData.Irritability = v },
                    { "muscle_pain", v => symptomData.MusclePain = v },
                    { "altered_sensorium", v => symptomData.AlteredSensorium = v },
                    { "red_spots_over_body", v => symptomData.RedSpotsOverBody = v },
                    { "belly_pain", v => symptomData.BellyPain = v },
                    { "abnormal_menstruation", v => symptomData.AbnormalMenstruation = v },
                    { "dischromic_patches", v => symptomData.DischromicPatches = v },
                    { "watering_from_eyes", v => symptomData.WateringFromEyes = v },
                    { "increased_appetite", v => symptomData.IncreasedAppetite = v },
                    { "polyuria", v => symptomData.Polyuria = v },
                    { "family_history", v => symptomData.FamilyHistory = v },
                    { "mucoid_sputum", v => symptomData.MucoidSputum = v },
                    { "rusty_sputum", v => symptomData.RustySputum = v },
                    { "lack_of_concentration", v => symptomData.LackOfConcentration = v },
                    { "visual_disturbances", v => symptomData.VisualDisturbances = v },
                    { "receiving_blood_transfusion", v => symptomData.ReceivingBloodTransfusion = v },
                    { "receiving_unsterile_injections", v => symptomData.ReceivingUnsterileInjections = v },
                    { "coma", v => symptomData.Coma = v },
                    { "stomach_bleeding", v => symptomData.StomachBleeding = v },
                    { "distention_of_abdomen", v => symptomData.DistentionOfAbdomen = v },
                    { "history_of_alcohol_consumption", v => symptomData.HistoryOfAlcoholConsumption = v },
                    { "fluid_overload_1", v => symptomData.FluidOverload1 = v },
                    { "blood_in_sputum", v => symptomData.BloodInSputum = v },
                    { "prominent_veins_on_calf", v => symptomData.ProminentVeinsOnCalf = v },
                    { "palpitations", v => symptomData.Palpitations = v },
                    { "painful_walking", v => symptomData.PainfulWalking = v },
                    { "pus_filled_pimples", v => symptomData.PusFilledPimples = v },
                    { "blackheads", v => symptomData.Blackheads = v },
                    { "scurring", v => symptomData.Scurring = v },
                    { "skin_peeling", v => symptomData.SkinPeeling = v },
                    { "silver_like_dusting", v => symptomData.SilverLikeDusting = v },
                    { "small_dents_in_nails", v => symptomData.SmallDentsInNails = v },
                    { "inflammatory_nails", v => symptomData.InflammatoryNails = v },
                    { "blister", v => symptomData.Blister = v },
                    { "red_sore_around_nose", v => symptomData.RedSoreAroundNose = v }
                };

                foreach (var symptom in input.Symptoms)
                {
                    if (symptomSetters.TryGetValue(symptom.Key, out var setter))
                    {
                        setter(symptom.Value);
                    }
                }

                var predictionEngine = _mlContext.Model.CreatePredictionEngine<SymptomDiagnosis, SymptomDiagnosisPrediction>(_model);
                var predictionResult = predictionEngine.Predict(symptomData);

                var confidenceScores = predictionResult.Score;
                var predictedLabel = predictionResult.PredictedLabel;

                if (confidenceScores != null && predictedLabel != null)
                {
                    var maxScore = confidenceScores.Max();
                    var confidence = Math.Round(maxScore * 100, 2);
                    DiagnosisLabel.Content = $"Diagnosis: {predictedLabel}";
                    ConfidenceProgress.Value = confidence;
                    LogToSuggestionText($"Diagnosis: {predictedLabel} (Confidence: {confidence}%)\n");
                }
                else
                {
                    DiagnosisLabel.Content = "Diagnosis: N/A";
                    ConfidenceProgress.Value = 0;
                    LogToSuggestionText("Prediction failed.\n");
                }
            }
            catch (Exception ex)
            {
                LogToSuggestionText($"Error during diagnosis: {ex.Message}\n{ex.StackTrace}\n");
                DiagnosisLabel.Content = "Diagnosis: N/A";
                ConfidenceProgress.Value = 0;
            }
        }
    }
}