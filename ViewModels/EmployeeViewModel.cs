using System;
using System.Collections.ObjectModel;
using System.Windows;
using TestApp.Models;
using TestApp.Models.Enums;
using TestApp.Repositories;
using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IEmployeeRepository _repository;

        private ObservableCollection<Employee> _employees = new();
        private Employee? _selectedEmployee;
        private Employee _editModel = new();
        private bool _isEditing;
        private string _fullName = string.Empty;
        private Position _position = Position.Работник;
        private DateTime _birthDate = DateTime.Today;

        public EmployeeViewModel(IEmployeeRepository repository)
        {
            _repository = repository;

            AddCommand = new RelayCommand(_ => BeginAdd());
            EditCommand = new RelayCommand(_ => BeginEdit(), _ => SelectedEmployee is not null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedEmployee is not null);
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadData();
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public Position Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public Array PositionValues => Enum.GetValues(typeof(Position));

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        private void LoadData()
        {
            Employees = new ObservableCollection<Employee>(_repository.GetAll());
        }

        private void BeginAdd()
        {
            _editModel = new Employee();
            FullName = string.Empty;
            Position = Position.Работник;
            BirthDate = DateTime.Today;
            IsEditing = true;
        }

        private void BeginEdit()
        {
            if (SelectedEmployee is null) return;
            _editModel = SelectedEmployee;
            FullName = SelectedEmployee.FullName;
            Position = SelectedEmployee.Position;
            BirthDate = SelectedEmployee.BirthDate;
            IsEditing = true;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Введите ФИО сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _editModel.FullName = FullName;
            _editModel.Position = Position;
            _editModel.BirthDate = BirthDate;

            _repository.Save(_editModel);
            IsEditing = false;
            LoadData();
        }

        private void Cancel()
        {
            IsEditing = false;
        }

        private void Delete()
        {
            if (SelectedEmployee is null) return;
            var result = MessageBox.Show("Удалить выбранного сотрудника?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _repository.Delete(SelectedEmployee);
                LoadData();
            }
        }
    }
}
