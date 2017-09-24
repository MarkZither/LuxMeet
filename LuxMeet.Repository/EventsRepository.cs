using System;
using LuxMeet.EFCoreTemp;
using System.Linq;
using LuxMeet.DbModels;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Polly;
using System.Threading.Tasks;
using System.Net;

namespace LuxMeet.Repository
{
    //private readonly  Eve
    public class EventsRepository
    {
        private SqliteConnection connection { get; set; }
        public string accessToken { get; set; }

        public EventsRepository(SqliteConnection _connection)
        {
            connection = _connection;
        }

        protected static SqliteConnection GetSQLiteConnection(bool open = true)
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            if (open)
            {
                connection.Open();
            }
            return connection;
        }

        public async Task<Event> GetRemote()
        {
            //http://groupspaces.com/LuxembourgLGBTAllyNetwork/api/events/1134126

            //var request = new HttpRequestMessage()
            //{
            //    RequestUri = new Uri("https://api.tidyhq.com/v1/events/16139"),
            //    Method = HttpMethod.Get,
            //};
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "d8cfc758860187bc6e0cc76e33d6d10b65e505e2d1c9864a40d7c91f268131ec"); 
            var policy = Policy
                  .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
            .RetryAsync(1, (exception, retryCount) =>
            {
                GetTidyHQAuthToken();
            });

            HttpResponseMessage result = await policy.ExecuteAsync(GetEventFromTidyHQ); // retry once
            HttpResponseMessage response = await GetEventFromTidyHQ();
            var obj = JsonConvert.DeserializeObject<Event>(
                response.Content.ReadAsStringAsync().Result);
            var obj2 = JsonConvert.DeserializeObject<Event>(
                result.Content.ReadAsStringAsync().Result);
            return obj2;
        }

        private async Task<RequestTokenObject> GetTidyHQAuthToken()
        {
            /*{ "client_id": "", "client_secret": "", 
             * "username": "", "password": "", 
             * "grant_type": "password","domain_prefix": ""}*/
            //if (string.IsNullOrWhiteSpace(code))
            //    return null;
            string ClientId = Environment.GetEnvironmentVariable("TIDYHQ_CLIENTID");
            string ClientSecret = Environment.GetEnvironmentVariable("TIDYHQ_CLIENTSECRET");
            string Username = Environment.GetEnvironmentVariable("TIDYHQ_USERNAME");
            string Password = Environment.GetEnvironmentVariable("TIDYHQ_PASSWORD");
            string Domain = Environment.GetEnvironmentVariable("TIDYHQ_DOMAIN");

            string RedirectUrl = "";
            //using (var client = CreateClient())
            //{
            HttpClient client = new HttpClient();
            try
            {
                if (client.DefaultRequestHeaders.CacheControl == null)
                {
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
                }
                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                client.DefaultRequestHeaders.CacheControl.NoStore = true;
                client.Timeout = new TimeSpan(0, 0, 30);

                var content = new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("client_id", ClientId),
                            new KeyValuePair<string, string>("client_secret", ClientSecret),
                            new KeyValuePair<string, string>("username", Username),
                            new KeyValuePair<string, string>("password", Password),
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("redirect_uri", RedirectUrl),
                            new KeyValuePair<string, string>("domain_prefix", Domain),
                        });

                var result = await client.PostAsync("https://accounts.tidyhq.com/oauth/token", content);
                var response = await result.Content.ReadAsStringAsync();
                var refreshResponse = await DeserializeObjectAsync<RequestTokenObject>(response);
                return refreshResponse;
            }
            catch (Exception ex)
            {
                //if (Settings.Insights)
                //    Xamarin.Insights.Report(ex);
            }

            //}

            return null;
        }

        private async Task<HttpResponseMessage> GetEventFromTidyHQ()
        {
            HttpClient httpClient = new HttpClient();
            
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await httpClient.GetAsync("https://api.tidyhq.com/v1/events/16139");
            return response;
        }

        public IQueryable<Event> GetEvents()
        {
            //using (var connection = GetSQLiteConnection())
            //{
                SqlMapper.ResetTypeHandlers();
            //var Item = connection.Query<Item>("select * from Author where id = @id", new { id = 1 });
            var Events = connection.Query<Event>("select * from Events").AsQueryable();

            //}
            return Events;
        }

        public Task<T> DeserializeObjectAsync<T>(string value)
        {
            return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
        }
    }

    public interface IEventsRepository
    {
        IQueryable<Event> GetItems();
        void GetRemote();
    }

    public class RequestTokenObject
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }
    }
}
