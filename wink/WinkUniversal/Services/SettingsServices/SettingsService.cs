using System;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml;

namespace WinkUniversal.Services.SettingsServices
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                    BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public  string WinkApiUrl
        {
            get
            {
                var winkApiUrl = "https://winkapi.quirky.com/";
                var value = _helper.Read<string>(nameof(WinkApiUrl), winkApiUrl);
                return value;
            }

            set
            {
                _helper.Write(nameof(WinkApiUrl), value);
            }
        }

        public string UserName
        {
            get
            {
                var value = _helper.Read<string>(nameof(UserName), string.Empty);
                return value;
            }
            set
            {
                _helper.Write(nameof(UserName), value);
            }
        }

        public string Password
        {
            get
            {
                var value = _helper.Read<string>(nameof(Password), string.Empty);
                return value;
            }
            set
            {
                _helper.Write(nameof(Password), value);
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }
    }
}

