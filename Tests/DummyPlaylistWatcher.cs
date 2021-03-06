﻿
using System.Collections.Generic;
using MusicData;

namespace Tests
{
    public class DummyPlaylistWatcher : IPlaylistWatcher
    {
        public List<string> PlayHistory { get; set; }

        public DummyPlaylistWatcher()
        {
            PlayHistory = new List<string>();
        }

        public void AttachToPlaylist(Playlist playlist, ILibraryRepository library)
        {
            throw new System.NotImplementedException();
        }

        public void SongStarting(MusicInfo filename)
        {
            PlayHistory.Add(filename.FullPath);
        }

        public void SongEnding(MusicInfo song)
        {
            
        }


        public void PlaylistChanged()
        {
        }
    }
}
