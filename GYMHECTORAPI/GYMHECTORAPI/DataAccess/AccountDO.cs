using GYMHECTORAPI.Entities;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace GYMHECTORAPI.DataAccess
{
    public class AccountDO : IAccountDO
    {
        private readonly GymHectorContext _dbContext;
        public AccountDO(GymHectorContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AuthorizationResponse> ReturnToken(AuthorizationRequest authorization)
        {
            try
            {
                AuthorizationResponse response = new AuthorizationResponse();
                var usuarioParam = new SqlParameter("@IN_Usuario", SqlDbType.VarChar)
                {
                    Value = string.IsNullOrEmpty(authorization.Usuario) ? (object)DBNull.Value : authorization.Usuario,
                    Size = 30
                };
                var claveParam = new SqlParameter("@IN_Clave", SqlDbType.VarChar)
                {
                    Value = string.IsNullOrEmpty(authorization.Clave) ? (object)DBNull.Value : authorization.Clave,
                    Size = 100
                };
                var Respuesta = _dbContext.Set<SP_PRUEBA_TOKEN_Result>()
                  .FromSqlRaw("EXEC SP_PRUEBA_TOKEN @IN_Usuario, @IN_Clave", usuarioParam, claveParam)
                  .AsEnumerable()
                  .FirstOrDefault();

                if (Respuesta != null)
                {

                    if (Respuesta.Codigo == (int)HttpStatusCode.OK)
                    {
                        response.IdUsuario = Respuesta.IdUsuario;
                        response.Mensaje = Respuesta.Mensaje;
                        response.Codigo = (HttpStatusCode)Respuesta.Codigo;
                        response.Username = Respuesta.Username;
                        response.Password = Respuesta.Password;

                    }
                    else
                    {
                        response.Codigo = (HttpStatusCode)Respuesta.Codigo;
                        response.Mensaje = Respuesta.Mensaje;
                    }

                }

                return response;
            }
            catch (Exception ex)
            {

                return new AuthorizationResponse()
                {
                    Codigo = HttpStatusCode.InternalServerError,
                    Mensaje = "Ocurrió un error al validar usuario."
                };
            }
           
        }



    }
}
