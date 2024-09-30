namespace BioEngineerLab.UI
{
    public enum TabletPanelsType : byte
    {
        DebugPanel = 0,
        TaskFailedPanel = 1,
        MainPanel = 2,
        SliderTaskPanel = 3,
        DragLinePanel = 4,
        HintTabletPanel = 5,
        InfoTabletPanel = 6,
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