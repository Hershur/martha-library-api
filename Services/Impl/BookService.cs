using Data;
using Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Impl {

    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public BookService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<Book>?> GetBookssAsync()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            return book;
        }
        
        public async Task<IEnumerable<Book>?> GetBookByNameAsync(string name)
        {
            var book = await _context.Books.Where(b => b.BookName.ToLower().Contains(name.ToLower())).ToListAsync();

            return book;
        }

        public async Task<Book?> AddBookAsync(Book book)
        {
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return book;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<Book?> ReserveBorrowBookAsync(int id, bool reserve = true, string? email = null)
        {
            try
            {
                // check if book was reserved by user
                var reservedByUser = await _userService.GetUserByEmailAsync(email ?? "");
                var loggedInUserId = reservedByUser != null ? reservedByUser.Id : 0;

                // can only reserve or borrow if book is available
                var bookReserve = await _context.Books.Where(book =>  book.Id == id &&  (book.ReservedByUserId == loggedInUserId || book.Available == 1)).FirstOrDefaultAsync();

                if (bookReserve != null)
                {
                    if (reserve)
                    {
                        bookReserve.Reserved = 1;
                        bookReserve.ReservedByUserId = loggedInUserId;
                    }
                    else
                    {
                        bookReserve.Borrowed = 1;
                        bookReserve.Reserved = 0;
                        bookReserve.ReservedByUserId = 0;
                    }

                    await _context.SaveChangesAsync();
                    return bookReserve;
                }

                return null;
            }
            catch (System.Exception)
            {
                return null;
            }

        }

        public async Task<Book?> ReturnBookAsync(int id)
        {
            var book = await GetBookByIdAsync(id);

            if(book != null){
                book.Reserved = 0;
                book.Borrowed = 0;

                await _context.SaveChangesAsync();
                return book;
            }

            return book;
        }
    }
}