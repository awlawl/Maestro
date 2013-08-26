using MusicData;
using NUnit.Framework;
using RealTimeMessaging;
using Rhino.Mocks;

namespace Tests
{
    [TestFixture]
    public class MessagingPlaylistWatcherTests
    {
        [Test]
        public void ShouldSendMessageWhenSongIsPlayed()
        {
            var library = new MemoryLibraryRepository();
            library.ClearLibrary();

            var song = new MusicInfo()
                {
                    Album = "Test album",
                    FullPath = "Test song",
                    Artist = "Test artist",
                    Title = "Test title"
                };

            library.AddMusicToLibrary(
                new MusicInfo[] { 
                    song
                });

            var pubnubStub = MockRepository.GenerateStub<IRealTimeMessaging>();
            pubnubStub.Stub(X => X.SendNowPlaying(null)).IgnoreArguments();

            var messagingWatcher = new MessagingPlaylistWatcher();
            messagingWatcher.AssignMessaging(pubnubStub);
            var loopingWatcher = new LoopingPlaylistWatcher();
            var playlist = new Playlist(new IPlaylistWatcher[] { loopingWatcher, messagingWatcher });
            var dummyAudio = new DummyAudioInteractor();

            var player = new Player(playlist, dummyAudio);

            loopingWatcher.AttachToPlaylist(playlist, library);
            messagingWatcher.AttachToPlaylist(playlist, library);

            playlist.AddRange(library.GetAllMusic());

            player.MaxPlayCount = 1;
            player.Play();

            var args = pubnubStub.GetArgumentsForCallsMadeOn(X => X.SendNowPlaying(null), Y => Y.IgnoreArguments());

            Assert.AreEqual(1, args.Count, "There must be one message.");

            var nowPlayingSong = (MusicInfo)args[0][0];

            Assert.AreEqual(song.Album, nowPlayingSong.Album, "The album must be correct.");
            Assert.AreEqual(song.Title, nowPlayingSong.Title, "The title must be correct.");
        }

        /*[Test]
        public void OMGWTF()
        {
            dynamic x = new {Album = "test"};

            Assert.AreEqual("test",x.Album);

            var message = new PubnubMessage()
                {
                    Action = PubnubMessage.ACTION_NOWPLAYING,
                    Data = new {Album = "test"}
                };

            Assert.AreEqual("test", message.Data.Album);
        }*/
    }
}
