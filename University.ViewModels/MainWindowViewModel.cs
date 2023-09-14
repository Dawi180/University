using System;
using University.Interfaces;
using University.Data;
using University.Models;

namespace University.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly UniversityContext _context;
    private readonly IDialogService _dialogService;
    private readonly IDataAccessService _dataAccessService;

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

    private object? _studentsSubView = null;
    public object? StudentsSubView
    {
        get
        {
            return _studentsSubView;
        }
        set
        {
            _studentsSubView = value;
            OnPropertyChanged(nameof(StudentsSubView));
        }
    }

    private object? _coursesSubView = null;
    public object? CoursesSubView
    {
        get
        {
            return _coursesSubView;
        }
        set
        {
            _coursesSubView = value;
            OnPropertyChanged(nameof(CoursesSubView));
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
    private object? _facultyMembersSubView = null;
    public object? FacultyMembersSubView
    {
        get
        {
            return _facultyMembersSubView;
        }
        set
        {
            _facultyMembersSubView = value;
            OnPropertyChanged(nameof(FacultyMembersSubView));
        }
    }
    private object? _examsSubView = null;
    public object? ExamsSubView
    {
        get
        {
            return _examsSubView;
        }
        set
        {
            _examsSubView = value;
            OnPropertyChanged(nameof(ExamsSubView));
        }
    }

    private object? _studentOrganizationsSubView = null;
    public object? StudentOrganizationsSubView
    {
        get
        {
            return _studentOrganizationsSubView;
        }
        set
        {
            _studentOrganizationsSubView = value;
            OnPropertyChanged(nameof(StudentOrganizationsSubView));
        }
    }

    private static MainWindowViewModel? _instance = null;
    public static MainWindowViewModel? Instance()
    {
        return _instance;
    }

    public MainWindowViewModel(UniversityContext context, IDialogService dialogService, IDataAccessService dataAccessService)
    {
        _context = context;
        _dialogService = dialogService;
        _dataAccessService = dataAccessService;
        if (_instance is null)
        {
            _instance = this;
        }

        StudentsSubView = new StudentsViewModel(_dataAccessService, _dialogService);
        CoursesSubView = new CoursesViewModel(_context, _dialogService);
        SearchSubView = new SearchViewModel(_context, _dialogService);
        FacultyMembersSubView = new FacultyMembersViewModel(_context, _dialogService);
        ExamsSubView = new ExamsViewModel(_context, _dialogService);
        StudentOrganizationsSubView = new StudentOrganizationsViewModel(_context, _dialogService);
        _dataAccessService = dataAccessService;
    }
}
