using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class ReminderMessageRequiredRule : IValidationRule<Reminder>
{
    public bool IsValid(Reminder entity, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(entity.Message))
        {
            errorMessage = "Текст нагадування не може бути порожнім.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}
