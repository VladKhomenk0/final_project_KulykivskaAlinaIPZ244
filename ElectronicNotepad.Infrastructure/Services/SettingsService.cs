using System;
using System.IO;
using System.Text.Json;

namespace ElectronicNotepad.Infrastructure.Services;

public class AppSettings
{
    public bool IsDarkTheme { get; set; }
    public string LastOpenedFile { get; set; } = string.Empty;
    public double WindowWidth { get; set; } = 540;
    public double WindowHeight { get; set; } = 960;
}

public interface ISettingsService
{
    AppSettings LoadSettings();
    void SaveSettings(AppSettings settings);
}

public class SettingsService : ISettingsService
{
    private readonly string _filePath;

    public SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, "ElectronicNotepad");
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        _filePath = Path.Combine(folder, "settings.json");
    }

    public AppSettings LoadSettings()
    {
        if (!File.Exists(_filePath))
        {
            return new AppSettings();
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void SaveSettings(AppSettings settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch
        {
        }
    }
}