using System.Collections.ObjectModel;
using System.Windows;
using TestApp.Models;
using TestApp.Repositories;
using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    public class CounterpartyViewModel : ViewModelBase
    {
        private readonly ICounterpartyRepository _counterpartyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        private ObservableCollection<Counterparty> _counterparties = new();
        private Counterparty? _selectedCounterparty;
        private Counterparty _editModel = new();
        private bool _isEditing;
        private string _name = string.Empty;
        private string _inn = string.Empty;
        private Employee? _selectedCurator;
        private ObservableCollection<Employee> _employees = new();

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

        public ObservableCollection<Counterparty> Counterparties
        {
            get => _counterparties;
            set => SetProperty(ref _counterparties, value);
        }

        public Counterparty? SelectedCounterparty
        {
            get => _selectedCounterparty;
            set => SetProperty(ref _selectedCounterparty, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Inn
        {
            get => _inn;
            set => SetProperty(ref _inn, value);
        }

        public Employee? SelectedCurator
        {
            get => _selectedCurator;
            set => SetProperty(ref _selectedCurator, value);
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
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
            Counterparties = new ObservableCollection<Counterparty>(_counterpartyRepository.GetAll());
            Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
        }

        private void BeginAdd()
        {
            _editModel = new Counterparty();
            Name = string.Empty;
            Inn = string.Empty;
            SelectedCurator = null;
            LoadData();
            IsEditing = true;
        }

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

        private void Cancel()
        {
            IsEditing = false;
        }

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
