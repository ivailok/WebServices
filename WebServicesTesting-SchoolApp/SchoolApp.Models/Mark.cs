using System;
using System.Linq;

namespace SchoolApp.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public decimal Value { get; set; }
        public virtual Student Student { get; set; }
    }
}