using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBookModels
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
