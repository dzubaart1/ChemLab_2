namespace BioEngineerLab.Activities
{
    public class ButtonClickedActivity : Activity
    {
        public ButtonType ButtonType;

        public ButtonClickedActivity()
        {
            ActivityType = ActivityType.ButtonClickedActivity;
        }
        
        public ButtonClickedActivity(ButtonType button)
        {
            ActivityType = ActivityType.ButtonClickedActivity;
            ButtonType = button;
        }
        
        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not ButtonClickedActivity buttonClicked)
            {
                return false;
            }
            
            return ButtonType == buttonClicked.ButtonType;
        }
    }

    public enum ButtonType : byte
    {
        ApplicationButton,
        MethodButton,
        CreateNewMeasurementButton,
        NextButton,
        SettingsButton,
        RulerModeButton,
        CalibrationButton,
        RotatePistonButton,
        PauseButton,
        FinishMeasurementButton,
        OkWarningButton,
        StirringButton,
        HeatingButton,
        CloseAppButton,
        KrussButton
    }
}