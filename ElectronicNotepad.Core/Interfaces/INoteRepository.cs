using System;
using System.Collections.Generic;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Interfaces;

public interface INoteRepository
{
    IEnumerable<Note> GetAllNotes();
    Note GetNoteById(Guid id);
    void AddNote(Note note);
    void UpdateNote(Note note);
    void DeleteNote(Guid id);
    
    IEnumerable<Category> GetAllCategories();
    void AddCategory(Category category);
    void DeleteCategory(Guid id);
    
    IEnumerable<Tag> GetAllTags();
    void AddTag(Tag tag);
    void DeleteTag(Guid id);
}