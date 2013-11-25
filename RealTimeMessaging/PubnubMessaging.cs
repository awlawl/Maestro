using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Dynamic;
using MusicData;
using PubNub_Messaging;
using System.Net.NetworkInformation;

namespace RealTimeMessaging
{
    public class PubnubMessaging : IRealTimeMessaging
    {
        private string PUBLISH_KEY = "demo";
        private string SUBSCRIBE_KEY = "demo";
        private string SECRET_KEY = "";
        private string _channel;

        private IPlayer _player = null;
        private bool _doThreading = false;
        private Pubnub _pubnub;

        public PubnubMessaging(IPlayer player, bool doThreading)
        {
            _player = player;
            _doThreading = doThreading;
            _channel = System.Configuration.ConfigurationManager.AppSettings["PubNubChannel"];
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
            _pubnub.subscribe(_channel);
        }


        

        public PubnubMessage Translate(Dictionary<string, object> rawNubs)
        {
            var message = new PubnubMessage()
                {
                    action = rawNubs["action"].ToString(),
                    data = new ExpandoObject()
                };

            if (rawNubs.ContainsKey("data"))
            {
                var dictionary = (Dictionary<string, object>)rawNubs["data"];
                if ((dictionary!=null) && (dictionary.ContainsKey("PlaylistIndex")))
                    message.data.PlaylistIndex = int.Parse(dictionary["PlaylistIndex"].ToString());
            }

            return message;
        }

        public void HandleMessage(Dictionary<string, object> rawNubs)
        {
            PubnubMessage message = this.Translate(rawNubs);

            switch (message.action)
            {
                case PubnubMessage.ACTION_PLAY:
                    if (_player.IsPaused)
                    {
                        _player.Resume();
                    }
                    else
                    {
                        if (_doThreading)
                            (new Thread(new ThreadStart(_player.Play))).Start();
                        else
                            _player.Play();
                    }
                    break;
                    
                case PubnubMessage.ACTION_STOP:
                    _player.Stop();
                    break;
                    
                case PubnubMessage.ACTION_NEXT:
                     _player.Next();
                    break;

                case PubnubMessage.ACTION_BACK:
                    if (_doThreading)
                        (new Thread(new ThreadStart(_player.Back))).Start();
                    else
                        _player.Back();
                    break;

                case PubnubMessage.ACTION_PAUSE:
                    _player.Pause();
                    break;
                
                case PubnubMessage.ACTION_PING:
                    SendPingReply();
                    break;
                case PubnubMessage.ACTION_PLAY_FROM_PLAYLIST:
                    _player.JumpToPlaylistIndex(message.data.PlaylistIndex);
                    break;

                case PubnubMessage.ACTION_PING_REPLY: break; //we sent these, ignore them
                case PubnubMessage.ACTION_NOWPLAYING: break;
                case PubnubMessage.ACTION_ADDED_SONG_TO_LIBRARY: break;
                default:
                    Log.Debug("Unknown action type: "+ message.action);
                    break;
                    
            }

        }
        
        public void SendMessage(PubnubMessage message)
        {
            _pubnub.publish(_channel, message);
        }

        public void SendNowPlaying(MusicInfo song)
        {
            SendMessage(new PubnubMessage()
            {
                action = PubnubMessage.ACTION_NOWPLAYING,
                data = song
            });
        }

        public void SendPingReply()
        {
            SendNowPlaying(Player.Current.Playlist.CurrentSong);
        }

        public void SendSongsAdded(MusicInfo[] song)
        {
            PubnubMessage message;

            if (song.Length == 1)
            {
                message = new PubnubMessage()
                    {
                        action = PubnubMessage.ACTION_ADDED_SONG_TO_LIBRARY,
                        data = new
                            {
                                howMany = 1,
                                firstSong = song[0]
                            }
                        
                    };
            }
            else
            {
                message = new PubnubMessage()
                    {
                        action = PubnubMessage.ACTION_ADDED_SONG_TO_LIBRARY,
                        data = new
                            {
                                howMany = song.Length,
                                firstSong = song[0]
                            }
                        
                    };
            }
        }

        private string GetIP()
        {
            var props = NetworkInterface.GetAllNetworkInterfaces()
                .Where(Z => Z.OperationalStatus == OperationalStatus.Up)
                .SelectMany(X => X.GetIPProperties().UnicastAddresses)
                .Where(G => G.Address.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork)
                .ToList();
                
            var ips=props.Select(Y => Y.Address.ToString())
                .ToList();

            return ips.FirstOrDefault();
        }

    }
}
