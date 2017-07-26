using MaratonaApp.Helpers;
using MaratonaApp.Pages;
using MaratonaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MaratonaApp.ViewModels
{
    public class LoginViewModel
    {
        public Command LoginCommand { get; }

        private IMaratonaAppService _maratonaAppService;

        public LoginViewModel()
        {
            LoginCommand = new Command(ExecuteLoginCommand);

            _maratonaAppService = new MaratonaAppService();
        }

        async void ExecuteLoginCommand()
        {
            if (!(await _maratonaAppService.LoginAsync()))
                return;
            else
            {
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.DarkCyan,
                    BarTextColor = Color.White
                };
            }

            
        }
    }
}
