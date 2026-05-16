using System;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.UI.Utils;

public interface IUndoableCommand
{
    void Execute();
    void Undo();
}

public class AddNoteCommand : IUndoableCommand
{
    private readonly INoteRepository _repository;
    private readonly Note _note;
    private readonly Action _onChanged;

    public AddNoteCommand(INoteRepository repository, Note note, Action onChanged)
    {
        _repository = repository;
        _note = note;
        _onChanged = onChanged;
    }

    public void Execute()
    {
        _repository.AddNote(_note);
        _onChanged();
    }

    public void Undo()
    {
        _repository.DeleteNote(_note.Id);
        _onChanged();
    }
}

public class DeleteNoteCommand : IUndoableCommand
{
    private readonly INoteRepository _repository;
    private readonly Note _note;
    private readonly Action _onChanged;

    public DeleteNoteCommand(INoteRepository repository, Note note, Action onChanged)
    {
        _repository = repository;
        _note = note;
        _onChanged = onChanged;
    }

    public void Execute()
    {
        _repository.DeleteNote(_note.Id);
        _onChanged();
    }

    public void Undo()
    {
        _repository.AddNote(_note);
        _onChanged();
    }
}

public class UpdateNoteCommand : IUndoableCommand
{
    private readonly INoteRepository _repository;
    private readonly Note _oldState;
    private readonly Note _newState;
    private readonly Action _onChanged;

    public UpdateNoteCommand(INoteRepository repository, Note oldState, Note newState, Action onChanged)
    {
        _repository = repository;
        _oldState = oldState;
        _newState = newState;
        _onChanged = onChanged;
    }

    public void Execute()
    {
        _repository.UpdateNote(_newState);
        _onChanged();
    }

    public void Undo()
    {
        _repository.UpdateNote(_oldState);
        _onChanged();
    }
}