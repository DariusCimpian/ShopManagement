using Shop.App.Views;

namespace Shop.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

           Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
           Routing.RegisterRoute(nameof(ProductListPage), typeof(ProductListPage));
           Routing.RegisterRoute(nameof(CartPage),typeof(CartPage));
           Routing.RegisterRoute(nameof(AdminDashboard), typeof(AdminDashboard));
           Routing.RegisterRoute(nameof(UserManagement), typeof(UserManagement));
           Routing.RegisterRoute(nameof(ProductManagementPage), typeof(ProductManagementPage));
        }
    }
}