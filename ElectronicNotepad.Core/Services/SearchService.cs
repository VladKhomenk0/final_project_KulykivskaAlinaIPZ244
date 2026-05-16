using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;
using ElectronicNotepad.Core.Enums;

namespace ElectronicNotepad.Core.Services;

public class SearchService : ISearchService
{
    public IEnumerable<Note> AdvancedFilter(
        IEnumerable<Note> notes, 
        string searchText, 
        Guid? categoryId, 
        PriorityLevel? priority)
    {
        var filtered = notes.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var lowerSearch = searchText.ToLower();
            filtered = filtered.Where(n => 
                n.Title.ToLower().Contains(lowerSearch) || 
                n.Content.ToLower().Contains(lowerSearch));
        }

        if (categoryId.HasValue)
        {
            filtered = filtered.Where(n => n.CategoryId == categoryId.Value);
        }

        if (priority.HasValue)
        {
            filtered = filtered.Where(n => n.Priority == priority.Value);
        }

        return filtered;
    }
}