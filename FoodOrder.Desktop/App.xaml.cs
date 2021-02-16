using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FoodOrder.Desktop.Model;
using FoodOrder.Desktop.View;
using FoodOrder.Desktop.ViewModel;

namespace FoodOrder.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IFoodOrderService _service;
        private MainWindowViewModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private NewItemViewModel _newItemViewModel;
        private MainWindow _view;
        private LoginWindow _loginView;
        private NewItemWindow _newItemView;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new FoodOrderService(ConfigurationManager.AppSettings["baseAddress"]);

            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
        }

        public async void App_Exit(object sender, EventArgs e)
        {
            if (_service.IsUserLoggedIn)
            {
                await _service.LogoutAsync();
            }
            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
            _view.Close();
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainWindowViewModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.NewItemOpen += ViewModel_OpenItem;
            _mainViewModel.LogoutEvent += App_Exit;
            _view = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _view.Show();
            _loginView.Close();
        }
        private void ViewModel_Back(object sender, EventArgs e)
        {
            _mainViewModel = new MainWindowViewModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.NewItemOpen += ViewModel_OpenItem;
            _mainViewModel.LogoutEvent += App_Exit;
            _view = new MainWindow
            {
                DataContext = _mainViewModel
            };
            _view.Show();
            _newItemView.Close();
        }
        private void ViewModel_OpenItem(object sender, EventArgs e)
        {
            _newItemViewModel = new NewItemViewModel(_service);
            _newItemViewModel.MessageApplication += ViewModel_MessageApplication;
            _newItemViewModel.BackToMain += ViewModel_Back;

            _newItemView = new NewItemWindow
            {
                DataContext = _newItemViewModel
            };

            _newItemView.Show();
            _view.Close();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Msg", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
