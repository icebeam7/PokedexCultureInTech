using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace Pokedex.Services
{
    public class TextToSpeechService
    {
        public static async Task<bool> Speak(string text, string isoLanguageCode)
        {
            try
            {
                var locales = await TextToSpeech.GetLocalesAsync();

                if (locales.Count() > 0)
                {
                    var localeOptions = locales.Where(x => x.Language == isoLanguageCode || x.Name == isoLanguageCode);
                    var locale = localeOptions.FirstOrDefault();

                    var settings = new SpeechOptions()
                    {
                        Volume = 0.75f,
                        Pitch = 1.0f,
                        Locale = locale
                    };

                    await TextToSpeech.SpeakAsync(text, settings);
                    return true;
                }
            }
            catch(Exception ex)
            {
            }

            return false;
        }
    }
}
