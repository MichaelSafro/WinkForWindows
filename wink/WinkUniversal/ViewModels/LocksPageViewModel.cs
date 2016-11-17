
using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinkUniversal.ViewModels
{
    public class LocksPageViewModel : ViewModelBase
    {
        public LocksPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                //      Value = "Designtime value";
            }
        }

        //string _Value = "Gas";
        //public string Value { get { return _Value; } set { Set(ref _Value, value); } }


        ObservableCollection<WinkApiWrapper.Entities.Lock> _locks;

        public ObservableCollection<WinkApiWrapper.Entities.Lock> Locks
        {
            get { return this._locks; }
            set { this._locks = value; }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            //if (suspensionState.Any())
            //{
            //    //// bind grid

            //    var lightController = new WinkApiWrapper.Controllers.Devices();
            //    _lights = await lightController.GetLights();
            //    //Value = suspensionState[nameof(Value)]?.ToString();
            //}
            var locksController = new WinkApiWrapper.Controllers.Devices();
            _locks = await locksController.GetLocks();

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            //if (suspending)
            //{
            //    suspensionState[nameof(Value)] = Value;
            //}
            //var lightController = new WinkApiWrapper.Controllers.Devices();
            //_lights = await lightController.GetLights();



            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }


        public async void toggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                //var dialog = new MessageDialog(toggleSwitch.Tag.ToString());
                //await dialog.ShowAsync();
                var LockId = int.Parse(toggleSwitch.Tag.ToString());
                var boolOnOff = toggleSwitch.IsOn;
                var lightController = new WinkApiWrapper.Controllers.Devices();
                await lightController.SetLockObject(LockId, boolOnOff);
            }
        }

        //public async void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        //{
        //    Slider slider = sender as Slider;
        //    if (slider != null)
        //    {
        //        //media.Volume = slider.Value;
        //        //var dialog = new MessageDialog(slider.Value.ToString());
        //        //await dialog.ShowAsync();
        //        var lightBulbId = int.Parse(slider.Tag.ToString());
        //        var lightController = new WinkApiWrapper.Controllers.Devices();
        //        var lightBulb = await lightController.GetLightObject(lightBulbId);
        //        //System.Diagnostics.Debug.WriteLine("value changed " + slider.Value + " current value " + lightBulb.ConvertedBrightness);
        //        if (slider.Value != lightBulb.ConvertedBrightness)
        //        {
        //            await lightController.SetightObjectBrightness(lightBulbId, (float)slider.Value);
        //        }
        //    }
        //}

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}


