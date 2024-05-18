using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicApplicationCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService ?? throw new ArgumentNullException(nameof(programService));
        }

        /// <summary>
        /// Creates a new program.
        /// </summary>
        /// <param name="applicationFormModel">The program details to create.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost("CreateProgram")]
        public async Task<IActionResult> CreateProgramAsync([FromBody] ApplicationFormModel applicationFormModel)
        {
            try
            {
                if (string.IsNullOrEmpty(applicationFormModel?.ProgramName))
                {
                    return BadRequest($"{nameof(applicationFormModel.ProgramName)} should not be null or empty");
                }

                if (string.IsNullOrEmpty(applicationFormModel?.ProgramDesc))
                {
                    return BadRequest($"{nameof(applicationFormModel.ProgramDesc)} should not be null or empty");
                }

                applicationFormModel.ProgramId = Guid.NewGuid().ToString();

                await _programService.CreateProgramAsync(applicationFormModel);

                return Ok("Program Created Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a program by its ID.
        /// </summary>
        /// <param name="programId">The ID of the program to retrieve.</param>
        /// <returns>The program details if found, otherwise a not found response.</returns>
        [HttpGet("GetProgram/{programId}")]
        public async Task<IActionResult> GetProgramAsync(string programId)
        {
            try
            {
                ApplicationFormModel applicationFormModel = await _programService.GetProgramByIdAsync(programId);

                if (applicationFormModel == null)
                {
                    return NotFound(new { message = "Program not found" });
                }

                return Ok(applicationFormModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
