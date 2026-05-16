using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class NoteContentLengthRule : IValidationRule<Note>
{
    private readonly int _maxLength;
    public NoteContentLengthRule(int maxLength = 5000) => _maxLength = maxLength;

    public bool IsValid(Note entity, out string errorMessage)
    {
        if (entity.Content.Length > _maxLength)
        {
            errorMessage = $"Зміст нотатки занадто об'ємний (макс. {_maxLength} символів).";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}
