using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using DynamicApplicationCP.Services;
using DynamicApplicationCP.Test;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DynamicApplicationCP.Tests
{
    public class ProgramServiceTests
    {

        private Mock<IConfiguration>? _configuration;

        public ProgramServiceTests()
        {
            _configuration = Utility.GetConfiguration();
        }

        [Fact]
        public async Task CreateProgramAsync_ValidApplicationFormModel_Success()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.CreateOrUpdateItemAsync(It.IsAny<ApplicationFormFields>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var questionService = new Mock<IQuestionService>(MockBehavior.Strict);
            questionService.Setup(s => s.AddMultipleQuestionsAsync(It.IsAny<List<QuestionModel>>()))
                .Returns(Task.CompletedTask);

            var programService = new ProgramService(_configuration.Object, cosmosDBService.Object, questionService.Object);

            var applicationFormModel = new ApplicationFormModel
            {
                ProgramId = Guid.NewGuid().ToString(),
                ProgramName = "Test Program",
                ProgramDesc = "Test Program Description",
                FormFields = new List<FormFields>(),
                Questions = new List<QuestionModel>
                {
                    new QuestionModel { QuestionId = Guid.NewGuid().ToString(), Question = "Question 1" },
                    new QuestionModel { QuestionId = Guid.NewGuid().ToString(), Question = "Question 2" }
                }
            };

            // Act
            await programService.CreateProgramAsync(applicationFormModel);

            // Assert

            // Verify that CreateOrUpdateItemAsync was called with the correct parameters for cosmosDBService
            cosmosDBService.Verify(
                s => s.CreateOrUpdateItemAsync(It.IsAny<ApplicationFormFields>(), applicationFormModel.ProgramId, "testDatabase", "testContainer"),
                Times.Once);

            // Verify that AddMultipleQuestionsAsync was called with the correct parameters for questionService
            questionService.Verify(
                s => s.AddMultipleQuestionsAsync(applicationFormModel.Questions),
                Times.Once);
        }

        [Fact]
        public async Task CreateProgramAsync_NullApplicationFormModel_ThrowsArgumentNullException()
        {
            // Arrange
            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            var questionService = new Mock<IQuestionService>(MockBehavior.Strict);

            var programService = new ProgramService(_configuration.Object, cosmosDBService.Object, questionService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await programService.CreateProgramAsync(null);
            });
        }

        [Fact]
        public async Task GetProgramByIdAsync_ValidProgramId_ReturnsApplicationFormModel()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.GetDocumentsAsync<ApplicationFormFields>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<ApplicationFormFields>
                {
                    new ApplicationFormFields
                    {
                        ProgramId = "program1",
                        ProgramName = "Test Program",
                        ProgramDesc = "Test Program Description",
                        FormFields = new List<FormFields>()
                    }
                });

            var questionService = new Mock<IQuestionService>(MockBehavior.Strict);
            questionService.Setup(s => s.GetQuestionsByProgramIdAsync("program1"))
                .ReturnsAsync(new List<QuestionModel>
                {
                    new QuestionModel { QuestionId = "1", Question = "Question 1" },
                    new QuestionModel { QuestionId = "2", Question = "Question 2" }
                });

            var programService = new ProgramService(_configuration.Object, cosmosDBService.Object, questionService.Object);

            var programId = "program1";

            // Act
            var result = await programService.GetProgramByIdAsync(programId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("program1", result.ProgramId);
            Assert.Equal("Test Program", result.ProgramName);
            Assert.Equal("Test Program Description", result.ProgramDesc);
            Assert.Equal(new List<FormFields>(), result.FormFields);
            Assert.Equal(2, result.Questions.Count);

            // Verify that GetDocumentsAsync and GetQuestionsByProgramIdAsync were called with the correct parameters
            cosmosDBService.Verify(
                s => s.GetDocumentsAsync<ApplicationFormFields>($"SELECT * FROM c WHERE c.id = '{programId}'", "testDatabase", "testContainer"),
                Times.Once);

            questionService.Verify(
                s => s.GetQuestionsByProgramIdAsync(programId),
                Times.Once);
        }

        [Fact]
        public async Task GetProgramByIdAsync_NullProgramId_ThrowsArgumentException()
        {
            // Arrange
            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            var questionService = new Mock<IQuestionService>(MockBehavior.Strict);

            var programService = new ProgramService(_configuration.Object, cosmosDBService.Object, questionService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await programService.GetProgramByIdAsync(null);
            });

            // Verify that GetDocumentsAsync and GetQuestionsByProgramIdAsync were not called
            cosmosDBService.Verify(
                s => s.GetDocumentsAsync<ApplicationFormFields>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            questionService.Verify(
                s => s.GetQuestionsByProgramIdAsync(It.IsAny<string>()),
                Times.Never);
        }
       
    }
}
