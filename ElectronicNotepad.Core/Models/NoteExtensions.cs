using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ElectronicNotepad.Core.Models;

public static class NoteExtensions
{
    public static int GetWordCount(this Note note)
    {
        if (string.IsNullOrWhiteSpace(note.Content)) return 0;
        return Regex.Matches(note.Content, @"\b\w+\b").Count;
    }

    public static int GetCharacterCount(this Note note, bool includeWhitespace = true)
    {
        if (string.IsNullOrWhiteSpace(note.Content)) return 0;
        return includeWhitespace ? note.Content.Length : note.Content.Replace(" ", "").Replace("\n", "").Replace("\r", "").Length;
    }

    public static int GetLineCount(this Note note)
    {
        if (string.IsNullOrWhiteSpace(note.Content)) return 0;
        return note.Content.Split('\n').Length;
    }

    public static double GetEstimatedReadingTimeMinutes(this Note note, int wordsPerMinute = 200)
    {
        int words = note.GetWordCount();
        if (words == 0) return 0;
        return (double)words / wordsPerMinute;
    }

    public static string GetFormattedReadingTime(this Note note)
    {
        double minutes = note.GetEstimatedReadingTimeMinutes();
        if (minutes < 1) return "Менше хвилини";
        return $"{Math.Ceiling(minutes)} хв.";
    }
}
