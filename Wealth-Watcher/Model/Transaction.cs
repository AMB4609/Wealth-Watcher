using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wealth_Watcher.Model
{
    public class Transaction
    {
        [Key]
        public int transactionCode { get; set; }

        [ForeignKey("User")]
        public int userId { get; set; }
        public string title { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; } 
        public DateTime transactionDate { get; set; }
        public string note { get; set; }
        public string tags { get; set; }

   
    }
}
