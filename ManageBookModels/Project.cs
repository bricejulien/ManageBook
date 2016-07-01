using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBookModels
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public double Invoice { get; set; }
        public double ExpectedHours { get; set; }
        public double ActualHours { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public int QuickbookId { get; set; }
        public bool Retainer { get; set; }
        public int Actions { get; set; }
        public int Contacts { get; set; }
        public int Priority { get; set; }
    }
}
