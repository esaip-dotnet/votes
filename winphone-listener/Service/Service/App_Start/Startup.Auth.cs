using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Service.Providers;
using Service.Models;

namespace Service
{
  public partial class Startup
  {
    public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
    public static string PublicClientId { get; private set; }

    public void ConfigureAuth(IAppBuilder app)
    {
      // Configure context data base and User Manager to use a single instance per application
      app.CreatePerOwinContext(ApplicationDbContext.Create);
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      // Enable the application to use a cookie to store the user information logged
      // and use a cookie to temporarily store information about a user who connects with third connection provider
      app.UseCookieAuthentication(new CookieAuthenticationOptions());
      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      // Configure the application to the flow based on OAuth
      PublicClientId = "self";
      OAuthOptions = new OAuthAuthorizationServerOptions
      {
        TokenEndpointPath = new PathString("/Token"),
        Provider = new ApplicationOAuthProvider(PublicClientId),
        AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
        AllowInsecureHttp = true
      };

      // Enable application to use the carrier of tokens to authenticate users
      app.UseOAuthBearerTokens(OAuthOptions);

      // Uncomment the following lines to enable connection with third connection providers
      /*  app.UseMicrosoftAccountAuthentication(
          clientId: "",
          clientSecret: "");

      app.UseTwitterAuthentication(
          consumerKey: "",
          consumerSecret: "");

      app.UseFacebookAuthentication(
          appId: "",
          appSecret: "");

      app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
      {
          ClientId = "",
          ClientSecret = ""
      });*/
    }
  }
}
