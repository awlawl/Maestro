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
            var testDirectory = @"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles";
            //var testDirectory = @"C:\Users\Public\Music\Manchester Orchestra\Simple Math";
            var loopingWatcher = new LoopingPlaylistWatcher();
            var playlist = new Playlist(loopingWatcher);
            var dummyAudio = new NAudioInteractor();

            var player = new Player(playlist, dummyAudio);

            var musicInfoReader = new MusicInfoReader();
            var files = musicInfoReader.CrawlDirectory(testDirectory).Select(X => X.FullPath).ToArray();

            foreach (var file in files)
                loopingWatcher.AddToLoop(file);
            
            loopingWatcher.AttachToPlaylist(playlist);
            
        }

       
    }
}
