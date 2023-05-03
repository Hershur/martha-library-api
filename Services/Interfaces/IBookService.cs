

using Models;

namespace Services.Interfaces {
    public interface IBookService
    {
        Task<Book?> AddBookAsync(Book book);
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>?> GetBookssAsync();
        Task<Book?> ReserveBorrowBookAsync(int id, bool reserve = true, string? email = null);
        Task<IEnumerable<Book>?> GetBookByNameAsync(string name);
         Task<Book?> ReturnBookAsync(int id);
    }
}
