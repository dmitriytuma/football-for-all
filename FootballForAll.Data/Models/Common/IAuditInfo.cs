using System;

namespace FootballForAll.Data.Models.Common
{
    public interface IAuditInfo
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
