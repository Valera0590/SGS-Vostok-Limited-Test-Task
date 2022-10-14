using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStaff.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkshopId { get; set; }
        public Workshop Workshop { get; set; }

    }
}
