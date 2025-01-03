using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wealth_Watcher.Model
{
    public class Debt
    {
        [Key]
        public int debtId { get; set; } 

        [ForeignKey("User")]
        public int userId { get; set; } 

        public int price { get; set; }
        public string source { get; set; }
        public DateTime dueDate { get; set; }
        public bool cleared { get; set; }
    }
}
