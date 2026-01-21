using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shop.Data.Services;
using Shop.App.Helpers;

namespace Shop.App.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly UserService _userService;

        public RegisterViewModel(UserService userService)
        {
            _userService = userService;
        }

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

  
       [RelayCommand]
private async Task Register()
{
    if (ValidationHelper.AreFieldsEmpty(Username, Password, ConfirmPassword, Email))
    {
        await Shell.Current.DisplayAlertAsync("Eroare", "Completează toate câmpurile!", "OK");
        return;
    }

    
    if (!ValidationHelper.IsValidEmail(Email))
    {
        await Shell.Current.DisplayAlertAsync("Eroare", "Formatul email-ului nu este valid.", "OK");
        return;
    }

   if (!ValidationHelper.IsValidPassword(Password))
    {
        await Shell.Current.DisplayAlertAsync("Parolă Slabă", 
            "Parola trebuie să aibă minim 6 caractere și să conțină:\n" +
            "- O literă mare (A-Z)\n" +
            "- O cifră (0-9)\n" +
            "- Un caracter special (!@#$)", 
            "Am înțeles");
        return;
    }

   
    if (Password != ConfirmPassword)
    {
        await Shell.Current.DisplayAlertAsync("Eroare", "Parolele nu coincid!", "OK");
        return;
    }

    bool success = await _userService.RegisterUserAsync(Username, Password, Email);

    if (!success)
    {
        await Shell.Current.DisplayAlertAsync("Eșec", "Acest user există deja.", "OK");
        return;
    }

    await Shell.Current.DisplayAlertAsync("Succes", "Cont creat! Te poți loga acum.", "OK");
    await Shell.Current.GoToAsync(".."); 
}

        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}