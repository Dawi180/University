using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class StudentOrganizationsViewModel : ViewModelBase
{
    private readonly UniversityContext _context;
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

    private ObservableCollection<StudentOrganization>? _studentOrganizations = null;
    public ObservableCollection<StudentOrganization>? StudentOrganizations
    {
        get
        {
            if (_studentOrganizations is null)
            {
                _studentOrganizations = new ObservableCollection<StudentOrganization>();
                return _studentOrganizations;
            }
            return _studentOrganizations;
        }
        set
        {
            _studentOrganizations = value;
            OnPropertyChanged(nameof(StudentOrganizations));
        }
    }

    private ICommand? _add = null;
    public ICommand? Add
    {
        get
        {
            if (_add is null)
            {
                _add = new RelayCommand<object>(AddNewStudentOrganization);
            }
            return _add;
        }
    }
    
    private void AddNewStudentOrganization(object? obj)
    {
        var instance = MainWindowViewModel.Instance();
        if (instance is not null)
        {
            instance.StudentOrganizationsSubView = new AddStudentOrganizationViewModel(_context, _dialogService);

        }
    }
    
    private ICommand? _edit = null;
    public ICommand? Edit
    {
        get
        {
            if (_edit is null)
            {
                _edit = new RelayCommand<object>(EditStudentOrganization);
            }
            return _edit;
        }
    }
    
    private void EditStudentOrganization(object? obj)
    {
        if (obj is not null)
        {
            long orgId = (long)obj;
            EditStudentOrganizationViewModel editStudentOrganizationViewModel = new EditStudentOrganizationViewModel(_context, _dialogService)
            {
                OrgId = orgId
            };
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.StudentOrganizationsSubView = editStudentOrganizationViewModel;
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
                _remove = new RelayCommand<object>(RemoveStudentOrganization);
            }
            return _remove;
        }
    }

    private void RemoveStudentOrganization(object? obj)
    {
        if (obj is not null)
        {
            long orgId = (long)obj;
            StudentOrganization? studentOrganization = _context.StudentOrganizations.Find(orgId);
            if (studentOrganization is not null)
            {
                DialogResult = _dialogService.Show(studentOrganization.Name);
                if (DialogResult == false)
                {
                    return;
                }

                _context.StudentOrganizations.Remove(studentOrganization);
                _context.SaveChanges();
            }
        }
    }

    public StudentOrganizationsViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;

        _context.Database.EnsureCreated();
        _context.StudentOrganizations.Load();
        StudentOrganizations = _context.StudentOrganizations.Local.ToObservableCollection();
    }
}
