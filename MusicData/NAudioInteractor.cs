
using System;
using System.Threading;
using NAudio.Wave;

namespace MusicData
{
    public class NAudioInteractor : IAudioInteractor
    {
        private Thread _playingThread = null;
        private WaveOut _waveOutDevice = null;

        public void PlaySong(string filename)
        {
            _playingThread = new Thread(new ParameterizedThreadStart(PlayInThread));
            _playingThread.Name = "Audio Interactor Player";
            _playingThread.Start(filename);
            _playingThread.Join();
        }

        
        private void PlayInThread(object data)
        {
            string filename = (string)data;

            _waveOutDevice = new WaveOut();
            WaveChannel32 inputStream = null;
            WaveStream mp3Reader = null;

            try
            {
                if (filename.EndsWith(".mp3"))
                {
                    mp3Reader = new Mp3FileReader(filename);

                    inputStream = new WaveChannel32(mp3Reader);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported extension");
                }

                _waveOutDevice.Init(inputStream);
                _waveOutDevice.Volume = 0.1f;
                _waveOutDevice.Play();

                Log.Debug("Playing " + filename);

                Thread.Sleep(mp3Reader.TotalTime);
            }
            finally
            {
                _waveOutDevice.Stop();
                if (mp3Reader!=null)
                    mp3Reader.Close();

                if (inputStream!=null)
                    inputStream.Close();

                _waveOutDevice.Dispose();
            }

            
        }

        public event EventHandler SongChanged;
        public void PauseSong()
        {

        }

        public void ResumeSong()
        {
            throw new NotImplementedException();
        }

        public void StopSong()
        {
            if (_waveOutDevice!=null)
                _waveOutDevice.Stop();

            _playingThread.Abort();
            _playingThread.Join();
        }
    }
}
