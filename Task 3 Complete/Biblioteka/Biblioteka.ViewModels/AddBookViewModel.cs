using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Biblioteka.Data;
using Biblioteka.Interfaces;
using Biblioteka.Models;

namespace Biblioteka.ViewModels;

public class AddBookViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly BibliotekaContext _context;
    private readonly IDialogService _dialogService;
    private Book _book = new Book();

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

    private ICommand? _back = null;
    public ICommand? Back
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
    public ICommand? Save
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

        Book book = new Book
        {
            Tytul = this.Tytul,
            Autor = this.Autor,
            RokWydania = this.RokWydania,
            Gatunek = this.Gatunek,
            Status = this.SelectedStatus, 
            UserId = this.SelectedBorrower?.UserId 
        };

        _context.Books.Add(book);
        _context.SaveChanges();

        Response = "Data Saved";
    }

    public AddBookViewModel(BibliotekaContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
    }
    private ObservableCollection<User> LoadUsers()
    {
        _context.Database.EnsureCreated();
        _context.Users.Load();
        return _context.Users.Local.ToObservableCollection();
    }

    private bool IsValid()
    {
        string[] properties = { "Tytul","Autor","RokWydania","Gatunek" };
        foreach (string property in properties)
        {
            if (!string.IsNullOrEmpty(this[property]))
            {
                return false;
            }
        }
        return true;
    }
}
