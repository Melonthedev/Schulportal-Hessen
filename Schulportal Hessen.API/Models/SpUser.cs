using Schulportal_Hessen.API.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.API.Models {
    public class SpUser {
        public string? DisplayName { get; set; }
        public string? LoginName { get; set; }
        public UserRole UserRole { get; set; }
        public string? SchoolId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Class { get; set; }
        public int? ClassLevel { get; set; }
        public string? Email { get; set; }
        public string? ProfilePictureUri { get; set; }
    }
}
