using System.Windows;
using ElectronicNotepad.UI.ViewModels;

namespace ElectronicNotepad.UI.Views;

public partial class AddNoteWindow : Window
{
    public AddNoteWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close(); 
    }
}