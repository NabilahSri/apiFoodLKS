using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LKSApi.Models
{
    public class Authorization : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserRepository _repo = new UserRepository())
            {
                var user = _repo.ValidateUser(context.UserName, context.Password);
                if(user == null)
                {
                    context.SetError("Invalid", "Username & Password is incorrect");
                    return;
                }

                var identify = new ClaimsIdentity(context.Options.AuthenticationType);
                identify.AddClaim(new Claim("username", user.username));

                context.Validated(identify);
            }
        }
    }
}