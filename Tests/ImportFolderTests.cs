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
    public class ImportFolderTests
    {
        [Test]
        public void NoFiles()
        {
            var importFolderInteractor = new FakeFolderInteractor(new string[0],"");
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, null, null);

            importFolderWatcher.ProcessFiles();

            Assert.AreEqual(0, importFolderInteractor.MovedFiles.Count(), "There must not be any moved files");
        }

        [Test]
        public void OneMp3file()
        {
            var fakeFiles = new string[] { "one.mp3" };
            var fakeMusicInfo = new MusicInfo() {
                Album = "onealbum",
                Artist = "oneartist"
            };

            var fakeNewPath = "ASDASDASDASDASD";
            var fakeMusicInfoReader = new FakeMusicInfoReader(fakeMusicInfo);
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles, fakeNewPath);
            var library = new MemoryLibraryRepository();
            library.ClearLibrary();
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, fakeMusicInfoReader, library);

            importFolderWatcher.ProcessFiles();

            Assert.AreEqual(1, importFolderInteractor.MovedFiles.Count(), "There must one moved file");
            Assert.AreEqual(fakeFiles[0], importFolderInteractor.MovedFiles[0].FileName, "The file to move must be correct.");
            Assert.AreEqual(fakeMusicInfo.Artist, importFolderInteractor.MovedFiles[0].ArtistFolder, "The file must be moved to the correct artist folder.");
            Assert.AreEqual(fakeMusicInfo.Album, importFolderInteractor.MovedFiles[0].AlbumFolder, "The file must be moved to the correct album folder.");

            var libraryFiles = library.GetAllMusic();

            Assert.AreEqual(1, libraryFiles.Count(), "There must be one file in the library.");
            Assert.AreEqual(fakeMusicInfo.Artist, libraryFiles[0].Artist, "The artist added to the library must be correct.");
            Assert.AreEqual(fakeNewPath, libraryFiles[0].FullPath, "The full path of the file in the library must be correct.");
        }

        [Test]
        public void OneMp3file_NoInfo()
        {
            var fakeFiles = new string[] { "one.mp3" };
            var fakeMusicInfo = new MusicInfo()
            {
                Album = "",
                Artist = ""
            };

            var fakeMusicInfoReader = new FakeMusicInfoReader(fakeMusicInfo);
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles,"");
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, fakeMusicInfoReader, new MemoryLibraryRepository());

            importFolderWatcher.ProcessFiles();

            Assert.AreEqual(1, importFolderInteractor.MovedFiles.Count(), "There must one moved file");
            Assert.AreEqual(fakeFiles[0], importFolderInteractor.MovedFiles[0].FileName, "The file to move must be correct.");
            //Assert.AreEqual("Unknown Artist", importFolderInteractor.MovedFiles[0].ArtistFolder, "The file must be moved to the correct artist folder.");
            Assert.AreEqual("Unknown Album", importFolderInteractor.MovedFiles[0].AlbumFolder, "The file must be moved to the correct album folder.");
        }

        [Test]
        public void UnknownFileType()
        {
            var fakeFiles = new string[] { "readme.txt" };
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles, "");
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, null, null);

            importFolderWatcher.ProcessFiles();

            Assert.AreEqual(0, importFolderInteractor.MovedFiles.Count(), "There must no moved files.");
        }



    }

    public class FakeFolderInteractor : IImportFolderInteractor
    {
        public FakeFolderInteractor(string[] fakeFiles, string fakeNewPath)
        {
            MovedFiles = new List<dynamic>();
            Files = fakeFiles;
            FakeNewPath = fakeNewPath;
        }

        public List<dynamic> MovedFiles { get; set; }
        public string[] Files { get; set; }
        public string FakeNewPath { get; set; }

        public string[] GetFilesForFolder(string folder)
        {
            return Files;
        }

        public string MoveToLibraryFolder(string file, string artist, string album)
        {
            MovedFiles.Add(new { FileName = file, AlbumFolder = album, ArtistFolder=artist });
            return FakeNewPath;
        }
    }

    public class FakeMusicInfoReader : IMusicInfoReader
    {
        private MusicInfo _musicInfo = null;

        public FakeMusicInfoReader(MusicInfo musicInfo)
        {
            _musicInfo = musicInfo;
        }

        public MusicInfo GetInfoForFile(string fullPath)
        {
            return _musicInfo;
        }
    }

}
