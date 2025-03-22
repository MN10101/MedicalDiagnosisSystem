using Microsoft.ML.Data;

namespace MedicalDiagnosisSystem
{
    public class SymptomDiagnosis
    {
        [ColumnName("Disease")]
        public string? Disease { get; set; }

        [ColumnName("itching")]
        public float Itching { get; set; }

        [ColumnName("skin_rash")]
        public float SkinRash { get; set; }

        [ColumnName("nodal_skin_eruptions")]
        public float NodalSkinEruptions { get; set; }

        [ColumnName("continuous_sneezing")]
        public float ContinuousSneezing { get; set; }

        [ColumnName("shivering")]
        public float Shivering { get; set; }

        [ColumnName("chills")]
        public float Chills { get; set; }

        [ColumnName("joint_pain")]
        public float JointPain { get; set; }

        [ColumnName("stomach_pain")]
        public float StomachPain { get; set; }

        [ColumnName("acidity")]
        public float Acidity { get; set; }

        [ColumnName("ulcers_on_tongue")]
        public float UlcersOnTongue { get; set; }

        [ColumnName("muscle_wasting")]
        public float MuscleWasting { get; set; }

        [ColumnName("vomiting")]
        public float Vomiting { get; set; }

        [ColumnName("burning_micturition")]
        public float BurningMicturition { get; set; }

        [ColumnName("spotting_urination")]
        public float SpottingUrination { get; set; }

        [ColumnName("fatigue")]
        public float Fatigue { get; set; }

        [ColumnName("weight_gain")]
        public float WeightGain { get; set; }

        [ColumnName("anxiety")]
        public float Anxiety { get; set; }

        [ColumnName("cold_hands_and_feets")]
        public float ColdHandsAndFeets { get; set; }

        [ColumnName("mood_swings")]
        public float MoodSwings { get; set; }

        [ColumnName("weight_loss")]
        public float WeightLoss { get; set; }

        [ColumnName("restlessness")]
        public float Restlessness { get; set; }

        [ColumnName("lethargy")]
        public float Lethargy { get; set; }

        [ColumnName("patches_in_throat")]
        public float PatchesInThroat { get; set; }

        [ColumnName("irregular_sugar_level")]
        public float IrregularSugarLevel { get; set; }

        [ColumnName("cough")]
        public float Cough { get; set; }

        [ColumnName("high_fever")]
        public float HighFever { get; set; }

        [ColumnName("sunken_eyes")]
        public float SunkenEyes { get; set; }

        [ColumnName("breathlessness")]
        public float Breathlessness { get; set; }

        [ColumnName("sweating")]
        public float Sweating { get; set; }

        [ColumnName("dehydration")]
        public float Dehydration { get; set; }

        [ColumnName("indigestion")]
        public float Indigestion { get; set; }

        [ColumnName("headache")]
        public float Headache { get; set; }

        [ColumnName("yellowish_skin")]
        public float YellowishSkin { get; set; }

        [ColumnName("dark_urine")]
        public float DarkUrine { get; set; }

        [ColumnName("nausea")]
        public float Nausea { get; set; }

        [ColumnName("loss_of_appetite")]
        public float LossOfAppetite { get; set; }

        [ColumnName("pain_behind_the_eyes")]
        public float PainBehindTheEyes { get; set; }

        [ColumnName("back_pain")]
        public float BackPain { get; set; }

        [ColumnName("constipation")]
        public float Constipation { get; set; }

        [ColumnName("abdominal_pain")]
        public float AbdominalPain { get; set; }

        [ColumnName("diarrhoea")]
        public float Diarrhoea { get; set; }

        [ColumnName("mild_fever")]
        public float MildFever { get; set; }

        [ColumnName("yellow_urine")]
        public float YellowUrine { get; set; }

        [ColumnName("yellowing_of_eyes")]
        public float YellowingOfEyes { get; set; }

        [ColumnName("acute_liver_failure")]
        public float AcuteLiverFailure { get; set; }

        [ColumnName("fluid_overload")]
        public float FluidOverload { get; set; }

        [ColumnName("swelling_of_stomach")]
        public float SwellingOfStomach { get; set; }

        [ColumnName("swelled_lymph_nodes")]
        public float SwelledLymphNodes { get; set; }

        [ColumnName("malaise")]
        public float Malaise { get; set; }

        [ColumnName("blurred_and_distorted_vision")]
        public float BlurredAndDistortedVision { get; set; }

        [ColumnName("phlegm")]
        public float Phlegm { get; set; }

        [ColumnName("throat_irritation")]
        public float ThroatIrritation { get; set; }

        [ColumnName("redness_of_eyes")]
        public float RednessOfEyes { get; set; }

        [ColumnName("sinus_pressure")]
        public float SinusPressure { get; set; }

        [ColumnName("runny_nose")]
        public float RunnyNose { get; set; }

        [ColumnName("congestion")]
        public float Congestion { get; set; }

        [ColumnName("chest_pain")]
        public float ChestPain { get; set; }

        [ColumnName("weakness_in_limbs")]
        public float WeaknessInLimbs { get; set; }

        [ColumnName("fast_heart_rate")]
        public float FastHeartRate { get; set; }

        [ColumnName("pain_during_bowel_movements")]
        public float PainDuringBowelMovements { get; set; }

        [ColumnName("pain_in_anal_region")]
        public float PainInAnalRegion { get; set; }

        [ColumnName("bloody_stool")]
        public float BloodyStool { get; set; }

        [ColumnName("irritation_in_anus")]
        public float IrritationInAnus { get; set; }

        [ColumnName("neck_pain")]
        public float NeckPain { get; set; }

        [ColumnName("dizziness")]
        public float Dizziness { get; set; }

        [ColumnName("cramps")]
        public float Cramps { get; set; }

        [ColumnName("bruising")]
        public float Bruising { get; set; }

        [ColumnName("obesity")]
        public float Obesity { get; set; }

        [ColumnName("swollen_legs")]
        public float SwollenLegs { get; set; }

        [ColumnName("swollen_blood_vessels")]
        public float SwollenBloodVessels { get; set; }

        [ColumnName("puffy_face_and_eyes")]
        public float PuffyFaceAndEyes { get; set; }

        [ColumnName("enlarged_thyroid")]
        public float EnlargedThyroid { get; set; }

        [ColumnName("brittle_nails")]
        public float BrittleNails { get; set; }

        [ColumnName("swollen_extremeties")]
        public float SwollenExtremeties { get; set; }

        [ColumnName("excessive_hunger")]
        public float ExcessiveHunger { get; set; }

        [ColumnName("extra_marital_contacts")]
        public float ExtraMaritalContacts { get; set; }

        [ColumnName("drying_and_tingling_lips")]
        public float DryingAndTinglingLips { get; set; }

        [ColumnName("slurred_speech")]
        public float SlurredSpeech { get; set; }

        [ColumnName("knee_pain")]
        public float KneePain { get; set; }

        [ColumnName("hip_joint_pain")]
        public float HipJointPain { get; set; }

        [ColumnName("muscle_weakness")]
        public float MuscleWeakness { get; set; }

        [ColumnName("stiff_neck")]
        public float StiffNeck { get; set; }

        [ColumnName("swelling_joints")]
        public float SwellingJoints { get; set; }

        [ColumnName("movement_stiffness")]
        public float MovementStiffness { get; set; }

        [ColumnName("spinning_movements")]
        public float SpinningMovements { get; set; }

        [ColumnName("loss_of_balance")]
        public float LossOfBalance { get; set; }

        [ColumnName("unsteadiness")]
        public float Unsteadiness { get; set; }

        [ColumnName("weakness_of_one_body_side")]
        public float WeaknessOfOneBodySide { get; set; }

        [ColumnName("loss_of_smell")]
        public float LossOfSmell { get; set; }

        [ColumnName("bladder_discomfort")]
        public float BladderDiscomfort { get; set; }

        [ColumnName("foul_smell_of_urine")]
        public float FoulSmellOfUrine { get; set; }

        [ColumnName("continuous_feel_of_urine")]
        public float ContinuousFeelOfUrine { get; set; }

        [ColumnName("passage_of_gases")]
        public float PassageOfGases { get; set; }

        [ColumnName("internal_itching")]
        public float InternalItching { get; set; }

        [ColumnName("toxic_look_typhos")]
        public float ToxicLookTyphos { get; set; }

        [ColumnName("depression")]
        public float Depression { get; set; }

        [ColumnName("irritability")]
        public float Irritability { get; set; }

        [ColumnName("muscle_pain")]
        public float MusclePain { get; set; }

        [ColumnName("altered_sensorium")]
        public float AlteredSensorium { get; set; }

        [ColumnName("red_spots_over_body")]
        public float RedSpotsOverBody { get; set; }

        [ColumnName("belly_pain")]
        public float BellyPain { get; set; }

        [ColumnName("abnormal_menstruation")]
        public float AbnormalMenstruation { get; set; }

        [ColumnName("dischromic_patches")]
        public float DischromicPatches { get; set; }

        [ColumnName("watering_from_eyes")]
        public float WateringFromEyes { get; set; }

        [ColumnName("increased_appetite")]
        public float IncreasedAppetite { get; set; }

        [ColumnName("polyuria")]
        public float Polyuria { get; set; }

        [ColumnName("family_history")]
        public float FamilyHistory { get; set; }

        [ColumnName("mucoid_sputum")]
        public float MucoidSputum { get; set; }

        [ColumnName("rusty_sputum")]
        public float RustySputum { get; set; }

        [ColumnName("lack_of_concentration")]
        public float LackOfConcentration { get; set; }

        [ColumnName("visual_disturbances")]
        public float VisualDisturbances { get; set; }

        [ColumnName("receiving_blood_transfusion")]
        public float ReceivingBloodTransfusion { get; set; }

        [ColumnName("receiving_unsterile_injections")]
        public float ReceivingUnsterileInjections { get; set; }

        [ColumnName("coma")]
        public float Coma { get; set; }

        [ColumnName("stomach_bleeding")]
        public float StomachBleeding { get; set; }

        [ColumnName("distention_of_abdomen")]
        public float DistentionOfAbdomen { get; set; }

        [ColumnName("history_of_alcohol_consumption")]
        public float HistoryOfAlcoholConsumption { get; set; }

        [ColumnName("fluid_overload_1")]
        public float FluidOverload1 { get; set; }

        [ColumnName("blood_in_sputum")]
        public float BloodInSputum { get; set; }

        [ColumnName("prominent_veins_on_calf")]
        public float ProminentVeinsOnCalf { get; set; }

        [ColumnName("palpitations")]
        public float Palpitations { get; set; }

        [ColumnName("painful_walking")]
        public float PainfulWalking { get; set; }

        [ColumnName("pus_filled_pimples")]
        public float PusFilledPimples { get; set; }

        [ColumnName("blackheads")]
        public float Blackheads { get; set; }

        [ColumnName("scurring")]
        public float Scurring { get; set; }

        [ColumnName("skin_peeling")]
        public float SkinPeeling { get; set; }

        [ColumnName("silver_like_dusting")]
        public float SilverLikeDusting { get; set; }

        [ColumnName("small_dents_in_nails")]
        public float SmallDentsInNails { get; set; }

        [ColumnName("inflammatory_nails")]
        public float InflammatoryNails { get; set; }

        [ColumnName("blister")]
        public float Blister { get; set; }

        [ColumnName("red_sore_around_nose")]
        public float RedSoreAroundNose { get; set; }
    }

    public class PredictionInput
    {
        public Dictionary<string, float> Symptoms { get; set; } = new Dictionary<string, float>();
    }

    public class SymptomDiagnosisPrediction
    {
        [ColumnName("PredictedLabel")]
        public string? PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float[]? Score { get; set; }
    }
}