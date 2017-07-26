using MaratonaApp.Helpers;
using MaratonaApp.Models;
using MaratonaApp.Pages;
using MaratonaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MaratonaApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IMaratonaAppService _maratonaAppService;

        public Command AboutCommand { get; }

        public Command CartCommand { get; }

        public Command NewCartCommand { get; }

        public Command UpdateCommand { get; }

        public Command LogoutCommand { get; }

        public Command ToggleCommand { get; }

        public Command DeleteCommand { get; }

        public Cart Cart { get; }

        public DateTime MinDate { get; } = DateTime.Today;

        public ObservableCollection<Cart> Carts { get; }

        public MainViewModel()
        {
            _maratonaAppService = new MaratonaAppService();

            Cart = new Cart() { Id_User = Settings.UserId, PlannedFor = MinDate };
            AboutCommand = new Command(ExecuteAboutCommand);
            CartCommand = new Command<Cart>(ExecuteCartCommand);
            NewCartCommand = new Command(ExecuteNewCartCommand);
            UpdateCommand = new Command(ExecuteUpdateCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);
            ToggleCommand = new Command<Cart>(ExecuteToggleCommand);
            DeleteCommand = new Command<Cart>(ExecuteDeleteCommand);

            Carts = new ObservableCollection<Cart>();
        }

        public async void ExecuteDeleteCommand(Cart cart)
        {
            IsLoading = true;
            await _maratonaAppService.DeleteCartAsync(cart);
            LoadAsync();
        }

        public async void ExecuteToggleCommand(Cart cart)
        {
            await _maratonaAppService.SaveCartAsync(cart);
        }

        private async void ExecuteLogoutCommand()
        {
            await _maratonaAppService.LogoutAsync();
            App.Current.MainPage = new LoginPage();
        }

        private void ExecuteUpdateCommand()
        {
            LoadAsync();
        }

        async void ExecuteAboutCommand()
        {
            await PushAsync<AboutViewModel>();
        }

        async void ExecuteCartCommand(Cart cart)
        {
            await PushAsync<ItemsViewModel>(cart);
        }

        private async void ExecuteNewCartCommand()
        {
            await _maratonaAppService.SaveCartAsync(Cart);

            LoadAsync();
        }

        public async void LoadAsync()
        {
            IsLoading = true;

            List<Cart> carts = await _maratonaAppService.GetCartsAsync(Settings.UserId);

            Carts.Clear();
            foreach (Cart c in carts)
            {
                Carts.Add(c);
            }
            IsLoading = false;
        }

    }
}
