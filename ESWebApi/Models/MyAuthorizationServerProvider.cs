using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;
using System.Data;
using NLog;

namespace ESWebApi.Models
{

    public class MyAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserMasterRepository _repo = new UserMasterRepository())
            {
                var user = _repo.ValidateUser(context.UserName, context.Password);              
                if (user.username == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect" + user.emailid);
                    return;
}
                logger.Info(user.username + " generated token " + Environment.NewLine + DateTime.Now);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRoles));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.username));
                identity.AddClaim(new Claim("Email", user.emailid));
                context.Validated(identity);
            }
        }

    }
}