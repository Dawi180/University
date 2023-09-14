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

public class AddExamViewModel : ViewModelBase, IDataErrorInfo
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
            if (columnName == "CourseCode")
            {
                if (string.IsNullOrEmpty(CourseCode))
                {
                    return "CourseCode is Required";
                }
            }
            if (columnName == "Date")
            {
                if (Date is null)
                {
                    return "Date is Required";
                }
            }
            if (columnName == "StartTime")
            {
                if (StartTime is null)
                {
                    return "StartTime is Required";
                }
            }
            if (columnName == "EndTime")
            {
                if (EndTime is null)
                {
                    return "EndTime is Required";
                }
            }
            if (columnName == "Location")
            {
                if (string.IsNullOrEmpty(Location))
                {
                    return "Location is Required";
                }
            }
            if (columnName == "Description")
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return "Description is Required";
                }
            }
            if (columnName == "Professor")
            {
                if (string.IsNullOrEmpty(Professor))
                {
                    return "Professor is Required";
                }
            }

            return string.Empty;
        }
    }

    private string _courseCode = string.Empty;
    public string CourseCode
    {
        get
        {
            return _courseCode;
        }
        set
        {
            _courseCode = value;
            OnPropertyChanged(nameof(CourseCode));
        }
    }
    private DateTime? _date = null;
    public DateTime? Date
    {
        get
        {
            return _date;
        }
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Date));
        }
    }
    private TimeSpan? _startTime = null;
    public TimeSpan? StartTime
    {
        get
        {
            return _startTime;
        }
        set
        {
            _startTime = value;
            OnPropertyChanged(nameof(StartTime));
        }
    }
    private TimeSpan? _endTime = null;
    public TimeSpan? EndTime
    {
        get
        {
            return _endTime;
        }
        set
        {
            _endTime = value;
            OnPropertyChanged(nameof(EndTime));
        }
    }

    private string _location = string.Empty;
    public string Location
    {
        get
        {
            return _location;
        }
        set
        {
            _location = value;
            OnPropertyChanged(nameof(Location));
        }
    }
    private string _description = string.Empty;
    public string Description
    {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }
    private string _professor = string.Empty;
    public string Professor
    {
        get
        {
            return _professor;
        }
        set
        {
            _professor = value;
            OnPropertyChanged(nameof(Professor));
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
            instance.ExamsSubView = new ExamsViewModel(_context, _dialogService);
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

        Exam exam = new Exam
        {
            CourseCode = this.CourseCode,
            Date = this.Date,
            StartTime = this.StartTime,
            EndTime = this.EndTime,
            Location = this.Location,
            Description = this.Description,
            Professor = this.Professor,
        };

        _context.Exams.Add(exam);
        _context.SaveChanges();

        Response = "Data Saved";
    }

    public AddExamViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
    }

    private bool IsValid()
    {
        string[] properties = { "CourseCode", "Date", "StartTime", "EndTime", "Location", "Description", "Professor" };
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
