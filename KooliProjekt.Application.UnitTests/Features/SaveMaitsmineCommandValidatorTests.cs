using KooliProjekt.Application.Features.Maitsmised;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 06.02: isolated validator tests. No database or repository is required.
    public class SaveMaitsmineCommandValidatorTests
    {
        [Fact]
        public async Task Valid_command_has_no_errors()
        {
            var result = await new SaveMaitsmineCommandValidator().ValidateAsync(ValidCommand());
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
        public async Task Degusteerija_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Degusteerija = value;
            await AssertValidation(command, "Degusteerija", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(99, true)]
        [InlineData(100, true)]
        [InlineData(101, false)]
        public async Task Degusteerija_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Degusteerija = new string('x', length);
            await AssertValidation(command, "Degusteerija", valid);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        public async Task Kommentaar_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Kommentaar = value;
            await AssertValidation(command, "Kommentaar", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(254, true)]
        [InlineData(255, true)]
        [InlineData(256, false)]
        public async Task Kommentaar_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Kommentaar = new string('x', length);
            await AssertValidation(command, "Kommentaar", valid);
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

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public async Task Hinne_checks_range_boundaries(int value, bool valid)
        {
            var command = ValidCommand();
            command.Hinne = value;
            await AssertValidation(command, "Hinne", valid);
        }

        private static async Task AssertValidation(SaveMaitsmineCommand command, string property, bool valid)
        {
            var result = await new SaveMaitsmineCommandValidator().ValidateAsync(command);
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

        private static SaveMaitsmineCommand ValidCommand() => new SaveMaitsmineCommand
        {
            Id = 0,
            Degusteerija = "Valid text",
            Kommentaar = "Valid text",
            PartiiId = 1,
            Kuupaev = new DateTime(2026, 2, 6),
            Hinne = 7,
        };
    }
}
