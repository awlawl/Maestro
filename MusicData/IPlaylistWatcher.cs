
using System.Collections.Generic;

namespace MusicData
{
    public interface IPlaylistWatcher
    {
        void AttachToPlaylist(Playlist playlist);
        void PlaySong(string filename);
    }
}
