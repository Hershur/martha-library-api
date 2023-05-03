using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Shared;

namespace martha_library_api.Controllers;

[Authorize]
[ApiController]
[Route("/api/books")]
public class BooksController : ControllerBase {

    private readonly IBookService _bookService;
    private string  email;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;

        // get loggedin user
        email = HttpContext.User.Claims?.First(claim => claim.Type.Contains("emailaddress")).Value ?? "";
    }

    // GET: /api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooksAsync ([FromQuery] string? name = null)
    {

        IEnumerable<Book?>? result = null;
        
        if(string.IsNullOrWhiteSpace(name)){
            result =  await _bookService.GetBookssAsync();
        } else {
            result = await _bookService.GetBookByNameAsync(name);
        }

        if(result != null){
            return SharedUtils.CustomResult(StatusCodes.Status200OK, "Books retrieved successfully", result);
        }

        return SharedUtils.CustomResult(StatusCodes.Status404NotFound, name == null ? "No book added yet": $"No book with name:{name} was found");
    }


    // GET: /api/users/2
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookByIdAsync (int id)
    {
        var bookResult = await _bookService.GetBookByIdAsync(id);

        if(bookResult != null){
            return SharedUtils.CustomResult(StatusCodes.Status200OK, "Book retrieved successfully", bookResult);
        }

        return SharedUtils.CustomResult(StatusCodes.Status404NotFound, "Book does not exist");
    }


    [HttpPost]
    public async Task<IActionResult> AddBookAsync([FromBody] Book book)
    {
        var bookAdded = await _bookService.AddBookAsync(book);

        if(bookAdded == null){

            return SharedUtils.CustomResult(StatusCodes.Status500InternalServerError, "An error occurred while adding book");
        }
        
        return  SharedUtils.CustomResult(StatusCodes.Status201Created, "User created successfully", bookAdded);
    }

    [HttpPatch("reserve/{id}")]
    public async Task<IActionResult> ReserveBookAsync(int id) 
    {
        var reserveBook = await _bookService.ReserveBorrowBookAsync(id);

        if(reserveBook == null){
            return SharedUtils.CustomResult(StatusCodes.Status400BadRequest, "Cannot reserve book at this time");
        }

        return SharedUtils.CustomResult(StatusCodes.Status200OK, "Book reserved successfully for 24 hours", reserveBook);
    }
    
    [HttpPatch("borrow/{id}")]
    public async Task<IActionResult> BorrowBookAsync(int id) 
    {
        
        var borrowBook = await _bookService.ReserveBorrowBookAsync(id, false, email);

        if(borrowBook == null){
            return SharedUtils.CustomResult(StatusCodes.Status400BadRequest, "Cannot borrow book at this time");
        }

        return SharedUtils.CustomResult(StatusCodes.Status200OK, "Book borrowed successfully", borrowBook);
    }

    [HttpPatch("return/{id}")]
    public async Task<IActionResult> ReturnBookAsync(int id) 
    {
        var result = await _bookService.ReturnBookAsync(id);

        if(result == null)
        {
            return SharedUtils.CustomResult(StatusCodes.Status400BadRequest, "Specified book does not exist");
        }

        return SharedUtils.CustomResult(StatusCodes.Status200OK, "Book returned successfully", result);


    }

}