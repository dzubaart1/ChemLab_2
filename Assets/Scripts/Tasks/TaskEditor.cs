using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;
using UnityEditor;
using UnityEngine;

namespace BioEngineerLab.Tasks
{
#if UNITY_EDITOR
    [CustomEditor(typeof(TasksPropertyScriptableObject))]
    public class TaskEditor : Editor
    {
        private TasksPropertyScriptableObject _taskPropertyScriptableObject;
        private TaskProperty _taskProperty;
        private SerializedObject _serializedObject;

        private TransferActivity _transferActivity;
        private MachineActivity _machineActivity;
        private ButtonClickedActivity _buttonClickedActivity;
        private SliderValueChangedActivity _sliderValueChangedActivity;
        private SocketActivity _socketActivity;
        private InputFieldActivity _inputFieldActivity;
        private DragLineActivity _dragLineActivity;
        private DropdownActivity _dropdownActivity;
        private AnchorActivity _anchorActivity;
        private WashingActivity _washingActivity;
        
        private void OnEnable()
        {
            _transferActivity = new TransferActivity();
            _machineActivity = new MachineActivity();
            _buttonClickedActivity = new ButtonClickedActivity();
            _sliderValueChangedActivity = new SliderValueChangedActivity();
            _socketActivity = new SocketActivity();
            _inputFieldActivity = new InputFieldActivity();
            _dragLineActivity = new DragLineActivity();
            _dropdownActivity = new DropdownActivity();
            _anchorActivity = new AnchorActivity();
            _washingActivity = new WashingActivity();
            
            _taskPropertyScriptableObject = (TasksPropertyScriptableObject)target;
            _taskProperty = _taskPropertyScriptableObject.TaskProperty;
            _serializedObject = new SerializedObject(target);
        }
        
        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            InitTaskInfoFields();
            InitSubstanceAddingFields();
            InitTaskChangeSpriteFields();
            InitTaskHintSpriteFields();
            InitTaskChangeSyringePosFields();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Activity Fields", EditorStyles.boldLabel);
            _taskProperty.ActivityType = (ActivityType)EditorGUILayout.EnumPopup("Activity Type", _taskProperty.ActivityType);

            switch (_taskProperty.ActivityType)
            {
                case ActivityType.TransferActivity:
                    InitTransferActivity();
                    break;
                case ActivityType.MachineActivity:
                    InitMachineActivity();
                    break;
                case ActivityType.ButtonClickedActivity:
                    InitButtonClickedActivity();
                    break;
                case ActivityType.SliderValueChangedActivity:
                    InitSliderValueChangedActivity();
                    break;
                case ActivityType.SocketActivity:
                    InitSocketActivity();
                    break;
                case ActivityType.InputFieldActivity:
                    InitInputFieldActivity();
                    break;
                case ActivityType.DragLineActivity:
                    InitDragLineActivity();
                    break;
                case ActivityType.DropdownActivity:
                    InitDropdownActivity();
                    break;
                case ActivityType.AnchorActivity:
                    InitAnchorActivity();
                    break;
                case ActivityType.WashingActivity:
                    InitWashingActivity();
                    break;
            }

            InitSerialisedButtons();

            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_taskPropertyScriptableObject);
        }

        private void InitTaskInfoFields()
        {
            _taskProperty.Number = EditorGUILayout.IntField("Number", _taskProperty.Number);
            _taskProperty.Title = EditorGUILayout.TextField("Title", _taskProperty.Title);
            _taskProperty.Description = EditorGUILayout.TextField("Description", _taskProperty.Description);
            _taskProperty.Warning = EditorGUILayout.TextField("Warning", _taskProperty.Warning);
            _taskProperty.SaveableTask = EditorGUILayout.Toggle("Saveable Task", _taskProperty.SaveableTask);
            _taskProperty.UnlockSyringe = EditorGUILayout.Toggle("Unlock Syringe", _taskProperty.UnlockSyringe);
            _taskProperty.ShouldChangeGateOpening = EditorGUILayout.Toggle("Should Change Gate Opening", _taskProperty.ShouldChangeGateOpening);
            
            if (_taskProperty.ShouldChangeGateOpening)
            {
                _taskProperty.IsOpenGate = EditorGUILayout.Toggle("Is Open Gate", _taskProperty.IsOpenGate);
            }
        }

        private void InitTaskChangeSyringePosFields()
        {
            _taskProperty.IsTaskChangeSyringePos = EditorGUILayout.Toggle("Is Task Change Syringe Pos", _taskProperty.IsTaskChangeSyringePos);
            
            if (!_taskProperty.IsTaskChangeSyringePos)
            {
                return;
            }
            
            _taskProperty.SyringePos = (SyringeCupMove.SyringePos) EditorGUILayout.EnumPopup("Syringe Pos", _taskProperty.SyringePos);
        }
        
        private void InitTaskHintSpriteFields()
        {
            _taskProperty.HasHintSprite = EditorGUILayout.Toggle("Has Hint Sprite", _taskProperty.HasHintSprite);
            
            if (!_taskProperty.HasHintSprite)
            {
                return;
            }
            
            _taskProperty.HintSpriteName = EditorGUILayout.TextField("Hint Sprite Name", _taskProperty.HintSpriteName);
        }

        private void InitTaskChangeSpriteFields()
        {
            _taskProperty.IsTaskChangeSprite = EditorGUILayout.Toggle("Is Task Change Sprite", _taskProperty.IsTaskChangeSprite);
            
            if (!_taskProperty.IsTaskChangeSprite)
            {
                return;
            }
            
            _taskProperty.SpriteName = EditorGUILayout.TextField("Sprite Name", _taskProperty.SpriteName);
        }

        private void InitSubstanceAddingFields()
        {
            _taskProperty.IsSubstanceAdding = EditorGUILayout.Toggle("Is Substance Adding", _taskProperty.IsSubstanceAdding);

            if (!_taskProperty.IsSubstanceAdding)
            {
                return;
            }
            
            _taskProperty.SubstanceName = (SubstanceName) EditorGUILayout.EnumPopup("Reagents Name", _taskProperty.SubstanceName);
            _taskProperty.SubstanceWeight = EditorGUILayout.FloatField("Reagents Weight", _taskProperty.SubstanceWeight);
        }
        
        private void InitDragLineActivity()
        {
            if (_taskProperty.TaskActivity is DragLineActivity dragLineActivity)
            {
                _dragLineActivity = dragLineActivity;
            }
            
            _dragLineActivity.DragLineType = (DragLineType) EditorGUILayout.EnumPopup("Drag Line Type", _dragLineActivity.DragLineType);
            _dragLineActivity.YOffset = EditorGUILayout.DoubleField("Y Offset", _dragLineActivity.YOffset);
            
            _taskProperty.TaskActivity = _dragLineActivity;
        }

        private void InitInputFieldActivity()
        {
            if (_taskProperty.TaskActivity is InputFieldActivity inputFieldActivity)
            {
                _inputFieldActivity = inputFieldActivity;
            }
            
            _inputFieldActivity.InputFieldType = (InputFieldType) EditorGUILayout.EnumPopup("Input Field Type", _inputFieldActivity.InputFieldType);
            _inputFieldActivity.Value = EditorGUILayout.FloatField("Value", _inputFieldActivity.Value);
            
            _taskProperty.TaskActivity = _inputFieldActivity;
        }
        
        private void InitWashingActivity()
        {
            if (_taskProperty.TaskActivity is WashingActivity washingActivity)
            {
                _washingActivity = washingActivity;
            }
            
            _washingActivity.Container = (ContainerType) EditorGUILayout.EnumPopup("Container", _washingActivity.Container);
            
            _taskProperty.TaskActivity = _washingActivity;
        }

        private void InitTransferActivity()
        {
            if (_taskProperty.TaskActivity is TransferActivity transferActivity)
            {
                _transferActivity = transferActivity;
            }

            _transferActivity.FromContainer = (ContainerType)EditorGUILayout.EnumPopup("From Container Type", _transferActivity.FromContainer);
            _transferActivity.ToContainer = (ContainerType)EditorGUILayout.EnumPopup("To Container Type", _transferActivity.ToContainer);
            _transferActivity.TransferSubstanceName = (SubstanceName)EditorGUILayout.EnumPopup("Transfer Substance Name", _transferActivity.TransferSubstanceName);
            _transferActivity.TransferSubstanceMode = (SubstanceMode)EditorGUILayout.EnumPopup("Transfer Substance Mode", _transferActivity.TransferSubstanceMode);

            _taskProperty.TaskActivity = _transferActivity;
        }

        private void InitMachineActivity()
        {
            if (_taskProperty.TaskActivity is MachineActivity machineActivity)
            {
                _machineActivity = machineActivity;
            }

            _machineActivity.MachineActivityType = (MachineActivityType)EditorGUILayout.EnumPopup("Machine Activity Type", _machineActivity.MachineActivityType);
            _machineActivity.MachineType = (MachineType)EditorGUILayout.EnumPopup("Machine Type", _machineActivity.MachineType);
                    
            _taskProperty.TaskActivity = _machineActivity;
        }
        
        private void InitButtonClickedActivity()
        {
            if (_taskProperty.TaskActivity is ButtonClickedActivity buttonClickedActivity)
            {
                _buttonClickedActivity = buttonClickedActivity;
            }

            _buttonClickedActivity.ButtonType = (ButtonType)EditorGUILayout.EnumPopup("Button Type", _buttonClickedActivity.ButtonType);
            
            _taskProperty.TaskActivity = _buttonClickedActivity;
        }

        private void InitSliderValueChangedActivity()
        {
            if (_taskProperty.TaskActivity is SliderValueChangedActivity sliderValueChangedActivity)
            {
                _sliderValueChangedActivity = sliderValueChangedActivity;
            }

            _sliderValueChangedActivity.SliderType = (SliderType) EditorGUILayout.EnumPopup("Slider Type", _sliderValueChangedActivity.SliderType);
            _sliderValueChangedActivity.Value = EditorGUILayout.FloatField("Value", _sliderValueChangedActivity.Value);
            
            _taskProperty.TaskActivity = _sliderValueChangedActivity;
        }
        
        private void InitSocketActivity()
        {
            if (_taskProperty.TaskActivity is SocketActivity socketActivity)
            {
                _socketActivity = socketActivity;
            }

            _socketActivity.SocketType = (SocketType) EditorGUILayout.EnumPopup("Socket Type", _socketActivity.SocketType);
            _socketActivity.SocketActivityType = (SocketActivityType) EditorGUILayout.EnumPopup("Socket Activity Type", _socketActivity.SocketActivityType);
            
            _taskProperty.TaskActivity = _socketActivity;
        }

        private void InitDropdownActivity()
        {
            if (_taskProperty.TaskActivity is DropdownActivity dropdownActivity)
            {
                _dropdownActivity = dropdownActivity;
            }

            _dropdownActivity.DropdownType = (DropdownType) EditorGUILayout.EnumPopup("Dropdown Type", _dropdownActivity.DropdownType);
            _dropdownActivity.Value = EditorGUILayout.IntField("Value", _dropdownActivity.Value);
            
            _taskProperty.TaskActivity = _dropdownActivity;
        }
        
        private void InitAnchorActivity()
        {
            if (_taskProperty.TaskActivity is AnchorActivity anchorActivity)
            {
                _anchorActivity = anchorActivity;
            }

            _anchorActivity.SubstanceName = (SubstanceName) EditorGUILayout.EnumPopup("Reagents Name", _anchorActivity.SubstanceName);
            _taskProperty.TaskActivity = _anchorActivity;
        }

        private void InitSerialisedButtons()
        {
            if (GUILayout.Button("Load"))
            {
                _taskPropertyScriptableObject.Load();
            }
            
            if (GUILayout.Button("Save"))
            {
                _taskPropertyScriptableObject.Save();
            }
            
            if (GUILayout.Button("Load All Tasks"))
            {
                TasksService.LoadAllTasksToScriptableObjects();
            }
            
            if (GUILayout.Button("Save All Tasks"))
            {
                TasksService.SaveAllTasksFromScriptableObjects();
            }
        }
    }
#endif
}
