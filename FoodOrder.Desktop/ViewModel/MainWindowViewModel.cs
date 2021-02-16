using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FoodOrder.Desktop.Model;
using FoodOrder.Persistence;
using Microsoft.Expression.Interactivity.Core;

namespace FoodOrder.Desktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Order> _lists;
        private ObservableCollection<DrinkOrDish> _items;
        private readonly IFoodOrderService _service;

        public ObservableCollection<Order> Lists
        {
            get => _lists;
            set
            {
                _lists = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DrinkOrDish> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand DeliverCommand { get; set; }
        public DelegateCommand DeliveredCommand { get; set; }
        public DelegateCommand UndeliveredCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand AllCommand { get; set; }
        public DelegateCommand NewItemCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

        public MainWindowViewModel(IFoodOrderService service)
        {
            _service = service;
            LoadAsync();

            SelectCommand = new DelegateCommand(LoadItems);
            DeliverCommand = new DelegateCommand(Deliver);
            DeliveredCommand = new DelegateCommand(Delivered);
            UndeliveredCommand = new DelegateCommand(Undelivered);
            SearchCommand = new DelegateCommand(Search);
            AllCommand = new DelegateCommand(Load);
            NewItemCommand = new DelegateCommand(NewItem);
            LogoutCommand = new DelegateCommand(Logout);
        }


        public event EventHandler NewItemOpen;
        private void OnNewItemOpen()
        {
            NewItemOpen?.Invoke(this, EventArgs.Empty);
        }

        private void NewItem(object param)
        {
            try
            {
                OnNewItemOpen();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public event EventHandler LogoutEvent;
        private void OnLogout()
        {
            LogoutEvent?.Invoke(this, EventArgs.Empty);
        }

        private void Logout(object param)
        {
            try
            {
                OnLogout();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void Deliver(object param)
        {
            try
            {
                Items = new ObservableCollection<DrinkOrDish>();
                var id = ((Order)param).Id;
                Lists = new ObservableCollection<Order>(await _service.DeliveryAsync(id));
                OnMessageApplication($"Teljesített rendelés: {((Order)param).Name}");

            }
            catch (NetworkException e)
            {
                OnMessageApplication($"Váratlan hiba történt! ({e.Message})");
            }
        }
        public void Load(object param)
        {
            try
            {
                LoadAsync();
            }
            catch (NetworkException e)
            {
                OnMessageApplication($"Váratlan hiba történt! ({e.Message})");
            }
        }

        public async void Search(object param)
        {
            try
            {
                Items = new ObservableCollection<DrinkOrDish>();
                Lists = new ObservableCollection<Order>(await _service.SearchAsync(((TextBox)param).Text.ToString()));
            }
            catch (NetworkException e)
            {
                OnMessageApplication($"Váratlan hiba történt! ({e.Message})");
            }
        }

        public async void Delivered(object param)
        {
            try
            {
                Items = new ObservableCollection<DrinkOrDish>();
                Lists = new ObservableCollection<Order>(await _service.DeliveredAsync());
            }
            catch (NetworkException e)
            {
                OnMessageApplication($"Váratlan hiba történt! ({e.Message})");
            }
        }

        public async void Undelivered(object param)
        {
            try
            {
                Items = new ObservableCollection<DrinkOrDish>();
                Lists = new ObservableCollection<Order>(await _service.UndeliveredAsync());
            }
            catch (NetworkException e)
            {
                OnMessageApplication($"Váratlan hiba történt! ({e.Message})");
            }
        }

        public async void LoadItems(object param)
        {
            try
            {
                Items = new ObservableCollection<DrinkOrDish>(await _service.LoadItemsAsync(((Order)param).Id));
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void LoadAsync()
        {
            try
            {
                Items = new ObservableCollection<DrinkOrDish>();
                Lists = new ObservableCollection<Order>(await _service.LoadListsAsync());
                Lists.Remove(null);
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }
    }
}
