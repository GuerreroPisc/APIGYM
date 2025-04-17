using GYMHECTORAPI.Entities;

namespace GYMHECTORAPI.Bussiness
{
    public interface IAccountBO
    {
        Task<GenerarTokenResponse> ReturnToken(AuthorizationRequest auth);

    }
}
