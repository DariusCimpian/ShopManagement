using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Shop.Data.Models;
using Shop.Data.Services;
using Shop.Data.Helpers;

namespace Shop.App.ViewModels
{
    public partial class UserManagementViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private int? _editingUserId = null; 

        public UserManagementViewModel(UserService userService)
        {
            _userService = userService;
            Users = new ObservableCollection<User>();
        }

        [ObservableProperty] ObservableCollection<User> users;
        [ObservableProperty] bool isBusy;
        [ObservableProperty] bool isAddFormVisible;
        
       
        [ObservableProperty] string newUsername = string.Empty;
        [ObservableProperty] string newEmail = string.Empty;
        [ObservableProperty] string newPassword = string.Empty;
        [ObservableProperty] string newRole = "User";
        
        [ObservableProperty] string saveButtonText = "SALVEAZĂ";

        public async Task LoadUsers()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _userService.GetAllUsersAsync();
                Users.Clear();
                foreach (var item in list) Users.Add(item);
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        void ToggleAddForm()
        {
           
            ResetForm();
            IsAddFormVisible = !IsAddFormVisible;
        }

        [RelayCommand]
        void EditUser(User user)
        {
           
            _editingUserId = user.Id; 
            NewUsername = user.Username;
            NewEmail = user.Email;
            NewRole = user.Role;
            NewPassword = ""; 
            
            SaveButtonText = "ACTUALIZEAZĂ"; 
            IsAddFormVisible = true;
        }

        [RelayCommand]
        async Task SaveAction()
        {
            if (string.IsNullOrWhiteSpace(NewUsername))
            {
                await Shell.Current.DisplayAlertAsync("Eroare", "Username obligatoriu", "OK");
                return;
            }

            if (_editingUserId == null)
            {
               
                if (string.IsNullOrWhiteSpace(NewPassword)) 
                {
                    await Shell.Current.DisplayAlertAsync("Eroare", "Parola e obligatorie la utilizatori noi!", "OK");
                    return;
                }
                
                bool success = await _userService.RegisterUserAsync(NewUsername, NewPassword, NewEmail, NewRole);
                if (!success) await Shell.Current.DisplayAlertAsync("Eroare", "Userul exista deja", "OK");
            }
            else
            {
                
                var userToUpdate = new User
                {
                    Id = _editingUserId.Value,
                    Username = NewUsername,
                    Email = NewEmail,
                    Role = NewRole
                };

                
                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    var (hash, salt) = PasswordHasher.HashPassword(NewPassword);
                    userToUpdate.PasswordHash = hash;
                    userToUpdate.PasswordSalt = salt;
                }

                await _userService.UpdateUserAsync(userToUpdate);
                await Shell.Current.DisplayAlertAsync("Succes", "Date actualizate!", "OK");
            }

            
            IsAddFormVisible = false;
            ResetForm();
        }

        [RelayCommand]
        void CancelAdd()
        {
            IsAddFormVisible = false;
            ResetForm();
        }

        [RelayCommand]
        async Task DeleteUser(User user)
        {
            if (user.Username.ToLower() == "admin")
            {
                await Shell.Current.DisplayAlertAsync("Eroare", "Nu poți șterge Adminul!", "OK");
                return;
            }

            bool confirm = await Shell.Current.DisplayAlertAsync("Stergere", $"Sigur stergi pe {user.Username}?", "DA", "NU");
            if (confirm)
            {
                await _userService.DeleteUserAsync(user.Id);
                Users.Remove(user);
            }
        }

        [RelayCommand]
        async Task GoBack() => await Shell.Current.GoToAsync("..");

        private void ResetForm()
        {
            _editingUserId = null;
            NewUsername = "";
            NewEmail = "";
            NewPassword = "";
            NewRole = "User";
            SaveButtonText = "SALVEAZĂ";
        }
    }
}