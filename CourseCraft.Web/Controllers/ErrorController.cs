using Microsoft.AspNetCore.Mvc;

namespace CourseCraft.Web.Controllers;

public class ErrorController : Controller
{
    #region PageNotFound

    [Route("Error/PageNotFound")]
    [HttpGet]
    public IActionResult PageNotFound()
    {
        return View();
    }

    #endregion

    #region AccessDenied

    [HttpGet]
    [Route("Error/AccessDenied")]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #endregion

    #region GenericError

    [Route("Error/500")]
    public IActionResult GenericError()
    {
        return View();
    }

    #endregion

    #region HandleError

    [Route("Error/{code}")]
    public IActionResult HandleError(int code)
    {
        if (code == 404)
        {
            return RedirectToAction("PageNotFound");
        }
        if (code == 403 || code == 405)
        {
            return RedirectToAction("AccessDenied");
        }
        if (code == 500)
        {
            return RedirectToAction("GenericError");
        }

        ViewBag.ErrorCode = code;
        return View("GenericError");
    }

    #endregion
}
