using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV21T1020546.Shop
{
    public class WebUserData
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Address { get; set; } = "";
        public string Province { get; set; } = "";
        public string Phone { get; set; } = "";
        public List<string>? Roles { get; set; }

        public ClaimsPrincipal CreatePrincipal()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(UserName), UserName),
                new Claim(nameof(DisplayName), DisplayName),
                new Claim(nameof(Photo), Photo),
                new Claim(nameof(Address), Address),
                new Claim(nameof(Phone), Phone),
                new Claim(nameof(Province), Province),
            };
            if (Roles != null)
                foreach(var role in Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
