using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CrossCutting.Spotify
{
    public class SpotifyAPI
    {
        private string urlCredentials = "https://accounts.spotify.com/api/token";
        private string urlBase = "https://api.spotify.com/v1/browse/categories";

        public SpotifyAPI()
        {
            Client = new HttpClient();
        }

        private HttpClient Client { get; }

        private void GetCredentials()
        {
            var credentials = new SpotifyCredentials()
            {
                ClientId = "ab7872cd1d2a40389813bc8abb42d741",
                ClientSecret = "0ece7c65118047639a4b598e7874f47b"
            };

            //string stringData = JsonConvert.SerializeObject(credentials);
            //var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            //var requestUri = new Uri(urlCredentials);
            //HttpResponseMessage responseMessage = Client.PostAsync(requestUri, contentData).Result;

            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    var content = responseMessage.Content.ReadAsStringAsync().Result;
            //    var token = JsonConvert.DeserializeObject(content);
            //    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            //}
            //else
            //{
            //    throw new InvalidOperationException("Erro ao requisitar token de acesso.");
            //}

            //request to get the access token
            var encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", credentials.ClientId, credentials.ClientSecret)));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlCredentials);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encode_clientid_clientsecret);

            var request = ("grant_type=client_credentials");
            byte[] req_bytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = req_bytes.Length;

            Stream strm = webRequest.GetRequestStream();
            strm.Write(req_bytes, 0, req_bytes.Length);
            strm.Close();

            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();

            String resp_str = "";

            using (Stream respStr = resp.GetResponseStream())
            {
                using (StreamReader rdr = new StreamReader(respStr, Encoding.UTF8))
                {
                    //should get back a string i can then turn to json and parse for accesstoken
                    resp_str = rdr.ReadToEnd();
                    rdr.Close();
                }
            }

            JObject json = JObject.Parse(resp_str);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", json["access_token"].ToString());         
        }

        public async Task<List<Playlist>> GetPlaylistsByGenre(string filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                    throw new InvalidOperationException("Genre of playlist is required");

                var playLists = new List<Playlist>();

                this.GetCredentials();

                var listInt = new List<int>();

                Random randNum = new Random();
                for (int i = 0; i <= 1; i++)
                    listInt.Add(randNum.Next(50));

                Client.BaseAddress = new Uri(urlBase + $@"/{filter}/playlists?limit=" + listInt.LastOrDefault());

                HttpResponseMessage response = await Client.GetAsync(Client.BaseAddress);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Error: " + response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var playlists = json["playlists"]["items"].Children();

                    string urlTrack = "";

                    urlTrack = playlists.LastOrDefault()["tracks"]["href"].ToString();

                    //foreach (var item in playlists)
                    //{
                    //    urlTrack = item["tracks"]["href"].ToString();
                    //}

                    var ClientTrack = new HttpClient();

                    ClientTrack.DefaultRequestHeaders.Authorization = Client.DefaultRequestHeaders.Authorization;
                    ClientTrack.BaseAddress = new Uri(urlTrack);

                    response = await ClientTrack.GetAsync(ClientTrack.BaseAddress);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException("Erro: " + response.StatusCode);
                    }
                    else
                    {
                        content = await response.Content.ReadAsStringAsync();
                        json = JObject.Parse(content);
                        playlists = json["items"].Children();

                        foreach (var item in playlists)
                        {
                            var playList = new Playlist()
                            {
                                Name = urlTrack = item["track"]["album"]["name"].ToString()
                            };

                            playLists.Add(playList);
                        }
                        //var itrm = JsonSerializer.Deserialize<string>(content);
                    }

                    ClientTrack.Dispose();
                }

                Client.Dispose();

                return playLists;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
