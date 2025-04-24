using GYMHECTORAPI.DataAccess;
using GYMHECTORAPI.Entities;
using GYMHECTORAPI.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GYMHECTORAPI.Bussiness
{
    public class AccountBO : IAccountBO
    {
        private readonly IAccountDO _iAccountDO;
        public AccountBO(IAccountDO iAccountDO)
        {
            _iAccountDO = iAccountDO;
        }

        public async Task<GenerarTokenResponse> ReturnToken(AuthorizationRequest authorization)
        {
            var usuario_encontrado = await _iAccountDO.ReturnToken(authorization);


            if (usuario_encontrado == null)
            {
                return await Task.FromResult<GenerarTokenResponse>(null);
            }

            string tokenCreado = await GenerarToken(usuario_encontrado.IdUsuario.ToString(), usuario_encontrado.Username, usuario_encontrado.Password);

            return new GenerarTokenResponse() { Token = tokenCreado, Resultado = true, Message = "Ok" };
        }



        private async Task<string> GenerarToken(string idUsuario, string username, string password)
        {
            IServiceCollection services = new ServiceCollection();
            var key = "GYMHECTOR-32-char-key-2024202320240000";
            //var keyTokenEncriptado = Environment.GetEnvironmentVariable("KEY_TOKEN_TMS");
            //var keyToken = Desencriptacion.Decrypt(services, key, keyTokenEncriptado);
            var keyBytes = Encoding.ASCII.GetBytes(key);

            //var usuario_encontrado = await _iAccountBO.GetUsuario(username, password);
            //var persona = await _iAccountBO.GetPersona((int)usuario_encontrado.IdPersona);
            //var rol = await _iAccountBO.GetRol((int)persona.IdRol);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));
            claims.AddClaim(new Claim("Username", username));
            //claims.AddClaim(new Claim("rol", rol.CodRol));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(3600),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }
    }
}
