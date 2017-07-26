using MaratonaApp.Models;
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
    public class ItemsViewModel : BaseViewModel
    {
        private IMaratonaAppService _maratonaAppService;

        public DateTime MinDate { get; } = DateTime.Today;

        public Cart Cart { get; }

        public Command AddItemCommand { get; }

        public Command SaveCartCommand { get; }

        public Command UpdateCommand { get; }

        public Command ToggleCommand { get; }

        public Command DeleteCommand { get; }

        public Command DeleteCartCommand { get; }

        public ObservableCollection<Item> Items { get; }

        private string itemText;

        public string ItemText
        {
            get { return itemText; }
            set { SetProperty(ref itemText, value); }
        }

        public ItemsViewModel(Cart cart)
        {
            _maratonaAppService = new MaratonaAppService();

            AddItemCommand = new Command(ExecuteAddItemCommand);
            UpdateCommand = new Command(ExecuteUpdateCommand);
            SaveCartCommand = new Command(ExecuteSaveCartCommand);
            ToggleCommand = new Command<Item>(ExecuteToggleCommand);
            DeleteCommand = new Command<Item>(ExecuteDeleteCommand);
            DeleteCartCommand = new Command(ExecuteDeleteCartCommand);

            Items = new ObservableCollection<Item>();
            Cart = cart;

            LoadAsync();
        }

        private async void ExecuteDeleteCartCommand()
        {
            await _maratonaAppService.DeleteCartAsync(Cart);
            PopAsync();
        }

        public async void ExecuteDeleteCommand(Item item)
        {
            IsLoading = true;
            await _maratonaAppService.DeleteItemAsync(item);
            LoadAsync();
        }

        public async void ExecuteToggleCommand(Item item)
        {
            await _maratonaAppService.SaveItemAsync(item);
        }

        private async void ExecuteSaveCartCommand()
        {
            await _maratonaAppService.SaveCartAsync(Cart);
        }

        private void ExecuteUpdateCommand()
        {
            LoadAsync();
        }

        private async void ExecuteAddItemCommand()
        {
            await _maratonaAppService.SaveItemAsync(new Item { Name = ItemText, Done = false, Id_Cart = Cart.Id });
            LoadAsync();
        }

        private async void LoadAsync()
        {
            List<Item> items = await _maratonaAppService.GetItemsAsync(Cart.Id);

            Items.Clear();
            foreach (Item it in items)
            {
                Items.Add(it);
            }
        }
    }
}
