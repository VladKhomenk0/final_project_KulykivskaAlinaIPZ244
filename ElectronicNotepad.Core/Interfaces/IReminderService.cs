using System;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Interfaces;

public interface IReminderService
{
    void SetReminder(Reminder reminder);
    void CancelReminder(Guid reminderId);
    void ClearRemindersForNote(Guid noteId);
    event Action<Reminder> OnReminderTriggered;
}