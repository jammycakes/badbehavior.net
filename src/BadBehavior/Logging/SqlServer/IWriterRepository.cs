namespace BadBehavior.Logging.SqlServer
{
    public interface IWriterRepository
    {
        void AddEntry(BadBehavior.Logging.LogEntry entry);
        void ClearLog();
        void CreateTable();
        void PurgeOldEntries();
    }
}
