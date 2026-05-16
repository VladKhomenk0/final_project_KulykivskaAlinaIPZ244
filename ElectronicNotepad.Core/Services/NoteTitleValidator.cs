using System.Collections.Generic;
using ElectronicNotepad.Core.Interfaces;

namespace ElectronicNotepad.Core.Services;

public class NoteTitleValidator : IPropertyValidator<string>
{
    public IEnumerable<string> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            yield return "Заголовок не може бути порожнім.";
        if (value?.Length > 100)
            yield return "Заголовок не може бути довшим за 100 символів.";
        if (value?.Length < 3)
            yield return "Заголовок занадто короткий (мін. 3 символи).";
    }
}
