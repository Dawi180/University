using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Extensions;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class AddStudentViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly UniversityContext _context;
    private readonly IDialogService _dialogService;

    public string Error
    {
        get { return string.Empty; }
    }

    public string this[string columnName]
    {
        get
        {
            if (columnName == "Name")
            {
                if (string.IsNullOrEmpty(Name))
                { 
                    return "Name is Required";
                }
            }
            if (columnName == "LastName")
            {
                if (string.IsNullOrEmpty(LastName))
                {
                    return "Last Name is Required";
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
            if (columnName == "BirthDate")
            {
                if (BirthDate is null)
                {
                    return "Birth Date is Required";
                }
            }
            if (columnName == "Gender")
            {
                if (string.IsNullOrEmpty(Gender))
                {
                    return "Gender is Required";
                }
            }
            if (columnName == "PlaceOfBirth")
            {
                if (string.IsNullOrEmpty(PlaceOfBirth))
                {
                    return "Place of Birth is Required";
                }
            }
            if (columnName == "PlaceOfResidence")
            {
                if (string.IsNullOrEmpty(PlaceOfResidence))
                {
                    return "Place of Residence is Required";
                }
            }
            if (columnName == "AddressLine1")
            {
                if (string.IsNullOrEmpty(AddressLine1))
                {
                    return "Address Line 1 is Required";
                }
            }
            if (columnName == "AddressLine2")
            {
                if (string.IsNullOrEmpty(AddressLine2))
                {
                    return "Address Line 2 is Required";
                }
            }
            if (columnName == "PostalCode")
            {
                if (string.IsNullOrEmpty(PostalCode))
                {
                    return "Postal Code is Required";
                }
            }


            return string.Empty;
        }
    }

    private string _name = string.Empty;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _lastName = string.Empty;
    public string LastName
    {
        get
        {
            return _lastName;
        }
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
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

    private string _gender = string.Empty;
    public string Gender
    {
        get
        {
            return _gender;
        }
        set
        {
            _gender = value;
            OnPropertyChanged(nameof(Gender));
        }
    }
    private string _PlaceOfBirth = string.Empty;
    public string PlaceOfBirth
    {
        get
        {
            return _PlaceOfBirth;
        }
        set
        {
            _PlaceOfBirth = value;
            OnPropertyChanged(nameof(PlaceOfBirth));
        }
    }
    private string _PlaceOfResidence = string.Empty;
    public string PlaceOfResidence
    {
        get
        {
            return _PlaceOfResidence;
        }
        set
        {
            _PlaceOfResidence = value;
            OnPropertyChanged(nameof(PlaceOfResidence));
        }
    }
    private string _AddressLine1 = string.Empty;
    public string AddressLine1
    {
        get
        {
            return _AddressLine1;
        }
        set
        {
            _AddressLine1 = value;
            OnPropertyChanged(nameof(AddressLine1));
        }
    }
    private string _AddressLine2 = string.Empty;
    public string AddressLine2
    {
        get
        {
            return _AddressLine2;
        }
        set
        {
            _AddressLine2 = value;
            OnPropertyChanged(nameof(AddressLine2));
        }
    }
    private string _PostalCode = string.Empty;
    public string PostalCode
    {
        get
        {
            return _PostalCode;
        }
        set
        {
            _PostalCode = value;
            OnPropertyChanged(nameof(PostalCode));
        }
    }


    private DateTime? _birthDate = null;
    public DateTime? BirthDate
    {
        get
        {
            return _birthDate;
        }
        set
        {
            _birthDate = value;
            OnPropertyChanged(nameof(BirthDate));
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

    private ObservableCollection<Course>? _assignedCourses = null;
    public ObservableCollection<Course>? AssignedCourses
    {
        get
        {
            if (_assignedCourses is null)
            {
                _assignedCourses = LoadCourses();
                return _assignedCourses;
            }
            return _assignedCourses;
        }
        set
        {
            _assignedCourses = value;
            OnPropertyChanged(nameof(AssignedCourses));
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
            instance.StudentsSubView = new StudentsViewModel(_context, _dialogService);
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

    private void SaveData(object? obj)
    {
        if (!IsValid())
        {
            Response = "Please complete all required fields";
            return;
        }

        Student student = new Student
        {
            Name = this.Name,
            LastName = this.LastName,
            PESEL = this.PESEL,
            BirthDate = this.BirthDate,
            Gender = this.Gender,  // Set the new properties
            PlaceOfBirth = this.PlaceOfBirth,
            PlaceOfResidence = this.PlaceOfResidence,
            AddressLine1 = this.AddressLine1,
            AddressLine2 = this.AddressLine2,
            PostalCode = this.PostalCode,
            Courses = AssignedCourses?.Where(s => s.IsSelected).ToList()
        };

        _context.Students.Add(student);
        _context.SaveChanges();

        Response = "Data Saved";
    }

    public AddStudentViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
    }

    private ObservableCollection<Course> LoadCourses()
    {
        _context.Database.EnsureCreated();
        _context.Courses.Load();
        return _context.Courses.Local.ToObservableCollection();
    }

    private bool IsValid()
    {
        string[] properties = { "Name", "LastName", "PESEL", "BirthDay","Gender", "PlaceOfBirth", "PlaceOfResidence", "AddressLine1", "AddressLine2", "PostalCode" };
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
