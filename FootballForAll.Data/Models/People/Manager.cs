using System.ComponentModel.DataAnnotations;

namespace FootballForAll.Data.Models.People
{
    public class Manager : PersonBaseModel
    {
        [Required]
        public Club Club { get; set; }
    }
}
