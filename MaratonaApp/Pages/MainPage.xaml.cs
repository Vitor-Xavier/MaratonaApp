using MaratonaApp.Models;
using MaratonaApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MaratonaApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var cart = ((sender as Switch).BindingContext as Cart);
            if (cart == null) return;

            (BindingContext as MainViewModel).ExecuteToggleCommand(cart);
        }

        protected override void OnAppearing()
        {
            (BindingContext as MainViewModel).LoadAsync();
            base.OnAppearing();
        }
    }
}
