using Microsoft.AspNetCore.Mvc;
using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Interfaces;
using CourseCraft.Service.Utilities;
using System.Text;

namespace CourseCraft.Web.Controllers;

public class AuthenticationController(IAuthenticationService authenticationService, IJwtService jwtService) : Controller
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IJwtService _jwtService = jwtService;

    #region UserLogin GET

    public IActionResult UserLogin()
    {
        return View();
    }

    #endregion

    #region UserLogin POST

    [HttpPost]
    public async Task<IActionResult> UserLogin(UserLoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserLoginViewModel? userLoginViewModel = await _authenticationService.AuthenticateUserUsingEmailPasswordAsync(
                model.UserEmail!.ToLower(),
                model.UserPassword!.ToLower()!
            );

            if (userLoginViewModel == null)
            {
                bool isUserExists = await _authenticationService
                                            .CheckIfUserExistsAsync(model.UserEmail.ToLower());
                if (isUserExists)
                {
                    ModelState.AddModelError(
                        "UserPassword",
                        "Invalid password. Try again or reset your password."
                    );
                }
                return View(model);
            }
            if (userLoginViewModel.UserEmail == null)
            {
                ModelState.AddModelError(
                    "UserEmail",
                    "Invalid email. Try again or reset your password."
                );
                return View(model);
            }
            string? token = await _jwtService.
                                    GenerateJwtTokenAsync(userLoginViewModel.UserEmail, model.RememberMe);

            CookieUtils.SaveJWTToken(Response, token);

            if (model.RememberMe)
            {
                CookieUtils.SaveUserData(Response, userLoginViewModel);
            }
            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
            return View();
        }
    }

    #endregion

    #region NewUser GET

    [HttpGet]
    public IActionResult NewUser()
    {
        return View();
    }

    #endregion

    #region NewUser Post

    [HttpPost]
    public async Task<IActionResult> NewUser(AddUserViewModel addUserViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(addUserViewModel);
            }

            bool isUserExist = await _authenticationService
                                        .CheckIfUserExistsAsync(addUserViewModel.UserEmail!.ToLower());

            if (isUserExist == true)
            {
                ModelState.AddModelError(
                    "UserEmail",
                    "Email already exists. Please use a different email."
                );
                return View(addUserViewModel);
            }
            else
            {
                // Create a new user
                AddUserViewModel? newUser = new()
                {
                    UserEmail = addUserViewModel.UserEmail.ToLower(),
                    UserPassword = HashPassword(addUserViewModel.UserPassword!),
                    ConfirmPassword = HashPassword(addUserViewModel.UserPassword!),
                    UserName = addUserViewModel.UserName,
                    UserRole = "Student"
                };

                bool isUserAdded = await _authenticationService.RegisterUserAsync(newUser);

                return RedirectToAction("UserLogin", "Authentication");
            }
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
            return View(addUserViewModel);
        }
    }

    #endregion

    #region LogOut 

    public IActionResult Logout()
    {
        try
        {
            CookieUtils.ClearCookies(HttpContext);
            SessionUtils.ClearSession(HttpContext);
            return RedirectToAction("UserLogin", "Authentication");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
            return RedirectToAction("UserLogin", "Authentication");
        }
    }

    #endregion

    #region HelperMethod 

    private static string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }

    #endregion
}
