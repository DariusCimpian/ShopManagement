using Shop.App.ViewModels;

namespace Shop.App.Views;

public partial class CartPage : ContentPage
{
	public CartPage(ProductListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}