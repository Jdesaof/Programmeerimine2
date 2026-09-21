using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Partiid;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class PartiiTests
    {
        [Fact]
        public async Task Get_should_return_dto_when_entity_exists()
        {
            var repository = new Mock<IPartiiRepository>(MockBehavior.Strict);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var entity = new Partii
            {
                Id = 17,
                OluId = 3,
                Kood = "Test Kood",
                Kuupaev = new DateTime(2026, 1, 16, 12, 0, 0),
                Kirjeldus = "Test Kirjeldus",
                Tulemus = "Test Tulemus",
            };
            repository.Setup(x => x.GetAsync(17, token)).ReturnsAsync(entity);
            var handler = new GetPartiiQueryHandler(repository.Object);

            var result = await handler.Handle(new GetPartiiQuery { Id = 17 }, token);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(17, result.Value.Id);
            Assert.Equal(3, result.Value.OluId);
            Assert.Equal("Test Kood", result.Value.Kood);
            Assert.Equal(new DateTime(2026, 1, 16, 12, 0, 0), result.Value.Kuupaev);
            Assert.Equal("Test Kirjeldus", result.Value.Kirjeldus);
            Assert.Equal("Test Tulemus", result.Value.Tulemus);
            repository.Verify(x => x.GetAsync(17, token), Times.Once);
            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_return_null_value_when_entity_does_not_exist()
        {
            var repository = new Mock<IPartiiRepository>(MockBehavior.Strict);
            repository.Setup(x => x.GetAsync(999, CancellationToken.None))
                .ReturnsAsync((Partii)null);
            var handler = new GetPartiiQueryHandler(repository.Object);

            var result = await handler.Handle(new GetPartiiQuery { Id = 999 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
            repository.Verify(x => x.GetAsync(999, CancellationToken.None), Times.Once);
            repository.VerifyNoOtherCalls();
        }

        // 22.01 supersedes the earlier 16.01 requirement for a null request.
        [Fact]
        public async Task Get_should_throw_ArgumentNullException_when_request_is_null()
        {
            var repository = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new GetPartiiQueryHandler(repository.Object);

            var error = await Assert.ThrowsAsync<ArgumentNullException>(
                () => handler.Handle(null, CancellationToken.None));

            Assert.Equal("request", error.ParamName);
            repository.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Get_should_not_call_repository_when_id_is_zero_or_negative(int id)
        {
            var repository = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new GetPartiiQueryHandler(repository.Object);

            var result = await handler.Handle(new GetPartiiQuery { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_propagate_repository_exception()
        {
            var repository = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var expected = new InvalidOperationException("Simulated repository failure.");
            repository.Setup(x => x.GetAsync(17, CancellationToken.None)).ThrowsAsync(expected);
            var handler = new GetPartiiQueryHandler(repository.Object);

            var actual = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(new GetPartiiQuery { Id = 17 }, CancellationToken.None));

            Assert.Same(expected, actual);
            repository.Verify(x => x.GetAsync(17, CancellationToken.None), Times.Once);
            repository.VerifyNoOtherCalls();
        }
    }
}
