using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
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
            _candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        }

        /// <summary>
        /// Adds a new candidate application.
        /// </summary>
        /// <param name="candidateModel">The candidate model to add.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost("AddCandidateApplication")]
        public async Task<IActionResult> AddCandidateApplicationAsync([FromBody] CandidateModel candidateModel)
        {
            try
            {
                if (string.IsNullOrEmpty(candidateModel.FirstName))
                {
                    return BadRequest($"{nameof(candidateModel.FirstName)} should not be null or empty");
                }

                if (string.IsNullOrEmpty(candidateModel.LastName))
                {
                    return BadRequest($"{nameof(candidateModel.LastName)} should not be null or empty");
                }

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
