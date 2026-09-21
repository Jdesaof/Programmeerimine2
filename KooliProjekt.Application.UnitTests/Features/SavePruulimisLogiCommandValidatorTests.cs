using KooliProjekt.Application.Features.PruulimisLogid;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 06.02: isolated validator tests. No database or repository is required.
    public class SavePruulimisLogiCommandValidatorTests
    {
        [Fact]
        public async Task Valid_command_has_no_errors()
        {
            var result = await new SavePruulimisLogiCommandValidator().ValidateAsync(ValidCommand());
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2147483647, true)]
        public async Task Id_must_be_nonnegative(int value, bool valid)
        {
            var command = ValidCommand();
            command.Id = value;
            await AssertValidation(command, "Id", valid);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2147483647, true)]
        public async Task PartiiId_must_be_positive(int value, bool valid)
        {
            var command = ValidCommand();
            command.PartiiId = value;
            await AssertValidation(command, "PartiiId", valid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public async Task Kasutaja_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Kasutaja = value;
            await AssertValidation(command, "Kasutaja", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(99, true)]
        [InlineData(100, true)]
        [InlineData(101, false)]
        public async Task Kasutaja_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Kasutaja = new string('x', length);
            await AssertValidation(command, "Kasutaja", valid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public async Task Kirjeldus_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Kirjeldus = value;
            await AssertValidation(command, "Kirjeldus", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(1999, true)]
        [InlineData(2000, true)]
        [InlineData(2001, false)]
        public async Task Kirjeldus_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Kirjeldus = new string('x', length);
            await AssertValidation(command, "Kirjeldus", valid);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public async Task Kuupaev_must_be_set(bool setDate, bool valid)
        {
            var command = ValidCommand();
            command.Kuupaev = setDate ? new DateTime(2026, 2, 6) : default;
            await AssertValidation(command, "Kuupaev", valid);
        }

        private static async Task AssertValidation(SavePruulimisLogiCommand command, string property, bool valid)
        {
            var result = await new SavePruulimisLogiCommandValidator().ValidateAsync(command);
            Assert.Equal(valid, result.IsValid);
            if (valid) Assert.Empty(result.Errors);
            else
            {
                Assert.NotEmpty(result.Errors);
                Assert.All(result.Errors, error =>
                {
                    Assert.Equal(property, error.PropertyName);
                    Assert.False(string.IsNullOrWhiteSpace(error.ErrorMessage));
                });
            }
        }

        private static SavePruulimisLogiCommand ValidCommand() => new SavePruulimisLogiCommand
        {
            Id = 0,
            Kasutaja = "Valid text",
            Kirjeldus = "Valid text",
            PartiiId = 1,
            Kuupaev = new DateTime(2026, 2, 6),
        };
    }
}
