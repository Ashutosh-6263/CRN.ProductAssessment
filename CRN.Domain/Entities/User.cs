using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime CreatedOn { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
            = new List<RefreshToken>();
    }
}
