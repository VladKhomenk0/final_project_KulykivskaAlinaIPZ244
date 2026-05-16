using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class NoteTitleRequiredRule : IValidationRule<Note>
{
    public bool IsValid(Note entity, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(entity.Title))
        {
            errorMessage = "Заголовок нотатки не може бути порожнім.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}
