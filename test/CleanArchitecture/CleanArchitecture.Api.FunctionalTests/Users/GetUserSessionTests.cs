using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CleanArchitecture.Api.FunctionalTests.Infrastructure;
using CleanArchitecture.Application.Users.GetUserSession;
using CleanArchitecture.Application.Users.LoginUser;
using CleanArchitecture.Application.Users.RegisterUser;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Xunit;

namespace CleanArchitecture.Api.FunctionalTests.Users;

public class GetUserSessionTests : BaseFunctionalTest
{
    public GetUserSessionTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnUNauthorized_WhenTokenIsMissing()
    {
        var response = await Httpclient.GetAsync("/api/v1/users/me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ShouldReturnUser_WhenTokenExists()
    {
        var token = await GetToken();
        Httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme, 
            token
        );

        var response = await Httpclient.GetFromJsonAsync<UserResponse>("/api/v1/users/me");

        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenUserExists()
    {
        var request = new LoginUserRequest(UserData.RegisterUserRequest.Email, UserData.RegisterUserRequest.Password);
        
        var response = await Httpclient.PostAsJsonAsync("/api/v1/users/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRequestIsValid()
    {
        var request = new RegisterUserRequest("test@test.com", "test", "test", "1234");
        
        var response = await Httpclient.PostAsJsonAsync("/api/v1/users/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
}