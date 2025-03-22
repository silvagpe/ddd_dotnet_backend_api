using FluentAssertions;
using SharpAbp.Abp.Snowflakes;

namespace DeveloperStore.UnitTests.Utils;

public class IdGeneratorTests
{

    [Fact]
    public void SnowflakeIdGenerator_ShouldGenerateUniqueIds()
    {
        // Arrange
        var snowflakeIdGenerator = new SharpAbp.Abp.Snowflakes.Snowflake(workerId: 1, datacenterId: 1);

        // Act
        var id1 = snowflakeIdGenerator.NextId();
        var id2 = snowflakeIdGenerator.NextId();

        // Assert
        id1.Should().NotBe(id2);
        id1.Should().BeGreaterThan(0);
        id2.Should().BeGreaterThan(0);
    }
}