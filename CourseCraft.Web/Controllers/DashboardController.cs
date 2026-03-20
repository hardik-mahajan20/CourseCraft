using CourseCraft.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CourseCraft.Web.Controllers;

public class DashboardController() : Controller
{

    #region Dashboard

    [HttpGet]
    [CustomAuthorize]
    public IActionResult Index()
    {
        return View();
    }

    #endregion
}
