using MusicData;
using Newtonsoft.Json;

namespace Website.Routes
{
    public class VolumeRoute
    {
        public dynamic GetVolume()
        {
            return JsonConvert.SerializeObject(Player.Current.AudioInteractor.Volume);
        }

        internal Nancy.Response SetVolume(float value)
        {
            Player.Current.AudioInteractor.Volume = value;
            return JsonConvert.SerializeObject(value);
        }
    }
}
