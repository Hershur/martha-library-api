using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("users")]
    public class User {

        [Column("id")]
        public int Id { get; set;}

        [Column("name")]
        public string Name { get; set;} = string.Empty;

        [Column("email")]
        public string Email { get; set;} = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("createdon")]
        public DateTime CreateOn { get; private set; }
    }
}