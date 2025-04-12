using API.v1.api.Authentication.AuthenticateApplication.Requests;
using Oog.Domain;

namespace API.v1.api.Authentication.AuthenticateApplication.Interfaces;

public interface IAuthenticateAppRepository
{
    public Task<App?> Authenticate(AuthenticateAppRequest request);
}