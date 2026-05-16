using System;
using System.Collections.Generic;
using ElectronicNotepad.Core.Interfaces;

namespace ElectronicNotepad.Core.Services;

public class CategoryValidator : IPropertyValidator<Guid?>
{
    public IEnumerable<string> Validate(Guid? value)
    {
        if (value == Guid.Empty)
            yield return "Вибрана некоректна категорія.";
    }
}
