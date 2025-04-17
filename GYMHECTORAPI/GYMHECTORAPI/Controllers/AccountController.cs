using GYMHECTORAPI.Bussiness;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using GYMHECTORAPI.Entities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace GYMHECTORAPI.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountBO _iAccountBO;
        public AccountController(IAccountBO iAccountBO)
        {
            _iAccountBO = iAccountBO;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> IniciarSesion([FromBody] AuthorizationRequest auth)
        {
            try
            {
                var encriptado = "";
                StringBuilder sb = new StringBuilder();

                using (SHA256 hash = SHA256.Create())
                {
                    Encoding enc = Encoding.UTF8;

                    byte[] result = hash.ComputeHash(enc.GetBytes(auth.Clave));

                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    encriptado = sb.ToString();
                }
                auth.Clave = encriptado;

                var resultado = await _iAccountBO.ReturnToken(auth);
                if (resultado == null)
                {
                    return Unauthorized();
                }

                return Ok(resultado);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
