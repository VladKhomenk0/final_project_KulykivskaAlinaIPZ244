using System;

namespace ElectronicNotepad.Core.Models;

public class Reminder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    public DateTime ReminderTime { get; set; }
    public bool IsCompleted { get; set; }
    
    public Guid NoteId { get; set; }
}