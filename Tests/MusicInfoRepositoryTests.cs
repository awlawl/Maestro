using System.Collections.Generic;
using MusicData;
using NUnit.Framework;
using RealTimeMessaging;
using Rhino.Mocks;

namespace Tests
{
    [TestFixture]
    public class MusicInfoRepositoryTests
    {
        [Test]
        public void AddMusicToLibrary()
        {
            var music = new MusicInfo()
            {
                Album = "Test Album",
                Artist = "Test Artist",
                FullPath = "Test Path",
                Title = "Test Title"
            };

            var pubnubStub = MockRepository.GenerateStub<IRealTimeMessaging>();
            pubnubStub.Stub(X => X.SendSongsAdded(null));

            var repository = new MemoryLibraryRepository(pubnubStub);
            repository.ClearLibrary();

            repository.AddMusicToLibrary(new MusicInfo[] {music});

            List<MusicInfo> result = repository.GetAllMusic();

            Assert.IsNotNull(result, "The result must not be null.");
            Assert.AreEqual(1, result.Count, "There must only be one song.");
            Assert.AreEqual(music.Album, result[0].Album, "The result must be for the correct album.");
            Assert.AreEqual(music.Artist, result[0].Artist, "The artist must be correct.");
            Assert.AreEqual(music.Title, result[0].Title, "The title must be correct.");
            Assert.AreEqual(music.FullPath, result[0].FullPath, "The full path must be correct.");

            pubnubStub.AssertWasCalled(X => X.SendSongsAdded(null), Y => Y.IgnoreArguments());
        }

        [Test]
        public void AddFolderToLibrary()
        {
            var testDirectory = @"..\..\..\TestFiles";
            var title = "one";

            var repository = new MemoryLibraryRepository();
            repository.ClearLibrary();

            repository.AddDirectoryToLibrary(testDirectory);

            List<MusicInfo> result = repository.GetAllMusic();

            Assert.IsNotNull(result, "The result must not be null.");
            Assert.Greater(result.Count, 0, "There must many songs.");
            Assert.AreEqual(title, result[0].Title, "The title must be correct.");
        }


        [Test]
        public void MemoryLibraryShouldBeSingleton()
        {
            var music = new MusicInfo()
            {
                Album = "Test Album",
                Artist = "Test Artist",
                FullPath = "Test Path",
                Title = "Test Title"
            };

            var repository = new MemoryLibraryRepository();
            repository.ClearLibrary();

            repository.AddMusicToLibrary(new MusicInfo[] { music });

            List<MusicInfo> result = repository.GetAllMusic();

            Assert.IsNotNull(result, "The result must not be null.");
            Assert.AreEqual(1, result.Count, "There must only be one song.");

            var repository2 = new MemoryLibraryRepository();

            music.FullPath += 2;

            repository2.AddMusicToLibrary(new MusicInfo[] { music });
            result = repository2.GetAllMusic();

            Assert.AreEqual(2, result.Count, "There must two songs.");

        }
    }
}
