using System;
using System.Collections.Generic;
using ElectronicNotepad.Core.Models;
using ElectronicNotepad.Core.Enums;

namespace ElectronicNotepad.Core.Interfaces;

public interface ISearchService
{
    IEnumerable<Note> AdvancedFilter(
        IEnumerable<Note> notes, 
        string searchText, 
        Guid? categoryId, 
        PriorityLevel? priority);
}