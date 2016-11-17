using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;

namespace WinkUniversal.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {




        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = Services.SettingsServices.SettingsService.Instance;
              

            }
        }

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        private string _BusyText = "Please wait...";
        public string BusyText
        {
            get { return _BusyText; }
            set
            {
                Set(ref _BusyText, value);
                _ShowBusyCommand.RaiseCanExecuteChanged();
            }
        }

        public string WinkApiUrl
        {
            get { return _settings.WinkApiUrl; }
            set {

                //record
                 _settings.WinkApiUrl = value;
                //add to API settings
                var settings = WinkApiWrapper.Settings.Instance;
                settings.baseAddress = new Uri(Services.SettingsServices.SettingsService.Instance.WinkApiUrl);
            }
        }

        public string UserName
        {
            get
            {
                return _settings.UserName;
            }
            set
            {
                _settings.UserName = value;

            }

        }

        public string Password
        {
            get
            {
                return string.Empty; //_settings.Password;
            }
            set
            {
                _settings.Password = value;

            }

        }



        DelegateCommand _SaveWinkApiUrl;
        public DelegateCommand SaveWinkApiUrl
            => _SaveWinkApiUrl ?? (_SaveWinkApiUrl = new DelegateCommand(async () =>
           {

               Views.Busy.SetBusy(true, _settings.WinkApiUrl);
               await Task.Delay(2000);
               Views.Busy.SetBusy(false);                              
           },() => !string.IsNullOrEmpty(WinkApiUrl)
            ));

        DelegateCommand _ShowBusyCommand;
        public DelegateCommand ShowBusyCommand
            => _ShowBusyCommand ?? (_ShowBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, _BusyText);
                await Task.Delay(5000);
                Views.Busy.SetBusy(false);
            }, () => !string.IsNullOrEmpty(BusyText)));


        DelegateCommand _Authenticate;
        public DelegateCommand Authenticate
            => _Authenticate ?? (_Authenticate = new DelegateCommand(async () =>
            {
                var settings = WinkApiWrapper.Settings.Instance;

                var _settings = Services.SettingsServices.SettingsService.Instance;

                if (!string.IsNullOrEmpty(Services.SettingsServices.SettingsService.Instance.WinkApiUrl) && !string.IsNullOrEmpty(_settings.UserName) && !string.IsNullOrEmpty(_settings.Password))
                {
                    settings.baseAddress = new Uri(Services.SettingsServices.SettingsService.Instance.WinkApiUrl);
                    var security = WinkApiWrapper.Security.Instance;

                    if (await security.Authenticate(_settings.UserName, _settings.Password))
                    {
                        App.Current.NavigationService.Navigate(typeof(Views.MainPage));
                        //await Task.CompletedTask;
                    }
                    else
                    {
                        App.Current.NavigationService.Navigate(typeof(Views.SettingsPage));
                       // await Task.CompletedTask;
                    }
                }
                else
                {
                    App.Current.NavigationService.Navigate(typeof(Views.SettingsPage));
                    //await Task.CompletedTask;
                }
            }, () => !string.IsNullOrEmpty(BusyText)));

    }


    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }
}

