using MaratonaApp.Models;
using MaratonaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaratonaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
            //BindingContext = new ItemsViewModel();
        }

        private void SwitchCell_OnChanged_1(object sender, ToggledEventArgs e)
        {
            var it = ((sender as SwitchCell).BindingContext as Item);
            if (it == null) return;

            (BindingContext as ItemsViewModel).ExecuteToggleCommand(it);
        }
    }
}
