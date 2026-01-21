using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Shop.Data.Models;
using Shop.Data.Services;

namespace Shop.App.ViewModels
{
    public partial class ProductManagementViewModel : ObservableObject
    {
        private readonly ProductService _productService;
        private int? _editingProductId = null; 
        public ProductManagementViewModel(ProductService productService)
        {
            _productService = productService;
            
            
            Products = new ObservableCollection<Product>();
            Categories = new ObservableCollection<Category>();
        }

       
        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private ObservableCollection<Category> categories;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isAddFormVisible;

        [ObservableProperty]
        private string saveButtonText = "SALVEAZĂ PRODUS";

       
        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string imageUrl = string.Empty;

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private int stock;

      
        [ObservableProperty]
        private Category selectedCategory;

        public async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
               
                SelectedCategory = null;


                var catList = await _productService.GetCategoriesAsync();
                Categories.Clear();
                foreach (var c in catList)
                {
                    Categories.Add(c);
                }

                var prodList = await _productService.GetAllProductsAsync();
                Products.Clear();
                foreach (var p in prodList)
                {
                    Products.Add(p);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Eroare Încărcare", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task AddCategory()
        {
            string categoryName = await Shell.Current.DisplayPromptAsync("Categorie Nouă", "Introdu numele categoriei:");
            
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                try
                {
                    await _productService.AddCategoryAsync(new Category { Name = categoryName });
                    await LoadDataAsync(); 
                    await Shell.Current.DisplayAlertAsync("Succes", "Categoria a fost adăugată!", "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Eroare", "Nu am putut adăuga categoria: " + ex.Message, "OK");
                }
            }
        }


        [RelayCommand]
        async Task SaveAction()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlertAsync("Atenție", "Te rog introdu numele produsului.", "OK");
                return;
            }
            if (SelectedCategory == null)
            {
                await Shell.Current.DisplayAlertAsync("Atenție", "Trebuie să selectezi o categorie din listă.", "OK");
                return;
            }
            if (Price <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Atenție", "Prețul trebuie să fie mai mare ca 0.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                if (_editingProductId == null)
                {
    
                    var newProduct = new Product
                    {
                        Name = Name,
                        Description = Description ?? "",
                        ImageUrl = ImageUrl ?? "",
                        Price = Price,
                        Stock = Stock,
                        CategoryId = SelectedCategory.Id 
                    };

                    await _productService.AddProductAsync(newProduct);
                    await Shell.Current.DisplayAlertAsync("Succes", "Produsul a fost adăugat!", "OK");
                }
                else
                {
                    var productToUpdate = new Product
                    {
                        Id = _editingProductId.Value,
                        Name = Name,
                        Description = Description ?? "",
                        ImageUrl = ImageUrl ?? "",
                        Price = Price,
                        Stock = Stock,
                        CategoryId = SelectedCategory.Id
                    };

                    await _productService.UpdateProductAsync(productToUpdate);
                    await Shell.Current.DisplayAlertAsync("Succes", "Produsul a fost actualizat!", "OK");
                }

                
                IsAddFormVisible = false;
                ResetForm();
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Eroare Salvare", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        void EditProduct(Product p)
        {
            try
            {
                _editingProductId = p.Id;
                Name = p.Name;
                Description = p.Description;
                Price = p.Price;
                Stock = p.Stock;
                ImageUrl = p.ImageUrl;

              
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == p.CategoryId);

                SaveButtonText = "ACTUALIZEAZĂ PRODUS";
                IsAddFormVisible = true;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlertAsync("Eroare", "Nu s-a putut deschide editarea: " + ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task DeleteProduct(Product p)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync("Confirmare", $"Ștergi produsul '{p.Name}'?", "DA", "NU");
            if (confirm)
            {
                try
                {
                    await _productService.DeleteProductAsync(p.Id);
                    Products.Remove(p);
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Eroare", "Nu s-a putut șterge: " + ex.Message, "OK");
                }
            }
        }

        [RelayCommand]
        void ToggleAddForm()
        {
            if (IsAddFormVisible)
            {
                IsAddFormVisible = false;
                ResetForm();
            }
            else
            {
                ResetForm();
                IsAddFormVisible = true;
            }
        }

        [RelayCommand]
        void CancelAdd()
        {
            IsAddFormVisible = false;
            ResetForm();
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void ResetForm()
        {
            _editingProductId = null;
            Name = string.Empty;
            Description = string.Empty;
            ImageUrl = string.Empty;
            Price = 0;
            Stock = 0;
            SelectedCategory = null;
            SaveButtonText = "SALVEAZĂ PRODUS";
        }
    }
}