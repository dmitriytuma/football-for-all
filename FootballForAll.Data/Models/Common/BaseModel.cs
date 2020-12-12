using System;
using System.ComponentModel.DataAnnotations;

namespace FootballForAll.Data.Models.Common
{
    public class BaseModel : IAuditInfo
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
