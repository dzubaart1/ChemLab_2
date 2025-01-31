namespace Core
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
        KspectrometerButton,
        TeleportCellResButton,
        TeleportScanElecButton,
        TeleportRFAButton,
        AutoClavePowerButton,
        AutoClaveOnButton,
        LaminBoxLButton,
        LaminBoxFButton,
        LaminBoxUVButton,
        LaminBoxSoundButton,
        LaminBoxIButton,
        LaminBoxUpButton,
        LaminBoxDownButton,
        LaminBoxOpenButton,
        NumberButton,
        BacteriumButton,
        LightButton,
        TaraButton,
        ShakerPowerButton,
        ShakerRPMButton,
        TermostatPowerButton,
        TermostatUpButton,
        TermostatPButton,
        TrashGloversButton,
        TrashShoeCoversButton,
        LabCoatButton,
        Light2Button,
        KeyButton,
        ShpatelButton,
        AutoClavePullButton
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
        ButtonClickedActivity,
        DoorActivity,
        PulveriazatorActivity,
        BadActivity
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
        DarkPuttyKnifeContainer,
        CentrifugaContainer,
        TweezersContainer,
        AtomicMicContainer,
        KspectrometerContainer,
        BottleContainer,
        WeighingContainer,
        StirringContainer,
        PenicilliumContainer,
    }
    
    public enum ESideEffect : byte
    {
        AddReagentsSideEffect,
        SetDozatorVolumeSideEffect,
        SpawnDocSideEffect,
        ConstructorSideEffect,
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

    public enum ELab : byte
    {
        Lab1,
        Lab2,
        Lab3
    }

    public enum EDoor : byte
    {
        DryMachineDoor,
        AutoClaveDoor,
        EnterDoor1,
        EnterDoor2,
        ShakerDoor,
        TermostatDoor,
        
    }

    public enum EDoorActivity : byte
    {
        Open,
        Closed
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
        PaperTrayMachine,
        TeleportCellResMachine,
        TeleportScanElecMachine,
        TeleportRFAMachine,
        KSpectrometerMachine,
        AtomicMicMachine,
        AutoClaveMachine,
        LaminBoxMachine,
        WaterDropsMachine,
        KeyboardMachine,
        LabCoatMachine,
        ShoeCoversMachine,
        LabGlovesMachine,
        ShakerMachine,
        MiniTrashMachine,
        PenSinkMachine
    }
    
    public enum ESocketActivity : byte
    {
        Enter,
        Exit
    }
    
    public enum ESocket : byte
    {
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
        TeleportCellResSocket,
        TeleportScanElecSocket,
        TeleportRFASocket,
        BottleGlukozaSocket,
        BottleSaharozaSocket,
        BottleLaktozaSocket,
        BankPetronSocket,
        BankNaClSocket,
        BankBCPSocket,
        BankGlukozaSocket,
        BankSaharozaSocket,
        BankLaktozaSocket,
        AutoclaveSocket,
        KeySocket,
        PeniciliumSocket,
        LaminBoxBottleSocket,
        PenicilliumCapSocket,
        PenicilliumPlenkaSocket,
        ShakerSocket,
        TermostatSocket,
        BottleCapSocket
    }

    public enum EInteractable : byte
    {
        PenicilliumCapInteractable,
        PenicilliumPlenkaInteractable,
        PetriDishCapInteractable,
        Door1Interactable,
        Door2Interactable
    }

    public enum EPulverizatorTarget : byte
    {
        LeftHandHit,
        RightHandHit,
        PenicilliumHit,
        GlukozaHit,
        LaktozaHit,
        SaharozaHit,
        CleaningSurface,
    }
}
