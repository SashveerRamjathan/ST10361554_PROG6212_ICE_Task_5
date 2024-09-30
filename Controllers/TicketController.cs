using Microsoft.AspNetCore.Mvc;
using ST10361554_PROG6212_ICE_Task_5.Models;

namespace ST10361554_PROG6212_ICE_Task_5.Controllers
{
    public class TicketController : Controller
    {
        private readonly MovieManager _movieManager;
        private readonly ILogger<TicketController> _logger;

        public TicketController(MovieManager movieManager, ILogger<TicketController> logger)
        {
            _movieManager = movieManager;
            _logger = logger;
        }

        [HttpGet]
        // GET: Ticket
        public IActionResult Index()
        {
            // Retrieve all tickets
            var tickets = _movieManager.GetTickets();

            // Retrieve all showtimes to use for mapping titles and times
            var showtimes = _movieManager.GetShowtimes();

            // Create a dictionary for fast lookup of showtime information
            var showtimeLookup = showtimes.ToDictionary(st => st.Id, st => new
            {
                MovieTitle = st.MovieTitle,
                ShowTime = st.ShowTime
            });

            // Populate the navigation properties for each ticket
            foreach (var ticket in tickets)
            {
                if (showtimeLookup.TryGetValue(ticket.ShowtimeId, out var showtimeInfo))
                {
                    ticket.MovieTitle = showtimeInfo.MovieTitle;
                    ticket.ShowTime = showtimeInfo.ShowTime;
                }
            }

            // Check for any success or error messages to display
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];

            return View(tickets);
        }

        [HttpGet]
        // GET: Ticket/Create
        public IActionResult Create()
        {
            // Get the showtimes and format them with the movie name and date
            var showtimes = _movieManager.GetShowtimes();

            // Filter out showtimes with no available seats
            var availableShows = showtimes.Where(st => st.AvailableSeats > 0).ToList();

            ViewBag.Showtimes = availableShows.Select(st => new
            {
                Id = st.Id,
                DisplayName = $"{st.MovieTitle} - {st.ShowTime:dddd, MMMM dd, yyyy h:mm tt}" // Formatting the display name
            });

            return View();
        }


        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketModel ticket)
        {
            if (ModelState.IsValid)
            {
                var availableSeats = _movieManager.GetAvailableSeatsByShowtimeId(ticket.ShowtimeId);
                if (availableSeats >= ticket.NumberOfTickets)
                {
                    _movieManager.AddTicket(ticket);
                    _movieManager.UpdateAvailableSeatsByShowtimeId(ticket.ShowtimeId, ticket.NumberOfTickets);

                    // Log success message and set a success notification
                    _logger.LogInformation($"Ticket created successfully for Showtime ID {ticket.ShowtimeId}. Number of tickets: {ticket.NumberOfTickets}");
                    TempData["SuccessMessage"] = "Ticket created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                // Log warning and set an error message for insufficient seats
                _logger.LogWarning($"Insufficient available seats for Showtime ID {ticket.ShowtimeId}. Available: {availableSeats}, Requested: {ticket.NumberOfTickets}");
                TempData["ErrorMessage"] = "Not enough available seats.";

                return RedirectToAction(nameof(Index));
            }

            // Log error and set an error message for invalid model state
            _logger.LogError("Error occurred while creating tickets");
            TempData["ErrorMessage"] = "Error occurred while creating tickets";
            return RedirectToAction(nameof(Index));
        }
    }
}
