using FoodOrder.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrder.Persistence;

namespace FoodOrder.Desktop.ViewModel
{
    public class NewItemViewModel : ViewModelBase
    {
        private readonly IFoodOrderService _model;

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }


        public string DishName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool Spicy { get; set; }
        public bool Vegetarian { get; set; }
        public int Category { get; set; }

        public event EventHandler ExitApplication;

        public event EventHandler BackToMain;

        public NewItemViewModel(IFoodOrderService model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;

            ExitCommand = new DelegateCommand(param => OnExitApplication());
            BackCommand = new DelegateCommand(param => OnBackToMain());
            SaveCommand = new DelegateCommand(param => Save());
        }

        private async void Save()
        {
            try
            {
                var drinkOrDish = new DrinkOrDish
                {
                    Name = DishName,
                    Description = Description,
                    Price = Price,
                    Spicy = Spicy,
                    CategoryId = Category + 1,
                    Vegetarian = Vegetarian
                };
                var result = await _model.MakeDrinkOrDishAsync(drinkOrDish);
                if (result)
                {
                    OnMessageApplication("Mentve");
                }
            }
            catch (NetworkException e)
            {
                OnMessageApplication($"Váratlan hiba történt! ({e.Message})");
            }
        }

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }
        private void OnBackToMain()
        {
            BackToMain?.Invoke(this, EventArgs.Empty);
        }

    }
}
