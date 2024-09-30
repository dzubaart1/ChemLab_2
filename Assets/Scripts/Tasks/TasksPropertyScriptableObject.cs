using BioEngineerLab.JSON;
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
            TaskProperty.UnlockSyringe = taskProperty.UnlockSyringe;
            TaskProperty.ShouldChangeGateOpening = taskProperty.ShouldChangeGateOpening;
            TaskProperty.IsOpenGate = taskProperty.IsOpenGate;
            TaskProperty.IsTaskChangeSprite = taskProperty.IsTaskChangeSprite;
            TaskProperty.SpriteName = taskProperty.SpriteName;
            TaskProperty.HasHintSprite = taskProperty.HasHintSprite;
            TaskProperty.HintSpriteName = taskProperty.HintSpriteName;
            TaskProperty.IsSubstanceAdding = taskProperty.IsSubstanceAdding;
            TaskProperty.SubstanceName = taskProperty.SubstanceName;
            TaskProperty.SubstanceWeight = taskProperty.SubstanceWeight;
            TaskProperty.ActivityType = taskProperty.ActivityType;
            TaskProperty.TaskActivity = taskProperty.TaskActivity;
            TaskProperty.IsTaskChangeSyringePos = taskProperty.IsTaskChangeSyringePos;
        }

        private string FilePath()
        {
            return $"{Application.streamingAssetsPath}/tasks/task_{TaskProperty.Number}.txt";
        }
    }
}
