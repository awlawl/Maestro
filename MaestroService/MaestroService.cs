using System.ServiceProcess;
using MusicData;
using RealTimeMessaging;

namespace MaestroService
{
    partial class MaestroService : ServiceBase
    {
        private PubnubMessaging _pubnub = null;
        
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
            Player.Current.Stop();
            
        }

        public void Start()
        {
            
            StartPlayer();

        }

        private void StartPlayer()
        {
            //POC code for playing in the service
            //var testDirectory = @"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles";
            //var testDirectory = @"C:\Users\awl\Dropbox\Stuff\Maestro\TestFiles";
            var testDirectory = @"C:\Users\alyons2\Music\Deftones\White Pony";
            var loopingWatcher = new LoopingPlaylistWatcher();
            var messagingWatcher = new MessagingPlaylistWatcher();
            var playlist = new Playlist(new IPlaylistWatcher[] {loopingWatcher,messagingWatcher});
            var dummyAudio = new NAudioInteractor();
            var library = new MemoryLibraryRepository();

            var player = new Player(playlist, dummyAudio);
            _pubnub = new PubnubMessaging(player,true);
            
            messagingWatcher.AssignMessaging(_pubnub);
            
            library.AddDirectoryToLibrary(testDirectory);
            
            loopingWatcher.AttachToPlaylist(playlist, library);

            _pubnub.StartListening();
        }

       
    }
}
