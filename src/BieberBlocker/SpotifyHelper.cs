using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using BieberBlocker.Configuration;
using SpotifyAPI.Local;

namespace BieberBlocker
{
    internal class SpotifyHelper
    {
        readonly IList<string> blockedArtists = Enumerable.Empty<string>().ToList();

        public SpotifyHelper()
        {
            InitializeFromConfigurationFile();

            var connectionHandle = ConnectToSpotify();
            Task.WaitAll(connectionHandle);

            ConfigureEvents(connectionHandle.Result);
        }

        private void InitializeFromConfigurationFile()
        {
            var serviceConfigSection = ConfigurationManager.GetSection("doNotListenTo") as BlockedArtistsSection;

            if (serviceConfigSection == null) return;

            var serviceConfig = serviceConfigSection.Artists;
            for (var i = 0; i < serviceConfig.Count; i++)
                IgnoreArtist(serviceConfig[i].Name);
        }


        private void ConfigureEvents(SpotifyLocalAPI spotify)
        {
            spotify.OnTrackChange += Spotify_OnTrackChange;
            spotify.ListenForEvents = true;
        }

        private void Spotify_OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            if (e.NewTrack == null)
            {
                return;
            }

            if (blockedArtists.FirstOrDefault(t => e.NewTrack.ArtistResource.Name.ToLowerInvariant().Contains(t)) != null)
            {
                Console.WriteLine(e.NewTrack.ArtistResource.Name + " tries to sing but fails");
                var spotifyLocalApi = sender as SpotifyLocalAPI;
                if (spotifyLocalApi != null)
                    spotifyLocalApi.Skip();
            }
            else
            {
                Console.WriteLine("Track: {0} - {1}", e.NewTrack.ArtistResource.Name, e.NewTrack.TrackResource.Name);
            }
        }

        private void IgnoreArtist(string artistName)
        {
            if (blockedArtists.Contains(artistName))
                return;
            blockedArtists.Add(artistName);
        }

        private static async Task<SpotifyLocalAPI> ConnectToSpotify()
        {
            var spotify = new SpotifyLocalAPI();

            while (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                await Task.Delay(1000);
            }

            bool successful = spotify.Connect();
            if (successful)
                Console.WriteLine("Connected to spottify");
            else
                throw new NotSupportedException("Cannot connect to spotify");

            return spotify;
        }
    }
}