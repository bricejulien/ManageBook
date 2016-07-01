using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ManageBook.Models;
//using System.ComponentModel.DataAnnotations.Schema;

namespace ManageBookModels
{
    public class Entry
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public double InvoicableHours { get; set; }
        public double ActualHours { get; set; }
        public bool Invoiced { get; set; }
        //[ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public string Information { get; set; }
        //[ForeignKey("User")]
        public string UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}
