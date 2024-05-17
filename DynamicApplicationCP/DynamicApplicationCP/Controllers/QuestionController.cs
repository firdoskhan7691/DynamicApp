using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicApplicationCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestionAsync(QuestionModel questionModel)
        {
            try
            {
                questionModel.QuestionId = Guid.NewGuid().ToString();
                await _questionService.AddQuestionAsync(questionModel);

                return Ok("Question Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetQuestions/{programId}")]
        public async Task<IActionResult> GetQuestionAsync(string programId)
        {
            try
            {
                List<QuestionModel> lstQuestionModel = await _questionService.GetQuestionsByProgramIdAsync(programId);

                if (lstQuestionModel == null || lstQuestionModel.Count <= 0)
                    return NotFound(new { message = "Question not found" });

                return Ok(lstQuestionModel);
            }
            catch (Exception ex)
            {             
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> GetQuestionAsync(QuestionModel questionModel)
        {
            try
            {
                await _questionService.AddQuestionAsync(questionModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> DeleteQuestionAsync(string questionId)
        {
            try
            {
                await _questionService.DeleteQuestionByIdAsync(questionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
