namespace ElectronicNotepad.Core.Interfaces;

public interface IValidationRule<T>
{
    bool IsValid(T entity, out string errorMessage);
}
