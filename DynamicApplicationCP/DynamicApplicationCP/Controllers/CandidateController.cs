using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicApplicationCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost("AddCandidateApplication")]
        public async Task<IActionResult> AddCandidateApplicarionAsync(CandidateModel candidateModel)
        {
            try
            {
                candidateModel.CandidateId = Guid.NewGuid().ToString();
                await _candidateService.AddCandidateApplication(candidateModel);

                return Ok("Candidate Application Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
