using GYMHECTORAPI.Entities;

namespace GYMHECTORAPI.DataAccess
{
    public interface IAccountDO
    {
        Task<AuthorizationResponse> ReturnToken(AuthorizationRequest Login);
    }
}
