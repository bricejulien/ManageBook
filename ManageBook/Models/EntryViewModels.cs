using ManageBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageBook.Models
{
    public class EntryIndexViewModel
    {
        public IList<Entry> Entries { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Project> Projects { get; set; }
    }
}