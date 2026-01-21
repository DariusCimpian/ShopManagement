using Microsoft.Extensions.Logging;
using Shop.Data.Data;   
using Shop.Data.Services;
using Shop.App.ViewModels; 
using Shop.App.Views;

namespace Shop.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

          builder.Services.AddDbContext<ShopDbContext>();

            builder.Services.AddTransient<UserService>();    
            builder.Services.AddTransient<ProductService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

            return builder.Build();
        }
    }
}