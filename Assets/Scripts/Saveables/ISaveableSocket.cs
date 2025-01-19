namespace Saveables
{
    public interface ISaveableSocket
    {
        public void Save();
        public void ReleaseAllLoad();
        public void PutSavedInteractable();
        public void PutSavedLocks();
        public void ReleaseLocks();
    }
}