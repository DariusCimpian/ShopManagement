using Shop.App.ViewModels;

namespace Shop.App.Views;

public partial class UserManagement: ContentPage
{
    private readonly UserManagementViewModel _vm;

	public UserManagement(UserManagementViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadUsers();
    }
}