namespace TestApp.Views;

/// <summary>
/// UserControl для отображения и редактирования заказов.
/// DataContext привязывается к OrderViewModel через MainViewModel.
/// </summary>
public partial class OrderView : System.Windows.Controls.UserControl
{
    public OrderView()
    {
        InitializeComponent();
    }
}
