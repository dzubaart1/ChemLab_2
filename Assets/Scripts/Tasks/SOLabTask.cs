﻿using System.Collections.Generic;
using System.Linq;
using Activities.ActivitiesEditor;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using Database;
using JetBrains.Annotations;
using UnityEngine;

namespace BioEngineerLab.Tasks
{
    [CreateAssetMenu(fileName = "LabTask", menuName = "LabTasks/LabTask", order = 1)]
    public class SOLabTask : ScriptableObject
    {
        public LabTask LabTask = new LabTask();

        public void LoadHandlerByTaskNumber()
        {
            LabTask res = LabTasksDatabase.ReadWhere(task => task.Number == LabTask.Number, LabTask.Lab).FirstOrDefault();

            if (res != null)
            {
                LabTask = res;
            }
        }
        
        public void SaveHandlerByTaskNumber()
        {
            LabTasksDatabase.Save(LabTask);
        }

        public List<EditorSideEffect> GetSideEffects()
        {
            List<EditorSideEffect> res = new List<EditorSideEffect>();

            foreach (var labSideEffect in LabTask.LabSideEffects)
            {
                res.Add(GetEditorSideEffect(labSideEffect));
            }

            return res;
        }

        public void AddSideEffect()
        {
            LabTask.LabSideEffects.Add(new AddReagentsLabSideEffect());
        }

        public void RemoveSideEffect(int id)
        {
            LabTask.LabSideEffects.RemoveAt(id);
        }

        public void SetSideEffect(LabSideEffect sideEffect, int i)
        {
            LabTask.LabSideEffects[i] = sideEffect;
        }

        public void SetActivity(LabActivity labActivity)
        {
            LabTask.LabActivity = labActivity;
        }
        
        public EditorActivity GetActivity()
        {
            return GetEditorActivity(LabTask.LabActivity);
        }

        [CanBeNull]
        private EditorActivity GetEditorActivity(LabActivity labActivity)
        {
#if UNITY_EDITOR
            switch (labActivity.ActivityType)
            {
                case EActivity.AnchorActivity:
                    return new AnchorActivityEditor(labActivity);
                case EActivity.AddSubstanceActivity:
                    return new AddSubstanceActivityEditor(labActivity);
                case EActivity.CraftSubstanceActivity:
                    return new CraftSubstanceActivityEditor(labActivity);
                case EActivity.MachineActivity:
                    return new MachineLabActivityEditor(labActivity);
                case EActivity.SocketSubstancesActivity:
                    return new SocketSubstancesLabActivityEditor(labActivity);
                case EActivity.ButtonClickedActivity:
                    return new ButtonClickedActivityEditor(labActivity);
                case EActivity.SocketActivity:
                    return new SocketLabActivityEditor(labActivity);
                case EActivity.DoorActivity:
                    return new DoorLabActivityEditor(labActivity);
                case EActivity.PulveriazatorActivity:
                    return new PulverizatorLabActivityEditor(labActivity);
            }
#endif

            return null;
        }

        [CanBeNull]
        private EditorSideEffect GetEditorSideEffect(LabSideEffect labSideEffect)
        {
#if UNITY_EDITOR
            switch (labSideEffect.SideEffectType)
            {
                case ESideEffect.AddReagentsSideEffect:
                    return new AddReagentsSideEffectEditor(labSideEffect);
                case ESideEffect.SetDozatorVolumeSideEffect:
                    return new SetDozatorVolumeLabSideEffectEditor(labSideEffect);
                case ESideEffect.SpawnDocSideEffect:
                    return new SpawnDocSideEffectEditor(labSideEffect);
                case ESideEffect.ConstructorSideEffect:
                    return new ConstructorLabSideEffectEditor(labSideEffect);
                case ESideEffect.SetInteractableSideEffect:
                    return new SetInteractableLabSideEffectEditor(labSideEffect);
            }
#endif
            return null;
        }
    }
}