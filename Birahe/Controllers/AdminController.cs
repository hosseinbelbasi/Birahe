using Birahe.EndPoint.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;


[Route("/Admin/[action]")]
[Authorize(Roles = "admin")]
public class AdminController : Controller {
    private UserRepository _userRepository;

    public AdminController(UserRepository userRepository) {
        _userRepository = userRepository;
    }




}