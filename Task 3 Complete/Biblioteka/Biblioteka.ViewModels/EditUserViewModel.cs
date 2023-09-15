using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Biblioteka.Data;
using Biblioteka.Interfaces;
using Biblioteka.Models;
using Biblioteka.Extensions;

namespace Biblioteka.ViewModels;

public class EditUserViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly BibliotekaContext _context;
    private readonly IDialogService _dialogService;
    private User? _user = new User();

    public string Error
    {
        get { return string.Empty; }
    }

    public string this[string columnName]
    {
        get
        {
            if (columnName == "Imie")
            {
                if (string.IsNullOrEmpty(Imie))
                {
                    return "Imie is Required";
                }
            }
            if (columnName == "Nazwisko")
            {
                if (string.IsNullOrEmpty(Nazwisko))
                {
                    return "Nazwisko is Required";
                }
            }
            if (columnName == "PESEL")
            {
                if (string.IsNullOrEmpty(PESEL))
                {
                    return "PESEL is Required";
                }
                if (!PESEL.IsValidPESEL())
                {
                    return "PESEL is Invalid";
                }
            }
            if (columnName == "DataUrodzenia")
            {
                if (DataUrodzenia is null)
                {
                    return "Data Urodzenia is Required";
                }
            }

            return string.Empty;
        }
    }

    private string _imie = string.Empty;
    public string Imie
    {
        get
        {
            return _imie;
        }
        set
        {
            _imie = value;
            OnPropertyChanged(nameof(Imie));
        }
    }

    private string _nazwisko = string.Empty;
    public string Nazwisko
    {
        get
        {
            return _nazwisko;
        }
        set
        {
            _nazwisko = value;
            OnPropertyChanged(nameof(Nazwisko));
        }
    }

    private string _pesel = string.Empty;
    public string PESEL
    {
        get
        {
            return _pesel;
        }
        set
        {
            _pesel = value;
            OnPropertyChanged(nameof(PESEL));
        }
    }

    private DateTime? _dataUrodzenia = null;
    public DateTime? DataUrodzenia
    {
        get
        {
            return _dataUrodzenia;
        }
        set
        {
            _dataUrodzenia = value;
            OnPropertyChanged(nameof(DataUrodzenia));
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

    private int _userId = 0;
    public int UserId
    {
        get
        {
            return _userId;
        }
        set
        {
            _userId = value;
            OnPropertyChanged(nameof(UserId));
            LoadUserData();
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
            instance.UsersSubView = new UsersViewModel(_context, _dialogService);
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

    private void SaveData(object? obj)
    {
        if (!IsValid())
        {
            Response = "Please complete all required fields";
            return;
        }

        if (_user is null)
        {
            return;
        }
        _user.Imie = Imie;
        _user.Nazwisko = Nazwisko;
        _user.PESEL = PESEL;
        _user.DataUrodzenia = DataUrodzenia;

        _context.Entry(_user).State = EntityState.Modified;
        _context.SaveChanges();

        Response = "Data Updated";
    }

    public EditUserViewModel(BibliotekaContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
    }

    private bool IsValid()
    {
        string[] properties = { "Imie", "Nazwisko", "PESEL", "DataUrodzenia" };
        foreach (string property in properties)
        {
            if (!string.IsNullOrEmpty(this[property]))
            {
                return false;
            }
        }
        return true;
    }

    private void LoadUserData()
    {
        if (_context?.Users is null)
        {
            return;
        }
        _user = _context.Users.Find(UserId);
        if (_user is null)
        {
            return;
        }
#pragma warning disable CS8601 // Możliwe przypisanie odwołania o wartości null.
        this.Imie = _user.Imie;
#pragma warning disable CS8601 // Możliwe przypisanie odwołania o wartości null.
        this.Nazwisko = _user.Nazwisko;
#pragma warning disable CS8601 // Możliwe przypisanie odwołania o wartości null.
        this.PESEL = _user.PESEL;
        this.DataUrodzenia = _user.DataUrodzenia;

    }
}
