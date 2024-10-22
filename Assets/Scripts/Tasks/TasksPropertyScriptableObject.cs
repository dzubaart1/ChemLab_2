using BioEngineerLab.JSON;
using UnityEngine;

namespace BioEngineerLab.Tasks
{
    [CreateAssetMenu(fileName = "Task", menuName = "Tasks/Task", order = 1)]
    public class TasksPropertyScriptableObject : ScriptableObject
    {
        public TaskProperty TaskProperty = new TaskProperty();
        
        public void Save()
        {
            JSONSaver.Save(TaskProperty, FilePath());
        }

        public void Load()
        {
            var obj = JSONSaver.Load<TaskProperty>(FilePath());
            FillFields(obj);
        }

        public void DeleteEffect(SideEffectConfig target)
        {
            foreach (var effect in TaskProperty.SideEffectConfigs)
            {
                if (effect == target)
                {
                    TaskProperty.SideEffectConfigs.Remove(effect);
                    break;
                }
            }
        }

        public void AddEffect()
        {
            TaskProperty.SideEffectConfigs.Add(new SideEffectConfig()); ;
        }

        public void FillFields(TaskProperty taskProperty)
        {
            TaskProperty.Number = taskProperty.Number;
            TaskProperty.Title = taskProperty.Title;
            TaskProperty.Description = taskProperty.Description;
            TaskProperty.Warning = taskProperty.Warning;
            TaskProperty.SaveableTask = taskProperty.SaveableTask;
            TaskProperty.SideEffectConfigs = taskProperty.SideEffectConfigs;
            TaskProperty.ActivityConfig = taskProperty.ActivityConfig;
        }

        private string FilePath()
        {
            return $"{Application.streamingAssetsPath}/tasks/task_{TaskProperty.Number}.txt";
        }
    }
}
