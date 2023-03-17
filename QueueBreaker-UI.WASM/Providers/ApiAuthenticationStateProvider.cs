using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
       // private readonly JwtSecurityTokenHandler _tokenHandler;
        public ApiAuthenticationStateProvider(ILocalStorageService localStorage
          //  , JwtSecurityTokenHandler tokenHandler
            )
        {
            _localStorage = localStorage;
          //  _tokenHandler = tokenHandler;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedToken = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var tokenContent = savedToken;
                //var tokenContent1 = _tokenHandler.ReadJwtToken(savedToken);
                //var expiry = tokenContent1.ValidTo;
                //if (expiry < DateTime.Now)
                //{
                //    await _localStorage.RemoveItemAsync("authToken");
                //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                //}

                //Get Claims from token and Build auth user object
                  //var claims1 = ParseClaims(tokenContent1);
                var claims = ParseClaimsFromJwt(tokenContent);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"jwt",ClaimTypes.Name,ClaimTypes.Role));
                //return authenticted person
                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task LoggedIn()
        {

            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
             //var tokenContent1 = _tokenHandler.ReadJwtToken(savedToken);
             //var claims1 = ParseClaims(tokenContent1);
            var tokenContent = savedToken;
            var claims = ParseClaimsFromJwt(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        //private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        //{
        //    var claims = tokenContent.Claims.ToList();
        //    claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        //    return claims;
        //}

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
           
            //claims.AddRange(keyValuePairs.Select(kvp => new Claim(ClaimTypes.Name, kvp.Value.ToString())));
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key.ToString(), kvp.Value.ToString())));
            //claims.Add(new Claim(ClaimTypes.Name,"" ));
            return claims;
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
