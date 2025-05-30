using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Forms;

namespace DMSR.Models
{
    public class Doc_Management
    {
        [Key]
        public int DocId { get; set; }
        [Required(ErrorMessage = "Please enter Title")]
        public string Document_title { get; set; }
        public byte[] Docfile { get; set; }  //for images

        [Required(ErrorMessage = "Please enter Type")]
        public string Document_type { get; set; }

        [Required(ErrorMessage = "Please enter Author Name")]
        public string Author { get; set; }

        [DataType(DataType.Date)]
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

       // [DataType(DataType.Time)]
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public TimeSpan TimeSinceLastModified => DateTime.Now - LastModifiedDate;

        public string LastModifiedDuration
        {
            get
            {
                var now = DateTime.Now;
                var diff = now - LastModifiedDate;

                if (diff.TotalDays < 1)
                {
                    if (diff.TotalHours < 1)
                        return $"{(int)diff.TotalMinutes} minutes ago";
                    else
                        return $"{(int)diff.TotalHours} hours ago";
                }

                if (diff.TotalDays < 7)
                {
                    return LastModifiedDate.ToString("ddd 'at' h:mm tt"); // Example: Tue at 3:45 AM
                }

                return LastModifiedDate.ToString("MMM dd, yyyy 'at' h:mm tt"); // Example: Apr 12, 2025 at 4:20 PM
            }
        }

        public string Version { get; set; }

        public string FileType { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }

        //  many to many  with user management
          public ICollection<User_Management> Users { get; set; }

        public List<byte[]> FileDataList { get; set; } = new();
        public List<string> FileTypeList { get; set; } = new();
        public List<string> FileNames { get; set; } = new();


    }
}
