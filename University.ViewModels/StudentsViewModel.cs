using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.X86;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class StudentsViewModel : ViewModelBase
{
    private readonly IDataAccessService _dataAccessService;
    private readonly IDialogService _dialogService;
    private readonly IValidationService _validationService;

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

    private ObservableCollection<IStudent>? _students = null;
    public ObservableCollection<IStudent>? Students
    {
        get
        {
            if (_students is null)
            {
                _students = new ObservableCollection<IStudent>();
                return _students;
            }
            return _students;
        }
        set
        {
            _students = value;
            OnPropertyChanged(nameof(Students));
        }
    }

    private ICommand? _add = null;
    public ICommand? Add
    {
        get
        {
            if (_add is null)
            {
                _add = new RelayCommand<object>(AddNewStudent);
            }
            return _add;
        }
    }

    public void AddNewStudent(object? obj)
    {
        var instance = MainWindowViewModel.Instance();
        if (instance is not null)
        {
            instance.StudentsSubView = new AddStudentViewModel(_dataAccessService, _dialogService);

        }
    }

    private ICommand? _edit = null;
    public ICommand? Edit
    {
        get
        {
            if (_edit is null)
            {
                _edit = new RelayCommand<object>(EditStudent);
            }
            return _edit;
        }
    }

    public void EditStudent(object? obj)
    {
        if (obj is not null)
        {
            long studentId = (long)obj;
            EditStudentViewModel editStudentViewModel = new EditStudentViewModel(_dataAccessService, _dialogService, _validationService)
            {
                StudentId = studentId
            };
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.StudentsSubView = editStudentViewModel;
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

    public void RemoveStudent(object? obj)
    {
        if (obj is not null)
        {
            long studentId = (long)obj;
            IStudent? student = _students.FirstOrDefault(s => s.StudentId == studentId);
            if (student is not null)
            {
                DialogResult = _dialogService.Show(student.Name + " " + student.LastName);
                if (DialogResult == false)
                {
                    return;
                }

                _students.Remove(student);
                _dataAccessService.SaveData("Data.json", _students); // Zapisz zmiany za pomocą IDataAccessService
            }
        }
    }

    public StudentsViewModel(IDataAccessService dataAccessService, IDialogService dialogService)
    {
        _dataAccessService = dataAccessService; // Wstrzyknięcie IDataAccessService
        _dialogService = dialogService;

        // _context.Database.EnsureCreated(); // Niepotrzebne, bo korzystamy z IDataAccessService
        _students = _dataAccessService.LoadData<ObservableCollection<IStudent>>("\\University\\Data.json") ?? new ObservableCollection<IStudent>();
    }
}
