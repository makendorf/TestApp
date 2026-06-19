using System;
using System.Collections.ObjectModel;
using System.Windows;
using TestApp.Models;
using TestApp.Repositories;
using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    /// <summary>
    /// ViewModel для управления заказами.
    /// Реализует CRUD: добавление, редактирование, удаление заказов.
    /// Позволяет выбрать сотрудника и контрагента для заказа.
    /// </summary>
    public class OrderViewModel : ViewModelBase
    {
        /// <summary>
        /// Репозиторий заказов.
        /// </summary>
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Репозиторий сотрудников (для заполнения ComboBox).
        /// </summary>
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Репозиторий контрагентов (для заполнения ComboBox).
        /// </summary>
        private readonly ICounterpartyRepository _counterpartyRepository;

        /// <summary>
        /// Коллекция заказов для отображения в списке.
        /// </summary>
        private ObservableCollection<Order> _orders = new();

        /// <summary>
        /// Выбранный в списке заказ.
        /// </summary>
        private Order? _selectedOrder;

        /// <summary>
        /// Редактируемая модель заказа.
        /// </summary>
        private Order _editModel = new();

        /// <summary>
        /// Флаг: true — открыт режим редактирования/добавления.
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Редактируемая дата заказа.
        /// </summary>
        private DateTime _date = DateTime.Today;

        /// <summary>
        /// Редактируемая сумма заказа.
        /// </summary>
        private decimal _amount;

        /// <summary>
        /// Выбранный сотрудник для заказа.
        /// </summary>
        private Employee? _selectedEmployee;

        /// <summary>
        /// Выбранный контрагент для заказа.
        /// </summary>
        private Counterparty? _selectedCounterparty;

        /// <summary>
        /// Список сотрудников для ComboBox.
        /// </summary>
        private ObservableCollection<Employee> _employees = new();

        /// <summary>
        /// Список контрагентов для ComboBox.
        /// </summary>
        private ObservableCollection<Counterparty> _counterparties = new();

        /// <summary>
        /// Создать ViewModel и загрузить данные из БД.
        /// </summary>
        /// <param name="orderRepository">Репозиторий заказов.</param>
        /// <param name="employeeRepository">Репозиторий сотрудников.</param>
        /// <param name="counterpartyRepository">Репозиторий контрагентов.</param>
        public OrderViewModel(IOrderRepository orderRepository, IEmployeeRepository employeeRepository,
            ICounterpartyRepository counterpartyRepository)
        {
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
            _counterpartyRepository = counterpartyRepository;

            AddCommand = new RelayCommand(_ => BeginAdd());
            EditCommand = new RelayCommand(_ => BeginEdit(), _ => SelectedOrder is not null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedOrder is not null);
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadData();
        }

        /// <summary>
        /// Коллекция заказов для привязки к DataGrid.
        /// </summary>
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        /// <summary>
        /// Выбранный заказ в списке.
        /// </summary>
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        /// <summary>
        /// Дата заказа (для формы редактирования).
        /// </summary>
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        /// <summary>
        /// Сумма заказа (для формы редактирования).
        /// </summary>
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        /// <summary>
        /// Выбранный сотрудник для заказа (для формы редактирования).
        /// </summary>
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        /// <summary>
        /// Выбранный контрагент для заказа (для формы редактирования).
        /// </summary>
        public Counterparty? SelectedCounterparty
        {
            get => _selectedCounterparty;
            set => SetProperty(ref _selectedCounterparty, value);
        }

        /// <summary>
        /// Список сотрудников для ComboBox.
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        /// <summary>
        /// Список контрагентов для ComboBox.
        /// </summary>
        public ObservableCollection<Counterparty> Counterparties
        {
            get => _counterparties;
            set => SetProperty(ref _counterparties, value);
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
        /// Команда добавления нового заказа.
        /// </summary>
        public RelayCommand AddCommand { get; }

        /// <summary>
        /// Команда редактирования выбранного заказа.
        /// </summary>
        public RelayCommand EditCommand { get; }

        /// <summary>
        /// Команда удаления выбранного заказа.
        /// </summary>
        public RelayCommand DeleteCommand { get; }

        /// <summary>
        /// Команда сохранения данных заказа.
        /// </summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>
        /// Команда отмены редактирования.
        /// </summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// Загрузить данные заказов, сотрудников и контрагентов из репозиториев.
        /// </summary>
        private void LoadData()
        {
            Orders = new ObservableCollection<Order>(_orderRepository.GetAll());
            Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
            Counterparties = new ObservableCollection<Counterparty>(_counterpartyRepository.GetAll());
        }

        /// <summary>
        /// Начать добавление нового заказа.
        /// Устанавливает значения по умолчанию и открывает форму.
        /// </summary>
        private void BeginAdd()
        {
            _editModel = new Order();
            Date = DateTime.Today;
            Amount = 0;
            SelectedEmployee = null;
            SelectedCounterparty = null;
            LoadData();
            IsEditing = true;
        }

        /// <summary>
        /// Начать редактирование выбранного заказа.
        /// Заполняет поля формы текущими данными заказа.
        /// </summary>
        private void BeginEdit()
        {
            if (SelectedOrder is null) return;
            _editModel = SelectedOrder;
            Date = SelectedOrder.Date;
            Amount = SelectedOrder.Amount;
            SelectedEmployee = SelectedOrder.Employee;
            SelectedCounterparty = SelectedOrder.Counterparty;
            LoadData();
            IsEditing = true;
        }

        /// <summary>
        /// Сохранить данные заказа в БД.
        /// Выполняет валидацию: сумма > 0, сотрудник и контрагент обязательны.
        /// </summary>
        private void Save()
        {
            if (Amount <= 0)
            {
                MessageBox.Show("Сумма заказа должна быть больше нуля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedEmployee is null)
            {
                MessageBox.Show("Выберите сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedCounterparty is null)
            {
                MessageBox.Show("Выберите контрагента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _editModel.Date = Date;
            _editModel.Amount = Amount;
            _editModel.Employee = SelectedEmployee;
            _editModel.Counterparty = SelectedCounterparty;

            _orderRepository.Save(_editModel);
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
        /// Удалить выбранный заказ после подтверждения пользователем.
        /// </summary>
        private void Delete()
        {
            if (SelectedOrder is null) return;
            var result = MessageBox.Show("Удалить выбранный заказ?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _orderRepository.Delete(SelectedOrder);
                LoadData();
            }
        }
    }
}
