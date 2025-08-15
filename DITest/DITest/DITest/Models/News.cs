using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITest.Models
{
    public class News
    {
        public int id;
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public DateTime date { get; set; }
    }
}
