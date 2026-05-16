using System.Windows;

namespace ElectronicNotepad.UI.Views;

public partial class NoteAnalyticsWindow : Window
{
    public NoteAnalyticsWindow()
    {
        InitializeComponent();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}