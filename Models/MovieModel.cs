using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ST10361554_PROG6212_ICE_Task_5.Models
{
    public class MovieModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // This is the ID of the movie

        [Required]
        [MaxLength(100)]
        [DisplayName("Movie Title")]
        public string Title { get; set; } // This is the title of the movie

        [Required]
        [DisplayName("Movie Genre")]
        public string Genre { get; set; } // This is the genre of the movie

        [Required]
        [Range(30, 300)]
        [DisplayName("Movie Duration")]
        public int Duration { get; set; } // This is the duration of the movie in minutes
    }
}
