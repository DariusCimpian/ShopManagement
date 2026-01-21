using Microsoft.Extensions.DependencyInjection;
using Shop.App.Views;

namespace Shop.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

          this.MainPage = new AppShell();
                
        }
    }
	
  }
