using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using DynamicApplicationCP.Services;
using DynamicApplicationCP.Test;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DynamicApplicationCP.Tests
{
    public class QuestionServiceTests
    {

        private Mock<IConfiguration>? _configuration;

        public QuestionServiceTests()
        {
            _configuration = Utility.GetConfiguration();
        }

        [Fact]
        public async Task AddQuestionAsync_ValidQuestionModel_Success()
        {
            // Arrange
            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.CreateOrUpdateItemAsync(It.IsAny<QuestionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var questionService = new QuestionService(_configuration?.Object, cosmosDBService.Object);

            var questionModel = new QuestionModel
            {
                Question = "Test Question",
                ProgramId = "program1"
            };

            // Act
            await questionService.AddQuestionAsync(questionModel);

            // Assert
            // Verify that CreateOrUpdateItemAsync was called with the correct parameters
            cosmosDBService.Verify(
                s => s.CreateOrUpdateItemAsync(questionModel, questionModel.QuestionId, "testDatabase", "testContainer"),
                Times.Once);
        }

        [Fact]
        public async Task AddQuestionAsync_NullQuestionModel_ThrowsArgumentNullException()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await questionService.AddQuestionAsync(null);
            });
        }

        [Fact]
        public async Task AddMultipleQuestionsAsync_ValidQuestionList_Success()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.CreateOrUpdateItemAsync(It.IsAny<QuestionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            var questionList = new List<QuestionModel>
            {
                new QuestionModel { Question = "Question 1", ProgramId = "program1" },
                new QuestionModel { Question = "Question 2", ProgramId = "program1" }
            };

            // Act
            await questionService.AddMultipleQuestionsAsync(questionList);

            // Assert
            // Verify that CreateOrUpdateItemAsync was called twice with the correct parameters
            cosmosDBService.Verify(
                s => s.CreateOrUpdateItemAsync(It.IsAny<QuestionModel>(), It.IsAny<string>(), "testDatabase", "testContainer"),
                Times.Exactly(questionList.Count));
        }

        [Fact]
        public async Task AddMultipleQuestionsAsync_NullQuestionList_ThrowsArgumentNullException()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await questionService.AddMultipleQuestionsAsync(null);
            });

            // Verify that CreateOrUpdateItemAsync was not called
            cosmosDBService.Verify(
                s => s.CreateOrUpdateItemAsync(It.IsAny<QuestionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteQuestionByIdAsync_ValidQuestionId_Success()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.DeleteDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("Deleted document with ID: testId");

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            var questionId = "testId";

            // Act
            var result = await questionService.DeleteQuestionByIdAsync(questionId);

            // Assert
            Assert.Equal("Deleted document with ID: testId", result);

            // Verify that DeleteDocumentAsync was called with the correct parameters
            cosmosDBService.Verify(
                s => s.DeleteDocumentAsync(questionId, "testDatabase", "testContainer"),
                Times.Once);
        }

        [Fact]
        public async Task DeleteQuestionByIdAsync_NullQuestionId_ThrowsArgumentException()
        {
            // Arrange
            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await questionService.DeleteQuestionByIdAsync(null);
            });

            // Verify that DeleteDocumentAsync was not called
            cosmosDBService.Verify(
                s => s.DeleteDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task GetQuestionsByProgramIdAsync_ValidProgramId_ReturnsQuestionList()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.GetDocumentsAsync<QuestionModel>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<QuestionModel>
                {
                    new QuestionModel { QuestionId = "1", Question = "Question 1" },
                    new QuestionModel { QuestionId = "2", Question = "Question 2" }
                });

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            var programId = "program1";

            // Act
            var result = await questionService.GetQuestionsByProgramIdAsync(programId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            // Verify that GetDocumentsAsync was called with the correct parameters
            cosmosDBService.Verify(
                s => s.GetDocumentsAsync<QuestionModel>($"SELECT * FROM c WHERE c.programId = '{programId}'", "testDatabase", "testContainer"),
                Times.Once);
        }

        [Fact]
        public async Task GetQuestionsByProgramIdAsync_NullProgramId_ThrowsArgumentException()
        {
            // Arrange
            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await questionService.GetQuestionsByProgramIdAsync(null);
            });

            // Verify that GetDocumentsAsync was not called
            cosmosDBService.Verify(
                s => s.GetDocumentsAsync<QuestionModel>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateQuestionByIdAync_ValidQuestionModel_Success()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.CreateOrUpdateItemAsync(It.IsAny<QuestionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var questionService = new QuestionService(_configuration.Object, cosmosDBService.Object);

            var questionModel = new QuestionModel
            {
                QuestionId = "testId",
                Question = "Updated Question",
                ProgramId = "program1"
            };

            // Act
            await questionService.UpdateQuestionByIdAync(questionModel);

            // Assert
            // Verify that CreateOrUpdateItemAsync was called with the correct parameters
            cosmosDBService.Verify(
                s => s.CreateOrUpdateItemAsync(questionModel, questionModel.QuestionId, "testDatabase", "testContainer"),
                Times.Once);
        }
    }
}

