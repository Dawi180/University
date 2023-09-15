using System;
using Biblioteka.Interfaces;
using Biblioteka.Data;
using Biblioteka.Models;

namespace Biblioteka.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly BibliotekaContext _context;
    private readonly IDialogService _dialogService;

    private int _selectedTab;
    public int SelectedTab
    {
        get
        {
            return _selectedTab;
        }
        set
        {
            _selectedTab = value;
            OnPropertyChanged(nameof(SelectedTab));
        }
    }
    private object? _booksSubView = null;
    public object? BooksSubView
    {
        get
        {
            return _booksSubView;
        }
        set
        {
            _booksSubView = value;
            OnPropertyChanged(nameof(BooksSubView));
        }
    }
    private object? _searchSubView = null;
    public object? SearchSubView
    {
        get
        {
            return _searchSubView;
        }
        set
        {
            _searchSubView = value;
            OnPropertyChanged(nameof(SearchSubView));
        }
    }
    private object? _usersSubView = null;
    public object? UsersSubView
    {
        get
        {
            return _usersSubView;
        }
        set
        {
            _usersSubView = value;
            OnPropertyChanged(nameof(UsersSubView));
        }
    }



    private static MainWindowViewModel? _instance = null;
    public static MainWindowViewModel? Instance()
    {
        return _instance;
    }

    public MainWindowViewModel(BibliotekaContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;

        if (_instance is null)
        {
            _instance = this;
        }
        BooksSubView = new BooksViewModel(_context, _dialogService);
        SearchSubView = new SearchViewModel(_context, _dialogService);
        UsersSubView = new UsersViewModel(_context, _dialogService);
    }
}
