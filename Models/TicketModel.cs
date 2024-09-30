using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ST10361554_PROG6212_ICE_Task_5.Models
{
    public class TicketModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // This is the ID of the ticket

        [Required]
        public string ShowtimeId { get; set; } // This is the ID of the showtime

        [Required]
        [MaxLength(100)]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; } // This is the name of the customer

        [Required]
        [Range(1, 10)]
        [DisplayName("Number Of Tickets")]
        public int NumberOfTickets { get; set; } // This is the number of tickets purchased

        // navigation property to show the movie title
        public string? MovieTitle { get; set; }

        // navigation property to show the showtime
        public DateTime? ShowTime { get; set; }
    }
}
