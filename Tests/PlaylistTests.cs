
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
            var music = new MusicInfo() { FullPath = filename};
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);

            playlist.Enqueue(music);

            Assert.AreEqual(1, playlist.Count, "There must be one song in the playlist.");

            MusicInfo song = playlist.GetNextSong();

            Assert.AreEqual(filename, song.FullPath, "The next song must be the one that was enqueued.");
            Assert.AreEqual(1, dummyPlaylistWatcher.PlayHistory.Count, "Only one song must have been played.");
            Assert.AreEqual(filename, dummyPlaylistWatcher.PlayHistory[0], "The song that played was ");
            
        }

        [Test]
        public void SimpleStartTwoWatchers()
        {
            string filename = "song1";
            var music = new MusicInfo() { FullPath = filename };
            var dummyPlaylistWatcher1 = new DummyPlaylistWatcher();
            var dummyPlaylistWatcher2 = new DummyPlaylistWatcher();
            var playlist = new Playlist(new IPlaylistWatcher[] { dummyPlaylistWatcher1, dummyPlaylistWatcher2});

            playlist.Enqueue(music);

            Assert.AreEqual(1, playlist.Count, "There must be one song in the playlist.");

            MusicInfo song = playlist.GetNextSong();

            Assert.AreEqual(filename, song.FullPath, "The next song must be the one that was enqueued.");
            Assert.AreEqual(1, dummyPlaylistWatcher1.PlayHistory.Count, "Only one song must have been played.");
            Assert.AreEqual(filename, dummyPlaylistWatcher1.PlayHistory[0], "The song that played was ");

            Assert.AreEqual(dummyPlaylistWatcher1.PlayHistory.Count, dummyPlaylistWatcher2.PlayHistory.Count, "Both watchers must have the same stufff.");

        }
        
        [Test]
        public void WhenThePlayListIsEmpty()
        {
            string filename = "song1";
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);
            
            Assert.IsFalse(playlist.AreSongsAvailable(), "There must not be any songs available.");
            
        }

        [Test]
        public void PlaylistsCanGoBack()
        {
            string filename = "song1";
            var music = new MusicInfo() { FullPath = filename };
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);

            playlist.Enqueue(music);

            Assert.AreEqual(1, playlist.Count, "There must be one song in the playlist.");

            MusicInfo song = playlist.GetNextSong();

            Assert.IsTrue(playlist.HistoryIsAvailable(), "There must be songs in the history.");

            MusicInfo backOneSong = playlist.GetLastSong();

            Assert.AreEqual(song, backOneSong, "The last song must be the same as the first.");
        }
    }
}
