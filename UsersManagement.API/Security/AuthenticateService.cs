using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.API.Interface;
using UsersManagement.API.Models;
using UsersManagement.Domain.Home;

namespace UsersManagement.API.Security
{
    public class AuthenticateService : IAuthenticateServices
    {
        private readonly AppSettings appSettings;
        public AuthenticateService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }
        public async Task<User> Authenticate(string UserName, string Password)
        {
            //Get user details for the user who is trying to login - JRozario
            var user = UserList.SingleOrDefault(x => x.USERID == UserName);

            //Authenticate User, Check if its a registered user in DB  - JRozario
            if (user == null)
                return null;

            //If its registered user, check user password stored in DB - JRozario
            //For demo, password is not hashed. Its just a string comparison - JRozario
            //In reality, password would be hashed and stored in DB. Before comparing, hash the password - JRozario
            if (Password == user.PASSWORD)
            {
                //Authentication successful, Issue Token with user credentials - JRozario
                //Provide the security key which was given in the JWToken configuration in Startup.cs - JRozario
                var key = Encoding.ASCII.GetBytes(appSettings.Key);
                //Generate Token for user - JRozario
                var JWToken = new JwtSecurityToken(
                    issuer: appSettings.Issuer,
                    audience: appSettings.Issuer,
                    claims: GetUserClaims(user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    //Using HS256 Algorithm to encrypt Token - JRozario
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);


                user.Token = token;
                
                return user;
            }
            else
            {
                return null;
            }
        }

        List<User> UserList = new List<User>
        {
            new User { USERID = "jsmith@email.com", PASSWORD = "test", EMAILID = "jsmith@email.com", FIRST_NAME = "John", LAST_NAME = "Smith", PHONE = "356-735-2748", ACCESS_LEVEL = Roles.DIRECTOR.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "srob@email.com", PASSWORD = "test", FIRST_NAME = "Steve", LAST_NAME = "Rob", EMAILID = "srob@email.com", PHONE = "567-479-8537", ACCESS_LEVEL = Roles.SUPERVISOR.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "dwill@email.com", PASSWORD = "test", FIRST_NAME = "DJ", LAST_NAME = "Will", EMAILID = "dwill@email.com", PHONE = "599-306-6010", ACCESS_LEVEL = Roles.ANALYST.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "JBlack@email.com", PASSWORD = "test", FIRST_NAME = "Joe", LAST_NAME = "Black", EMAILID = "JBlack@email.com", PHONE = "764-460-8610", ACCESS_LEVEL = Roles.ANALYST.ToString(), WRITE_ACCESS = "" }
        };
        

        //Using hard coded collection list as Data Store for demo. In reality, User data comes from Database or some other Data Source - JRozario
        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, user.FIRST_NAME + " " + user.LAST_NAME);
            claims.Add(_claim);
            _claim = new Claim("USERID", user.USERID);
            claims.Add(_claim);
            _claim = new Claim("EMAILID", user.EMAILID);
            claims.Add(_claim);

            _claim = new Claim("PHONE", user.PHONE);
            claims.Add(_claim);
            _claim = new Claim(user.ACCESS_LEVEL, user.ACCESS_LEVEL);
            claims.Add(_claim);

            if (user.WRITE_ACCESS != "")
            {
                _claim = new Claim(user.WRITE_ACCESS, user.WRITE_ACCESS);
                claims.Add(_claim);
            }
            return claims.AsEnumerable<Claim>();
        }
    }
}
