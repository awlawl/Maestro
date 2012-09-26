﻿
using MusicData;
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
            var player = new Player(playlist, dummyAudio);

            var song1 = "song1";
            var song2 = "song2";
            playlist.Enqueue(song1);
            playlist.Enqueue(song2);

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
            var player = new Player(playlist, dummyAudio);
            
            player.Play();

            Assert.AreEqual(0, dummyAudio.PlayHistory.Count, "There must be not be any songs played.");
        }

        [Test]
        public void PlayerShouldStopIfThePlaylistRunsOut()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio);

            var song = "song1";
            playlist.Enqueue(song);

            player.Play();
            
            Assert.AreEqual(1, dummyAudio.PlayHistory.Count, "There must be one song that was played.");
            Assert.AreEqual(song, dummyAudio.PlayHistory[0], "The song that was played must be the right one.");
        }

        [Test]
        public void PlayerShouldBePausableAndResumeable()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio);

            var song = "song1";
            playlist.Enqueue(song);

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
            var player = new Player(playlist, dummyAudio);

            var song = "song1";
            playlist.Enqueue(song);

            player.Play();
            player.Resume();

            Assert.IsFalse(dummyAudio.WasResumed, "It should not resume if it wasn't paused.");
        }

        [Test]
        public void PlayerShouldBeStoppable()
        {
            var playlist = new Playlist(new DummyPlaylistWatcher());
            var dummyAudio = new DummyAudioInteractor();
            var player = new Player(playlist, dummyAudio);

            var song = "song1";
            playlist.Enqueue(song);

            player.Play();
            player.Stop();

            Assert.IsTrue(dummyAudio.WasStopped, "It should stop.");
        }
    }
}
