using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Birahe.EndPoint.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Birahe.EndPoint.Controllers;

public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger) {
        _logger = logger;
    }


    public IActionResult Privacy() {
        return Ok();
    }
}