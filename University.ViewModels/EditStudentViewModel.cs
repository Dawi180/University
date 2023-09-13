using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using University.Data;
using University.Extensions;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class EditStudentViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly UniversityContext _context;
    private readonly IDataAccessService _dataAccessService;
    private readonly IDialogService _dialogService;
    private Student? _student = new Student();

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
                    return "BirthDate is Required";
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

    private string _placeOfBirth = string.Empty;
    public string PlaceOfBirth
    {
        get
        {
            return _placeOfBirth;
        }
        set
        {
            _placeOfBirth = value;
            OnPropertyChanged(nameof(PlaceOfBirth));
        }
    }

    private string _placeOfResidence = string.Empty;
    public string PlaceOfResidence
    {
        get
        {
            return _placeOfResidence;
        }
        set
        {
            _placeOfResidence = value;
            OnPropertyChanged(nameof(PlaceOfResidence));
        }
    }

    private string _addressLine1 = string.Empty;
    public string AddressLine1
    {
        get
        {
            return _addressLine1;
        }
        set
        {
            _addressLine1 = value;
            OnPropertyChanged(nameof(AddressLine1));
        }
    }

    private string _addressLine2 = string.Empty;
    public string AddressLine2
    {
        get
        {
            return _addressLine2;
        }
        set
        {
            _addressLine2 = value;
            OnPropertyChanged(nameof(AddressLine2));
        }
    }

    private string _postalCode = string.Empty;
    public string PostalCode
    {
        get
        {
            return _postalCode;
        }
        set
        {
            _postalCode = value;
            OnPropertyChanged(nameof(PostalCode));
        }
    }

    private DateTime? birthDate = null;
    public DateTime? BirthDate
    {
        get
        {
            return birthDate;
        }
        set
        {
            birthDate = value;
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

    private long _studentId = 0;
    public long StudentId
    {
        get
        {
            return _studentId;
        }
        set
        {
            _studentId = value;
            OnPropertyChanged(nameof(StudentId));
            LoadStudentData();
        }
    }

    private ObservableCollection<Course>? _assignedCourses = null;
    public ObservableCollection<Course> AssignedCourses
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
            instance.StudentsSubView = new StudentsViewModel(_dataAccessService, _dialogService);
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

        if (_student is null)
        {
            return;
        }
        _student.Name = Name;
        _student.LastName = LastName;
        _student.PESEL = PESEL;
        _student.BirthDate = BirthDate;
        _student.Gender = Gender;
        _student.PlaceOfBirth = PlaceOfBirth;
        _student.PlaceOfResidence = PlaceOfResidence;
        _student.AddressLine1 = AddressLine1;
        _student.AddressLine2 = AddressLine2;
        _student.PostalCode = PostalCode;
        _student.Courses = AssignedCourses.Where(s => s.IsSelected).ToList();

        _context.Entry(_student).State = EntityState.Modified;
        _context.SaveChanges();

        Response = "Data Updated";
    }

    public EditStudentViewModel(UniversityContext context, IDialogService dialogService)
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

    private void LoadStudentData()
    {
        if (_context?.Students is null)
        {
            return;
        }
        _student = _context.Students.Find(StudentId);
        if (_student is null)
        {
            return;
        }
        this.Name = _student.Name;
        this.LastName = _student.LastName;
        this.PESEL = _student.PESEL;
        this.BirthDate = _student.BirthDate;
        this.Gender = _student.Gender;
        this.PlaceOfBirth = _student.PlaceOfBirth;
        this.PlaceOfResidence = _student.PlaceOfResidence;
        this.AddressLine1 = _student.AddressLine1;
        this.AddressLine2 = _student.AddressLine2;
        this.PostalCode = _student.PostalCode;
        if (_student.Courses is null)
        {
            return;
        }
        foreach (Course course in _student.Courses)
        {
            if (course is not null && AssignedCourses is not null)
            {
                var assignedCourse = AssignedCourses
                    .FirstOrDefault(s => s.CourseId == course.CourseId);
                if (assignedCourse is not null)
                { 
                    assignedCourse.IsSelected = true;
                }
            }
        }
        
    }
}
