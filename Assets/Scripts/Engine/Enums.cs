namespace BioEngineerLab
{
    public enum ESubstanceMode : byte
    {
        Normal,
        Dry,
        HeatStir,
    }

    public enum EButton : byte
    {
        StirringMachineHeatBtn,
        StirringMachineStirBtn,
        DozatorButton,
        ScannerButton,
        DryBoxMachineButton,
        CentrifugaContainerButton,
        CentrifugaPowerButton,
        CentrifugaStartButton,
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
        SocketSubstancesActivity,
        SocketActivity,
        AnchorActivity,
        CraftSubstanceActivity,
        ButtonClickedActivity
    }
    
    public enum EContainer : byte
    {
        ChemicGlassContainer,
        BankContainer,
        SpoonContainer,
        LodochkaContainer,
        WaterDeContainer,
        MeasureContainer,
        DozatorContainer,
        PetriDishContainer,
        TestTubeContainer,
        LunkaContainer,
        PuttyKnifeContainer,
        CentrifugaContainer,
        //KuvetkaContainer,
        //SyringeContainer,
        //ReagentsContainer,
        //NeedleContainer,
        //PistonContainer
    }
    
    public enum ESideEffect : byte
    {
        Effect1,
        Effect2,
        AddReagentsSideEffect,
        SetDozatorVolumeSideEffect
    }

    public enum ESideEffectTime : byte
    {
        StartTask,
        EndTask
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
        SyringeCupMoveMachine,
        WashingMachine,
        ExpTabletMachine,
        TrashMachine,
        CentrifugaContainerMachine,
        CentrifugaMachine,
    }
    
    public enum ESocketActivity : byte
    {
        Enter,
        Exit
    }
    
    public enum ESocket : byte
    {
        //PistonSocket,
        //NeedleSocket,
        //KRUSSSyringeSocket,
        //KRUSSKuvetkaSocket,
        //PAFBodySocket,
        //HeptanBodySocket,
        //KRUSSInteractablePanelSocket,
        //StirringMachineSocket,
        //LabCoatSocket,
        WeighingMachineSocket,
        BankCaCL2Socket,
        BankAgarSocket,
        BankNa2HPO4Socket,
        StirringMachineSocket,
        ChemicGlassContainerSocket,
        PipetDozatorSocket,
        PetriDishCupSocket,
        TestTubeSocket,
        TestTubeCupSocket,
        ScannerSocket,
        DryBoxMachineSocket,
        PipetkaRackSocket,
        CentrifugaSocket,
    }
}
