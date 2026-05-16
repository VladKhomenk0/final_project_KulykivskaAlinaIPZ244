namespace ElectronicNotepad.Core.Interfaces;

public interface IBackupService
{
    string CreateBackup(string sourceFilePath);
    void RestoreBackup(string backupFilePath, string targetFilePath);
}
