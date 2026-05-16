using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Infrastructure.Services;

public class ReminderTimerService : IReminderService
{
    private readonly System.Timers.Timer _timer;
    private readonly List<Reminder> _reminders = new();
    public event Action<Reminder>? OnReminderTriggered;

    public ReminderTimerService()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += CheckReminders;
        _timer.Start();
    }

    public void SetReminder(Reminder reminder)
    {
        var existing = _reminders.FirstOrDefault(r => r.Id == reminder.Id);
        if (existing != null)
        {
            existing.Message = reminder.Message;
            existing.ReminderTime = reminder.ReminderTime;
            existing.IsCompleted = reminder.IsCompleted;
        }
        else
        {
            _reminders.Add(reminder);
        }
    }

    public void CancelReminder(Guid reminderId)
    {
        var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
        if (reminder != null)
        {
            _reminders.Remove(reminder);
        }
    }

    public void ClearRemindersForNote(Guid noteId)
    {
        _reminders.RemoveAll(r => r.NoteId == noteId);
    }

    private void CheckReminders(object? sender, System.Timers.ElapsedEventArgs e)
    {
        var now = DateTime.Now;
        var triggeredReminders = _reminders.Where(r => r.ReminderTime <= now && !r.IsCompleted).ToList();

        foreach (var reminder in triggeredReminders)
        {
            reminder.IsCompleted = true;
            OnReminderTriggered?.Invoke(reminder);
        }
    }
}