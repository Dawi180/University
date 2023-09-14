using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class AddStudentOrganizationViewModel : ViewModelBase, IDataErrorInfo
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
            if (columnName == "Advisor")
            {
                if (string.IsNullOrEmpty(Advisor))
                {
                    return "Advisor is Required";
                }
            }
            if (columnName == "President")
            {
                if (string.IsNullOrEmpty(President))
                {
                    return "President is Required";
                }
            }
            if (columnName == "Description")
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return "Description is Required";
                }
            }
            if (columnName == "MeetingSchedule")
            {
                if (string.IsNullOrEmpty(MeetingSchedule))
                {
                    return "Meeting Schedule is Required";
                }
            }
            if (columnName == "Email")
            {
                if (string.IsNullOrEmpty(Email))
                {
                    return "Email is Required";
                }
            }

            return string.Empty;
        }
    }

    private string _name = string.Empty;
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _advisor = string.Empty;
    public string Advisor
    {
        get { return _advisor; }
        set
        {
            _advisor = value;
            OnPropertyChanged(nameof(Advisor));
        }
    }

    private string _president = string.Empty;
    public string President
    {
        get { return _president; }
        set
        {
            _president = value;
            OnPropertyChanged(nameof(President));
        }
    }

    private string _description = string.Empty;
    public string Description
    {
        get { return _description; }
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    private string _meetingSchedule = string.Empty;
    public string MeetingSchedule
    {
        get { return _meetingSchedule; }
        set
        {
            _meetingSchedule = value;
            OnPropertyChanged(nameof(MeetingSchedule));
        }
    }

    private string _email = string.Empty;
    public string Email
    {
        get { return _email; }
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
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


    private ObservableCollection<Student>? _availableStudents = null;
    public ObservableCollection<Student> AvailableStudents
    {
        get
        {
            if (_availableStudents is null)
            {
                _availableStudents = LoadStudents();
                return _availableStudents;
            }
            return _availableStudents;
        }
        set
        {
            _availableStudents = value;
            OnPropertyChanged(nameof(AvailableStudents));
        }
    }

    private ObservableCollection<Student>? _assignedStudents = null;
    public ObservableCollection<Student> AssignedStudents
    {
        get
        {
            if (_assignedStudents is null)
            {
                _assignedStudents = new ObservableCollection<Student>();
                return _assignedStudents;
            }
            return _assignedStudents;
        }
        set
        {
            _assignedStudents = value;
            OnPropertyChanged(nameof(AssignedStudents));
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
            instance.StudentOrganizationsSubView = new StudentOrganizationsViewModel(_context, _dialogService);
        }
    }

    private ICommand? _add = null;
    public ICommand Add
    {
        get
        {
            if (_add is null)
            {
                _add = new RelayCommand<object>(AddStudent);
            }
            return _add;
        }
    }

    private void AddStudent(object? obj)
    {
        if (obj is Student student)
        {

            if (AssignedStudents.Contains(student))
            {
                return;
            }
            AssignedStudents.Add(student);
        }
    }

    private ICommand? _remove = null;
    public ICommand Remove
    {
        get
        {
            if (_remove is null)
            {
                _remove = new RelayCommand<object>(RemoveStudent);
            }
            return _remove;
        }
    }

    private void RemoveStudent(object? obj)
    {
        if (obj is Student student)
        {
            AssignedStudents.Remove(student);
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

        StudentOrganization studentOrganization = new StudentOrganization
        {

            Name = this.Name,
            Advisor = this.Advisor,
            President = this.President,
            Description = this.Description,
            MeetingSchedule = this.MeetingSchedule,
            Email = this.Email,
            Students = AssignedStudents
        };

        _context.StudentOrganizations.Add(studentOrganization);
        _context.SaveChanges();

        Response = "Data Saved";
    }

    public AddStudentOrganizationViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
    }

    private ObservableCollection<Student> LoadStudents()
    {
        _context.Database.EnsureCreated();
        _context.Students.Load();
        return _context.Students.Local.ToObservableCollection();
    }

    private bool IsValid()
    {
        string[] properties = { "Name", "Advisor", "President", "Description", "MeetingSchedule", "Email" };
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
