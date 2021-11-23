﻿using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Thesis.Service
{
    public static class FaceService
    {        
        private static FaceClient Client => new FaceClient(new ApiKeyServiceClientCredentials("ef50ac0be78e420cbf3960ac8843ff4f"), new HttpClient(), false) { Endpoint = "https://gadam.cognitiveservices.azure.com/" };        
        private static string _personGroupId => "person_group";

        public static async Task AddFace(string name, MediaFile photo)            
        {                       
            var stream = photo.GetStream();      

            if (await Client.PersonGroup.GetAsync(_personGroupId) == null)
            {
                await Client.PersonGroup.CreateAsync(_personGroupId, RecognitionModel.Recognition04);
            }            

            Person person = await Client.PersonGroupPerson.CreateAsync(_personGroupId, name);            
            
            await Client.PersonGroupPerson.AddFaceFromStreamAsync(_personGroupId, person.PersonId, stream);
            
            await Client.PersonGroup.TrainAsync(_personGroupId);

            var trainingStatus = await Client.PersonGroup.GetTrainingStatusAsync(_personGroupId);

            while (trainingStatus.Status != TrainingStatusType.Succeeded)
            {
                trainingStatus = await Client.PersonGroup.GetTrainingStatusAsync(_personGroupId);
            }                                   
        }

        public static async Task<Person> Identify(MediaFile photo)
        {
            var stream = photo.GetStream();            

            var detectedFaces = await Client.Face.DetectWithStreamAsync(stream);
            var faceIds = detectedFaces.Select(x => x.FaceId ?? throw new Exception("There is no face detected")).ToList();
            var identifiedFaces = await Client.Face.IdentifyAsync(faceIds, _personGroupId);

            return await Client.PersonGroupPerson.GetAsync(_personGroupId, identifiedFaces.First().Candidates[0].PersonId);
        }       

        public static async Task<MediaFile> GetMediaFileFromCamera()
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                throw new Exception("Device not supported!");
            }
            return await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                DefaultCamera = CameraDevice.Front,
                Directory = "Android/data/com.companyname.thesis/files/Pictures"
            });            
        }
    }
}
