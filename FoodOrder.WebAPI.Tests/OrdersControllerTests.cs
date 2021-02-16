using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FoodOrder.Persistence;
using Newtonsoft.Json;
using Xunit;

namespace FoodOrder.WebAPI.Tests
{
    public class OrdersControllerTests : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public OrdersControllerTests(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_GetOrderItems_WithInvalidId_ReturnsEmptyList()
        {
            // Arrange
            int Id = 66666;

            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/" + Id);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<DrinkOrDish>>(responseString);

            Assert.NotNull(responseObject);
            Assert.False(responseObject.Any());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Test_GetOrderItems_ReturnsAllRelevantItems(int id)
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/" + id);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<DrinkOrDish>>(responseString);

            Assert.NotNull(responseObject);
            var order = _fixture.Context.Orders.Find(id);
            Assert.All(responseObject, item => Assert.Contains(item.Id.ToString(), order.DrinksOrDishes));
            Assert.Equal(order.DrinksOrDishes.Split(" ").Count(), responseObject.Count());
        }

        [Fact]
        public async void Test_SearchEmpty_ReturnsAll()
        {
            // Arrange
            var search = "";

            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/search/" + search);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseList = await response.Content.ReadAsAsync<IEnumerable<Order>>();

            Assert.NotNull(responseList);
            Assert.Equal(_fixture.Context.Orders.Count(), responseList.Count());
        }

        [Fact]
        public async void Test_SearchTeszt_ReturnsAll()
        {
            // Arrange
            var search = "Teszt";

            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/search/" + search);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseList = await response.Content.ReadAsAsync<IEnumerable<Order>>();

            Assert.NotNull(responseList);
            Assert.Equal(_fixture.Context.Orders.Count(), responseList.Count());
        }

        [Fact]
        public async void Test_SearchForX_ReturnsNone()
        {
            // Arrange
            var search = "X";

            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/search/" + search);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseList = await response.Content.ReadAsAsync<IEnumerable<Order>>();

            Assert.NotNull(responseList);
            Assert.Empty(responseList);
        }

        [Fact]
        public async void Test_AddNewItem_DrinkOrDish()
        {
            // Arrange
            DrinkOrDish item = new DrinkOrDish()
            {
                CategoryId = 1,
                Description = "Teszt leírás",
                Name = "Teszt étel",
                Price = 1000,
                Spicy = false,
                Vegetarian = false
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync("api/Orders/NewItem", item);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseBool = await response.Content.ReadAsAsync<bool>();

            Assert.True(responseBool);
            Assert.True(_fixture.Context.DrinksOrDishes.Count(i => i.Name == item.Name) > 0);
        }

        [Fact]
        public async void Test_UnDeliveredCount()
        {
            // Act
            var undelivered = await _fixture.Client.GetAsync("api/Orders/Undelivered");

            // Assert
            undelivered.EnsureSuccessStatusCode();
            var undeliveredList = await undelivered.Content.ReadAsAsync<IEnumerable<Order>>();

            Assert.NotNull(undeliveredList);
            Assert.Equal(_fixture.Context.Orders.Count(l => l.Delivered == false), undeliveredList.Count());
        }

        [Fact]
        public async void Test_DeliveredCount()
        {
            // Act
            var delivered = await _fixture.Client.GetAsync("api/Orders/Delivered");

            // Assert
            delivered.EnsureSuccessStatusCode();
            var deliveredList = await delivered.Content.ReadAsAsync<IEnumerable<Order>>();

            Assert.NotNull(deliveredList);
            Assert.Equal(_fixture.Context.Orders.Count(l => l.Delivered == true), deliveredList.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Test_Deliver_CheckIfDeliveredCorrectly(int id)
        {
            // Act
            var undelivered = await _fixture.Client.GetAsync("api/Orders/Undelivered");
            var response = await _fixture.Client.PutAsJsonAsync("api/Orders/Deliver/" + id, true);
            var lateUndelivered = await _fixture.Client.GetAsync("api/Orders/Undelivered");

            // Assert
            undelivered.EnsureSuccessStatusCode();
            response.EnsureSuccessStatusCode();
            lateUndelivered.EnsureSuccessStatusCode();
            var undeliveredList = await undelivered.Content.ReadAsAsync<IEnumerable<Order>>();
            var lateUndeliveredList = await lateUndelivered.Content.ReadAsAsync<IEnumerable<Order>>();

            Assert.NotNull(undeliveredList);
            Assert.NotNull(lateUndeliveredList);
            var count = lateUndeliveredList.Count();
            Assert.Equal(undeliveredList.Count() - 1, count);
            Assert.Equal(_fixture.Context.Orders.Count(l => l.Delivered == false), count);
            Assert.True(_fixture.Context.Orders.Find(id).Delivered);
        }
    }
}
