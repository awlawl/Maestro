using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MusicData;

namespace Tests
{
    [TestFixture]
    public class RandomSongPlaylistWatcherTests_Attach
    {
        private DummyAudioInteractor _dummyAudio;
        private Playlist _playlist;
        private MusicInfo[] _songs;

        [SetUp]
        public void Test()
        {
            var song1 = "song1";
            var song2 = "song2";
            var song3 = "song3";
            var song4 = "song4";

            _songs = new MusicInfo[] { 
                    new MusicInfo() { FullPath = song1 }, 
                    new MusicInfo() { FullPath = song2 },
                    new MusicInfo() { FullPath = song3 },
                    new MusicInfo() { FullPath = song4 }
                };

            var library = new MemoryLibraryRepository();
            library.ClearLibrary();
            library.AddMusicToLibrary(_songs);

            var loopingWatcher = new RandomSongPlaylistWatcher(2);
            _playlist = new Playlist(loopingWatcher);
            _dummyAudio = new DummyAudioInteractor();

            var player = new Player(_playlist, _dummyAudio, library);

            loopingWatcher.AttachToPlaylist(_playlist, library);

            player.MaxPlayCount = 3;
            player.Play();
        }

        [Test]
        public void PlaylistShouldHaveFourItems()
        {
            Assert.AreEqual(4, _playlist.Count);
        }
    }
}
