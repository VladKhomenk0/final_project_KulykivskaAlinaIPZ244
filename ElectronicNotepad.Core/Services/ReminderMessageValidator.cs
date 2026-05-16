using System.Collections.Generic;
using ElectronicNotepad.Core.Interfaces;

namespace ElectronicNotepad.Core.Services;

public class ReminderMessageValidator : IPropertyValidator<string>
{
    public IEnumerable<string> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            yield return "Текст нагадування не може бути порожнім.";
        if (value?.Length > 200)
            yield return "Текст нагадування занадто довгий.";
    }
}
