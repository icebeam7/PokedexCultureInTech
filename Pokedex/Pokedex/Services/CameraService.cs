using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace Pokedex.Services
{
    public class CameraService
    {
        public static async Task<string> TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                return await LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
            }

            return string.Empty;
        }

        private static async Task<string> LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
                return string.Empty;

            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            return newFile;
        }
    }
}
