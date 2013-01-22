namespace MusicData
{
    public interface IPlayer
    {
        void Play();
        void Stop();
        void Next();
        void Back();
        void Pause();
        void Resume();
        bool IsPaused { get; set; }
    }
}