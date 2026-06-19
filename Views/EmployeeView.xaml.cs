namespace TestApp.Views;

/// <summary>
/// UserControl для отображения и редактирования сотрудников.
/// DataContext привязывается к EmployeeViewModel через MainViewModel.
/// </summary>
public partial class EmployeeView : System.Windows.Controls.UserControl
{
    public EmployeeView()
    {
        InitializeComponent();
    }
}
