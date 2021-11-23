using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thesis.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thesis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFacePage : ContentPage
    {        
        private MediaFile Photo { get; set; }         
        public AddFacePage()
        {
            InitializeComponent();
            RegisterBtn.IsVisible = false;
        }

        private async void TakePhoto_Button_Clicked(object sender, EventArgs e)
        {
            if(FaceName.Text == null)
            {
                await DisplayAlert("", "Name is Required", "Ok");
            }
            else
            {
                Photo = await FaceService.GetMediaFileFromCamera();

                if (Photo != null)
                {
                    RegisterBtn.IsVisible = true;
                    PhotoImage.Source = ImageSource.FromStream(() => { return Photo.GetStream(); });
                }
            }            
        }

        private async void RegisterFace_Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var name = FaceName.Text;
                await FaceService.AddFace(name, Photo);

                await DisplayAlert("Great!", "Face added to Azure!", "Ok");

                await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
             }
            catch(Exception)
            {
                await DisplayAlert ("Sorry", "Something went wrong!", "Ok");
            }
        }
    }
}