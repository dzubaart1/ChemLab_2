namespace UI
{
    public enum TabletPanelsType : byte
    {
        TaskFailedPanel = 1,
        MainPanel = 2,
        EndGamePanel = 7,
    }
    
    public enum DesctopPanelsType : byte
    {
        ApplicationPanel = 0,
        DesctopPanel = 1,
        FinalPanel = 2,
    }
    
    public enum ApplicationPanelsType : byte
    {
        MenuPanel = 0,
        MethodPanel = 1,
    }

    public enum LiveImgModesPanelsType
    {
        MainModePanel = 0,
        RulerModePanel = 1,
    }
    
    public enum LiveImgHeaderPanelsType
    {
        MainHeaderPanel = 0,
        ChooseFrameHeaderPanel = 1,
    }
}