using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicApplicationCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private IProgramService _programService;
        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpPost("CreateProgram")]
        public async Task<IActionResult> CreateProgram(ApplicationFormModel applicationFormModel)
        {
            try
            {
                if (string.IsNullOrEmpty(applicationFormModel.ProgramName))
                    return BadRequest($"{nameof(applicationFormModel.ProgramName)}, should not be null or empty");
                else if (string.IsNullOrEmpty(applicationFormModel.ProgramDesc))
                    return BadRequest($"{nameof(applicationFormModel.ProgramDesc)}, should not be null or empty");

                applicationFormModel.ProgramId = Guid.NewGuid().ToString();

                await _programService.CreateProgramAsync(applicationFormModel);

                return Ok("Program Created Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProgram/{programId}")]
        public async Task<IActionResult> GetQuestionAsync(string programId)
        {
            try
            {
                ApplicationFormModel applicationFormModel = await _programService.GetProgramByIdAsync(programId);

                if (applicationFormModel == null)
                    return NotFound(new { message = "Program not found" });

                return Ok(applicationFormModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
