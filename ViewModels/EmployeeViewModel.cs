using System;
using System.Collections.ObjectModel;
using System.Windows;
using TestApp.Models;
using TestApp.Models.Enums;
using TestApp.Repositories;
using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    /// <summary>
    /// ViewModel для управления сотрудниками.
    /// Реализует CRUD: добавление, редактирование, удаление сотрудников.
    /// Поддерживает режим редактирования (IsEditing) для показа/скрытия формы.
    /// </summary>
    public class EmployeeViewModel : ViewModelBase
    {
        /// <summary>
        /// Репозиторий для доступа к данным сотрудников.
        /// </summary>
        private readonly IEmployeeRepository _repository;

        /// <summary>
        /// Коллекция сотрудников для отображения в списке.
        /// </summary>
        private ObservableCollection<Employee> _employees = new();

        /// <summary>
        /// Выбранный в списке сотрудник.
        /// </summary>
        private Employee? _selectedEmployee;

        /// <summary>
        /// Редактируемая модель (новая или редактируемая сущность).
        /// </summary>
        private Employee _editModel = new();

        /// <summary>
        /// Флаг: true — открыт режим редактирования/добавления.
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Редактируемое ФИО сотрудника.
        /// </summary>
        private string _fullName = string.Empty;

        /// <summary>
        /// Редактируемая должность сотрудника.
        /// </summary>
        private Position _position = Position.Работник;

        /// <summary>
        /// Редактируемая дата рождения сотрудника.
        /// </summary>
        private DateTime _birthDate = DateTime.Today;

        /// <summary>
        /// Создать ViewModel и загрузить данные из БД.
        /// </summary>
        /// <param name="repository">Репозиторий сотрудников.</param>
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

        /// <summary>
        /// Коллекция сотрудников для привязки к DataGrid.
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        /// <summary>
        /// Выбранный сотрудник в списке.
        /// </summary>
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        /// <summary>
        /// ФИО сотрудника (для формы редактирования).
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        /// <summary>
        /// Должность сотрудника (для формы редактирования).
        /// </summary>
        public Position Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        /// <summary>
        /// Дата рождения сотрудника (для формы редактирования).
        /// </summary>
        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        /// <summary>
        /// Флаг режима редактирования. Управляет видимостью формы.
        /// </summary>
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        /// <summary>
        /// Значения перечисления Position для ComboBox.
        /// </summary>
        public Array PositionValues => Enum.GetValues(typeof(Position));

        /// <summary>
        /// Команда добавления нового сотрудника.
        /// </summary>
        public RelayCommand AddCommand { get; }

        /// <summary>
        /// Команда редактирования выбранного сотрудника.
        /// </summary>
        public RelayCommand EditCommand { get; }

        /// <summary>
        /// Команда удаления выбранного сотрудника.
        /// </summary>
        public RelayCommand DeleteCommand { get; }

        /// <summary>
        /// Команда сохранения данных сотрудника.
        /// </summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>
        /// Команда отмены редактирования.
        /// </summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// Загрузить список сотрудников из репозитория.
        /// </summary>
        private void LoadData()
        {
            Employees = new ObservableCollection<Employee>(_repository.GetAll());
        }

        /// <summary>
        /// Начать добавление нового сотрудника.
        /// Очищает поля формы и открывает режим редактирования.
        /// </summary>
        private void BeginAdd()
        {
            _editModel = new Employee();
            FullName = string.Empty;
            Position = Position.Работник;
            BirthDate = DateTime.Today;
            IsEditing = true;
        }

        /// <summary>
        /// Начать редактирование выбранного сотрудника.
        /// Заполняет поля формы текущими данными.
        /// </summary>
        private void BeginEdit()
        {
            if (SelectedEmployee is null) return;
            _editModel = SelectedEmployee;
            FullName = SelectedEmployee.FullName;
            Position = SelectedEmployee.Position;
            BirthDate = SelectedEmployee.BirthDate;
            IsEditing = true;
        }

        /// <summary>
        /// Сохранить данные сотрудника в БД.
        /// Выполняет валидацию: ФИО не должно быть пустым.
        /// </summary>
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

        /// <summary>
        /// Отменить редактирование и закрыть форму.
        /// </summary>
        private void Cancel()
        {
            IsEditing = false;
        }

        /// <summary>
        /// Удалить выбранного сотрудника после подтверждения пользователем.
        /// </summary>
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
