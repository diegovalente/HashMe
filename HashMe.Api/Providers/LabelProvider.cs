using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using System.Text;
using System.IO;
using System.Reflection;

namespace HashMe.Api.Providers
{
    public static class LabelProvider
    {
        /// <summary>
        /// Creates an authorized Cloud Vision client service using Application 
        /// Default Credentials.
        /// </summary>
        /// <returns>an authorized Cloud Vision client.</returns>
        private static VisionService CreateAuthorizedClient()
        {
            Stream stream = new MemoryStream(HashMe.Api.Properties.Resources.HashMe_851e1cc05df6);             
            
            GoogleCredential credential =
                GoogleCredential.FromStream(stream);
            // Inject the Cloud Vision scopes
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    VisionService.Scope.CloudPlatform
                });
            }
            return new VisionService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                GZipEnabled = false
            });
        }

        /// <summary>
        /// Detect labels for an image using the Cloud Vision API.
        /// </summary>
        /// <param name="vision">an authorized Cloud Vision client.</param>
        /// <param name="imagePath">the path where the image is stored.</param>
        /// <returns>a list of labels detected by the Vision API for the image.
        /// </returns>
        private static IList<AnnotateImageResponse> DetectLabels(
            VisionService vision, byte[] image)
        {
            Console.WriteLine("Detecting Labels...");
            // Convert image to Base64 encoded for JSON ASCII text based request               
            string imageContent = Convert.ToBase64String(image);
            // Post label detection request to the Vision API
            var responses = vision.Images.Annotate(
                new BatchAnnotateImagesRequest()
                {
                    Requests = new[] {
                    new AnnotateImageRequest() {
                        Features = new [] { new Feature() { Type =
                          "LABEL_DETECTION"}},
                        Image = new Image() { Content = imageContent }
                    }
               }
                }).Execute();
            return responses.Responses;
        }

        public static List<string> GetLabelsFromImage(byte[] image)
        {            
            List<string> labels = new List<string>();

            VisionService vision = CreateAuthorizedClient();
            
            IList<AnnotateImageResponse> result = DetectLabels(
                vision, image);
            
            if (result != null)
            {
                // Loop through and output label annotations for the image
                foreach (var response in result)
                {
                    
                    foreach (var label in response.LabelAnnotations)
                    {
                        labels.Add(label.Description);
                        Console.WriteLine(label.Description + " (score:"
                        + label.Score + ")");
                    }
                }
            }
            return labels;
        }
    }
}