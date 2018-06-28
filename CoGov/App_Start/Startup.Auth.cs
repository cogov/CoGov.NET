using System;
using System.Web;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using CoGov.Models;
using Microsoft.Owin.Security.Facebook;
using Owin.Security.Providers.GroupUp;
using System.Net.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Twitter;

namespace CoGov
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //var twitterauthopts = new TwitterAuthenticationOptions
            //{
            //    ConsumerKey = "luMUwH9KwVWYpPxlR33qoGa6y",
            //    ConsumerSecret = "KugSrnE7otgzI7z2m6JHeyqmfzgvabaJ42yh9a8RtiNtG9bxgI",
            //    BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
            //    {
            //        "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
            //        "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
            //        "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
            //        "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
            //        "‎add53f6680fe66e383cbac3e60922e3b4c412bed", // Symantec Class 3 EV SSL CA - G3
            //        "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
            //        "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
            //        "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
            //    })
            //};

            //app.UseTwitterAuthentication(twitterauthopts);

            // Facebook -----------------------------------------------------------

            var facebookAuthenticationOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1407179975980935",
                AppSecret = "fa3ae8146697bcafa120bf396fa4b220",
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name"
            };
            facebookAuthenticationOptions.Scope.Add("email");
            facebookAuthenticationOptions.Scope.Add("public_profile");
            app.UseFacebookAuthentication(facebookAuthenticationOptions);

            // Google -----------------------------------------------------------

            //var authopts = new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "283493049430-mnsrql5lspqfcag6vi6ijakunlffbg8r.apps.googleusercontent.com",
            //    ClientSecret = "ffavIDPH94pNUtEYrUw5O-s5",
            //    Provider = new GoogleOAuth2AuthenticationProvider()
            //};
            //authopts.Scope.Add("email");
            //app.UseGoogleAuthentication(authopts);


            // GroupUp

            var groupUpAuthenticationOptions = new GroupUpAuthenticationOptions()
            {
                AppKey = "7775ea3941bd71874ce86ee3181c9f40",
                AppSecret = "c65e52a09ec64bb2744b96229d6199bc",
            };

            app.UseGroupUpAuthentication(groupUpAuthenticationOptions);

        }

        public class FacebookOauthResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        public class FacebookBackChannelHandler : HttpClientHandler
        {
            protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
                {
                    request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
                }

                var result = await base.SendAsync(request, cancellationToken);
                if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                    return result;

                // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
                var content = await result.Content.ReadAsStringAsync();
                var facebookOauthResponse = Startbutton.Library.JsonDeserialize<FacebookOauthResponse>(content);

                var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
                outgoingQueryString.Add(nameof(facebookOauthResponse.access_token), facebookOauthResponse.access_token);
                outgoingQueryString.Add(nameof(facebookOauthResponse.expires_in), facebookOauthResponse.expires_in + string.Empty);
                outgoingQueryString.Add(nameof(facebookOauthResponse.token_type), facebookOauthResponse.token_type);
                var postdata = outgoingQueryString.ToString();

                var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(postdata)
                };

                return modifiedResult;
                
                // Replace the RequestUri so it's not malformed

                //return await base.SendAsync(request, cancellationToken);
            }
        }
    }

}