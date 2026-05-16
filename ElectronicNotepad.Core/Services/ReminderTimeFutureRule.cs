using System;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class ReminderTimeFutureRule : IValidationRule<Reminder>
{
    public bool IsValid(Reminder entity, out string errorMessage)
    {
        if (entity.ReminderTime < DateTime.Now.AddSeconds(-10))
        {
            errorMessage = "Час нагадування не може бути в минулому.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}
