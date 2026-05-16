using System;
using System.Collections.Generic;
using ElectronicNotepad.Core.Interfaces;

namespace ElectronicNotepad.Core.Services;

public class ReminderTimeValidator : IPropertyValidator<DateTime>
{
    public IEnumerable<string> Validate(DateTime value)
    {
        if (value < DateTime.Now.AddSeconds(-30))
            yield return "Час нагадування не може бути в минулому.";
        if (value > DateTime.Now.AddYears(10))
            yield return "Час нагадування занадто далекий у майбутньому.";
    }
}
