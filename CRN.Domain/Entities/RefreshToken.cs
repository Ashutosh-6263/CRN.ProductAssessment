using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? RevokedOn { get; set; }

        public User User { get; set; } = null!;
    }
}
