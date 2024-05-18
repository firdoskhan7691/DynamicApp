using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;
using DynamicApplicationCP.Services;
using DynamicApplicationCP.Test;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DynamicApplicationCP.Tests
{
    public class CandidateServiceTests
    {
        private readonly Mock<IConfiguration> _configuration;
        public CandidateServiceTests()
        {
            _configuration = Utility.GetConfiguration();
        }
        [Fact]
        public async Task AddCandidateApplication_ValidCandidateModel_Success()
        {
            // Arrange

            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);
            cosmosDBService.Setup(s => s.CreateOrUpdateItemAsync(It.IsAny<CandidateModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var candidateService = new CandidateService(_configuration.Object, cosmosDBService.Object);

            var candidateModel = new CandidateModel
            {
                FirstName = "John",
                LastName = "Doe",
                // Add other required properties here
            };

            // Act
            await candidateService.AddCandidateApplication(candidateModel);

            // Assert
            Assert.NotNull(candidateModel.CandidateId);
            Assert.NotEmpty(candidateModel.CandidateId);
        }

        [Fact]
        public async Task AddCandidateApplication_NullCandidateModel_ThrowsArgumentNullException()
        {

            // Arrange
            var cosmosDBService = new Mock<ICosmosDBService>(MockBehavior.Strict);

            var candidateService = new CandidateService(_configuration.Object, cosmosDBService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await candidateService.AddCandidateApplication(null);
            });
        }
    }
}
