using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMSR.Models
{
    public class DocActivityLogs
    {
        [Key]
        public int DocAId { get; set; }

        [Required(ErrorMessage = "Please enter title")]
        public string Document_title { get; set; }

        [Required(ErrorMessage = "Please enter Activity")]
        public string Document_activity { get; set; }

        [Required(ErrorMessage = "Please specify who performed the action")]
        public string Performedby { get; set; }

        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [DataType(DataType.Time)]
        public TimeOnly Time { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

        public byte[] Actfile { get; set; }  //for images

    }
}
