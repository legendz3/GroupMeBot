using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth2;
using RestSharp;

namespace KeBot.Site
{
    public class MainBot
    {
        public void Start()
        {
        }

        public static WebServerClient CreateClient()
        {
            var desc = GetAuthServerDescription();
            var client = new WebServerClient(desc, clientIdentifier: "xyz.apps.googleusercontent.com");
            client.ClientCredentialApplicator = ClientCredentialApplicator.PostParameter("some_password");
            return client;
        }

        public static AuthorizationServerDescription GetAuthServerDescription()
        {
            var authServerDescription = new AuthorizationServerDescription();
            authServerDescription.AuthorizationEndpoint = new Uri(@"https://accounts.google.com/o/oauth2/auth");
            authServerDescription.TokenEndpoint = new Uri(@"https://accounts.google.com/o/oauth2/token");
            authServerDescription.ProtocolVersion = ProtocolVersion.V20;
            return authServerDescription;
        }
    }
}