using System.Collections.Generic;
using ElectronicNotepad.Core.Models;
using ElectronicNotepad.UI.Utils;

namespace ElectronicNotepad.UI.ViewModels;

public class NoteAnalyticsViewModel : ViewModelBase
{
    private readonly Note _note;

    public string Title => _note.Title;
    public int WordCount => _note.GetWordCount();
    public int CharacterCount => _note.GetCharacterCount();
    public int LineCount => _note.GetLineCount();
    public string ReadingTime => _note.GetFormattedReadingTime();

    public NoteAnalyticsViewModel(Note note)
    {
        _note = note;
    }
}