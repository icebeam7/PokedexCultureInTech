using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pokedex.Models;

namespace Pokedex.Services
{
    public static class CustomVisionService
    {
        private const string PredictionKey = "";
        private const string PredictionURL = "customvision/v3.0/Prediction//classify/iterations/PokedexIt01/image";
        private static HttpClient httpClient = HttpClientService.CreateClient("https://backtoschoolcognitive.cognitiveservices.azure.com/");

        public async static Task<string> ClassifyImage(string photo)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Prediction-Key"))
                httpClient.DefaultRequestHeaders.Add("Prediction-Key", PredictionKey);

            try
            {
                using (var stream = File.OpenRead(photo))
                {
                    var streamCopy = new MemoryStream();
                    stream.CopyTo(streamCopy);
                    stream.Seek(0, SeekOrigin.Begin);

                    using (var content = new ByteArrayContent(streamCopy.ToArray()))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        var post = await httpClient.PostAsync(PredictionURL, content);
                        var result = await post.Content.ReadAsStringAsync();
                        var cv = JsonConvert.DeserializeObject<CustomVisionResult>(result);

                        if (cv.Predictions.Count > 0)
                        {
                            var prediccion = GetPrediction(cv);
                            return prediccion.Probability > 0.5 ? prediccion.TagName : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        static Prediction GetPrediction(CustomVisionResult cv)
        {
            return cv.Predictions.OrderByDescending(x => x.Probability).FirstOrDefault();
        }
    }
}
