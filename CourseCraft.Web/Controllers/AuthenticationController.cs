using Microsoft.AspNetCore.Mvc;
using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Intefaces;
using CourseCraft.Service.Utilities;

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
                model.UserPassword!
            );

            if (userLoginViewModel == null)
            {
                bool isUserExists = await _authenticationService.CheckIfUserExistsAsync(model.UserEmail.ToLower());
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
            string? token = await _jwtService.GenerateJwtTokenAsync(userLoginViewModel.UserEmail, model.RememberMe);



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
}
