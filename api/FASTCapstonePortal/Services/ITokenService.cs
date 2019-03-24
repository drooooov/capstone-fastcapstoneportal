using FASTCapstonePortal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(CapstoneUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
