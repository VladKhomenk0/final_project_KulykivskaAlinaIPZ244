using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class NoteTitleLengthRule : IValidationRule<Note>
{
    private readonly int _maxLength;
    public NoteTitleLengthRule(int maxLength = 100) => _maxLength = maxLength;

    public bool IsValid(Note entity, out string errorMessage)
    {
        if (entity.Title.Length > _maxLength)
        {
            errorMessage = $"Заголовок занадто довгий (макс. {_maxLength} символів).";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}
