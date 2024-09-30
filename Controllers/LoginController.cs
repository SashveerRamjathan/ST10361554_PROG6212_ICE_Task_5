using Microsoft.AspNetCore.Mvc;

namespace ST10361554_PROG6212_ICE_Task_5.Controllers
{
    public class LoginController : Controller
    {
        // password for accessing a secure page
        private const string PagePassword = "MovieAdmin123";
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger=logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null!)
        {
            ViewBag.ReturnUrl = returnUrl; // store the return URL in ViewBag

            ViewData["ErrorMessage"] = TempData["ErrorMessage"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string password, string returnUrl = null!)
        {
            try
            {
                // CHANGE BEFORE PUSHHHHHHH

                // check if the password is correct
                if (password == PagePassword)
                {
                    // Store a flag in session to indicate successful login
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    _logger.LogInformation("Admin has successfully logged in");

                    // If there's a return URL, redirect the user to that page
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // If no return URL, redirect to the default protected page
                    return RedirectToAction("Index", "ShowTime");
                }
                else
                {
                    // display an error message
                    TempData["ErrorMessage"] = "Invalid password";
                    ViewBag.ReturnUrl = returnUrl; // Preserve return URL in case of failure
                    _logger.LogWarning("Admin has entered an invalid password");

                    return RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {
                // log the error
                _logger.LogError($"An error occurred while trying to login: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while trying to login";
                return RedirectToAction("Login", returnUrl);
            }
        }
    }
}
