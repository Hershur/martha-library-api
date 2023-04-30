using System.Net;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Shared;

namespace martha_library_api.Controllers;

[ApiController]
[Route("/api/users")]
public class UsersController : ControllerBase {

    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: /api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync ()
    {
        var result = await _userService.GetUsersAsync();

        if(result.Any()){
            return SharedUtils.CustomResult(StatusCodes.Status200OK, "Users retrieved successfully", result);
        }

        return SharedUtils.CustomResult(StatusCodes.Status404NotFound, "No user added yet");
    }


    // GET: /api/users/2
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync (int id)
    {
        var userResult = await _userService.GetUserByIdAsync(id);

        if(userResult != null){
            return SharedUtils.CustomResult(StatusCodes.Status200OK, "User retrieved successfully", userResult);
        }

        return SharedUtils.CustomResult(StatusCodes.Status404NotFound, "User does not exist");
    }

    [HttpPost]
    public async Task<IActionResult> AddUserAsync([FromBody] User user)
    {
        var userAdded = await _userService.AddUserAsync(user);

        if(userAdded == null){

            return SharedUtils.CustomResult(StatusCodes.Status500InternalServerError, "An error occurred while creating user");
        }
        
        return  SharedUtils.CustomResult(StatusCodes.Status201Created, "User created successfully", userAdded);
    }

}