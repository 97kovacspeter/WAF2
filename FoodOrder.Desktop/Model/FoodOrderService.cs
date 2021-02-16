using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using FoodOrder.Persistence;

namespace FoodOrder.Desktop.Model
{
    public class FoodOrderService : IFoodOrderService
    {
        private readonly HttpClient _client;

        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn => _isUserLoggedIn;

        public FoodOrderService(string baseAddress)
        {
            _isUserLoggedIn = false;
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<bool> MakeDrinkOrDishAsync(DrinkOrDish dod)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Orders/NewItem", dod);
            if (response.IsSuccessStatusCode)
            {
                var tmp = await response.Content.ReadAsAsync<bool>();
                if (!tmp)
                {
                    throw new NetworkException("Létező név vagy rossz adatok");
                }
                return true;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<Order>> LoadListsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders");

            if (response.IsSuccessStatusCode)
            {
                var tmp = await response.Content.ReadAsAsync<IEnumerable<Order>>();
                return tmp;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<Order>> SearchAsync(string searchString)
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders/search/" + searchString);

            if (response.IsSuccessStatusCode)
            {
                var tmp = await response.Content.ReadAsAsync<IEnumerable<Order>>();
                return tmp;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<Order>> DeliveredAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders/Delivered");

            if (response.IsSuccessStatusCode)
            {
                var tmp = await response.Content.ReadAsAsync<IEnumerable<Order>>();
                return tmp;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<Order>> UndeliveredAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders/Undelivered");

            if (response.IsSuccessStatusCode)
            {
                var tmp = await response.Content.ReadAsAsync<IEnumerable<Order>>();
                return tmp;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<Order>> DeliveryAsync(int id)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync("api/Orders/Deliver/" + id, true);
            if (response.IsSuccessStatusCode)
            {
                var tmp = await response.Content.ReadAsAsync<IEnumerable<Order>>();
                return tmp;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<DrinkOrDish>> LoadItemsAsync(int listId)
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders/" + listId);
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<DrinkOrDish>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            AccountDto user = new AccountDto
            {
                UserName = name,
                Password = password
            };

            //            var response = await _client.PostAsync("api/Account/Login",
            //                new StringContent(JsonConvert.SerializeObject(user),
            //                    Encoding.UTF8,
            //                    "application/json"));
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Login", user);

            if (response.IsSuccessStatusCode)
            {
                _isUserLoggedIn = true;
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _client.PostAsJsonAsync("api/Account/Signout", "");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
    }
}
