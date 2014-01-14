
using System;

namespace MusicData
{
    public interface IAudioInteractor
    {
        void PlaySong(string filename);
        void PauseSong();
        void ResumeSong();
        void StopSong();
        float Volume { get; set; }
    }
}
