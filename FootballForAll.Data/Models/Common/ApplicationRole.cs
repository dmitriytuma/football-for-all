using System;
using Microsoft.AspNetCore.Identity;

namespace FootballForAll.Data.Models.Common
{
    public class ApplicationRole : IdentityRole, IAuditInfo
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }
}
