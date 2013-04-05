using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using RestSharp;

namespace KeBot.Service
{
    public class GroupmeApi
    {
        private readonly RestClient _v3Client;
        private readonly RestClient _v2Client;
        private readonly RestClient _defaultClient;
        private readonly string _token;

        public GroupmeApi(ApiVersion version)
        {
            string v3Url = ConfigurationManager.AppSettings.Get("V3_Url");
            string v2Url = ConfigurationManager.AppSettings.Get("V2_Url");
            _v3Client = new RestClient(v3Url);
            _v2Client = new RestClient(v2Url);

            _defaultClient = GeClientByVersion(version);
            _token = ConfigurationManager.AppSettings.Get("token");
        }

        public GroupmeApi()
            : this(ApiVersion.V3)
        {
        }

        public string GetGroups()
        {
            var result = SendJsonRequest("/groups", Method.GET);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error in getting group: " + result.Content);
            }
            return result.Content;
        }

        public string CreateBot(string groupId)
        {
            int intGroupId;
            if (!int.TryParse(groupId, out intGroupId))
            {
                throw new Exception("Cannot parse group number " + groupId);
            }
            var bot = new
                              {
                                  bot =
                                      new
                                          {
                                              name = "KeBot",
                                              group_id = intGroupId
                                          }
                              };
            var result = SendJsonRequest("/bots", Method.POST, body: bot, version: ApiVersion.V3);
            if (result.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception("Error in creating bot: " + result.Content);
            }
            return result.Content;
        }

        public string GetMessages(string groupId, string lastMessageId = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(lastMessageId))
            {
                parameters.Add("since_id", lastMessageId);
            }
            var result = SendJsonRequest("/groups/" + groupId + "/messages/", Method.GET, parameters: parameters, version: ApiVersion.V3);

            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.NotModified)
            {
                throw new Exception("Error in getting group messages: " + result.Content);
            }

            return result.Content;
        }

        public string BotPost(string botId, string message)
        {
            var input = new
                            {
                                bot_id = botId,
                                text = message
                            };
            var result = SendJsonRequest("/bots/post", Method.POST, body: input, version: ApiVersion.V3);
            if (result.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception("Error in creating bot message: " + result.Content);
            }
            return result.Content;
        }

        private RestResponse SendJsonRequest(string resource, Method method, object body = null, Dictionary<string, object> parameters = null, ApiVersion? version = null)
        {
            var request = new RestRequest(resource, method) { RequestFormat = DataFormat.Json };
            request.AddHeader("X-Access-Token", _token);

            if (body != null)
                request.AddBody(body);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            RestClient client = version.HasValue ? GeClientByVersion(version.Value) : _defaultClient;
            var response = (RestResponse)client.Execute(request);

            return response;
        }

        private RestClient GeClientByVersion(ApiVersion version)
        {
            RestClient client;
            switch (version)
            {
                case ApiVersion.V2:
                    client = _v2Client;
                    break;
                case ApiVersion.V3:
                    client = _v3Client;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return client;
        }
    }

    public enum ApiVersion
    {
        V2,
        V3
    }
}
