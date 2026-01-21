using Shop.App.ViewModels;

namespace Shop.App.Views;

public partial class ProductListPage : ContentPage
{
	public ProductListPage(ProductListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}