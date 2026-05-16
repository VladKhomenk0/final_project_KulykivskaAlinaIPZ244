namespace ElectronicNotepad.Core.Interfaces;

public interface IDialogService
{
    void ShowMessage(string message, string title);
    bool AskConfirmation(string message, string title);
}