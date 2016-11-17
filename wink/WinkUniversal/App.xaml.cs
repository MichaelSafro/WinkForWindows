using Windows.UI.Xaml;
using System.Threading.Tasks;
using WinkUniversal.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace WinkUniversal
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            await Task.Delay(5000);
            await Authenticate();

        }

        private async Task Authenticate()
        {
            var settings = WinkApiWrapper.Settings.Instance;

            var _settings = Services.SettingsServices.SettingsService.Instance;

            if (!string.IsNullOrEmpty(Services.SettingsServices.SettingsService.Instance.WinkApiUrl) && !string.IsNullOrEmpty(_settings.UserName) && !string.IsNullOrEmpty(_settings.Password))
            {
                settings.baseAddress = new Uri(Services.SettingsServices.SettingsService.Instance.WinkApiUrl);
                var security = WinkApiWrapper.Security.Instance;

                if (await security.Authenticate(_settings.UserName, _settings.Password))
                {
                    NavigationService.Navigate(typeof(Views.MainPage));
                    await Task.CompletedTask;
                }
                else
                {
                    NavigationService.Navigate(typeof(Views.SettingsPage));
                    await Task.CompletedTask;
                }
            }
            else
            {
                NavigationService.Navigate(typeof(Views.SettingsPage));
                await Task.CompletedTask;
            }
        }
    }
}

