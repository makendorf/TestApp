using System;
using System.Collections.ObjectModel;
using System.Windows;
using TestApp.Models;
using TestApp.Repositories;
using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICounterpartyRepository _counterpartyRepository;

        private ObservableCollection<Order> _orders = new();
        private Order? _selectedOrder;
        private Order _editModel = new();
        private bool _isEditing;
        private DateTime _date = DateTime.Today;
        private decimal _amount;
        private Employee? _selectedEmployee;
        private Counterparty? _selectedCounterparty;
        private ObservableCollection<Employee> _employees = new();
        private ObservableCollection<Counterparty> _counterparties = new();

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

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        public Counterparty? SelectedCounterparty
        {
            get => _selectedCounterparty;
            set => SetProperty(ref _selectedCounterparty, value);
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public ObservableCollection<Counterparty> Counterparties
        {
            get => _counterparties;
            set => SetProperty(ref _counterparties, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        private void LoadData()
        {
            Orders = new ObservableCollection<Order>(_orderRepository.GetAll());
            Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
            Counterparties = new ObservableCollection<Counterparty>(_counterpartyRepository.GetAll());
        }

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

        private void Cancel()
        {
            IsEditing = false;
        }

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
