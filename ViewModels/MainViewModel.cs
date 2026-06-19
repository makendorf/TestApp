using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _currentViewModel;

        public MainViewModel(EmployeeViewModel employeeViewModel, CounterpartyViewModel counterpartyViewModel, OrderViewModel orderViewModel)
        {
            EmployeeViewModel = employeeViewModel;
            CounterpartyViewModel = counterpartyViewModel;
            OrderViewModel = orderViewModel;

            ShowEmployeesCommand = new RelayCommand(_ => CurrentViewModel = EmployeeViewModel);
            ShowCounterpartiesCommand = new RelayCommand(_ => CurrentViewModel = CounterpartyViewModel);
            ShowOrdersCommand = new RelayCommand(_ => CurrentViewModel = OrderViewModel);

            CurrentViewModel = EmployeeViewModel;
        }

        public EmployeeViewModel EmployeeViewModel { get; }
        public CounterpartyViewModel CounterpartyViewModel { get; }
        public OrderViewModel OrderViewModel { get; }

        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand ShowEmployeesCommand { get; }
        public RelayCommand ShowCounterpartiesCommand { get; }
        public RelayCommand ShowOrdersCommand { get; }
    }
}
