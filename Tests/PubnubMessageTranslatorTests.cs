using System.Collections.Generic;
using NUnit.Framework;
using RealTimeMessaging;

namespace Tests
{
    [TestFixture]
    public class PubnubMessageTranslatorTests
    {
        [Test]
        public void SimpleTranslation()
        {
            var rawNubs = new Dictionary<string, object>();
            rawNubs.Add("action", PubnubMessage.ACTION_PLAY);

            var expected = new PubnubMessage()
                {
                    action = PubnubMessage.ACTION_PLAY
                };

            var pubnub = new PubnubMessaging(null, false);
            PubnubMessage result = pubnub.Translate(rawNubs);

            Assert.AreEqual(expected.action, result.action, "The action must be correct.");

        }
        
    }
}
