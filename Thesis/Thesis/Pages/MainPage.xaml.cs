using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

                var succes = false;

                if (result.Authenticated)
                {
                    succes = true;
                    await DisplayAlert("Succes", "Authenticaton succesful!", "Ok");                    
                }
                else
                {
                    await DisplayAlert("Sorry", "Authenticaton failed!", "Ok");
                }

                Service.Service.AddFingerprintToDatabase(succes);
            }
            else
            {
                await DisplayAlert("Sorry", "Not supported!", "Ok");
            }
        }

        private async void DetectFace_Button_Clicked(object sender, EventArgs e)
        {
            var photo = await Service.Service.GetMediaFileFromCamera();
            var person = new Person();
            var succes = false;

            try
            {
                person = await Service.Service.Identify(photo);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Something went wrong", ex.Message, "Ok");
            }            

            if (person.Name != null)
            {
                succes = true;
                await DisplayAlert("Identification Succesful!", $"Hello, {person.Name} !", "Ok");                
            }

            Service.Service.AddFaceIdToDatabase(person.Name, succes);
        }       

        private async void AddFace_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFacePage());
        }

        private async void ExportData_Button_Clicked(object sender, EventArgs e)
        {
            await Services.DatabaseService.CreateOutputFile();
        }
    }
}
