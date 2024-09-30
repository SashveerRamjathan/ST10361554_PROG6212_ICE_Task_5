using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10361554_PROG6212_ICE_Task_5.Models;

namespace ST10361554_PROG6212_ICE_Task_5.Controllers
{
    public class ShowtimeController : Controller
    {
        private readonly MovieManager _movieManager;
        private readonly ILogger<ShowtimeController> _logger;



        public ShowtimeController(MovieManager movieManager, ILogger<ShowtimeController> logger)
        {
            _movieManager = movieManager;
            _logger = logger;

        }

        // Check if the admin is authenticated
        private bool IsAdminAuthenticated()
        {
            return HttpContext.Session.GetString("IsAuthenticated") == "true";
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                if (!IsAdminAuthenticated())
                {
                    _logger.LogWarning("Unauthorized access attempt to the Showtimes Index.");
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Index", "Showtime") });
                }

                // Get all showtimes
                var showtimes = _movieManager.GetShowtimes();

                if (!showtimes.Any())
                {
                    _logger.LogInformation("No showtimes available.");
                    ViewData["MessageInfo"] = "No showtimes found at the moment.";
                    return View();
                }

                // Fetch the movie titles using the MovieId and add them to the showtime object
                foreach (var showtime in showtimes)
                {
                    var movie = _movieManager.GetMovieById(showtime.MovieId);
                    if (movie != null)
                    {
                        showtime.MovieTitle = movie.Title; // Assuming you have a MovieTitle property in the ShowtimeModel
                    }
                }

                // Pass any success/error messages to the view
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];

                _logger.LogInformation("Showtimes retrieved successfully.");
                return View(showtimes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving showtimes: {ex.Message}");
                ViewData["MessageInfo"] = "An error occurred while retrieving showtimes.";
                return View();
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdminAuthenticated())
            {
                _logger.LogWarning("Unauthorized access attempt to create a showtime.");
                return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Create", "Showtime") });
            }

            ViewBag.MovieList = _movieManager.GetMovies().Select(m => new SelectListItem
            {
                Value = m.Id,
                Text = m.Title
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ShowtimeModel showtimeModel)
        {
            try
            {
                if (!IsAdminAuthenticated())
                {
                    _logger.LogWarning("Unauthorized attempt to create a showtime.");
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Create", "Showtime") });
                }

                if (ModelState.IsValid)
                {
                    _movieManager.AddShowtime(showtimeModel);
                    _logger.LogInformation($"Showtime with ID {showtimeModel.Id} created successfully.");
                    TempData["SuccessMessage"] = "Showtime created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogError("Failed to create showtime due to invalid model state.");
                ViewBag.MovieList = _movieManager.GetMovies().Select(m => new SelectListItem
                {
                    Value = m.Id,
                    Text = m.Title
                }).ToList();

                return View(showtimeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a showtime: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while creating a showtime.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                if (!IsAdminAuthenticated())
                {
                    _logger.LogWarning($"Unauthorized access attempt to edit showtime with ID {id}.");
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Edit", "Showtime", new { id }) });
                }

                var showtime = _movieManager.GetShowtimeById(id);
                if (showtime == null)
                {
                    _logger.LogError($"Showtime with ID {id} not found.");
                    TempData["ErrorMessage"] = "Showtime not found.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogInformation($"Showtime with ID {id} retrieved for editing.");
                ViewBag.MovieList = _movieManager.GetMovies().Select(m => new SelectListItem
                {
                    Value = m.Id,
                    Text = m.Title
                }).ToList();

                return View(showtime);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving showtime for editing: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving the showtime for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, ShowtimeModel showtimeModel)
        {
            try
            {
                if (!IsAdminAuthenticated())
                {
                    _logger.LogWarning($"Unauthorized attempt to edit showtime with ID {id}.");
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Edit", "Showtime", new { id }) });
                }

                if (ModelState.IsValid)
                {
                    _movieManager.UpdateShowtime(showtimeModel);
                    _logger.LogInformation($"Showtime with ID {id} updated successfully.");
                    TempData["SuccessMessage"] = "Showtime updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogError($"Failed to update showtime with ID {id} due to invalid model state.");
                ViewBag.MovieList = _movieManager.GetMovies().Select(m => new SelectListItem
                {
                    Value = m.Id,
                    Text = m.Title
                }).ToList();

                return View(showtimeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the showtime: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while updating the showtime.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            try
            {
                if (!IsAdminAuthenticated())
                {
                    _logger.LogWarning($"Unauthorized access attempt to delete showtime with ID {id}.");
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Delete", "Showtime", new { id }) });
                }

                var showtime = _movieManager.GetShowtimeById(id);
                if (showtime == null)
                {
                    _logger.LogError($"Showtime with ID {id} not found.");
                    TempData["ErrorMessage"] = "Showtime not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(showtime);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving showtime for deletion: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving the showtime for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {
                if (!IsAdminAuthenticated())
                {
                    _logger.LogWarning($"Unauthorized attempt to delete showtime with ID {id}.");
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Delete", "Showtime", new { id }) });
                }

                _movieManager.DeleteShowtime(id);
                _logger.LogInformation($"Showtime with ID {id} deleted successfully.");
                TempData["SuccessMessage"] = "Showtime deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the showtime: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the showtime.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
