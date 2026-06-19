using TestApp.ViewModels.Base;

namespace TestApp.ViewModels
{
    /// <summary>
    /// Главная ViewModel приложения.
    /// Управляет переключением между вкладками (сотрудники, контрагенты, заказы)
    /// через свойство CurrentViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Текущая отображаемая ViewModel (определяет активную вкладку).
        /// </summary>
        private ViewModelBase? _currentViewModel;

        /// <summary>
        /// Создать главную ViewModel, регистрируя все дочерние ViewModel.
        /// По умолчанию отображается вкладка сотрудников.
        /// </summary>
        /// <param name="employeeViewModel">ViewModel сотрудников.</param>
        /// <param name="counterpartyViewModel">ViewModel контрагентов.</param>
        /// <param name="orderViewModel">ViewModel заказов.</param>
        public MainViewModel(EmployeeViewModel employeeViewModel, CounterpartyViewModel counterpartyViewModel, OrderViewModel orderViewModel)
        {
            EmployeeViewModel = employeeViewModel;
            CounterpartyViewModel = counterpartyViewModel;
            OrderViewModel = orderViewModel;

            // Команды переключения вкладок
            ShowEmployeesCommand = new RelayCommand(_ => CurrentViewModel = EmployeeViewModel);
            ShowCounterpartiesCommand = new RelayCommand(_ => CurrentViewModel = CounterpartyViewModel);
            ShowOrdersCommand = new RelayCommand(_ => CurrentViewModel = OrderViewModel);

            CurrentViewModel = EmployeeViewModel;
        }

        /// <summary>
        /// ViewModel для работы с сотрудниками.
        /// </summary>
        public EmployeeViewModel EmployeeViewModel { get; }

        /// <summary>
        /// ViewModel для работы с контрагентами.
        /// </summary>
        public CounterpartyViewModel CounterpartyViewModel { get; }

        /// <summary>
        /// ViewModel для работы с заказами.
        /// </summary>
        public OrderViewModel OrderViewModel { get; }

        /// <summary>
        /// Текущая ViewModel, отображаемая в основном окне.
        /// Изменение этого свойства переключает вкладку в UI.
        /// </summary>
        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        /// <summary>
        /// Команда переключения на вкладку «Сотрудники».
        /// </summary>
        public RelayCommand ShowEmployeesCommand { get; }

        /// <summary>
        /// Команда переключения на вкладку «Контрагенты».
        /// </summary>
        public RelayCommand ShowCounterpartiesCommand { get; }

        /// <summary>
        /// Команда переключения на вкладку «Заказы».
        /// </summary>
        public RelayCommand ShowOrdersCommand { get; }
    }
}
