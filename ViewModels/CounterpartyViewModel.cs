using System.Collections.ObjectModel;
using System.Windows;
using TestApp.Models;
using TestApp.Repositories;
using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    /// <summary>
    /// ViewModel для управления контрагентами.
    /// Реализует CRUD: добавление, редактирование, удаление контрагентов.
    /// Поддерживает выбор куратора из списка сотрудников.
    /// </summary>
    public class CounterpartyViewModel : ViewModelBase
    {
        /// <summary>
        /// Репозиторий контрагентов.
        /// </summary>
        private readonly ICounterpartyRepository _counterpartyRepository;

        /// <summary>
        /// Репозиторий сотрудников (для заполнения списка кураторов).
        /// </summary>
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Коллекция контрагентов для отображения в списке.
        /// </summary>
        private ObservableCollection<Counterparty> _counterparties = new();

        /// <summary>
        /// Выбранный в списке контрагент.
        /// </summary>
        private Counterparty? _selectedCounterparty;

        /// <summary>
        /// Редактируемая модель контрагента.
        /// </summary>
        private Counterparty _editModel = new();

        /// <summary>
        /// Флаг: true — открыт режим редактирования/добавления.
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Редактируемое наименование контрагента.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// Редактируемый ИНН контрагента.
        /// </summary>
        private string _inn = string.Empty;

        /// <summary>
        /// Выбранный куратор (сотрудник) для контрагента.
        /// </summary>
        private Employee? _selectedCurator;

        /// <summary>
        /// Список сотрудников для ComboBox выбора куратора.
        /// </summary>
        private ObservableCollection<Employee> _employees = new();

        /// <summary>
        /// Создать ViewModel и загрузить данные из БД.
        /// </summary>
        /// <param name="counterpartyRepository">Репозиторий контрагентов.</param>
        /// <param name="employeeRepository">Репозиторий сотрудников (для выбора куратора).</param>
        public CounterpartyViewModel(ICounterpartyRepository counterpartyRepository, IEmployeeRepository employeeRepository)
        {
            _counterpartyRepository = counterpartyRepository;
            _employeeRepository = employeeRepository;

            AddCommand = new RelayCommand(_ => BeginAdd());
            EditCommand = new RelayCommand(_ => BeginEdit(), _ => SelectedCounterparty is not null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedCounterparty is not null);
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadData();
        }

        /// <summary>
        /// Коллекция контрагентов для привязки к DataGrid.
        /// </summary>
        public ObservableCollection<Counterparty> Counterparties
        {
            get => _counterparties;
            set => SetProperty(ref _counterparties, value);
        }

        /// <summary>
        /// Выбранный контрагент в списке.
        /// </summary>
        public Counterparty? SelectedCounterparty
        {
            get => _selectedCounterparty;
            set => SetProperty(ref _selectedCounterparty, value);
        }

        /// <summary>
        /// Наименование контрагента (для формы редактирования).
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// ИНН контрагента (для формы редактирования).
        /// </summary>
        public string Inn
        {
            get => _inn;
            set => SetProperty(ref _inn, value);
        }

        /// <summary>
        /// Выбранный куратор (сотрудник) для контрагента.
        /// </summary>
        public Employee? SelectedCurator
        {
            get => _selectedCurator;
            set => SetProperty(ref _selectedCurator, value);
        }

        /// <summary>
        /// Список сотрудников для ComboBox выбора куратора.
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
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
        /// Команда добавления нового контрагента.
        /// </summary>
        public RelayCommand AddCommand { get; }

        /// <summary>
        /// Команда редактирования выбранного контрагента.
        /// </summary>
        public RelayCommand EditCommand { get; }

        /// <summary>
        /// Команда удаления выбранного контрагента.
        /// </summary>
        public RelayCommand DeleteCommand { get; }

        /// <summary>
        /// Команда сохранения данных контрагента.
        /// </summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>
        /// Команда отмены редактирования.
        /// </summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// Загрузить данные контрагентов и сотрудников из репозиториев.
        /// </summary>
        private void LoadData()
        {
            Counterparties = new ObservableCollection<Counterparty>(_counterpartyRepository.GetAll());
            Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
        }

        /// <summary>
        /// Начать добавление нового контрагента.
        /// Очищает поля формы и открывает режим редактирования.
        /// </summary>
        private void BeginAdd()
        {
            _editModel = new Counterparty();
            Name = string.Empty;
            Inn = string.Empty;
            SelectedCurator = null;
            LoadData();
            IsEditing = true;
        }

        /// <summary>
        /// Начать редактирование выбранного контрагента.
        /// Заполняет поля формы текущими данными.
        /// </summary>
        private void BeginEdit()
        {
            if (SelectedCounterparty is null) return;
            _editModel = SelectedCounterparty;
            Name = SelectedCounterparty.Name;
            Inn = SelectedCounterparty.Inn;
            SelectedCurator = SelectedCounterparty.Curator;
            LoadData();
            IsEditing = true;
        }

        /// <summary>
        /// Сохранить данные контрагента в БД.
        /// Выполняет валидацию: наименование, ИНН и куратор обязательны.
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Введите наименование контрагента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Inn))
            {
                MessageBox.Show("Введите ИНН контрагента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedCurator is null)
            {
                MessageBox.Show("Выберите куратора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _editModel.Name = Name;
            _editModel.Inn = Inn;
            _editModel.Curator = SelectedCurator;

            _counterpartyRepository.Save(_editModel);
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
        /// Удалить выбранного контрагента после подтверждения пользователем.
        /// </summary>
        private void Delete()
        {
            if (SelectedCounterparty is null) return;
            var result = MessageBox.Show("Удалить выбранного контрагента?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _counterpartyRepository.Delete(SelectedCounterparty);
                LoadData();
            }
        }
    }
}
