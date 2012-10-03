using System.Linq;
using System.Collections.Generic;
using MusicData;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MusicInfoReaderTests
    {
        [Test]
        public void ReadTestMP3()
        {
            var testFilePath = @"..\..\..\TestFiles\one.mp3";
            var artist = "awl";
            var title = "one";
            var album = "TestAlbum";
            
            var musicInfoReader = new MusicInfoReader();

            MusicInfo result = musicInfoReader.GetInfoForFile(testFilePath);

            Assert.AreEqual(title, result.Title, "The title must be correct.");
            Assert.AreEqual(artist, result.Artist, "The artist must be correct.");
            Assert.AreEqual(album, result.Album, "It must be for the correct album.");
            Assert.AreEqual(testFilePath, result.FullPath,"The full path must be correct.");
            
        }

        [Test]
        public void GetMusicInfoForDirectory()
        {
            var testDirectory = @"..\..\..\TestFiles";
            int count = 3;

            var artist = "awl";
            var title = "one";
            var album = "TestAlbum";

            var musicInfoReader = new MusicInfoReader();

            List<MusicInfo> result = musicInfoReader.CrawlDirectory(testDirectory);

            Assert.Greater(result.Count, 0, "There must be more than one item in the result.");

            var oneMp3 = result.FirstOrDefault(X => X.FullPath.IndexOf("one.mp3") > -1);

            Assert.IsNotNull(oneMp3, "There must be an entry for one.mp3.");
            Assert.AreEqual("one",oneMp3.Title, "The title must be correct.");
        }

        
    }
}
