using System.Collections.Generic;

namespace MusicData
{
    public interface ILibraryRepository
    {
        void AddMusicToLibrary(MusicInfo[] song);
        void AddOrUpdateMusicInLibrary(MusicInfo song);
        List<MusicInfo> GetAllMusic();
        void AddDirectoryToLibrary(string directoryPath);
        void ClearLibrary();
        List<MusicInfo> SearchLibrary(string term);
        MusicInfo GetSongById(string id);
        List<SavedPlaylist> GetAllSavedPlaylists();
        List<MusicInfo> GetAllSongsForSavedPlaylist(string savedPlaylistName);
        SavedPlaylist AddNewSavedPlaylist(string namesavedPlaylistName);
        void AddSongToSavedPlaylist(string savedPlaylistName, string songId);
        void RemoveSongFromSavedPlaylist(string savedPlaylistName, string songId);

    }
}
