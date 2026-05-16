using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Infrastructure.Services;

public class ReminderTimerService : IReminderService
{
    private readonly List<Reminder> _reminders = new();
    private readonly CancellationTokenSource _cts = new();
    public event Action<Reminder>? OnReminderTriggered;

    public ReminderTimerService()
    {
        Task.Run(CheckLoop, _cts.Token);
    }

    private async Task CheckLoop()
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            var now = DateTime.Now;
            List<Reminder> triggered;

            lock (_reminders)
            {
                triggered = _reminders
                    .Where(r => r.ReminderTime <= now && !r.IsCompleted)
                    .ToList();
            }

            foreach (var reminder in triggered)
            {
                reminder.IsCompleted = true;
                OnReminderTriggered?.Invoke(reminder);
            }

            await Task.Delay(1000);
        }
    }

    public void SetReminder(Reminder reminder)
    {
        lock (_reminders)
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
    }

    public void CancelReminder(Guid reminderId)
    {
        lock (_reminders)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            if (reminder != null)
            {
                _reminders.Remove(reminder);
            }
        }
    }

    public void ClearRemindersForNote(Guid noteId)
    {
        lock (_reminders)
        {
            _reminders.RemoveAll(r => r.NoteId == noteId);
        }
    }
}