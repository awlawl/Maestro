using System.ServiceProcess;
using System.Threading;
using MongoLibrary;
using MusicData;
using RealTimeMessaging;
using Website;

namespace MaestroService
{
    partial class MaestroService : ServiceBase
    {
        private PubnubMessaging _pubnub = null;
        private Thread _player = null;
        private SelfHostedWebsite _website = null;
        
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

            _player.Abort();

            _website.Stop();
        }

        public void Start()
        {
            _website = new SelfHostedWebsite();
            _website.Start();

            _player = new Thread(new ThreadStart(StartPlayer));
            _player.Name = "Main Player";
            _player.Start();

        }

        private void StartPlayer()
        {
            
            var playlistManager = GetPlaylistManager();
            var messagingWatcher = new MessagingPlaylistWatcher();
            var playlist = new Playlist(new IPlaylistWatcher[] { playlistManager, messagingWatcher });
            var dummyAudio = new NAudioInteractor();
            //var library = new MemoryLibraryRepository();
            var library = new MongoLibraryRepository();
            
            InsertTestSongs(library);

            //playlist.AddRange(library.GetAllMusic());

            var player = new Player(playlist, dummyAudio, library);
            _pubnub = new PubnubMessaging(player,true);
            
            messagingWatcher.AssignMessaging(_pubnub);
            playlistManager.AttachToPlaylist(playlist, library);

            _pubnub.StartListening();
            
            player.Play();
        }

        private IPlaylistWatcher GetPlaylistManager()
        {
            return new RandomSongPlaylistWatcher();
            //return new LoopingPlaylistWatcher();
        }

        private void InsertTestSongs(ILibraryRepository library)
        {
            //POC code for playing in the service
            //var testDirectory = @"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles";
            var testDirectory = @"f:\music\Manchester Orchestra\Mean Everything To Nothing";
            //var testDirectory = @"C:\Users\alyons2\Music\Amazon MP3\Deftones\Koi No Yokan [Explicit]";
            //var testDirectory2 = @"C:\Users\alyons2\Music\Amazon MP3\Manchester Orchestra\Mean Everything To Nothing";
            //var td3 = @"C:\Users\awl\Dropbox\Music\Ultra Rare Trax";
            //var testDirectory = @"C:\Users\awl\Music\deftones";
            //var testDirectory2 = @"E:\music\KOMPRESSOR";

            library.ClearLibrary();
            //library.AddDirectoryToLibrary(testDirectory2);
            library.AddDirectoryToLibrary(testDirectory);
            //library.AddDirectoryToLibrary(td3);
        }

       
    }
}
