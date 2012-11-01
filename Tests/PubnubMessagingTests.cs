using System.Collections.Generic;
using MusicData;
using NUnit.Framework;
using RealTimeMessaging;
using Rhino.Mocks;

namespace Tests
{
    [TestFixture]
    public class PubnubMessagingTests
    {
        [Test]
        public void PlayMessage()
        {
            var rawNubs = new Dictionary<string, object>();
            rawNubs.Add("action", PubnubMessage.ACTION_PLAY);

            var playerStub = MockRepository.GenerateStub<IPlayer>();
            playerStub.Stub(X => X.Play());
            

            var pubnub = new PubnubMessaging(playerStub, false);
            
            pubnub.HandleMessage(rawNubs);

            playerStub.AssertWasCalled(X => X.Play());
        }

        [Test]
        public void StopMessage()
        {
            var rawNubs = new Dictionary<string, object>();
            rawNubs.Add("action", PubnubMessage.ACTION_STOP);

            var playerStub = MockRepository.GenerateStub<IPlayer>();
            playerStub.Stub(X => X.Stop());
            
            var pubnub = new PubnubMessaging(playerStub, false);

            pubnub.HandleMessage(rawNubs);

            playerStub.AssertWasCalled(X => X.Stop());
        }
    }
}
