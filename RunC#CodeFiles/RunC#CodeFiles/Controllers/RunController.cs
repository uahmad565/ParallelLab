using Microsoft.AspNetCore.Mvc;
using RunC_CodeFiles.Models;
using RunC_CodeFiles.Services;

namespace RunC_CodeFiles.Controllers
{
    [ApiController]
    [Route("run")]
    public class RunController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CodeRunResponse>> Run([FromBody] CodeRunRequest request)
        {
            var timeout = (request.TimeoutMs.HasValue && request.TimeoutMs.Value > 0) ? request.TimeoutMs.Value : 10000;
            var result = await CodeRunner.RunAsync(request.Code ?? string.Empty, request.Input ?? string.Empty, timeout);
            return Ok(result);
        }
    }
}


