using MusicData;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class LoopingPlaylistTests
    {
        [Test]
        public void Loop()
        {
            var song1 = "song1";
            var song2 = "song2";

            var loopingWatcher = new LoopingPlaylistWatcher();
            var playlist = new Playlist(loopingWatcher);
            var dummyAudio = new DummyAudioInteractor();

            var player = new Player(playlist, dummyAudio);

            loopingWatcher.AddToLoop(song1);
            loopingWatcher.AddToLoop(song2);

            loopingWatcher.AttachToPlaylist(playlist);

            player.MaxPlayCount = 3;
            player.Play();      
            
            Assert.AreEqual(3, dummyAudio.PlayHistory.Count, "There must be three songs in the history.");
            Assert.AreEqual(song1, dummyAudio.PlayHistory[0], "The first song must play first.");
            Assert.AreEqual(song2, dummyAudio.PlayHistory[1], "The second song must play second.");
            Assert.AreEqual(song1, dummyAudio.PlayHistory[2], "The first song must play third.");

            Assert.AreEqual(2, playlist.Count, "After playing three songs there must still be 2 songs in the playlist.");
        }

    }
}
