using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Tasks.SideEffects;
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
        
        private void OnEnable()
        {
            _taskPropertyScriptableObject = (TasksPropertyScriptableObject)target;
            _taskProperty = _taskPropertyScriptableObject.TaskProperty;
            _serializedObject = new SerializedObject(target);
        }
        
        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            InitTaskInfoFields();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Activity", EditorStyles.boldLabel);
            
            EActivity activityType = (EActivity)EditorGUILayout.EnumPopup("Activity Type", _taskProperty.ActivityConfig.ActivityType);
            if (activityType != _taskProperty.ActivityConfig.ActivityType)
            {
                _taskProperty.ActivityConfig.ActivityType = activityType;
                InitActivityByType(_taskProperty.ActivityConfig.ActivityType);   
            }
            _taskProperty.ActivityConfig.Activity.ShowInEditor();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Side Effects", EditorStyles.boldLabel);

            for (int i = 0 ; i < _taskProperty.SideEffectConfigs.Length; i++)
            {
                ESideEffect eSideEffect = (ESideEffect)EditorGUILayout.EnumPopup("Side Effect Type", _taskProperty.SideEffectConfigs[i].SideEffectType);
                if (eSideEffect != _taskProperty.SideEffectConfigs[i].SideEffectType)
                {
                    _taskProperty.SideEffectConfigs[i].SideEffectType = eSideEffect;
                    InitSideEffectByType(_taskProperty.SideEffectConfigs[i].SideEffectType, i);   
                }
                
                _taskProperty.SideEffectConfigs[i].SideEffect.ShowInEditor();
            }
            
            InitSerialisedButtons();

            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_taskPropertyScriptableObject);
        }

        private void InitSideEffectByType(ESideEffect eSideEffect, int effectID)
        {
            switch (eSideEffect)
            {
                case ESideEffect.Effect1:
                    _taskProperty.SideEffectConfigs[effectID].SideEffect = new Effect1();
                    break;
                case ESideEffect.Effect2:
                    _taskProperty.SideEffectConfigs[effectID].SideEffect = new Effect2();
                    break;
                case ESideEffect.AddReagentsSideEffect:
                    _taskProperty.SideEffectConfigs[effectID].SideEffect = new AddReagentsSideEffect();
                    break;
                default:
                    Debug.LogError("Can't find Side effect!");
                    break;
            }
        }

        private void InitActivityByType(EActivity eActivity)
        {
            switch (eActivity)
            {
                case EActivity.CraftSubstanceActivity:
                    _taskProperty.ActivityConfig.Activity = new CraftSubstanceActivity();
                    break;
                case EActivity.AddSubstanceActivity:
                    _taskProperty.ActivityConfig.Activity = new AddSubstanceActivity();
                    break;
                case EActivity.MachineActivity:
                    _taskProperty.ActivityConfig.Activity = new MachineActivity();
                    break;
                case EActivity.SocketActivity:
                    _taskProperty.ActivityConfig.Activity = new SocketActivity();
                    break;
                case EActivity.AnchorActivity:
                    _taskProperty.ActivityConfig.Activity = new AnchorActivity();
                    break;
                default:
                    Debug.LogError("Can't find action!");
                    break;
            }
        }

        private void InitTaskInfoFields()
        {
            _taskProperty.Number = EditorGUILayout.IntField("Number", _taskProperty.Number);
            _taskProperty.Title = EditorGUILayout.TextField("Title", _taskProperty.Title);
            _taskProperty.Description = EditorGUILayout.TextField("Description", _taskProperty.Description);
            _taskProperty.Warning = EditorGUILayout.TextField("Warning", _taskProperty.Warning);
            _taskProperty.SaveableTask = EditorGUILayout.Toggle("Saveable Task", _taskProperty.SaveableTask);
        }
        
        private void InitSerialisedButtons()
        {
            if (GUILayout.Button("Add Side Effect"))
            {
                SideEffectConfig[] sideEffectConfigs = new SideEffectConfig[_taskProperty.SideEffectConfigs.Length + 1];

                int i;
                for(i = 0; i < _taskProperty.SideEffectConfigs.Length; i++)
                {
                    sideEffectConfigs[i] = _taskProperty.SideEffectConfigs[i];
                }

                sideEffectConfigs[i] = new SideEffectConfig();

                _taskProperty.SideEffectConfigs = sideEffectConfigs;
            }
            
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
