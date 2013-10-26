using System.Collections.Generic;

namespace MusicData
{
    public interface ILibraryRepository
    {
        void AddMusicToLibrary(MusicInfo[] song);
        List<MusicInfo> GetAllMusic();
        void AddDirectoryToLibrary(string directoryPath);
        void ClearLibrary();
        List<MusicInfo> SearchLibrary(string term);
        MusicInfo GetSongById(string id);
        List<SavedPlaylist> GetAllSavedPlaylists();
        List<MusicInfo> GetAllSongsForSavedPlaylist(string savedPlaylistName);
        void AddNewSavedPlaylist(string namesavedPlaylistName);
        void AddSongToSavedPlaylist(string savedPlaylistName, string songId);

    }
}
