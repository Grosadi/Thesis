using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using System.Net.Http;
using Thesis.Service;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Thesis.ViewModels;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace Thesis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {        
        public MainPage()
        {           
            InitializeComponent();            
        }

        private async void Fingerprint_Button_Clicked(object sender, EventArgs e)
        {      

            bool supported = await CrossFingerprint.Current.IsAvailableAsync(true);
            if (supported)
            {
                AuthenticationRequestConfiguration conf = new AuthenticationRequestConfiguration("Acces", "Acces");       

                var result = await CrossFingerprint.Current.AuthenticateAsync(conf);

                if (result.Authenticated)
                {
                    await DisplayAlert("Succes", "Authenticaton succesful!", "Ok");
                }
                else
                {
                    await DisplayAlert("Sorry", "Authenticaton failed!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Sorry", "Not supported!", "Ok");
            }
        }

        private async void DetectFace_Button_Clicked(object sender, EventArgs e)
        {
            var photo = await FaceService.GetMediaFileFromCamera();
            var person = new Person();

            try
            {
                person = await FaceService.Identify(photo);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Something went wrong", ex.Message, "Ok");
            }            

            if (person.Name != null)
            {
                await DisplayAlert("Identification Succesful!", $"Hello, {person.Name} !", "Ok");
            }            
            else
            {
                await DisplayAlert("Something went wrong", "Identification unsuccesful!", "Ok");
            }
        }       

        private async void AddFace_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFacePage());
        }
    }
}
