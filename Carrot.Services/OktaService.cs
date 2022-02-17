using System;
using System.Collections.Generic;
using Carrot.Contracts.Common;
using Carrot.Contracts.Services;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Carrot.Contracts.DTOs;
using Carrot.Contracts.DTOs.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Carrot.Services
{
    public class OktaService : IOktaService
    {
        private readonly IOptions<OktaConfig> _oktaSettings;

        public OktaService(IOptions<OktaConfig> oktaSettings) => _oktaSettings = oktaSettings;

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", _oktaSettings.Value.APIKey);
                var getUsersUrl = $"{_oktaSettings.Value.RootUrl}/v1/apps/{_oktaSettings.Value.ClientId}/users";
                var request = new HttpRequestMessage(HttpMethod.Get, getUsersUrl);

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<User>>(json);
                    return users;
                }

                throw new ApplicationException("Unable to retrieve access token from Okta");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<string> GetOktaUser(string userId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", _oktaSettings.Value.APIKey);
                var getUsersUrl = $"{_oktaSettings.Value.RootUrl}/v1/users/{userId}";
                var request = new HttpRequestMessage(HttpMethod.Get, getUsersUrl);

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return json;
                }

                throw new ApplicationException("Unable to retrieve access token from Okta");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<ServiceResponseModel<bool>> UpdateUser(User user)
        {
            var ret = new ServiceResponseModel<bool>();
            try
            {
                var jsonUser = await GetOktaUser(user.Id);
                var oktaUser = JsonConvert.DeserializeObject<dynamic>(jsonUser) as dynamic;
                if (oktaUser != null && oktaUser.profile != null)
                {
                    var profile = oktaUser.profile;
                    profile.firstName = user.Profile.Given_name;
                    profile.lastName = user.Profile.Family_name;

                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", _oktaSettings.Value.APIKey);
                    var getUsersUrl = $"{_oktaSettings.Value.RootUrl}/v1/users/{user.Id}";
                    var request = new HttpRequestMessage(HttpMethod.Post, getUsersUrl)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                           profile
                        }, Formatting.None, new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }), Encoding.UTF8, "application/json")
                    };

                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var u = JsonConvert.DeserializeObject<User>(json);
                        ret.Data = u != null;
                        return ret;
                    }

                }

                throw new ApplicationException("Unable to save data to Okta");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
