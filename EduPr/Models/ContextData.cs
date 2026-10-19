using EduPr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPr.Models
{
    class ContextData
    {
        public static User? SelectedUser { get; set; }
        public static User? CurrentUser { get; set; }
        public static Patient SelectedPatient { get; set; }
        public static bool isNewPatient { get; set; }
        public static Doctor SelectedDoctor { get; set; }
        public static Department? SelectedDepartment { get; set; }
    }
}
