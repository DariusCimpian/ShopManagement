using Microsoft.Extensions.DependencyInjection;
using Shop.App.Views;
using Shop.Data.Services;

namespace Shop.App
{
    public partial class App : Application
    {
        public App(UserService userService)
        {
            InitializeComponent();

          Task.Run(async () => 
            {
              await userService.InitAdminAsync();
            });
            MainPage = new AppShell();
                
        }
    }
	
  }
