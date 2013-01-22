
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

            MusicInfo song = playlist.CurrentSong;
            playlist.CurrentSongIsStarting();

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
            MusicInfo song = playlist.CurrentSong;
            playlist.CurrentSongIsStarting();

            Assert.AreEqual(filename, song.FullPath, "The next song must be the one that was enqueued.");
            Assert.AreEqual(1, dummyPlaylistWatcher1.PlayHistory.Count, "Only one song must have been played.");
            Assert.AreEqual(filename, dummyPlaylistWatcher1.PlayHistory[0], "The song that played was ");

            Assert.AreEqual(dummyPlaylistWatcher1.PlayHistory.Count, dummyPlaylistWatcher2.PlayHistory.Count, "Both watchers must have the same stufff.");

        }
        
        [Test]
        public void WhenThePlayListIsEmpty()
        {
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);

            Assert.IsFalse(playlist.AreMoreSongsAvailable(), "There must not be any songs available.");
        }

        [Test]
        public void PlaylistsCanGoBack()
        {
            string filename1 = "song1";
            string filename2 = "song1";
            var music1 = new MusicInfo() { FullPath = filename1 };
            var music2 = new MusicInfo() { FullPath = filename2 };
            var dummyPlaylistWatcher = new DummyPlaylistWatcher();
            var playlist = new Playlist(dummyPlaylistWatcher);

            playlist.Enqueue(music1);
            playlist.Enqueue(music2);

            Assert.AreEqual(2, playlist.Count, "There must be two songs in the playlist.");

            MusicInfo song = playlist.CurrentSong;
            Assert.AreEqual(filename1, song.FullPath, "The first item in the playlist must be correct.");
            
            playlist.MoveToNextSong();
            
            MusicInfo nextSong = playlist.CurrentSong;
            Assert.AreEqual(filename2, nextSong.FullPath, "The second item in the playlist must be correct.");
            
            playlist.MoveBackOneSong();
            Assert.AreEqual(filename1, playlist.CurrentSong.FullPath, "Going back one song should go back to the first one.");
            
        }
    }
}
