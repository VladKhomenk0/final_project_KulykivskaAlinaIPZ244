using ElectronicNotepad.Core.Enums;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Interfaces;

public interface IExportService
{
    string ExportNote(Note note, ExportFormat format);
}
