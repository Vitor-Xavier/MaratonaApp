using MaratonaApp.ViewModels;
using MaratonaApp.Pages;
using Xamarin.Forms;
using MaratonaApp.Services;
using MaratonaApp.Helpers;

namespace MaratonaApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            IMaratonaAppService _maratonaAppService = new MaratonaAppService();
            if (!Settings.IsLoggedIn)
                MainPage = new LoginPage();
            else
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.DarkCyan,
                    BarTextColor = Color.White
                };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
