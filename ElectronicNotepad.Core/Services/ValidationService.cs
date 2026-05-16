using System.Collections.Generic;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class ValidationService : IValidationService
{
    private readonly List<IValidationRule<Note>> _noteRules = new()
    {
        new NoteTitleRequiredRule(),
        new NoteTitleLengthRule(),
        new NoteContentLengthRule()
    };

    private readonly List<IValidationRule<Reminder>> _reminderRules = new()
    {
        new ReminderMessageRequiredRule(),
        new ReminderTimeFutureRule()
    };

    public IEnumerable<string> ValidateNote(Note note)
    {
        var errors = new List<string>();
        foreach (var rule in _noteRules)
        {
            if (!rule.IsValid(note, out var error))
                errors.Add(error);
        }
        return errors;
    }

    public IEnumerable<string> ValidateReminder(Reminder reminder)
    {
        var errors = new List<string>();
        foreach (var rule in _reminderRules)
        {
            if (!rule.IsValid(reminder, out var error))
                errors.Add(error);
        }
        return errors;
    }

    public void AddNoteRule(IValidationRule<Note> rule) => _noteRules.Add(rule);
    public void AddReminderRule(IValidationRule<Reminder> rule) => _reminderRules.Add(rule);
}
