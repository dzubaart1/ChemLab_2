namespace BioEngineerLab.Tasks
{
    public class ErrorTask
    {
        public string TaskText;
        public int TaskNumber;

        public ErrorTask(string taskText, int taskNumber)
        {
            TaskText = taskText;
            TaskNumber = taskNumber;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return TaskText.GetHashCode() + TaskNumber.GetHashCode();
        }
    }
}