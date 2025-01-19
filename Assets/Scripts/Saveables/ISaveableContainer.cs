namespace Saveables
{
    public interface ISaveableContainer
    {
        public void Save();
        public void PutSavedContainerType();
        public void PutSavedSubstances();
        public void PutSavedAnchor();
        public void ReleaseAnchor();
    }
}