using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Biblioteka.Data;
using Biblioteka.Interfaces;
using Biblioteka.Models;

namespace Biblioteka.ViewModels;

public class EditBookViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly BibliotekaContext _context;
    private readonly IDialogService _dialogService;
    private Book? _book = new Book();


public string Error
    {
        get { return string.Empty; }
    }

    public string this[string columnName]
    {
        get
        {
            if (columnName == "Tytul")
            {
                if (string.IsNullOrEmpty(Tytul))
                {
                    return "Tytuł is Required";
                }
            }
            if (columnName == "Autor")
            {
                if (string.IsNullOrEmpty(Autor))
                {
                    return "Autor is Required";
                }
            }
            if (columnName == "RokWydania")
            {
                if (RokWydania < 1800 || RokWydania > 2100)
                {
                    return "Rok wydania musi być w zakresie od 1800 do 2100.";
                }
            }
            if (columnName == "Gatunek")
            {
                if (string.IsNullOrEmpty(Gatunek))
                {
                    return "Gatunek is Required";
                }
            }
            if (columnName == "SelectedStatus")
            {
                if ((SelectedStatus == "Rezerwacja" || SelectedStatus == "Wypozyczenie") && SelectedBorrower == null)
                {
                    return "Wybierz użytkownika.";
                }
            }

            return string.Empty;
        }
    }

    private string _tytul = string.Empty;
    public string Tytul
    {
        get
        {
            return _tytul;
        }
        set
        {
            _tytul = value;
            OnPropertyChanged(nameof(Tytul));
        }
    }

    private string _autor = string.Empty;
    public string Autor
    {
        get
        {
            return _autor;
        }
        set
        {
            _autor = value;
            OnPropertyChanged(nameof(Autor));
        }
    }

    private int _rokWydania;
    public int RokWydania
    {
        get
        {
            return _rokWydania;
        }
        set
        {
            _rokWydania = value;
            OnPropertyChanged(nameof(RokWydania));
        }
    }

    private string _gatunek = string.Empty;
    public string Gatunek
    {
        get
        {
            return _gatunek;
        }
        set
        {
            _gatunek = value;
            OnPropertyChanged(nameof(Gatunek));
        }
    }

    private string _response = string.Empty;
    public string Response
    {
        get
        {
            return _response;
        }
        set
        {
            _response = value;
            OnPropertyChanged(nameof(Response));
        }
    }
    private string _selectedItem = string.Empty;
    public string SelectedItem
    {
        get
        {
            return _selectedItem;
        }
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    private int _bookId = 0;
    public int BookId
    {
        get
        {
            return _bookId;
        }
        set
        {
            _bookId = value;
            OnPropertyChanged(nameof(BookId));
            LoadBookData();
        }
    }

    private ICommand? _back = null;
    public ICommand Back
    {
        get
        {
            if (_back is null)
            {
                _back = new RelayCommand<object>(NavigateBack);
            }
            return _back;
        }
    }
    

    private void NavigateBack(object? obj)
    {
        var instance = MainWindowViewModel.Instance();
        if (instance is not null)
        {
            instance.BooksSubView = new BooksViewModel(_context, _dialogService);
        }
    }

    private ICommand? _save = null;
    public ICommand Save
    {
        get
        {
            if (_save is null)
            {
                _save = new RelayCommand<object>(SaveData);
            }
            return _save;
        }
    }

    private List<string> _availableStatuses = new List<string> { "Dostepna", "Rezerwacja", "Wypozyczenie" };
    public List<string> AvailableStatuses
    {
        get { return _availableStatuses; }
        set
        {
            _availableStatuses = value;
            OnPropertyChanged(nameof(AvailableStatuses));
        }
    }

    private string _selectedStatus;
    public string SelectedStatus
    {
        get { return _selectedStatus; }
        set
        {
            _selectedStatus = value;
            OnPropertyChanged(nameof(SelectedStatus));
            
        }
    }


    private bool _isAvailable;
    public bool IsAvailable
    {
        get { return _isAvailable; }
        set
        {
            _isAvailable = value;
            OnPropertyChanged(nameof(IsAvailable));
        }
    }

    private User _selectedBorrower;
    public User SelectedBorrower
    {
        get { return _selectedBorrower; }
        set
        {
            _selectedBorrower = value;
            OnPropertyChanged(nameof(SelectedBorrower));

           
            if (_selectedBorrower != null && _selectedBorrower.Imie == "Brak wypożyczenia")
            {
                _book.UserId = null;
            }
            else
            {
                _book.UserId = _selectedBorrower?.UserId;
            }
        }
    }
    private ObservableCollection<User>? _availableUsers = null;
    public ObservableCollection<User> AvailableUsers
    {
        get
        {
            if (_availableUsers is null)
            {
                _availableUsers = LoadUsers();
                return _availableUsers;
            }
            return _availableUsers;
        }
        set
        {
            _availableUsers = value;
            OnPropertyChanged(nameof(AvailableUsers));
        }
    }
    private void SaveData(object? obj)
    {
        if (!IsValid())
        {
            Response = "Please complete all required fields";
            return;
        }

        if (_book is null)
        {
            return;
        }
        _book.Tytul = Tytul;
        _book.Autor = Autor;
        _book.RokWydania = RokWydania;
        _book.Gatunek = Gatunek;
        _book.Status = SelectedStatus;

        _context.Entry(_book).State = EntityState.Modified;
        _context.SaveChanges();

        Response = "Data Updated";
    }
    private ObservableCollection<User> LoadUsers()
    {
        _context.Database.EnsureCreated();
        _context.Users.Load();
        return _context.Users.Local.ToObservableCollection();
    }
    public EditBookViewModel(BibliotekaContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
    }

    private bool IsValid()
    {
        string[] properties = { "Tytul", "Autor", "RokWydania", "Gatunek" };
        foreach (string property in properties)
        {
            if (!string.IsNullOrEmpty(this[property]))
            {
                return false;
            }
        }
        return true;
    }

    private void LoadBookData()
    {
        if (_context?.Books is null)
        {
            return;
        }
        _book = _context.Books.Find(BookId);
        if (_book is null)
        {
            return;
        }
#pragma warning disable CS8601 // Możliwe przypisanie odwołania o wartości null.
        this.Tytul = _book.Tytul;
        this.Autor = _book.Autor;
        this.RokWydania = _book.RokWydania;
        this.Gatunek = _book.Gatunek;
        this.SelectedStatus = _book.Status;

    }
}
