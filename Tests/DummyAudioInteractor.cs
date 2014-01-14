
using System;
using System.Collections.Generic;
using MusicData;

namespace Tests
{
    public class DummyAudioInteractor : IAudioInteractor
    {
        public List<string> PlayHistory { get; set; }

        public DummyAudioInteractor()
        {
            PlayHistory = new List<string>();
        }

        public void PlaySong(string filename)
        {
            PlayHistory.Add(filename);
        }

        public void PauseSong()
        {
            WasPaused = true;
        }

        public void ResumeSong()
        {
            WasResumed = true;
        }

        public void StopSong()
        {
            WasStopped = true;
        }
        

        public bool WasPaused { get; set; }

        public bool WasResumed { get; set; }

        public bool WasStopped { get; set; }


        public float Volume { get; set; }
    }
}
