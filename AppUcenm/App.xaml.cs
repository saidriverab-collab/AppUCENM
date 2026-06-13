using Microsoft.Extensions.DependencyInjection;
using AppUCENM.Views;

namespace AppUCENM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Establecer PageAddPersonas como pantalla principal envuelta en NavigationPage
            MainPage = new NavigationPage(new PageAddPersonas());
        }
    }
}