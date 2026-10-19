using System.ComponentModel.DataAnnotations;

namespace EduPr.Models
{
    public class Supplier
    {
        public int IdSupplier { get; set; }

        public string OrganizationName { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}