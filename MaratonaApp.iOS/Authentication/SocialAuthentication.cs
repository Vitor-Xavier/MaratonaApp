using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using MaratonaApp.iOS;
using MaratonaApp.Helpers;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace MaratonaApp.iOS
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client,
            MobileServiceAuthenticationProvider provider,
            IDictionary<string, string> parameters = null)
        {
            try
            {
                var current = GetController();
                var user = await client.LoginAsync(current, provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;

                return user;
            } catch (Exception e)
            {
                throw;
            }
        }

        public async Task LogoutAsync(MobileServiceClient client)
        {
            var cookieUrl = new Uri(Helpers.Constants.ApplicationURL);
            var cookieJar = NSHttpCookieStorage.SharedStorage;
            cookieJar.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
            foreach (var aCookie in cookieJar.Cookies)
            {
                cookieJar.DeleteCookie(aCookie);
            }
            await client.LogoutAsync();
        }

        private UIKit.UIViewController GetController()
        {
            var windows = UIKit.UIApplication.SharedApplication.KeyWindow;
            var root = windows.RootViewController;

            if (root == null) return null;

            var current = root;
            while(root.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }

            return current;
        }
    }
}