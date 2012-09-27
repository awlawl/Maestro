using System;
using System.ServiceProcess;
using System.Threading;
using MusicData;
using RestAPI;

namespace MaestroService
{
    partial class MaestroService : ServiceBase
    {
        private ApiHosting _apiHosting = null;
        
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
           _apiHosting.Stop();
            Player.Current.Stop();
            
        }

        public void Start()
        {
            
            StartPlayer();

            _apiHosting = new ApiHosting();
            _apiHosting.Start();

        }

        private void StartPlayer()
        {
            //POC code for playing in the service
            var loopingWatcher = new LoopingPlaylistWatcher();
            var playlist = new Playlist(loopingWatcher);
            var dummyAudio = new NAudioInteractor();

            var player = new Player(playlist, dummyAudio);

            loopingWatcher.AddToLoop(@"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles\one.mp3");
            loopingWatcher.AddToLoop(@"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles\two.mp3");
            loopingWatcher.AddToLoop(@"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles\three.mp3");
            
            loopingWatcher.AttachToPlaylist(playlist);
        }

       
    }
}
