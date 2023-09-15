using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Biblioteka.Data;
using Biblioteka.Interfaces;
using Biblioteka.Models;

namespace Biblioteka.ViewModels;

public class SearchViewModel : ViewModelBase
{
    private readonly BibliotekaContext _context;
    private readonly IDialogService _dialogService;

    private bool? _dialogResult = null;
    public bool? DialogResult
    {
        get
        {
            return _dialogResult;
        }
        set
        {
            _dialogResult = value;
        }
    }

    private string _firstCondition = string.Empty;
    public string FirstCondition
    {
        get
        {
            return _firstCondition;
        }
        set
        {
            _firstCondition = value;
            OnPropertyChanged(nameof(FirstCondition));
        }
    }

    private string _secondCondition = string.Empty;
    public string SecondCondition
    {
        get
        {
            return _secondCondition;
        }
        set
        {
            _secondCondition = value;
            OnPropertyChanged(nameof(SecondCondition));
        }
    }

    private bool _isVisible;
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            _isVisible = value;
            OnPropertyChanged(nameof(IsVisible));
        }
    }

    private bool _areBooksVisible;
    public bool AreBooksVisible
    {
        get
        {
            return _areBooksVisible;
        }
        set
        {
            _areBooksVisible = value;
            OnPropertyChanged(nameof(AreBooksVisible));
        }
    }

    private ObservableCollection<Book>? _books = null;
    public ObservableCollection<Book>? Books
    {
        get
        {
            if (_books is null)
            {
                _books = new ObservableCollection<Book>();
                return _books;
            }
            return _books;
        }
        set
        {
            _books = value;
            OnPropertyChanged(nameof(Books));
        }
    }

    private ICommand? _comboBoxSelectionChanged = null;
    public ICommand? ComboBoxSelectionChanged
    {
        get
        {
            if (_comboBoxSelectionChanged is null)
            {
                _comboBoxSelectionChanged = new RelayCommand<object>(UpdateCondition);
            }
            return _comboBoxSelectionChanged;
        }
    }

    private void UpdateCondition(object? obj)
    {
        if (obj is string objAsString)
        {
            IsVisible = true;
            string selectedValue = objAsString;
            SecondCondition = string.Empty;
            if (selectedValue == "Tytul")
            {
                FirstCondition = "Tytul ksiazki:";
            }
            else if (selectedValue == "Autor")
            {
                FirstCondition = "Autor ksiazki:";
            }
            else if (selectedValue == "Gatunek")
            {
                FirstCondition = "Gatunek ksiazki:";
            }
        }
    }

    private ICommand? _search = null;
    public ICommand? Search
    {
        get
        {
            if (_search is null)
            {
                _search = new RelayCommand<object>(SelectData);
            }
            return _search;
        }
    }

    private void SelectData(object? obj)
    {
        _context.Database.EnsureCreated();

        if (FirstCondition == "Tytul ksiazki:")
        {
            string searchTitle = SecondCondition;

            var books = _context.Books
                .Where(book => book.Tytul == searchTitle)
                .ToList();

            Books = new ObservableCollection<Book>(books);
            AreBooksVisible = true;
        }
        else if (FirstCondition == "Autor ksiazki:")
        {
            string searchAuthor = SecondCondition;

            var books = _context.Books
                .Where(book => book.Autor == searchAuthor)
                .ToList();

            Books = new ObservableCollection<Book>(books);
            AreBooksVisible = true;
        }
        else if (FirstCondition == "Gatunek ksiazki:")
        {
            string searchGenre = SecondCondition;

            var books = _context.Books
                .Where(book => book.Gatunek == searchGenre)
                .ToList();

            Books = new ObservableCollection<Book>(books);
            AreBooksVisible = true;
        }
    }
    
    private ICommand? _edit = null;
    public ICommand? Edit
    {
        get
        {
            if (_edit is null)
            {
                _edit = new RelayCommand<object>(EditItem);
            }
            return _edit;
        }
    }
    
    private void EditItem(object? obj)
    {
        if (obj is not null)
        {
            if (FirstCondition == "Tytul ksiazki:")
            {
                int bookId = (int)obj;
                EditBookViewModel editBookViewModel = new EditBookViewModel(_context, _dialogService)
                {
                    BookId = bookId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.BooksSubView = editBookViewModel;
                    instance.SelectedTab = 0;
                }
            }
            else if (FirstCondition == "Autor ksiazki:")
            {
                int bookId = (int)obj;
                EditBookViewModel editBookViewModel = new EditBookViewModel(_context, _dialogService)
                {
                    BookId = bookId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.BooksSubView = editBookViewModel;
                    instance.SelectedTab = 0;
                }
            }
            else if (FirstCondition == "Gatunek ksiazki:")
            {
                int bookId = (int)obj;
                EditBookViewModel editBookViewModel = new EditBookViewModel(_context, _dialogService)
                {
                    BookId = bookId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.BooksSubView = editBookViewModel;
                    instance.SelectedTab = 0;
                }
            }
        }
    }

    private ICommand? _remove = null;
    public ICommand? Remove
    {
        get
        {
            if (_remove is null)
            {
                _remove = new RelayCommand<object>(RemoveItem);
            }
            return _remove;
        }
    }

    private void RemoveItem(object? obj)
    {
        if (obj is not null)
        {
            if (FirstCondition == "Tytul ksiazki:")
            {
                int bookId = (int)obj;
                Book? book = _context.Books.Find(bookId);
                if (book is null)
                {
                    return;
                }

                DialogResult = _dialogService.Show(book.Tytul + " " + book.Autor);
                if (DialogResult == false)
                {
                    return;
                }
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            else if (FirstCondition == "Autor ksiazki:")
            {
                int bookId = (int)obj;
                Book? book = _context.Books.Find(bookId);
                if (book is null)
                {
                    return;
                }

                DialogResult = _dialogService.Show(book.Tytul + " " + book.Autor);
                if (DialogResult == false)
                {
                    return;
                }
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            else if (FirstCondition == "Gatunek ksiazki:")
            {
                int bookId = (int)obj;
                Book? book = _context.Books.Find(bookId);
                if (book is null)
                {
                    return;
                }

                DialogResult = _dialogService.Show(book.Tytul + " " + book.Autor);
                if (DialogResult == false)
                {
                    return;
                }
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
    public SearchViewModel(BibliotekaContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;

        IsVisible = false;
        AreBooksVisible = false;
    }
}
