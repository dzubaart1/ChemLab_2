namespace BioEngineerLab
{
    public enum ETargetPlatform : byte
    {
        Auto,
        VR,
        PC
    }
    
    public enum ESubstanceMode : byte
    {
        Normal,
        Dry,
        HeatStir,
    }

    public enum ECraft : byte
    {
        Dry,
        Split,
        HeatStir,
        Mix
    }
    
    public enum ESubstanceLayer : byte
    {
        Top = 0,
        Middle = 1,
        Bottom = 2
    }
    
    public enum EActivity : byte
    {
        AddSubstanceActivity,
        MachineActivity,
        SocketActivity,
        AnchorActivity,
        CraftSubstanceActivity
    }
    
    public enum EContainer : byte
    {
        ChemicGlassContainer,
        KuvetkaContainer,
        SyringeContainer,
        ReagentsContainer,
        NeedleContainer,
        PistonContainer
    }
    
    public enum ESideEffect : byte
    {
        Effect1,
        Effect2,
        AddReagentsSideEffect
    }

    public enum ESideEffectTime : byte
    {
        StartTask,
        EndTask
    }

    public enum ESubstanceColor : byte
    {
        Color1,
        Color2,
        Color3
    }
    public enum EMachineActivity : byte
    {
        OnEnter,
        OnStart,
        OnFinish,
        OnExit,
    }

    public enum EMachine : byte
    {
        StirringMachine,
        HandModelChangerMachine,
        CoatMachine,
        SyringeCupMoveMachine
    }
    
    public enum ESocketActivity : byte
    {
        Enter,
        Exit
    }
    
    public enum ESocket : byte
    {
        PistonSocket,
        NeedleSocket,
        KRUSSSyringeSocket,
        KRUSSKuvetkaSocket,
        PAFBodySocket,
        HeptanBodySocket,
        KRUSSInteractablePanelSocket,
        StirringMachineSocket,
        LabCoatSocket,
    }
}
