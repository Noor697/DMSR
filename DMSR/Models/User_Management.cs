using System.ComponentModel.DataAnnotations;

namespace DMSR.Models
{
    public class User_Management
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter UserName")]
        public byte[] UserImage { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please enter Role")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Please enter Status")]
        public string Status { get; set; }
        [Required(ErrorMessage ="Enter Password")]
        public string Password { get; set; }

        //// ✅ One user can have many documents
        public ICollection<Doc_Management> Documents { get; set; }  //Many to many with doc-management




    }
}
