using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicNotepad.Core.Models;
using ElectronicNotepad.UI.Utils;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Enums;

namespace ElectronicNotepad.UI.ViewModels;

public class NoteEditorViewModel : ViewModelBase
{
    private readonly INoteRepository _repository;
    public Note CurrentNote { get; set; }
    
    public ObservableCollection<Category> Categories { get; }
    public IEnumerable<PriorityLevel> PriorityLevels => Enum.GetValues(typeof(PriorityLevel)).Cast<PriorityLevel>();

    private bool _hasReminder;
    public bool HasReminder
    {
        get => _hasReminder;
        set 
        {
            if (SetProperty(ref _hasReminder, value))
            {
                if (!value)
                {
                    Reminders.Clear();
                    CurrentNote.Reminders.Clear();
                    RemoveErrors(nameof(ReminderMessage));
                }
                else if (Reminders.Count == 0)
                {
                    AddDefaultReminder();
                }
            }
        }
    }

    public ObservableCollection<Reminder> Reminders { get; }
    
    public Reminder? ActiveReminder => Reminders.FirstOrDefault();

    public string NoteTitle
    {
        get => CurrentNote.Title;
        set
        {
            CurrentNote.Title = value;
            ValidateTitle();
            OnPropertyChanged();
        }
    }

    public string NoteColor
    {
        get => CurrentNote.ColorHex ?? "#A8D5BA";
        set
        {
            CurrentNote.ColorHex = value;
            OnPropertyChanged();
        }
    }

    public List<string> AvailableColors { get; } = new()
    {
        "#A8D5BA", "#FFD1DC", "#FFECB3", "#B3E5FC", "#D1C4E9", "#E1F5FE", "#FFFFFF"
    };

    public string ReminderMessage
    {
        get => ActiveReminder?.Message ?? string.Empty;
        set 
        {
            if (ActiveReminder != null)
            {
                ActiveReminder.Message = value;
                ValidateReminderMessage();
                OnPropertyChanged();
            }
        }
    }

    public DateTime ReminderDate
    {
        get => ActiveReminder?.ReminderTime ?? DateTime.Now.AddHours(1);
        set
        {
            if (ActiveReminder != null)
            {
                var time = ActiveReminder.ReminderTime.TimeOfDay;
                ActiveReminder.ReminderTime = value.Date.Add(time);
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReminderTimeText));
            }
        }
    }

    public string ReminderTimeText
    {
        get => ActiveReminder?.ReminderTime.ToString("HH:mm") ?? "12:00";
        set
        {
            if (ActiveReminder != null && TimeSpan.TryParse(value, out var timeSpan))
            {
                ActiveReminder.ReminderTime = ActiveReminder.ReminderTime.Date.Add(timeSpan);
                OnPropertyChanged();
            }
        }
    }

    public Category? SelectedCategory
    {
        get => Categories.FirstOrDefault(c => c.Id == CurrentNote.CategoryId);
        set
        {
            CurrentNote.CategoryId = value?.Id;
            ValidateCategory();
            OnPropertyChanged();
        }
    }

    public PriorityLevel Priority
    {
        get => CurrentNote.Priority;
        set
        {
            CurrentNote.Priority = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand SaveCommand { get; }
    public RelayCommand SetColorCommand { get; }

    public NoteEditorViewModel(Note note, INoteRepository repository)
    {
        CurrentNote = note;
        _repository = repository;
        Categories = new ObservableCollection<Category>(_repository.GetAllCategories());
        Reminders = new ObservableCollection<Reminder>(note.Reminders);
        _hasReminder = Reminders.Count > 0;
        
        SaveCommand = new RelayCommand(obj => Save(), obj => !HasErrors);
        SetColorCommand = new RelayCommand(color => { NoteColor = color?.ToString() ?? "#A8D5BA"; });
        
        ValidateAll();
    }

    private void ValidateAll()
    {
        ValidateTitle();
        ValidateCategory();
        if (HasReminder) ValidateReminderMessage();
    }

    private void ValidateTitle()
    {
        RemoveErrors(nameof(NoteTitle));
        if (string.IsNullOrWhiteSpace(NoteTitle))
            AddError(nameof(NoteTitle), "Заголовок не може бути порожнім");
    }

    private void ValidateCategory()
    {
        RemoveErrors(nameof(SelectedCategory));
        if (SelectedCategory == null)
            AddError(nameof(SelectedCategory), "Обов'язково виберіть категорію");
    }

    private void ValidateReminderMessage()
    {
        RemoveErrors(nameof(ReminderMessage));
        if (HasReminder && string.IsNullOrWhiteSpace(ReminderMessage))
            AddError(nameof(ReminderMessage), "Текст нагадування не може бути порожнім");
    }

    private void AddDefaultReminder()
    {
        var reminder = new Reminder
        {
            NoteId = CurrentNote.Id,
            Message = "Нагадування для: " + CurrentNote.Title,
            ReminderTime = DateTime.Now.AddHours(1)
        };
        Reminders.Add(reminder);
        CurrentNote.Reminders.Add(reminder);
        OnPropertyChanged(nameof(ActiveReminder));
        OnPropertyChanged(nameof(ReminderMessage));
        ValidateReminderMessage();
    }

    private void Save()
    {
        _repository.UpdateNote(CurrentNote);
    }
}