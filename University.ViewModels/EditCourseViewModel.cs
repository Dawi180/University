﻿using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class EditCourseViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly UniversityContext _context;
    private readonly IDialogService _dialogService;
    private Course? _course = new Course();

    public string Error => string.Empty;

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
            if (columnName == "Title")
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return "Title is Required";
                }
            }
            if (columnName == "Instructor")
            {
                if (string.IsNullOrEmpty(Instructor))
                {
                    return "Instructor is Required";
                }
            }
            if (columnName == "Schedule")
            {
                if (string.IsNullOrEmpty(Schedule))
                {
                    return "Schedule is Required";
                }
            }
            if (columnName == "Description")
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return "Description is Required";
                }
            }
            if (columnName == "Credits")
            {
                if (Credits == 0)
                {
                    return "Credits is Required";
                }
            }
            if (columnName == "Department")
            {
                if (string.IsNullOrEmpty(Department))
                {
                    return "Department is Required";
                }
            }

            return string.Empty;
        }
    }

    private string _courseCode = string.Empty;
    public string CourseCode
    {
        get { return _courseCode; }
        set
        {
            _courseCode = value;
            OnPropertyChanged(nameof(CourseCode));
        }
    }

    private string _title = string.Empty;
    public string Title
    {
        get { return _title; }
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    private string _instructor = string.Empty;
    public string Instructor
    {
        get { return _instructor; }
        set
        {
            _instructor = value;
            OnPropertyChanged(nameof(Instructor));
        }
    }

    private string _schedule = string.Empty;
    public string Schedule
    {
        get { return _schedule; }
        set
        {
            _schedule = value;
            OnPropertyChanged(nameof(Schedule));
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

    private int _credits;
    public int Credits
    {
        get { return _credits; }
        set
        {
            _credits = value;
            OnPropertyChanged(nameof(Credits));
        }
    }

    private string _department = string.Empty;
    public string Department
    {
        get { return _department; }
        set
        {
            _department = value;
            OnPropertyChanged(nameof(Department));
        }
    }

    private ObservableCollection<Course>? _availablePrerequisites = null;
    public ObservableCollection<Course> AvailablePrerequisites
    {
        get
        {
            if (_availablePrerequisites is null)
            {
                _availablePrerequisites = LoadCourses();
                return _availablePrerequisites;
            }
            return _availablePrerequisites;
        }
        set
        {
            _availablePrerequisites = value;
            OnPropertyChanged(nameof(AvailablePrerequisites));
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

    private long _courseId = 0;
    public long CourseId
    {
        get
        {
            return _courseId;
        }
        set
        {
            _courseId = value;
            OnPropertyChanged(nameof(CourseId));
            LoadCourseData();
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
    public ObservableCollection<Student>? AssignedStudents
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
            instance.CoursesSubView = new CoursesViewModel(_context, _dialogService);
        }
    }

    private ICommand? _add;
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
            if (AssignedStudents is not null && !AssignedStudents.Contains(student))
            {
                AssignedStudents.Add(student);
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
                _remove = new RelayCommand<object>(RemoveStudent);
            }
            return _remove;
        }
    }

    private void RemoveStudent(object? obj)
    {
        if (obj is Student student)
        {
            if (AssignedStudents is not null)
            {
                AssignedStudents.Remove(student);
            }
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

        if (_course is null)
        {
            return;
        }

        _course.Instructor = Instructor;
        _course.Schedule = Schedule;
        _course.Description = Description;
        _course.Credits = Credits;
        _course.Department = Department;
        _course.Title = Title;
        _course.CourseCode = CourseCode;
        _course.Prerequisite = AvailablePrerequisites.Where(s => s.IsSelected).ToList();
        _course.Students = AssignedStudents;

        _context.Entry(_course).State = EntityState.Modified;
        _context.SaveChanges();

        Response = "Data Saved";
    }

    public EditCourseViewModel(UniversityContext context, IDialogService dialogService)
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

    private ObservableCollection<Course> LoadCourses()
    {
        _context.Database.EnsureCreated();
        _context.Courses.Load();

        return _context.Courses.Local.ToObservableCollection();
    }
    private bool IsValid()
    {
        string[] properties = { "CourseCode", "Title", "Instructor", "Schedule", "Description", "Credits", "Department" };
        foreach (string property in properties)
        {
            if (!string.IsNullOrEmpty(this[property]))
            {
                return false;
            }
        }
        return true;
    }

    private void LoadCourseData()
    {
        var courses = _context.Courses;
        if (courses is not null)
        {
            _course = courses.Find(CourseId);
            if (_course is null)
            {
                return;
            }
            this.CourseCode = _course.CourseCode;
            this.Title = _course.Title;
            this.Instructor = _course.Instructor;
            this.Schedule = _course.Schedule;
            this.Description = _course.Description;
            this.Credits = _course.Credits;
            this.Department = _course.Department;
            if (_course.Prerequisite is null)
            {
                return;
            }
            foreach (Course Prerequisite in _course.Prerequisite)
            {
                if (Prerequisite is not null && AvailablePrerequisites is not null)
                {
                    var availablePrerequisites = AvailablePrerequisites
                        .FirstOrDefault(s => s.CourseId == Prerequisite.CourseId);
                    if (availablePrerequisites is not null)
                    {
                        availablePrerequisites.IsSelected = true;
                    }
                }
            }

            if (_course.Students is not null)
            {
                this.AssignedStudents =
                    new ObservableCollection<Student>(_course.Students);
            }
            
        }
    }
}
 
