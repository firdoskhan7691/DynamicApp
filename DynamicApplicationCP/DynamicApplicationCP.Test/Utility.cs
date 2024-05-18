using Microsoft.Extensions.Configuration;
using Moq;

namespace DynamicApplicationCP.Test
{
    public class Utility
    {
        public static Mock<IConfiguration> GetConfiguration()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["CosmosDB:DatabaseName"]).Returns("testDatabase");
            configuration.Setup(c => c["CosmosDB:ContainerName"]).Returns("testContainer");
            configuration.Setup(c => c["CosmosDB:CandidateContainerName"]).Returns("testContainer");
            configuration.Setup(c => c["CosmosDB:QuestionContainerName"]).Returns("testContainer");
            return configuration;
        }
    }
}
