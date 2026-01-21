using CommunityToolkit.Mvvm.ComponentModel; 
using CommunityToolkit.Mvvm.Input;     
using Shop.App.Models;
using Shop.Data.Services;

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
        public string username=string.Empty;

        [ObservableProperty]
        public string password=string.Empty;

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Eroare", "Te rog introdu user si parola.", "OK");
                return;
            }

            var user = await _userService.LoginAsync(Username, Password);

            if (user == null)
            {
                await Shell.Current.DisplayAlert("Eșec", "Username sau parolă greșită, sau cont inactiv.", "OK");
                return;
            }

            CurrentUser.Id = user.Id;
            CurrentUser.Username = user.Username;
            CurrentUser.Role = user.Role;
            CurrentUser.Email = user.Email;

            await Shell.Current.GoToAsync("///MainPage");
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
           
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}