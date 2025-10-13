using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Birahe.EndPoint.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Birahe.EndPoint.Controllers;

[Route("/home/[action]")]
[ApiController]
public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger) {
        _logger = logger;
    }





    [HttpGet]
    public  IActionResult Index() {
         return Ok();
    }
}