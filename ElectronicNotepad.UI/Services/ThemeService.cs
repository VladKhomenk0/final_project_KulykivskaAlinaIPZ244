using System;
using System.Windows;
using ElectronicNotepad.Infrastructure.Services;
using System.Linq;

namespace ElectronicNotepad.UI.Services;

public interface IThemeService
{
    bool IsDarkTheme { get; }
    event Action<bool> ThemeChanged;
    void ToggleTheme();
    void SetTheme(bool isDark);
    void ApplyCurrentTheme();
}

public class ThemeService : IThemeService
{
    private const string DarkThemeUri = "Themes/DarkTheme.xaml";
    private const string LightThemeUri = "Themes/LightTheme.xaml";
    
    private readonly ISettingsService _settingsService;
    public bool IsDarkTheme { get; private set; }
    
    public event Action<bool>? ThemeChanged;

    public ThemeService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        var settings = _settingsService.LoadSettings();
        IsDarkTheme = settings.IsDarkTheme;
        ApplyCurrentTheme();
    }

    public void ToggleTheme()
    {
        SetTheme(!IsDarkTheme);
    }

    public void SetTheme(bool isDark)
    {
        IsDarkTheme = isDark;
        var settings = _settingsService.LoadSettings();
        settings.IsDarkTheme = isDark;
        _settingsService.SaveSettings(settings);
        ApplyCurrentTheme();
        ThemeChanged?.Invoke(IsDarkTheme);
    }

    public void ApplyCurrentTheme()
    {
        var uri = IsDarkTheme ? DarkThemeUri : LightThemeUri;
        var resourceDictionary = new ResourceDictionary
        {
            Source = new Uri(uri, UriKind.RelativeOrAbsolute)
        };

        var app = Application.Current;
        if (app != null)
        {
            var mergedDictionaries = app.Resources.MergedDictionaries;
            var existing = mergedDictionaries.FirstOrDefault(d => d.Source != null && (d.Source.OriginalString.Contains("Theme.xaml")));
            
            if (existing != null)
            {
                mergedDictionaries.Remove(existing);
            }
            mergedDictionaries.Add(resourceDictionary);
        }
    }
}