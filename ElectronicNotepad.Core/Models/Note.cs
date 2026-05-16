using System;
using System.Collections.Generic;
using ElectronicNotepad.Core.Enums;

namespace ElectronicNotepad.Core.Models;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public Guid? CategoryId { get; set; }
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    
    public List<Reminder> Reminders { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
}