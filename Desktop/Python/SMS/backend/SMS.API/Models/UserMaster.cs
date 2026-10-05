using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.API.Models
{
    [Table("UserMaster")]
    public class UserMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        public byte[] Password { get; set; } = Array.Empty<byte>();

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
