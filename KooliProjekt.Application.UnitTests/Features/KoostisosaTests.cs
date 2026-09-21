using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Koostisosad;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class KoostisosaTests
    {
        [Fact]
        public async Task Get_should_return_dto_when_entity_exists()
        {
            var repository = new Mock<IKoostisosaRepository>(MockBehavior.Strict);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var entity = new Koostisosa
            {
                Id = 17,
                PartiiId = 3,
                Nimetus = "Test Nimetus",
                Uhik = "Test Uhik",
                Hind = 2.5m,
                Kogus = 4m,
                Kirjeldus = "Test Kirjeldus",
            };
            repository.Setup(x => x.GetAsync(17, token)).ReturnsAsync(entity);
            var handler = new GetKoostisosaQueryHandler(repository.Object);

            var result = await handler.Handle(new GetKoostisosaQuery { Id = 17 }, token);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(17, result.Value.Id);
            Assert.Equal(3, result.Value.PartiiId);
            Assert.Equal("Test Nimetus", result.Value.Nimetus);
            Assert.Equal("Test Uhik", result.Value.Uhik);
            Assert.Equal(2.5m, result.Value.Hind);
            Assert.Equal(4m, result.Value.Kogus);
            Assert.Equal("Test Kirjeldus", result.Value.Kirjeldus);
            Assert.Equal(10m, result.Value.Summa);
            repository.Verify(x => x.GetAsync(17, token), Times.Once);
            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_return_null_value_when_entity_does_not_exist()
        {
            var repository = new Mock<IKoostisosaRepository>(MockBehavior.Strict);
            repository.Setup(x => x.GetAsync(999, CancellationToken.None))
                .ReturnsAsync((Koostisosa)null);
            var handler = new GetKoostisosaQueryHandler(repository.Object);

            var result = await handler.Handle(new GetKoostisosaQuery { Id = 999 }, CancellationToken.None);

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
            var repository = new Mock<IKoostisosaRepository>(MockBehavior.Strict);
            var handler = new GetKoostisosaQueryHandler(repository.Object);

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
            var repository = new Mock<IKoostisosaRepository>(MockBehavior.Strict);
            var handler = new GetKoostisosaQueryHandler(repository.Object);

            var result = await handler.Handle(new GetKoostisosaQuery { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_propagate_repository_exception()
        {
            var repository = new Mock<IKoostisosaRepository>(MockBehavior.Strict);
            var expected = new InvalidOperationException("Simulated repository failure.");
            repository.Setup(x => x.GetAsync(17, CancellationToken.None)).ThrowsAsync(expected);
            var handler = new GetKoostisosaQueryHandler(repository.Object);

            var actual = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(new GetKoostisosaQuery { Id = 17 }, CancellationToken.None));

            Assert.Same(expected, actual);
            repository.Verify(x => x.GetAsync(17, CancellationToken.None), Times.Once);
            repository.VerifyNoOtherCalls();
        }
    }
}
