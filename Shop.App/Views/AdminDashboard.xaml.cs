

namespace Shop.App.Views;

public partial class AdminDashboard : ContentPage
{
	public AdminDashboard()
	{
		InitializeComponent();
	}

    private async void OnUserManagementClicked(object sender, EventArgs e)
    {
  
            await Shell.Current.GoToAsync(nameof(UserManagement));
                
    }
    private async void OnProductsManagementClicked(object sender, EventArgs e)
    {
        try
    {
        await Shell.Current.GoToAsync(nameof(ProductManagementPage));
    }
    catch (Exception ex)
    {
        // Asta îți va spune exact ce lipsește
        await DisplayAlertAsync("Eroare Navigare", ex.Message, "OK");
    }
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}