﻿using System.ServiceProcess;
using MongoLibrary;
using MusicData;
using RealTimeMessaging;
using RestAPI;

namespace MaestroService
{
    partial class MaestroService : ServiceBase
    {
        private PubnubMessaging _pubnub = null;
        private ApiHosting _api = null;
        
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
            _api.Stop();
        }

        public void Start()
        {
            _api = new RestAPI.ApiHosting();
            _api.Start();
            
            StartPlayer();
           

        }

        private void StartPlayer()
        {
            
            var loopingWatcher = new LoopingPlaylistWatcher();
            var messagingWatcher = new MessagingPlaylistWatcher();
            var playlist = new Playlist(new IPlaylistWatcher[] {loopingWatcher,messagingWatcher});
            var dummyAudio = new NAudioInteractor();
            //var library = new MemoryLibraryRepository();
            var library = new MongoLibraryRepository();
            
            InsertTestSongs(library);

            var player = new Player(playlist, dummyAudio);
            _pubnub = new PubnubMessaging(player,true);
            
            messagingWatcher.AssignMessaging(_pubnub);
            loopingWatcher.AttachToPlaylist(playlist, library);

            _pubnub.StartListening();
            _pubnub.SendPingReply();
            _pubnub.SendNowPlaying(player.Playlist.CurrentSong);

            player.Play();
        }

        private void InsertTestSongs(ILibraryRepository library)
        {
            //POC code for playing in the service
            //var testDirectory = @"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles";
            //var testDirectory = @"C:\Users\alyons2\Documents\My Dropbox\Stuff\Maestro\TestFiles";
            var testDirectory = @"C:\Users\alyons2\Music\Amazon MP3\Deftones\Koi No Yokan [Explicit]";
            var testDirectory2 = @"C:\Users\alyons2\Music\Amazon MP3\Manchester Orchestra\Mean Everything To Nothing";

            library.ClearLibrary();
            library.AddDirectoryToLibrary(testDirectory);
            library.AddDirectoryToLibrary(testDirectory2);
        }

       
    }
}
