using System;
using System.ServiceProcess;
using System.Threading;
using MusicData;

namespace MaestroService
{
    partial class MaestroService : ServiceBase
    {
        private Thread _workerThread = null;

        public MaestroService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            _workerThread.Abort();
            
        }

        public void Start()
        {
            _workerThread = new Thread(new ThreadStart(WorkerThread));
            _workerThread.Name = "Worker Thread";
            _workerThread.Start();
        }

        private void WorkerThread()
        {
             var loopingWatcher = new LoopingPlaylistWatcher();
            var playlist = new Playlist(loopingWatcher);
            var dummyAudio = new NAudioInteractor();

            var player = new Player(playlist, dummyAudio);

            loopingWatcher.AddToLoop(@"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles\one.mp3");
            loopingWatcher.AddToLoop(@"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles\two.mp3");
            loopingWatcher.AddToLoop(@"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles\three.mp3");

            loopingWatcher.AttachToPlaylist(playlist);

            player.Play();   
           
        }
    }
}
