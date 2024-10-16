using BioEngineerLab.JSON;
using BioEngineerLab.Tasks.SideEffects;
using UnityEngine;

namespace BioEngineerLab.Tasks
{
    [CreateAssetMenu(fileName = "Task", menuName = "Tasks/Task", order = 1)]
    public class TasksPropertyScriptableObject : ScriptableObject
    {
        public TaskProperty TaskProperty;

        private void Awake()
        {
            if (TaskProperty is null)
            {
                TaskProperty = new TaskProperty();
            }
        }

        public void Save()
        {
            JSONSaver.Save(TaskProperty, FilePath());
        }

        public void Load()
        {
            var obj = JSONSaver.Load<TaskProperty>(FilePath());
            FillFields(obj);
        }

        public void FillFields(TaskProperty taskProperty)
        {
            TaskProperty.Number = taskProperty.Number;
            TaskProperty.Title = taskProperty.Title;
            TaskProperty.Description = taskProperty.Description;
            TaskProperty.Warning = taskProperty.Warning;
            TaskProperty.SaveableTask = taskProperty.SaveableTask;
            TaskProperty.ActivityConfig = taskProperty.ActivityConfig;
            TaskProperty.SideEffectConfigs = taskProperty.SideEffectConfigs;

            TaskProperty.SideEffectConfigs = new SideEffectConfig[taskProperty.SideEffectConfigs.Length];
            for (int i = 0; i < taskProperty.SideEffectConfigs.Length; i++)
            {
                if (TaskProperty.SideEffectConfigs[i] != null)
                {
                    TaskProperty.SideEffectConfigs[i].SideEffectType = taskProperty.SideEffectConfigs[i].SideEffectType;
                    TaskProperty.SideEffectConfigs[i].SideEffect = taskProperty.SideEffectConfigs[i].SideEffect;
                }
            }
        }

        private string FilePath()
        {
            return $"{Application.streamingAssetsPath}/tasks/task_{TaskProperty.Number}.txt";
        }
    }
}
