using System;
using System.ServiceProcess;
using System.Threading;
using System.Configuration;
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
        private ImportFolderWatcher _folderWatcher = null;
        
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

            _folderWatcher.Stop();
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
            try
            {
                var playlistManager = GetPlaylistManager();
                var messagingWatcher = new MessagingPlaylistWatcher();
                var playlist = new Playlist(new IPlaylistWatcher[] { playlistManager, messagingWatcher });
                var dummyAudio = new NAudioInteractor();
                var library = new MongoLibraryRepository(null);
                _folderWatcher = new ImportFolderWatcher(library);

                if (ConfigurationManager.AppSettings["ResetLibraryForTesting"]=="true")
                    InsertTestSongs(library);

                var player = new Player(playlist, dummyAudio, library);
                _pubnub = new PubnubMessaging(player, true);

                messagingWatcher.AssignMessaging(_pubnub);
                messagingWatcher.AttachToPlaylist(playlist, library);
                playlistManager.AttachToPlaylist(playlist, library);

                _pubnub.StartListening();

                library.Messaging = _pubnub;

                _folderWatcher.Start();

                if (ConfigurationManager.AppSettings["AutoStartPlaying"]=="true")
                    player.Play();

                _pubnub.SendNowPlaying(playlist[0]);
            }
            catch (Exception exc)
            {
                Log.Debug(exc.ToString());
            }
        }

        private IPlaylistWatcher GetPlaylistManager()
        {
            return new RandomSongPlaylistWatcher();
            //return new LoopingPlaylistWatcher();
        }

        private void InsertTestSongs(ILibraryRepository library)
        {
            //POC code for playing in the service
            
            var testDirectories = new string[] {
                //@"f:\music\Manchester Orchestra\Mean Everything To Nothing",
                //@"F:\music\Auf Der Maur\Auf Der Maur",
                //@"C:\Users\awl\Music\deftones",
                //@"F:\music\Nine Inch Nails",
                //@"c:\Shared\Maestro\Music\Manual",
                //@"C:\Users\awl\Dropbox\Music\Maestro",
                @"C:\Users\alyons2\Dropbox\Music\Maestro"
            };

            library.ClearLibrary();

            foreach(var dir in testDirectories)
               library.AddDirectoryToLibrary(dir);
        }

       
    }
}
