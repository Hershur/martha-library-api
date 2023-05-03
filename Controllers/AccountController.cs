
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Shared;
using Models;
using Services.Interfaces;

namespace martha_library_api.Controllers;

[AllowAnonymous, Route("/account")]
public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("signin-google")]
    public IActionResult LoginWithGoogle()
    {
        var properties = new AuthenticationProperties{ RedirectUri = Constants.GoogleCallBackPath };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);

    }

    [HttpGet("callback-google")]
    public async Task<IActionResult> LoginWithGoogleCallback()
    {
        try
        {
            
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = auth.Principal?.Identities?.FirstOrDefault()?.Claims.Select(claim => new {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var firstname = claims?.First(claim => claim.Type.Contains("givenname")).Value ?? "";
            var lastname = claims?.First(claim => claim.Type.Contains("surname")).Value ?? "";
            var email = claims?.First(claim => claim.Type.Contains("emailaddress")).Value ?? "";


            var user = new User {
                Name = $"{firstname} {lastname}",
                Email = email,
            };

    

            var saveUser = await _userService.AddUserAsync(user);

            return SharedUtils.CustomResult(StatusCodes.Status200OK, "Welcome, You're signed in successfully");
        }
        catch (System.Exception)
        {
            
            return SharedUtils.CustomResult(StatusCodes.Status500InternalServerError, "An error occurred while signing in");
        }
    }
}