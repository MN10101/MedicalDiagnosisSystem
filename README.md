# Medical Diagnosis System

The **Medical Diagnosis System** is a machine learning-based application designed to predict potential diseases based on user-provided symptoms. Built using **.NET** and **Microsoft ML.NET**, this app provides a user-friendly interface for entering symptoms and receiving a diagnosis along with a confidence score.

---

# Author
-  Mahmoud Najmeh


<img src="https://avatars.githubusercontent.com/u/78208459?u=c3f9c7d6b49fc9726c5ea8bce260656bcb9654b3&v=4" width="200px" style="border-radius: 50%;">

---


## Features

- **Symptom Input**: Users can describe their symptoms in natural language (e.g., "I have a fever and a cough").
- **Disease Prediction**: The app predicts the most likely disease based on the symptoms provided.
- **Confidence Score**: Displays the confidence level of the prediction as a percentage.
- **Model Training**: The app can train and save a machine learning model using a dataset of diseases and symptoms.
- **User-Friendly Interface**: Built with **WPF (Windows Presentation Foundation)**, the app provides an intuitive and responsive UI.

---

## Prerequisites

Before running the app, ensure you have the following installed:

- **.NET SDK** (version 6.0 or later)
- **Visual Studio** (optional, for easier development and debugging)
- **Microsoft ML.NET** (included in the project)

---

## How to Download and Run the App

1. **Download the App**:
   - Clone the repository:
     ```bash
     git clone https://github.com/MN10101/MedicalDiagnosisSystem
     ```
   - Alternatively, download the repository as a ZIP file and extract it.

2. **Run the App**:
   - Open the project in **Visual Studio**.
   - Build the solution by clicking `Build > Build Solution`.
   - Run the app by pressing `F5` or clicking `Debug > Start Debugging`.

3. **Using the App**:
   - Click **Initialize Model** to load or train the machine learning model.
   - Enter your symptoms in the text box (e.g., "I have a headache and nausea").
   - Click **Diagnose** to get the predicted disease and confidence score.

---

## How It Works

1. **Model Training**:
   - The app uses a dataset (`New_Diseases_and_Symptoms.csv`) to train a machine learning model.
   - The model is saved as `Model.zip` for future use.

2. **Symptom Parsing**:
   - The app parses user input to identify relevant symptoms.
   - Symptoms are matched against a predefined list of symptom values.

3. **Prediction**:
   - The trained model predicts the most likely disease based on the symptoms.
   - The prediction is displayed along with a confidence score.

---

## Dataset

The app uses a dataset (`New_Diseases_and_Symptoms.csv`) containing the following:
- **Diseases**: A list of diseases.
- **Symptoms**: A list of symptoms associated with each disease.

The dataset is used to train the machine learning model. You can replace the dataset with your own data if needed.

---

## Code Structure

- **ModelTrainer.cs**: Handles model training, saving, and loading.
- **SymptomDiagnosis.cs**: Defines the data structure for symptoms and diseases.
- **MainWindow.xaml**: Contains the UI design for the app.
- **MainWindow.xaml.cs**: Implements the app's logic and event handlers.

---

## Notes

- **Model Training**: If the `Model.zip` file is missing, the app will automatically train a new model using the provided dataset.
- **Dataset Updates**: If you update the dataset (`New_Diseases_and_Symptoms.csv`), delete the `Model.zip` file to force the app to retrain the model.


---

## Contributing

Contributions are welcome! If you'd like to contribute, please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Submit a pull request.

---

## Screenshot

https://github.com/user-attachments/assets/9f02eb52-aa10-48be-8a6b-70d10fb76de0

---

Enjoy using the **Medical Diagnosis System**! 🚀
