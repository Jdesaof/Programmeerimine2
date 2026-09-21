using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Koostisosad;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class KoostisosaTests : ServiceTestBase
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
        [Theory]
        [InlineData(1, 5, 1)]
        [InlineData(2, 5, 6)]
        [InlineData(3, 2, 11)]
        [InlineData(4, 0, 16)]
        public async Task List_should_return_requested_page_in_id_order(
            int page, int expectedCount, int firstId)
        {
            // Insert in reverse order to detect a missing OrderBy.
            for (int id = 12; id >= 1; id--)
                DbContext.Koostisosad.Add(CreateListEntity(id));
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var handler = new ListKoostisosadQueryHandler(DbContext);

            var result = await handler.Handle(
                new ListKoostisosadQuery { Page = page, PageSize = 5 },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(page, result.Value.CurrentPage);
            Assert.Equal(5, result.Value.PageSize);
            Assert.Equal(12, result.Value.RowCount);
            Assert.Equal(3, result.Value.PageCount);
            Assert.Equal(expectedCount, result.Value.Results.Count);
            Assert.Equal(
                Enumerable.Range(firstId, expectedCount),
                result.Value.Results.Select(x => x.Id));
            Assert.Empty(DbContext.ChangeTracker.Entries());
        }

        [Fact]
        public async Task List_should_return_empty_page_when_database_is_empty()
        {
            var handler = new ListKoostisosadQueryHandler(DbContext);

            var result = await handler.Handle(
                new ListKoostisosadQuery { Page = 1, PageSize = 5 },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
            Assert.Equal(0, result.Value.RowCount);
            Assert.Equal(0, result.Value.PageCount);
            Assert.Equal(1, result.Value.CurrentPage);
            Assert.Equal(5, result.Value.PageSize);
        }

        [Fact]
        public async Task List_should_accept_maximum_page_size()
        {
            DbContext.Koostisosad.Add(CreateListEntity(1));
            await DbContext.SaveChangesAsync();
            var handler = new ListKoostisosadQueryHandler(DbContext);

            var result = await handler.Handle(
                new ListKoostisosadQuery { Page = 1, PageSize = 100 },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(100, result.Value.PageSize);
            Assert.Equal(1, result.Value.RowCount);
            Assert.Equal(1, result.Value.PageCount);
            Assert.Equal(1, Assert.Single(result.Value.Results).Id);
        }

        [Fact]
        public async Task List_should_throw_ArgumentNullException_when_request_is_null()
        {
            using var context = GetFaultyDbContext();
            var handler = new ListKoostisosadQueryHandler(context);

            var error = await Assert.ThrowsAsync<ArgumentNullException>(
                () => handler.Handle(null, CancellationToken.None));

            Assert.Equal("request", error.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task List_should_throw_ArgumentException_when_page_is_invalid(int page)
        {
            var handler = new ListKoostisosadQueryHandler(DbContext);

            var error = await Assert.ThrowsAsync<ArgumentException>(
                () => handler.Handle(
                    new ListKoostisosadQuery { Page = page, PageSize = 5 },
                    CancellationToken.None));

            Assert.Equal("page", error.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(101)]
        public async Task List_should_throw_ArgumentException_when_page_size_is_invalid(int pageSize)
        {
            var handler = new ListKoostisosadQueryHandler(DbContext);

            var error = await Assert.ThrowsAsync<ArgumentException>(
                () => handler.Handle(
                    new ListKoostisosadQuery { Page = 1, PageSize = pageSize },
                    CancellationToken.None));

            Assert.Equal("pageSize", error.ParamName);
        }

        [Fact]
        public async Task List_should_propagate_database_failure()
        {
            using var context = GetFaultyDbContext();
            var handler = new ListKoostisosadQueryHandler(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(
                    new ListKoostisosadQuery { Page = 1, PageSize = 5 },
                    CancellationToken.None));
        }

        private static Koostisosa CreateListEntity(int id)
        {
            return new Koostisosa
            {
                Id = id,
                PartiiId = 3,
                Nimetus = "Test Nimetus",
                Uhik = "Test Uhik",
                Hind = 2.5m,
                Kogus = 4m,
                Kirjeldus = "Test Kirjeldus",
            };
        }
    }
}
