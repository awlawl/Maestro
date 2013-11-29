using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace MusicData
{
    public class ImportFolderWatcher
    {
        private const string UNKNOWN_ALBUM_NAME = "Unknown Album";
        private const string UNKNOWN_ARTIST_NAME = "Unknown Artist";

        private IImportFolderInteractor _folderInteractor = null;
        private IMusicInfoReader _musicInfoReader = null;
        private ILibraryRepository _library = null;
        private string _folderToWatch = "";

        private Thread _workerThread = null;

        public ImportFolderWatcher(ILibraryRepository library) : this(new ImportFolderInteractor(), new MusicInfoReader(),library)
        {
            _folderToWatch = System.Configuration.ConfigurationManager.AppSettings["ImportWatchFolder"];

        }

        public ImportFolderWatcher(IImportFolderInteractor folder, IMusicInfoReader musicInfoReader, ILibraryRepository library)
        {
            _folderInteractor = folder;
            _musicInfoReader = musicInfoReader;
            _library = library;
        }

        public void ProcessFiles()
        {
            var files = _folderInteractor.GetFilesForFolder(_folderToWatch);
            foreach (var file in files)
            {
                if (!IsSupportedFileType(file))
                {
                    continue; 
                }

                var musicInfo = _musicInfoReader.GetInfoForFile(file);
                var album = string.IsNullOrEmpty(musicInfo.Album) ? UNKNOWN_ALBUM_NAME : musicInfo.Album;
                var artist = string.IsNullOrEmpty(musicInfo.Artist) ? UNKNOWN_ARTIST_NAME : musicInfo.Artist;

                musicInfo.FullPath = _folderInteractor.MoveToLibraryFolder(file, musicInfo.Artist,album);
                _library.AddMusicToLibrary(new MusicInfo[] { musicInfo });
            }
        }

        private bool IsSupportedFileType(string filePath)
        {
            var extention = Path.GetExtension(filePath).ToLower();
            var allowedExtentions = new string[] { ".mp3",".ogg"};
            return allowedExtentions.Contains(extention);
        }

        private void WatchingWorkerThread()
        {
            Log.Debug("Now watching " + _folderToWatch);

            while (true)
            {
                Thread.Sleep(3000);
                try
                {
                    ProcessFiles();
                }
                catch (Exception exc)
                {
                    Log.Debug("Error while watching: " + exc.ToString());
                    Thread.Sleep(60 * 1000);
                }
            }
        }

        public void Start()
        {
            _workerThread = new Thread(new ThreadStart(WatchingWorkerThread));
            _workerThread.Name = "WatchFolderWorker";
            _workerThread.Start();
        }

        public void Stop()
        {
            try
            {
                _workerThread.Abort();
            }
            finally { }
        }


    }
}
