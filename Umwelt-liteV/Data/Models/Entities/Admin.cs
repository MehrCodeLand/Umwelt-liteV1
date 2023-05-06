using System.ComponentModel.DataAnnotations;

namespace Umwelt_liteV.Data.Models.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public int MyAdminId { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsDelete { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
