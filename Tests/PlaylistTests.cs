
using MusicData;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PlaylistTests
    {
        [Test]
        public void SimpleStart()
        {
            string filename = "song1";
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);
            
            playlist.Enqueue(filename);

            Assert.AreEqual(1, playlist.Count, "There must be one song in the playlist.");

            string song = playlist.GetNextSong();

            Assert.AreEqual(filename, song, "The next song must be the one that was enqueued.");
            Assert.AreEqual(1, dummyPlaylistWatcher.PlayHistory.Count, "Only one song must have been played.");
            Assert.AreEqual(filename, dummyPlaylistWatcher.PlayHistory[0], "The song that played was ");
            
        }
        
        [Test]
        public void WhenThePlayListIsEmpty()
        {
            string filename = "song1";
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);
            
            Assert.IsFalse(playlist.AreSongsAvailable(), "There must not be any songs available.");
            
        }
    }
}
