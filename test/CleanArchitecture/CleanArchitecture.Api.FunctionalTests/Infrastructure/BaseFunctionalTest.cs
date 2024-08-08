using System.Net.Http.Json;
using CleanArchitecture.Api.FunctionalTests.Users;
using CleanArchitecture.Application.Users.LoginUser;
using Xunit;

namespace CleanArchitecture.Api.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient Httpclient;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        Httpclient = factory.CreateClient();
    }

    protected async Task<string> GetToken()
    {
        HttpResponseMessage response = await Httpclient.PostAsJsonAsync("/api/v1/users/login",
            new LoginUserRequest(
                UserData.RegisterUserRequest.Email,
                UserData.RegisterUserRequest.Password
            ));
            
        return await response.Content.ReadAsStringAsync();
    }
}