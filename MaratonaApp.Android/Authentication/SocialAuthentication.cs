using Xamarin.Forms;
using MaratonaApp.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Webkit;
using MaratonaApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace MaratonaApp.Droid
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, 
            MobileServiceAuthenticationProvider provider, 
            IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(Forms.Context, provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;

                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task LogoutAsync(MobileServiceClient client)
        {
            try
            {
                CookieManager.Instance.RemoveAllCookie();
                await client.LogoutAsync();

                Settings.AuthToken = string.Empty;
                Settings.UserId = string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}