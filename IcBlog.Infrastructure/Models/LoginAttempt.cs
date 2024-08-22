using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcBlog.Infrastructure.Models
{
    public class LoginAttempt
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public bool Success { get; set; }  
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
        public virtual ApplicationUser User { get; set; }

       
    }
}
