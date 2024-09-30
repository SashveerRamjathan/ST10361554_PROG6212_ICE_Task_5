using Microsoft.AspNetCore.Mvc;
using ST10361554_PROG6212_ICE_Task_5.Models;

namespace ST10361554_PROG6212_ICE_Task_5.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieManager _movieManager;
        private readonly ILogger<MovieController> _logger;

        public MovieController(MovieManager manager, ILogger<MovieController> logger)
        {
            _movieManager = manager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                // Check if the admin is authenticated by verifying the session variable
                if (HttpContext.Session.GetString("IsAuthenticated") != "true")
                {
                    // If not authenticated, redirect to the password entry page with returnUrl
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Index", "Movie") });
                }

                // The user is authenticated, show the protected content

                // Get the list of movies
                var movies = _movieManager.GetMovies();

                // check if the list of movies is empty or null
                if (movies.Count == 0 || movies ==  null)
                {
                    _logger.LogInformation("No movies found at the moment");
                    ViewData["MessageInfo"] = "No movies found at the moment";
                    return View();
                }

                // Display success message or error messages
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];

                // Pass the list of movies to the view
                return View(movies);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"An error occurred while trying to display movies: {ex.Message}");
                ViewData["MessageInfo"] = "An error occurred while trying to display movies";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Check if the admin is authenticated by verifying the session variable
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                // If not authenticated, redirect to the password entry page with returnUrl
                return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Create", "Movie") });
            }

            // The user is authenticated, show the protected content

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieModel movie)
        {
            try
            {
                // Check if the admin is authenticated by verifying the session variable
                if (HttpContext.Session.GetString("IsAuthenticated") != "true")
                {
                    // If not authenticated, redirect to the password entry page with returnUrl
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Create", "Movie") });
                }

                // The user is authenticated, show the protected content

                // Check if the model is valid
                if (ModelState.IsValid)
                {
                    // Add the movie to the list of movies
                    _movieManager.AddMovie(movie);

                    // Display success message
                    TempData["SuccessMessage"] = $"Movie {movie.Title} added successfully";

                    // Redirect to the index page
                    return RedirectToAction(nameof(Index));
                }

                // Redirect back to the same page
                return View(movie);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"An error occurred while trying to add a movie: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while trying to add a movie";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                // Check if the admin is authenticated by verifying the session variable
                if (HttpContext.Session.GetString("IsAuthenticated") != "true")
                {
                    // If not authenticated, redirect to the password entry page with returnUrl
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Edit", "Movie") });
                }

                // The user is authenticated, show the protected content

                // Get the movie by ID
                var movie = _movieManager.GetMovieById(id);

                // Check if the movie is null
                if (movie == null)
                {
                    TempData["ErrorMessage"] = "Movie not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(movie);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"An error occurred while trying to edit a movie: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while trying to edit a movie";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieModel movie)
        {
            try
            {
                // Check if the admin is authenticated by verifying the session variable
                if (HttpContext.Session.GetString("IsAuthenticated") != "true")
                {
                    // If not authenticated, redirect to the password entry page with returnUrl
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Edit", "Movie") });
                }

                // The user is authenticated, show the protected content

                // Check if the model is valid
                if (ModelState.IsValid)
                {
                    // Update the movie
                    _movieManager.UpdateMovie(movie);

                    // Display success message
                    TempData["SuccessMessage"] = $"Movie {movie.Title} updated successfully";

                    // Redirect to the index page
                    return RedirectToAction(nameof(Index));
                }

                // Redirect back to the same page
                return View(movie);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"An error occurred while trying to update a movie: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while trying to update a movie";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            try
            {
                // Check if the admin is authenticated by verifying the session variable
                if (HttpContext.Session.GetString("IsAuthenticated") != "true")
                {
                    // If not authenticated, redirect to the password entry page with returnUrl
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Delete", "Movie") });
                }

                // The user is authenticated, show the protected content

                // Get the movie by ID
                var movie = _movieManager.GetMovieById(id);

                // Check if the movie is null
                if (movie == null)
                {
                    TempData["ErrorMessage"] = "Movie not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(movie);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"An error occurred while trying to delete a movie: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while trying to delete a movie";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(MovieModel movie)
        {
            try
            {
                // Check if the admin is authenticated by verifying the session variable
                if (HttpContext.Session.GetString("IsAuthenticated") != "true")
                {
                    // If not authenticated, redirect to the password entry page with returnUrl
                    return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("Delete", "Movie") });
                }

                // The user is authenticated, show the protected content

                // Delete the movie
                _movieManager.DeleteMovie(movie.Id);

                // Display success message
                TempData["SuccessMessage"] = $"Movie {movie.Title} deleted successfully";

                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"An error occurred while trying to delete a movie: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while trying to delete a movie";
                return RedirectToAction(nameof(Index));
            }
        }

        
    }
}
