using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Thesis.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand FingerPrintAuthenticateCommand => new Command(async () => await AuthenticateViaFingerprint());

        private async Task AuthenticateViaFingerprint()
        {
            bool supported = await CrossFingerprint.Current.IsAvailableAsync(true);
            if (supported)
            {
                AuthenticationRequestConfiguration conf = new AuthenticationRequestConfiguration("Acces", "Acces");

                var result = await CrossFingerprint.Current.AuthenticateAsync(conf);

                if (result.Authenticated)
                {
                    
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }
    }
}
