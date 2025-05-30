using System.ComponentModel.DataAnnotations;

namespace DMSR.Models
{
    public class UserActivityLog
    {
        [Key]
        public int UserAId { get; set; }
        public string UserName { get; set; }
        public string Activity { get; set; } // Login, Logout, etc.

        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [DataType(DataType.Time)]
        public TimeOnly Time { get; set; } = TimeOnly.FromDateTime(DateTime.Now);



    }
}
