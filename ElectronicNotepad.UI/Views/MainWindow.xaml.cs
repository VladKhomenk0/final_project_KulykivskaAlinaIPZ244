using System.Windows;
using ElectronicNotepad.UI.ViewModels;
using ElectronicNotepad.Infrastructure.Repositories;
using ElectronicNotepad.Infrastructure.Data;
using ElectronicNotepad.Core.Models;
using System.Windows.Input;
using ElectronicNotepad.Infrastructure.Services;
using System.Linq;
using ElectronicNotepad.Core.Interfaces;
using System;
using ElectronicNotepad.UI.Services;

namespace ElectronicNotepad.UI.Views;

public partial class MainWindow : Window
{
    private readonly NoteRepository _repo;
    private readonly ReminderTimerService _reminderService;
    private readonly IBackupService _backupService;
    private readonly IThemeService _themeService;
    private readonly ISettingsService _settingsService;

    public MainWindow()
    {
        InitializeComponent();
        var context = new FileStorageContext();
        _repo = new NoteRepository(context);
        _reminderService = new ReminderTimerService();
        _backupService = new BackupService();
        _settingsService = new SettingsService();
        _themeService = new ThemeService(_settingsService);
        
        _reminderService.OnReminderTriggered += ReminderService_OnReminderTriggered;
        
        DataContext = new MainViewModel(_repo, _themeService);
        
        LoadRemindersToService();
    }

    private void ShowAnalytics_Click(object sender, RoutedEventArgs e)
    {
        if (NotesList.SelectedItem is Note selectedNote)
        {
            var viewModel = new NoteAnalyticsViewModel(selectedNote);
            var window = new NoteAnalyticsWindow
            {
                DataContext = viewModel,
                Owner = this
            };
            window.ShowDialog();
        }
    }

    private void Backup_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var context = new FileStorageContext();
            var backupPath = _backupService.CreateBackup(context.FilePath);
            MessageBox.Show($"Резервну копію створено за шляхом:\n{backupPath}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка створення бекапу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadRemindersToService()
    {
        foreach (var note in _repo.GetAllNotes())
        {
            foreach (var reminder in note.Reminders.Where(r => !r.IsCompleted))
            {
                _reminderService.SetReminder(reminder);
            }
        }
    }

    private void ReminderService_OnReminderTriggered(Reminder reminder)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Нагадування: {reminder.Message}", "Електронний записник", MessageBoxButton.OK, MessageBoxImage.Information);
            _repo.UpdateNote(_repo.GetNoteById(reminder.NoteId));
        });
    }

    private void AddNote_Click(object sender, RoutedEventArgs e)
    {
        var newNote = new Note { Title = "Нова нотатка", Content = "" };
        ((MainViewModel)DataContext).AddNoteWithUndo(newNote);
        OpenEditor(newNote);
    }

    private void DuplicateNote_Click(object sender, RoutedEventArgs e)
    {
        if (NotesList.SelectedItem is Note selectedNote)
        {
            ((MainViewModel)DataContext).DuplicateNote(selectedNote);
        }
    }

    private void EditNote_Click(object sender, RoutedEventArgs e)
    {
        if (NotesList.SelectedItem is Note selectedNote)
        {
            OpenEditor(selectedNote);
        }
    }

    private void NotesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (NotesList.SelectedItem is Note selectedNote)
        {
            OpenEditor(selectedNote);
        }
    }

    private void OpenEditor(Note note)
    {
        var oldState = new Note
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CategoryId = note.CategoryId,
            Priority = note.Priority,
            Reminders = new System.Collections.Generic.List<Reminder>(note.Reminders)
        };

        var editorViewModel = new NoteEditorViewModel(note, _repo);
        var editorWindow = new AddNoteWindow
        {
            DataContext = editorViewModel,
            Owner = this
        };
        
        if (editorWindow.ShowDialog() == true)
        {
            ((MainViewModel)DataContext).UpdateNoteWithUndo(oldState, note);
            
            _reminderService.ClearRemindersForNote(note.Id);
            foreach (var reminder in note.Reminders.Where(r => !r.IsCompleted))
            {
                _reminderService.SetReminder(reminder);
            }
        }
        
        ((MainViewModel)DataContext).RefreshNotes();
    }

    private void DeleteNote_Click(object sender, RoutedEventArgs e)
    {
        if (NotesList.SelectedItem is Note selectedNote)
        {
            var result = MessageBox.Show($"Ви дійсно хочете видалити нотатку '{selectedNote.Title}'?", "Підтвердження", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ((MainViewModel)DataContext).DeleteNoteWithUndo(selectedNote);
            }
        }
    }
}