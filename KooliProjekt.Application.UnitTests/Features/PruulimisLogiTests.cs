using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.PruulimisLogid;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class PruulimisLogiTests : ServiceTestBase
    {
        [Fact]
        public async Task Get_should_return_dto_when_entity_exists()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var entity = new PruulimisLogi
            {
                Id = 17,
                PartiiId = 3,
                Kuupaev = new DateTime(2026, 1, 16, 12, 0, 0),
                Kasutaja = "Test Kasutaja",
                Kirjeldus = "Test Kirjeldus",
            };
            repository.Setup(x => x.GetAsync(17, token)).ReturnsAsync(entity);
            var handler = new GetPruulimisLogiQueryHandler(repository.Object);

            var result = await handler.Handle(new GetPruulimisLogiQuery { Id = 17 }, token);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(17, result.Value.Id);
            Assert.Equal(3, result.Value.PartiiId);
            Assert.Equal(new DateTime(2026, 1, 16, 12, 0, 0), result.Value.Kuupaev);
            Assert.Equal("Test Kasutaja", result.Value.Kasutaja);
            Assert.Equal("Test Kirjeldus", result.Value.Kirjeldus);
            repository.Verify(x => x.GetAsync(17, token), Times.Once);
            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_return_null_value_when_entity_does_not_exist()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            repository.Setup(x => x.GetAsync(999, CancellationToken.None))
                .ReturnsAsync((PruulimisLogi)null);
            var handler = new GetPruulimisLogiQueryHandler(repository.Object);

            var result = await handler.Handle(new GetPruulimisLogiQuery { Id = 999 }, CancellationToken.None);

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
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var handler = new GetPruulimisLogiQueryHandler(repository.Object);

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
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var handler = new GetPruulimisLogiQueryHandler(repository.Object);

            var result = await handler.Handle(new GetPruulimisLogiQuery { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_propagate_repository_exception()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var expected = new InvalidOperationException("Simulated repository failure.");
            repository.Setup(x => x.GetAsync(17, CancellationToken.None)).ThrowsAsync(expected);
            var handler = new GetPruulimisLogiQueryHandler(repository.Object);

            var actual = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(new GetPruulimisLogiQuery { Id = 17 }, CancellationToken.None));

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
                DbContext.PruulimisLogid.Add(CreateListEntity(id));
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var handler = new ListPruulimisLogidQueryHandler(DbContext);

            var result = await handler.Handle(
                new ListPruulimisLogidQuery { Page = page, PageSize = 5 },
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
            var handler = new ListPruulimisLogidQueryHandler(DbContext);

            var result = await handler.Handle(
                new ListPruulimisLogidQuery { Page = 1, PageSize = 5 },
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
            DbContext.PruulimisLogid.Add(CreateListEntity(1));
            await DbContext.SaveChangesAsync();
            var handler = new ListPruulimisLogidQueryHandler(DbContext);

            var result = await handler.Handle(
                new ListPruulimisLogidQuery { Page = 1, PageSize = 100 },
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
            var handler = new ListPruulimisLogidQueryHandler(context);

            var error = await Assert.ThrowsAsync<ArgumentNullException>(
                () => handler.Handle(null, CancellationToken.None));

            Assert.Equal("request", error.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task List_should_throw_ArgumentException_when_page_is_invalid(int page)
        {
            var handler = new ListPruulimisLogidQueryHandler(DbContext);

            var error = await Assert.ThrowsAsync<ArgumentException>(
                () => handler.Handle(
                    new ListPruulimisLogidQuery { Page = page, PageSize = 5 },
                    CancellationToken.None));

            Assert.Equal("page", error.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(101)]
        public async Task List_should_throw_ArgumentException_when_page_size_is_invalid(int pageSize)
        {
            var handler = new ListPruulimisLogidQueryHandler(DbContext);

            var error = await Assert.ThrowsAsync<ArgumentException>(
                () => handler.Handle(
                    new ListPruulimisLogidQuery { Page = 1, PageSize = pageSize },
                    CancellationToken.None));

            Assert.Equal("pageSize", error.ParamName);
        }

        [Fact]
        public async Task List_should_propagate_database_failure()
        {
            using var context = GetFaultyDbContext();
            var handler = new ListPruulimisLogidQueryHandler(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(
                    new ListPruulimisLogidQuery { Page = 1, PageSize = 5 },
                    CancellationToken.None));
        }

        private static PruulimisLogi CreateListEntity(int id)
        {
            return new PruulimisLogi
            {
                Id = id,
                PartiiId = 3,
                Kuupaev = new DateTime(2026, 1, 16, 12, 0, 0),
                Kasutaja = "Test Kasutaja",
                Kirjeldus = "Test Kirjeldus",
            };
        }
        [Fact]
        public async Task Delete_should_remove_only_requested_entity_and_persist_changes()
        {
            DbContext.PruulimisLogid.Add(CreateListEntity(1));
            DbContext.PruulimisLogid.Add(CreateListEntity(2));
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var handler = new DeletePruulimisLogiCommandHandler(DbContext);

            var result = await handler.Handle(
                new DeletePruulimisLogiCommand { Id = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            // Clear tracked state: the assertion must check the saved store.
            DbContext.ChangeTracker.Clear();
            Assert.False(await DbContext.PruulimisLogid.AnyAsync(x => x.Id == 1));
            Assert.Equal(2, (await DbContext.PruulimisLogid.SingleAsync()).Id);
        }

        [Fact]
        public async Task Delete_should_succeed_when_entity_does_not_exist()
        {
            DbContext.PruulimisLogid.Add(CreateListEntity(1));
            await DbContext.SaveChangesAsync();
            var handler = new DeletePruulimisLogiCommandHandler(DbContext);

            var result = await handler.Handle(
                new DeletePruulimisLogiCommand { Id = 999 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            DbContext.ChangeTracker.Clear();
            Assert.Equal(1, (await DbContext.PruulimisLogid.SingleAsync()).Id);
        }

        [Fact]
        public async Task Delete_should_succeed_when_called_twice()
        {
            DbContext.PruulimisLogid.Add(CreateListEntity(1));
            await DbContext.SaveChangesAsync();
            var handler = new DeletePruulimisLogiCommandHandler(DbContext);

            var first = await handler.Handle(new DeletePruulimisLogiCommand { Id = 1 }, CancellationToken.None);
            var second = await handler.Handle(new DeletePruulimisLogiCommand { Id = 1 }, CancellationToken.None);

            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.False(first.HasErrors);
            Assert.False(second.HasErrors);
            DbContext.ChangeTracker.Clear();
            Assert.Empty(await DbContext.PruulimisLogid.ToListAsync());
        }

        [Fact]
        public async Task Delete_should_throw_ArgumentNullException_when_request_is_null()
        {
            using var context = GetFaultyDbContext();
            var handler = new DeletePruulimisLogiCommandHandler(context);

            var error = await Assert.ThrowsAsync<ArgumentNullException>(
                () => handler.Handle(null, CancellationToken.None));

            Assert.Equal("request", error.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_return_id_error_without_query_when_id_is_invalid(int id)
        {
            using var context = GetFaultyDbContext();
            var handler = new DeletePruulimisLogiCommandHandler(context);

            var result = await handler.Handle(
                new DeletePruulimisLogiCommand { Id = id }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.NotNull(result.PropertyErrors);
            Assert.Single(result.PropertyErrors);
            Assert.True(result.PropertyErrors.ContainsKey("Id"));
            Assert.False(string.IsNullOrWhiteSpace(result.PropertyErrors["Id"]));
        }

        [Fact]
        public async Task Delete_should_propagate_database_query_failure()
        {
            using var context = GetFaultyDbContext();
            var handler = new DeletePruulimisLogiCommandHandler(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(new DeletePruulimisLogiCommand { Id = 1 }, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_should_propagate_save_failure_without_persisting_delete()
        {
            using var context = GetDeleteSaveFailingDbContext();
            context.PruulimisLogid.Add(CreateListEntity(1));
            await context.SaveChangesAsync();
            context.FailOnSave = true;
            var handler = new DeletePruulimisLogiCommandHandler(context);

            var error = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(new DeletePruulimisLogiCommand { Id = 1 }, CancellationToken.None));

            Assert.Same(context.SaveFailure, error);
            context.ChangeTracker.Clear();
            Assert.True(await context.PruulimisLogid.AnyAsync(x => x.Id == 1));
        }

        // 05.02: Save handler behavior; validator rule boundaries are tested separately.
        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        public async Task Save_should_map_fields_and_return_saved_dto(int id)
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var request = CreateValidSaveRequest(id);
            var existing = new PruulimisLogi { Id = id };
            if (id != 0) repository.Setup(x => x.GetAsync(id, token)).ReturnsAsync(existing);
            parent.Setup(x => x.ExistsAsync(7, token)).ReturnsAsync(true);
            PruulimisLogi saved = null;
            repository.Setup(x => x.SaveAsync(It.IsAny<PruulimisLogi>(), token))
                .Callback<PruulimisLogi, CancellationToken>((entity, _) => {
                    Assert.Equal(id, entity.Id);
                    saved = entity;
                    if (id == 0) entity.Id = 42;
                }).Returns(Task.CompletedTask);

            var result = await handler.Handle(request, token);

            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            if (id != 0) Assert.Same(existing, saved);
            Assert.Equal(request.PartiiId, saved.PartiiId);
            Assert.Equal(request.Kuupaev, saved.Kuupaev);
            Assert.Equal(request.Kasutaja, saved.Kasutaja);
            Assert.Equal(request.Kirjeldus, saved.Kirjeldus);
            Assert.NotNull(result.Value);
            Assert.Equal(id == 0 ? 42 : id, result.Value.Id);
            Assert.Equal(request.PartiiId, result.Value.PartiiId);
            Assert.Equal(request.Kuupaev, result.Value.Kuupaev);
            Assert.Equal(request.Kasutaja, result.Value.Kasutaja);
            Assert.Equal(request.Kirjeldus, result.Value.Kirjeldus);
            if (id != 0) repository.Verify(x => x.GetAsync(id, token), Times.Once);
            repository.Verify(x => x.SaveAsync(saved, token), Times.Once);
            repository.VerifyNoOtherCalls();
            parent.Verify(x => x.ExistsAsync(7, token), Times.Once); parent.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Save_should_reject_null_request_without_repository_calls()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var error = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, token));
            Assert.Equal("request", error.ParamName);
            repository.VerifyNoOtherCalls(); parent.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        public async Task Save_should_return_validation_errors_without_repository_calls(int id)
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var request = CreateValidSaveRequest(id);
            request.Kasutaja = "";
            var result = await handler.Handle(request, token);
            Assert.True(result.HasErrors);
            Assert.Null(result.Value);
            Assert.False(string.IsNullOrWhiteSpace(result.PropertyErrors[nameof(request.Kasutaja)]));
            repository.VerifyNoOtherCalls(); parent.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Save_should_return_empty_result_when_update_target_is_missing()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            repository.Setup(x => x.GetAsync(17, token)).ReturnsAsync((PruulimisLogi)null);
            var result = await handler.Handle(CreateValidSaveRequest(17), token);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
            repository.Verify(x => x.GetAsync(17, token), Times.Once);
            repository.VerifyNoOtherCalls(); parent.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Save_should_propagate_lookup_failure()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var failure = new InvalidOperationException("Lookup failed");
            repository.Setup(x => x.GetAsync(17, token)).ThrowsAsync(failure);
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(CreateValidSaveRequest(17), token));
            Assert.Same(failure, error);
            repository.Verify(x => x.GetAsync(17, token), Times.Once);
            repository.VerifyNoOtherCalls(); parent.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        public async Task Save_should_propagate_save_failure(int id)
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            if (id != 0) repository.Setup(x => x.GetAsync(id, token)).ReturnsAsync(new PruulimisLogi { Id = id });
            parent.Setup(x => x.ExistsAsync(7, token)).ReturnsAsync(true);
            var failure = new InvalidOperationException("Save failed");
            repository.Setup(x => x.SaveAsync(It.IsAny<PruulimisLogi>(), token)).ThrowsAsync(failure);
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(CreateValidSaveRequest(id), token));
            Assert.Same(failure, error);
            if (id != 0) repository.Verify(x => x.GetAsync(id, token), Times.Once);
            repository.Verify(x => x.SaveAsync(It.IsAny<PruulimisLogi>(), token), Times.Once);
            repository.VerifyNoOtherCalls();
            parent.Verify(x => x.ExistsAsync(7, token), Times.Once); parent.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        public async Task Save_should_reject_missing_parent_without_saving(int id)
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var existing = new PruulimisLogi { Id = id };
            if (id != 0) repository.Setup(x => x.GetAsync(id, token)).ReturnsAsync(existing);
            parent.Setup(x => x.ExistsAsync(7, token)).ReturnsAsync(false);
            var result = await handler.Handle(CreateValidSaveRequest(id), token);
            Assert.True(result.HasErrors);
            Assert.Null(result.Value);
            Assert.False(string.IsNullOrWhiteSpace(result.PropertyErrors["PartiiId"]));
            Assert.Equal(0, existing.PartiiId);
            if (id != 0) repository.Verify(x => x.GetAsync(id, token), Times.Once);
            repository.VerifyNoOtherCalls();
            parent.Verify(x => x.ExistsAsync(7, token), Times.Once); parent.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Save_should_propagate_parent_lookup_failure()
        {
            var repository = new Mock<IPruulimisLogiRepository>(MockBehavior.Strict);
            var parent = new Mock<IPartiiRepository>(MockBehavior.Strict);
            var handler = new SavePruulimisLogiCommandHandler(repository.Object, parent.Object);
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var failure = new InvalidOperationException("Parent lookup failed");
            parent.Setup(x => x.ExistsAsync(7, token)).ThrowsAsync(failure);
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(CreateValidSaveRequest(0), token));
            Assert.Same(failure, error);
            repository.VerifyNoOtherCalls();
            parent.Verify(x => x.ExistsAsync(7, token), Times.Once); parent.VerifyNoOtherCalls();
        }


        private static SavePruulimisLogiCommand CreateValidSaveRequest(int id)
        {
            return new SavePruulimisLogiCommand
            {
                Id = id,
                PartiiId = 7,
                Kuupaev = new DateTime(2026, 2, 5),
                Kasutaja = "Brewer",
                Kirjeldus = "New log",
            };
        }
    }
}
