using API.v1.api.Authentication.AuthenticateApplication.Requests;

namespace API.v1.api.Authentication.AuthenticateApplication.Interfaces;

public interface IAuthenticateAppHandler
{
    public Task<string> Authenticate(AuthenticateAppRequest request, string jwtSecret);

}