using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;
using ElectronicNotepad.UI.Utils;
using ElectronicNotepad.Core.Enums;
using System;
using ElectronicNotepad.Core.Services;
using ElectronicNotepad.UI.Services;

namespace ElectronicNotepad.UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INoteRepository _repository;
    private readonly ISearchService _searchService;
    private readonly IThemeService _themeService;
    private readonly UndoManager _undoManager = new();
    private List<Note> _allNotes;
    
    public ObservableCollection<Note> Notes { get; set; }
    public ObservableCollection<Category> Categories { get; }
    
    public RelayCommand UndoCommand { get; }
    public RelayCommand RedoCommand { get; }
    public RelayCommand ChangeThemeCommand { get; }

    public string ThemeIcon => _themeService.IsDarkTheme ? "☀️" : "🌙";
    public string ThemeName => _themeService.IsDarkTheme ? "Світла тема" : "Темна тема";

    public MainViewModel(INoteRepository repository, IThemeService themeService)
    {
        _repository = repository;
        _searchService = new SearchService();
        _themeService = themeService;
        _allNotes = _repository.GetAllNotes().ToList();
        Notes = new ObservableCollection<Note>(_allNotes);
        Categories = new ObservableCollection<Category>(_repository.GetAllCategories());

        UndoCommand = new RelayCommand(obj => { _undoManager.Undo(); }, obj => _undoManager.CanUndo);
        RedoCommand = new RelayCommand(obj => { _undoManager.Redo(); }, obj => _undoManager.CanRedo);
        ChangeThemeCommand = new RelayCommand(obj => { _themeService.ToggleTheme(); });

        _themeService.ThemeChanged += (isDark) => 
        {
            OnPropertyChanged(nameof(ThemeIcon));
            OnPropertyChanged(nameof(ThemeName));
        };
    }

    public void AddNoteWithUndo(Note note)
    {
        var command = new AddNoteCommand(_repository, note, RefreshNotes);
        _undoManager.Execute(command);
    }

    public void DeleteNoteWithUndo(Note note)
    {
        var command = new DeleteNoteCommand(_repository, note, RefreshNotes);
        _undoManager.Execute(command);
    }

    public void UpdateNoteWithUndo(Note oldState, Note newState)
    {
        var command = new UpdateNoteCommand(_repository, oldState, newState, RefreshNotes);
        _undoManager.Execute(command);
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set { if (SetProperty(ref _searchText, value)) ApplyFilters(); }
    }

    private Category? _selectedCategoryFilter;
    public Category? SelectedCategoryFilter
    {
        get => _selectedCategoryFilter;
        set { if (SetProperty(ref _selectedCategoryFilter, value)) ApplyFilters(); }
    }

    private PriorityLevel? _selectedPriorityFilter;
    public PriorityLevel? SelectedPriorityFilter
    {
        get => _selectedPriorityFilter;
        set { if (SetProperty(ref _selectedPriorityFilter, value)) ApplyFilters(); }
    }

    public IEnumerable<PriorityLevel> PriorityLevels => Enum.GetValues(typeof(PriorityLevel)).Cast<PriorityLevel>();

    public void RefreshNotes()
    {
        _allNotes = _repository.GetAllNotes().ToList();
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _searchService.AdvancedFilter(
            _allNotes, 
            SearchText, 
            SelectedCategoryFilter?.Id, 
            SelectedPriorityFilter);

        Notes.Clear();
        foreach (var note in filtered)
        {
            Notes.Add(note);
        }
    }
}