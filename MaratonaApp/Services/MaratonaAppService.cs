using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaratonaApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using MaratonaApp.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;
using Xamarin.Forms;
using MaratonaApp.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MaratonaAppService))]
namespace MaratonaApp.Services
{
    public class MaratonaAppService : IMaratonaAppService
    {
        public MobileServiceClient Client { get; set; } = null;

        public static bool UseAuth { get; set; } = false;

        public static string BaseUrl = "https://testeappbackend.azurewebsites.net/";

        public void Initialize()
        {
            Client = new MobileServiceClient(BaseUrl);

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
                Client.CurrentUser = new MobileServiceUser(Settings.UserId)
                {
                    MobileServiceAuthenticationToken = Settings.AuthToken
                };
        }



        public async Task<bool> LoginAsync()
        {
            Initialize();

            var auth = DependencyService.Get<IAuthentication>();
            var user = await auth.LoginAsync(Client, MobileServiceAuthenticationProvider.Facebook);

            if (user == null)
            {
                Settings.AuthToken = string.Empty;
                Settings.UserId = string.Empty;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Maratona App", "Não foi possível efetuar seu login, tente novamente mais tarde.", "Ok");
                });

                return false;
            }
            else
            {
                Settings.AuthToken = user.MobileServiceAuthenticationToken;
                Settings.UserId = user.UserId;
            }
            return true;
        }

        public async Task LogoutAsync()
        {
            Initialize();

            var auth = DependencyService.Get<IAuthentication>();
            await auth.LogoutAsync(Client);
        }

        public async Task<List<Cart>> GetCartsAsync(string userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            try
            {
                var response = await httpClient.GetAsync($"{BaseUrl}tables/Cart?$filter=(user+eq+'{userId}')")
                    .ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Cart>>(await response.Content.ReadAsStringAsync());
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debugger.Break();
            }
        
            return null;
        }

        public async Task<List<Item>> GetItemsAsync(string cartId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            try
            {
                var response = await httpClient.GetAsync($"{BaseUrl}tables/Item?$filter=(cartId+eq+'{cartId}')")
                    .ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debugger.Break();
            }

            return null;
        }

        //DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task DeleteCartAsync(Cart cart)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            if (!string.IsNullOrEmpty(cart.Id))
            {
                try
                {
                    List<Item> items = await GetItemsAsync(cart.Id);
                    foreach(Item it in items)
                    {
                        await DeleteItemAsync(it);
                    }
                    var response = await httpClient.DeleteAsync($"{BaseUrl}tables/Cart/{cart.Id}");

                    Debug.WriteLine(response.ReasonPhrase);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debugger.Break();
                }
            }
        }

        public async Task DeleteItemAsync(Item item)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            if (!string.IsNullOrEmpty(item.Id))
            {
                try
                {
                    var response = await httpClient.DeleteAsync($"{BaseUrl}tables/Item/{item.Id}");

                    Debug.WriteLine(response.ReasonPhrase);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debugger.Break();
                }
            }
        }

        public async Task SaveCartAsync(Cart cart)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var json = JsonConvert.SerializeObject(cart);
            var content = new StringContent(json, Encoding.UTF8, "application/json"); 
            if (string.IsNullOrEmpty(cart.Id))
            {
                try
                {
                    var response = await httpClient.PostAsync($"{BaseUrl}tables/Cart", content);

                } catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debugger.Break();
                }
            }
            else
            {
                var method = new HttpMethod("PATCH");

                var request = new HttpRequestMessage(method, $"{BaseUrl}tables/Cart/{cart.Id}")
                {
                    Content = content
                };
                var response = await httpClient.SendAsync(request);
            }
        }

        public async Task SaveItemAsync(Item item)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (string.IsNullOrEmpty(item.Id))
            {
                var response = await httpClient.PostAsync($"{BaseUrl}tables/Item", content).ConfigureAwait(false);
            }
            else
            {
                var method = new HttpMethod("PATCH");

                var request = new HttpRequestMessage(method, $"{BaseUrl}tables/Item/{item.Id}")
                {
                    Content = content
                };
                var response = await httpClient.SendAsync(request);
            }
        }

    }
}
