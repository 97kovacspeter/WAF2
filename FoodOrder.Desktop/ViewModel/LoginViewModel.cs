using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FoodOrder.Desktop.Model;

namespace FoodOrder.Desktop.ViewModel
{
    /// <summary>
    /// A bejelentkezés nézetmodell típusa.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly IFoodOrderService _model;

        /// <summary>
        /// Kilépés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Bejelentkezés parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoginCommand { get; private set; }

        /// <summary>
        /// Felhasználónév lekérdezése, vagy beállítása.
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// Alkalmazásból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitApplication;

        /// <summary>
        /// Sikeres bejelentkezés eseménye.
        /// </summary>
        public event EventHandler LoginSuccess;

        /// <summary>
        /// Sikertelen bejelentkezés eseménye.
        /// </summary>
        public event EventHandler LoginFailed;

        /// <summary>
        /// Nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell.</param>
        public LoginViewModel(IFoodOrderService model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            UserName = string.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        /// <summary>
        /// Bejelentkezés
        /// </summary>
        /// <param name="passwordBox">Jelszótároló vezérlő.</param>
        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                bool result = await _model.LoginAsync(UserName, passwordBox.Password);

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        /// <summary>
        /// Sikeres bejelentkezés eseménykiváltása.
        /// </summary>
        private void OnLoginSuccess()
        {
            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Alkalmazásból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sikertelen bejelentkezés eseménykiváltása.
        /// </summary>
        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
