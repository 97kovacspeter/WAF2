using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrder.Persistence;

namespace FoodOrder.Desktop.Model
{
    public interface IFoodOrderService
    {
        bool IsUserLoggedIn { get; }
        Task<IEnumerable<Order>> DeliveryAsync(int id);
        Task<IEnumerable<Order>> SearchAsync(string searchString);
        Task<IEnumerable<Order>> DeliveredAsync();
        Task<IEnumerable<Order>> UndeliveredAsync();
        Task<IEnumerable<Order>> LoadListsAsync();
        Task<IEnumerable<DrinkOrDish>> LoadItemsAsync(int listId);
        Task<bool> MakeDrinkOrDishAsync(DrinkOrDish dod);
        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}
