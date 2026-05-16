using System.Collections.Generic;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Interfaces;

public interface IValidationService
{
    IEnumerable<string> ValidateNote(Note note);
    IEnumerable<string> ValidateReminder(Reminder reminder);
    void AddNoteRule(IValidationRule<Note> rule);
    void AddReminderRule(IValidationRule<Reminder> rule);
}
