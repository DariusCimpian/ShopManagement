using CommunityToolkit.Mvvm.ComponentModel; 
using CommunityToolkit.Mvvm.Input;      
using Shop.App.Models;
using Shop.Data.Services;
using Shop.App.Views;
using Shop.App.Helpers;


namespace Shop.App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly UserService _userService;

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
        }

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

      [RelayCommand]
     private async Task Login()
      {
    
          if (ValidationHelper.AreFieldsEmpty(Username, Password))
              {
               await Shell.Current.DisplayAlertAsync("Eroare", "Te rog introdu user și parola.", "OK");
               return;
              }

   
          var user = await _userService.LoginAsync(Username, Password);

         if (user == null)
           {
             await Shell.Current.DisplayAlertAsync("Eșec", "Username sau parolă greșită.", "OK");
             return;
           }
      string userRole = "N/A";
     if (user.Username.ToLower().Trim() == "admin" || userRole=="Admin") 
        {
            await Shell.Current.GoToAsync(nameof(Views.AdminDashboard));
        }
        else 
        {
            await Shell.Current.GoToAsync(nameof(Views.ProductListPage));
        }
     }

       
       [RelayCommand]
       private async Task GoToRegister()
         {

        await Shell.Current.GoToAsync(nameof(RegisterPage));
   
         }
    }
}