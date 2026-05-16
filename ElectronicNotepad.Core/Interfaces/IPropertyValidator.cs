using System.Collections.Generic;

namespace ElectronicNotepad.Core.Interfaces;

public interface IPropertyValidator<T>
{
    IEnumerable<string> Validate(T value);
}
