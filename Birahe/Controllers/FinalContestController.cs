using Birahe.EndPoint.ControllerAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Authorize]
// [ContestTimeAuthorize("FinalContest")]
[Route("api/[controller]")]
public class FinalContestController : ControllerBase {

}