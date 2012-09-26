
using System;
using System.Threading;
using NAudio.Wave;

namespace MusicData
{
    public class NAudioInteractor : IAudioInteractor
    {
        
        
        public NAudioInteractor()
        {
        
            
        }
        

        public void PlaySong(string filename)
        {
            var waveOutDevice = new WaveOut();
            WaveChannel32 inputStream;
            WaveStream mp3Reader;
            if (filename.EndsWith(".mp3"))
            {
                mp3Reader= new Mp3FileReader(filename);

                inputStream = new WaveChannel32(mp3Reader);
            }
            else
            {
                throw new InvalidOperationException("Unsupported extension");
            }

            waveOutDevice.Init(inputStream);
            waveOutDevice.Play();

            Thread.Sleep(mp3Reader.TotalTime);

            waveOutDevice.Stop();
            mp3Reader.Close();
            inputStream.Close();
            
            waveOutDevice.Dispose();
        }

        public event EventHandler SongChanged;
        public void PauseSong()
        {
            throw new NotImplementedException();
        }

        public void ResumeSong()
        {
            throw new NotImplementedException();
        }

        public void StopSong()
        {
            throw new NotImplementedException();
        }
    }
}
