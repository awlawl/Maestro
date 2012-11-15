using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using MusicData;
using PubNub_Messaging;

namespace RealTimeMessaging
{
    public class PubnubMessaging : IRealTimeMessaging
    {
        private string PUBLISH_KEY = "demo";
        private string SUBSCRIBE_KEY = "demo";
        private string SECRET_KEY = "";
        private string channel = "maestrotest";

        private IPlayer _player = null;
        private bool _doThreading = false;
        private Pubnub _pubnub;

        public PubnubMessaging(IPlayer player, bool doThreading)
        {
            _player = player;
            _doThreading = doThreading;
        }

        public void StartListening()
        {
            _pubnub = new Pubnub(PUBLISH_KEY, SUBSCRIBE_KEY);
            _pubnub.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                    if (e.PropertyName=="ReturnMessage") {
                        Log.Debug("Got Message");
                        HandleMessage((Dictionary<string, object>)((Pubnub)sender).ReturnMessage);
                    }
                
            };
            _pubnub.subscribe(channel);
        }


        

        public PubnubMessage Translate(Dictionary<string, object> rawNubs)
        {
            return new PubnubMessage()
                {
                    action = rawNubs["action"].ToString()
                };
        }

        public void HandleMessage(Dictionary<string, object> rawNubs)
        {
            PubnubMessage message = this.Translate(rawNubs);

            switch (message.action)
            {
                case PubnubMessage.ACTION_PLAY:
                    if (_doThreading)
                        (new Thread(new ThreadStart(_player.Play))).Start();
                    else
                        _player.Play();
                    break;
                    
                case PubnubMessage.ACTION_STOP:
                    _player.Stop();
                    break;
                    
                case PubnubMessage.ACTION_NEXT:
                     if (_doThreading)
                        (new Thread(new ThreadStart(_player.Next))).Start();
                    else
                        _player.Next();
                    break;

                case PubnubMessage.ACTION_BACK:
                    if (_doThreading)
                        (new Thread(new ThreadStart(_player.Back))).Start();
                    else
                        _player.Back();
                    break;
                
                default:
                    Log.Debug("Unknow action type: "+ message.action);
                    break;
                    
            }

        }

        public void SendMessage(PubnubMessage message)
        {
            _pubnub.publish(this.channel, message);
        }
    }
}
