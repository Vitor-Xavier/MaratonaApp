using System;
using System.Collections.Generic;
using MaratonaApp.Models;
using System.Threading.Tasks;

namespace MaratonaApp.Services
{
    public interface IMaratonaAppService
    {
        Task<bool> LoginAsync();

        Task LogoutAsync();

        Task<List<Cart>> GetCartsAsync(string userId);

        Task<List<Item>> GetItemsAsync(string cartId);

        Task DeleteCartAsync(Cart cart);

        Task DeleteItemAsync(Item item);

        Task SaveItemAsync(Item item);

        Task SaveCartAsync(Cart cart);
    }
}
