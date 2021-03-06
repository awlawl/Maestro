﻿using MusicData;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PlayerTests
    {
        [Test]
        public void PlayerShouldPlayTheNextSongWhenTheSongFinishes()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song1 = "song1";
            var song2 = "song2";
            var music1 = new MusicInfo() { FullPath = song1 };
            var music2 = new MusicInfo() { FullPath = song2 };

            playlist.Enqueue(music1);
            playlist.Enqueue(music2);

            player.Play();
            
            Assert.AreEqual(2, dummyAudio.PlayHistory.Count, "There must be two songs that were played.");
            Assert.AreEqual(song1, dummyAudio.PlayHistory[0], "The first song that was played must be the right one.");
            Assert.AreEqual(song2, dummyAudio.PlayHistory[1], "The second song that was played must be the right one.");

        }

        [Test]
        public void PlayerShouldDoNothingIfThePlaylistIsEmpty()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);
            
            player.Play();

            Assert.AreEqual(0, dummyAudio.PlayHistory.Count, "There must be not be any songs played.");
        }

        [Test]
        public void PlayerShouldStopIfThePlaylistRunsOut()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song = "song1";
            var music = new MusicInfo() { FullPath = song };

            playlist.Enqueue(music);

            player.Play();
            
            Assert.AreEqual(1, dummyAudio.PlayHistory.Count, "There must be one song that was played.");
            Assert.AreEqual(song, dummyAudio.PlayHistory[0], "The song that was played must be the right one.");
        }

        [Test]
        public void PlayerShouldBePausableAndResumeable()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song = "song1";
            var music = new MusicInfo() { FullPath = song };

            playlist.Enqueue(music);

            player.Play();
            player.Pause();
            player.Resume();

            Assert.IsTrue(dummyAudio.WasPaused, "The audio must have been paused.");
            Assert.IsTrue(dummyAudio.WasResumed, "The audio must have been resumed.");

        }

        [Test]
        public void ResumingWhileNotPausedDoesNothing()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song = "song1";
            var music = new MusicInfo() { FullPath = song };

            playlist.Enqueue(music);

            player.Play();
            player.Resume();

            Assert.IsFalse(dummyAudio.WasResumed, "It should not resume if it wasn't paused.");
        }

        [Test]
        public void PlayerShouldBeStoppable()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song = "song1";
            var music = new MusicInfo() { FullPath = song };

            playlist.Enqueue(music);


            player.Play();
            player.Stop();

            Assert.IsTrue(dummyAudio.WasStopped, "It should stop.");
        }
        
        [Test]
        public void PlayerShouldHaveASingletonishInstance()
        {
            var player = new Player(null, null, null);

            Assert.AreEqual(player, Player.Current, "The global Current player must be the same as the one just created.");
        }

        [Test]
        public void PlayerCanGoBack()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song1 = new MusicInfo() { FullPath = "song1" };
            var song2 = new MusicInfo() { FullPath = "song2" };

            playlist.Enqueue(song1);
            playlist.Enqueue(song2);

            player.MaxPlayCount = 2;
            player.Play();

            Assert.AreEqual(1, playlist.CurrentPosition, "The song must have moved one song ahead.");

            player.Back();

            Assert.AreEqual(0, playlist.CurrentPosition, "The song must have moved one song back.");
            
        }

        [Test]
        public void PlayerCanBeGivenANewPlaylist()
        {
            var library = new MemoryLibraryRepository();
            var playlist = new Playlist();
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio, null);

            var song = "song1";

            library.ClearLibrary();
            library.AddMusicToLibrary(new MusicInfo[] { new MusicInfo() { FullPath = song } });

            playlist.AddRange(library.GetAllMusic());

            player.Play();

            Assert.AreEqual(song, playlist.CurrentSong.FullPath, "The last song played must be the only one in the library.");

            var song2 = "song 2";

            library.ClearLibrary();
            library.AddMusicToLibrary(new MusicInfo[] { new MusicInfo() { FullPath = song2 } });

            playlist.AddRange(library.GetAllMusic());

            player.PlayCount = 0;
            player.Play();

            Assert.AreEqual(song, playlist.PreviousSong.FullPath, "The previous played must be new song in the library.");
            Assert.AreEqual(song2, playlist.CurrentSong.FullPath, "The current played must be new song in the library.");
        }
    }
}
