using System.Collections.Generic;
using ElectronicNotepad.Core.Interfaces;

namespace ElectronicNotepad.Core.Services;

public class NoteContentValidator : IPropertyValidator<string>
{
    public IEnumerable<string> Validate(string value)
    {
        if (value?.Length > 5000)
            yield return "Зміст занадто великий (макс. 5000 символів).";
    }
}
