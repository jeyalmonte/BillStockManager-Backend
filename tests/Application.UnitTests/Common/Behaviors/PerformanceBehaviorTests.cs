namespace Application.UnitTests.Common.Behaviors;
using Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class PerformanceBehaviorTests
{

	private readonly Mock<ILogger<TestRequest>> _loggerMock = new();

	[Fact]
	public async Task PerformanceBehavior_WhenExecutionTakesMoreThan500ms_ShouldLogWarning()
	{
		// Arrange
		var behavior = new PerformanceBehavior<TestRequest, string>(_loggerMock.Object);

		var responseValue = "OK";
		var delayedHandler = new RequestHandlerDelegate<string>(async () =>
		{
			await Task.Delay(600);
			return responseValue;
		});

		// Act
		var result = await behavior.Handle(new TestRequest("slow"), delayedHandler, CancellationToken.None);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				It.Is<LogLevel>(level => level == LogLevel.Warning),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);

		result.Should().Be(responseValue);
	}

	[Fact]
	public async Task PerformanceBehavior_WhenExecutionIsFast_ShouldNotLogWarning()
	{
		// Arrange
		var behavior = new PerformanceBehavior<TestRequest, string>(_loggerMock.Object);

		var responseValue = "OK";
		var fastHandler = new RequestHandlerDelegate<string>(() => Task.FromResult(responseValue));

		// Act
		var result = await behavior.Handle(new TestRequest("fast"), fastHandler, CancellationToken.None);

		// Assert
		result.Should().Be(responseValue);
		_loggerMock.Verify(
			x => x.Log(
				It.Is<LogLevel>(level => level == LogLevel.Warning),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Never);
	}

	[Fact]
	public async Task PerformanceBehavior_ShouldReturnResponseFromHandler()
	{
		// Arrange
		var behavior = new PerformanceBehavior<TestRequest, string>(_loggerMock.Object);

		var responseValue = "expected";
		var handler = new RequestHandlerDelegate<string>(() => Task.FromResult(responseValue));

		// Act
		var result = await behavior.Handle(new TestRequest("any"), handler, CancellationToken.None);

		// Assert
		result.Should().Be(responseValue);
	}

	public record TestRequest(string Data) : IRequest<string>;
}
