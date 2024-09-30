using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ST10361554_PROG6212_ICE_Task_5.Models
{
    public class ShowtimeModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // This is the ID of the showtime

        [Required]
        public string MovieId { get; set; } // This is the ID of the movie

        [Required]
        [DisplayName("Show Time")]
        public DateTime ShowTime { get; set; } // This is the time of the showtime

        [Required]
        [Range(1, 100)]
        [DisplayName("Number Of Available Seats")]
        public int AvailableSeats { get; set; } // This is the number of available seats

        // navigation property to show the movie title
        public string? MovieTitle { get; set; }
    }
}
