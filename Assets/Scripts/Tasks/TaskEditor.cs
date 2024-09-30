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
        private SocketActivity _socketActivity;
        
        private void OnEnable()
        {
            _transferActivity = new TransferActivity();
            _machineActivity = new MachineActivity();
            _socketActivity = new SocketActivity();
            
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
                case ActivityType.SocketActivity:
                    InitSocketActivity();
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
