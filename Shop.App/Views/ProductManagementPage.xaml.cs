using Shop.App.ViewModels;

namespace Shop.App.Views;

public partial class ProductManagementPage : ContentPage
{
  
    public ProductManagementPage(ProductManagementViewModel vm)
    {

        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
           
            System.Diagnostics.Debug.WriteLine($"CRITICAL XAML ERROR: {ex.Message}");
            throw; 
        }

        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProductManagementViewModel vm)
        {
        
                await vm.LoadDataAsync();
            
           
        }
    }
}