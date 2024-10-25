using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks.SideEffects;
using Database;
using UnityEditor;
using UnityEngine;

namespace BioEngineerLab.Tasks
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SOLabTask))]
    public class SOLabTaskEditor : Editor
    {
        private SOLabTask _soLabTask;
        private SerializedObject _serializedObject;
        
        private void OnEnable()
        {
            _soLabTask = (SOLabTask)target;
            _serializedObject = new SerializedObject(target);
        }
        
        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            ShowTaskInfo();

            EditorGUILayout.Space();
            ShowActivity();
            
            EditorGUILayout.Space();
            ShowSideEffects();
            
            EditorGUILayout.Space();
            ShowButtons();

            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_soLabTask);
        }

        private void ShowActivity()
        {
            EditorGUILayout.LabelField("Activity", EditorStyles.boldLabel);
            
            EActivity activityType = (EActivity)EditorGUILayout.EnumPopup("Activity Type", _soLabTask.GetActivity().GetActivityType());
            if (activityType != _soLabTask.GetActivity().GetActivityType())
            {
                InitActivityByType(activityType);   
            }
            _soLabTask.GetActivity().ShowInEditor();
        }

        private void ShowSideEffects()
        {
            EditorGUILayout.LabelField("Side Effects", EditorStyles.boldLabel);

            List<EditorSideEffect> sideEffects = _soLabTask.GetSideEffects();
            
            for (int i = 0 ; i < sideEffects.Count; i++)
            {
                EditorGUILayout.LabelField($"{i} effect", EditorStyles.boldLabel);
                
                ESideEffect sideEffectType = (ESideEffect)EditorGUILayout.EnumPopup("Side Effect Type", sideEffects[i].GetSideEffectType());
                if (sideEffectType != sideEffects[i].GetSideEffectType())
                {
                    InitSideEffectByType(sideEffectType, i);   
                }
                
                sideEffects[i].ShowInEditor();
                if (GUILayout.Button("Delete Effect"))
                {
                    _soLabTask.RemoveSideEffect(i);
                }
            }
        }
        
        private void InitSideEffectByType(ESideEffect eSideEffect, int effectID)
        {
            switch (eSideEffect)
            {
                case ESideEffect.Effect1:
                    _soLabTask.SetSideEffect(new Effect1LabSideEffect(), effectID);
                    break;
                case ESideEffect.Effect2:
                    _soLabTask.SetSideEffect(new Effect2LabSideEffect(), effectID);
                    break;
                case ESideEffect.AddReagentsSideEffect:
                    _soLabTask.SetSideEffect(new AddReagentsLabSideEffect(), effectID);
                    break;
                default:
                    Debug.LogError("Can't find Side effect!");
                    break;
            }
        }
        
        private void InitActivityByType(EActivity activityType)
        {
            switch (activityType)
            {
                case EActivity.AnchorActivity:
                    _soLabTask.SetActivity(new AnchorLabActivity());
                    break;
                case EActivity.AddSubstanceActivity:
                    _soLabTask.SetActivity(new AddSubstanceLabActivity());
                    break;
                case EActivity.MachineActivity:
                    _soLabTask.SetActivity(new MachineLabActivity());
                    break;
                case EActivity.SocketActivity:
                    _soLabTask.SetActivity(new SocketLabActivity());
                    break;
                case EActivity.CraftSubstanceActivity:
                    _soLabTask.SetActivity(new CraftSubstanceLabActivity());
                    break;
                default:
                    Debug.LogError("Can't find action!");
                    break;
            }
        }

        private void ShowTaskInfo()
        {
            _soLabTask.LabTask.Number = EditorGUILayout.IntField("Number", _soLabTask.LabTask.Number);
            _soLabTask.LabTask.Title = EditorGUILayout.TextField("Title", _soLabTask.LabTask.Title);
            _soLabTask.LabTask.Description = EditorGUILayout.TextField("Description", _soLabTask.LabTask.Description);
            _soLabTask.LabTask.Warning = EditorGUILayout.TextField("Warning", _soLabTask.LabTask.Warning);
            _soLabTask.LabTask.SaveableTask = EditorGUILayout.Toggle("Saveable Task", _soLabTask.LabTask.SaveableTask);
        }

        private void ShowButtons()
        {
            if (GUILayout.Button("Add Side Effect"))
            {
                _soLabTask.AddSideEffect();
            }
            
            if (GUILayout.Button("Load"))
            {
                _soLabTask.LoadHandlerByTaskNumber();
            }
            
            if (GUILayout.Button("Save"))
            {
                _soLabTask.SaveHandlerByTaskNumber();
            }

            if (GUILayout.Button("Save All Task"))
            {
                ResourcesDatabase.SaveAllTaskToDataBase();
            }

            if (GUILayout.Button("Load All Task"))
            {
                ResourcesDatabase.LoadAllTaskFromDataBase();
            }
        }
    }
#endif
}
