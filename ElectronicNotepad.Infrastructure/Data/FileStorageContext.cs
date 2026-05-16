using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Infrastructure.Data;

public class FileStorageContext
{
    private readonly string _filePath = "notes.json";
    public string FilePath => _filePath;
    private readonly JsonSerializerOptions _options;

    public FileStorageContext()
    {
        _options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
    }

    public List<Note> LoadNotes()
    {
        if (!File.Exists(_filePath)) return new List<Note>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Note>>(json, _options) ?? new List<Note>();
    }

    public void SaveNotes(List<Note> notes)
    {
        var json = JsonSerializer.Serialize(notes, _options);
        File.WriteAllText(_filePath, json);
    }
}