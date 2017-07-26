using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaratonaApp.UWP;
using MaratonaApp.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using Windows.Web.Http;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace MaratonaApp.UWP
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client,
            MobileServiceAuthenticationProvider provider,
            IDictionary<string, string> parameters = null)
        {
            try
            {
                //var user = await client.LoginAsync(provider);
                var user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook);

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
                Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;
                HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri(Helpers.Constants.ApplicationURL));
                foreach (HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
                await client.LogoutAsync();

                Settings.UserId = string.Empty;
                Settings.AuthToken = string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
