using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shop.Data.Models; 
using Shop.Data.Services; 
using System.Collections.ObjectModel;
using Shop.App.Views;

namespace Shop.App.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly ProductService _productService;

        public ProductListViewModel(ProductService productService)
        {
            _productService = productService;

            Products = new ObservableCollection<Product>();
            CartItems = new ObservableCollection<Product>();
            
        }

        [ObservableProperty]
        ObservableCollection<Product> products;

        [ObservableProperty]
        ObservableCollection<Product> cartItems;

        [ObservableProperty]
        int cartCount;

        [ObservableProperty]
        decimal totalAmount;
        [ObservableProperty]
        string searchText=string.Empty;
     public async Task LoadProducts()
        {
            
                var dbProducts = await _productService.GetAllProductsAsync(SearchText);
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Products.Clear();
                    foreach (var p in dbProducts)
                    {
                        Products.Add(p);
                    }
                });
          
        
        }

      [RelayCommand]
      async Task PerformSearch()
       {
         await LoadProducts();
       }

        [RelayCommand]
        void AddToCart(Product product)
        {
            if (product == null) return;

            CartItems.Add(product);
            CartCount = CartItems.Count;
            TotalAmount += product.Price;
            
           Shell.Current.DisplayAlertAsync("Adaugat", $"{product.Name} a ajuns in cos!", "OK");
        }

       [RelayCommand]
        async Task GoToCart()
        {
            if (CartItems.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("Gol", "Nu ai nimic în coș!", "OK");
                return;
            }
            
            await Shell.Current.GoToAsync(nameof(Views.CartPage));
        }
        
        [RelayCommand]
        async Task PlaceOrder()
        {
            if (CartItems.Count == 0) return;

          
            await Shell.Current.DisplayAlertAsync("Succes", "Comanda a fost recepționată! 🎉\nÎți mulțumim.", "OK");

            CartItems.Clear();
            CartCount = 0;
            TotalAmount = 0;

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}