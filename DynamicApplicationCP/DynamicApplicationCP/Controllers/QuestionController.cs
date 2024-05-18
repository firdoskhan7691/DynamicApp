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
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
        }

        /// <summary>
        /// Adds a new question.
        /// </summary>
        /// <param name="questionModel">The question details to add.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestionAsync([FromBody] QuestionModel questionModel)
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

        /// <summary>
        /// Retrieves questions by program ID.
        /// </summary>
        /// <param name="programId">The ID of the program to retrieve questions for.</param>
        /// <returns>The list of questions if found, otherwise a not found response.</returns>
        [HttpGet("GetQuestions/{programId}")]
        public async Task<IActionResult> GetQuestionsAsync(string programId)
        {
            try
            {
                List<QuestionModel> questions = await _questionService.GetQuestionsByProgramIdAsync(programId);

                if (questions == null || questions.Count == 0)
                {
                    return NotFound(new { message = "Questions not found" });
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing question.
        /// </summary>
        /// <param name="questionModel">The question details to update.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestionAsync([FromBody] QuestionModel questionModel)
        {
            try
            {
                await _questionService.AddQuestionAsync(questionModel);
                return Ok("Question updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a question by its ID.
        /// </summary>
        /// <param name="questionId">The ID of the question to delete.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpDelete("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> DeleteQuestionAsync(string questionId)
        {
            try
            {
                await _questionService.DeleteQuestionByIdAsync(questionId);
                return Ok("Question deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
