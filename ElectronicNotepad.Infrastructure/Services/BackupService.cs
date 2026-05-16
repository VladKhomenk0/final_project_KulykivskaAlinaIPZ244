using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ElectronicNotepad.Core.Interfaces;

namespace ElectronicNotepad.Infrastructure.Services;

public class BackupService : IBackupService
{
    private readonly string _backupFolder;

    public BackupService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _backupFolder = Path.Combine(appData, "ElectronicNotepad", "Backups");
        if (!Directory.Exists(_backupFolder))
        {
            Directory.CreateDirectory(_backupFolder);
        }
    }

    public string CreateBackup(string sourceFilePath)
    {
        if (!File.Exists(sourceFilePath))
            throw new FileNotFoundException("Source file not found", sourceFilePath);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
        var backupPath = Path.Combine(_backupFolder, $"{fileName}_{timestamp}.bak");

        try
        {
            File.Copy(sourceFilePath, backupPath, true);
            
            var files = new DirectoryInfo(_backupFolder).GetFiles("*.bak")
                .OrderByDescending(f => f.CreationTime)
                .Skip(5);
            
            foreach (var file in files)
            {
                file.Delete();
            }

            return backupPath;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create backup: {ex.Message}", ex);
        }
    }

    public void RestoreBackup(string backupFilePath, string targetFilePath)
    {
        if (!File.Exists(backupFilePath))
            throw new FileNotFoundException("Backup file not found", backupFilePath);

        try
        {
            if (File.Exists(targetFilePath))
            {
                var tempBackup = targetFilePath + ".tmp";
                File.Copy(targetFilePath, tempBackup, true);
            }

            File.Copy(backupFilePath, targetFilePath, true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to restore backup: {ex.Message}", ex);
        }
    }

    public void ExportToZip(string sourceFilePath, string zipPath)
    {
        try
        {
            if (File.Exists(zipPath)) File.Delete(zipPath);
            
            using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            archive.CreateEntryFromFile(sourceFilePath, Path.GetFileName(sourceFilePath));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to export to ZIP: {ex.Message}", ex);
        }
    }
}