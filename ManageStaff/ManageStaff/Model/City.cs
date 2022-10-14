using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStaff.Model
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Workshop> Workshops { get; set; }
    }
}
