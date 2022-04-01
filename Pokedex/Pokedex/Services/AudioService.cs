using System.Net.Http;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;

namespace Pokedex.Services
{
    public static class AudioService
    {
        private static HttpClient httpClient = HttpClientService.CreateClient("");

        public static async Task PlayAudio(string httpFile)
        {
            await Task.Run(async () =>
            {
                await GetAndPlayAudio(httpFile);
            });
        }

        private static async Task GetAndPlayAudio(string httpFile)
        {
            var request = await httpClient.GetAsync(httpFile);

            if (request.IsSuccessStatusCode)
            {
                var fileStream = await request.Content.ReadAsStreamAsync();

                var player = CrossSimpleAudioPlayer.Current;
                player.Load(fileStream);
                player.Play();
            }
        }
    }
}