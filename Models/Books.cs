using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("books")]
    public class Book {

        [Column("id")]
        public int Id { get; set;}

        [Column("bookname")]
        public string BookName { get; set;} = string.Empty;

        [Column("bookcategory")]
        public string BookCategory { get; set;} = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("reserved")]
        public int Reserved { get; set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("borrowed")]
        public int Borrowed { get; set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("available")]
        public int Available { get; private set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("dateadded")]
        public DateTime DateAdded { get; private set; }

        public int ReservedByUserId { get; set; }
        public User? User { get; set; }
        
    }
}