using System.ServiceProcess;
using System.Linq;
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
            _apiHosting = new ApiHosting();
            _apiHosting.Start();

            StartPlayer();

        }

        private void StartPlayer()
        {
            //POC code for playing in the service
            //var testDirectory = @"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles";
            //var testDirectory = @"C:\Users\Public\Music\Manchester Orchestra\Simple Math";
            var testDirectory = @"C:\Users\awl\Dropbox\Stuff\Maestro\TestFiles";
            var loopingWatcher = new LoopingPlaylistWatcher();
            var playlist = new Playlist(loopingWatcher);
            var dummyAudio = new NAudioInteractor();
            var library = new MemoryLibraryRepository();

            var player = new Player(playlist, dummyAudio);

            library.AddDirectoryToLibrary(testDirectory);
            
            loopingWatcher.AttachToPlaylist(playlist, library);
            
        }

       
    }
}
