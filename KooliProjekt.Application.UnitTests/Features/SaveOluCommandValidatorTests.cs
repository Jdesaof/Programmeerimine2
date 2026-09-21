using KooliProjekt.Application.Features.Olud;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 06.02: isolated validator tests. No database or repository is required.
    public class SaveOluCommandValidatorTests
    {
        [Fact]
        public async Task Valid_command_has_no_errors()
        {
            var result = await new SaveOluCommandValidator().ValidateAsync(ValidCommand());
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
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public async Task Nimi_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Nimi = value;
            await AssertValidation(command, "Nimi", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(99, true)]
        [InlineData(100, true)]
        [InlineData(101, false)]
        public async Task Nimi_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Nimi = new string('x', length);
            await AssertValidation(command, "Nimi", valid);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        public async Task Kirjeldus_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Kirjeldus = value;
            await AssertValidation(command, "Kirjeldus", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(254, true)]
        [InlineData(255, true)]
        [InlineData(256, false)]
        public async Task Kirjeldus_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Kirjeldus = new string('x', length);
            await AssertValidation(command, "Kirjeldus", valid);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        public async Task Tuup_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Tuup = value;
            await AssertValidation(command, "Tuup", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(49, true)]
        [InlineData(50, true)]
        [InlineData(51, false)]
        public async Task Tuup_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Tuup = new string('x', length);
            await AssertValidation(command, "Tuup", valid);
        }

        [Theory]
        [InlineData("0", true)]
        [InlineData("100", true)]
        [InlineData("-0.01", false)]
        [InlineData("100.01", false)]
        [InlineData("5.12", true)]
        [InlineData("5.123", false)]
        [InlineData("5.1200", true)]
        public async Task Alkoholiprotsent_checks_range_and_precision(string value, bool valid)
        {
            var command = ValidCommand();
            command.Alkoholiprotsent = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            await AssertValidation(command, "Alkoholiprotsent", valid);
        }

        private static async Task AssertValidation(SaveOluCommand command, string property, bool valid)
        {
            var result = await new SaveOluCommandValidator().ValidateAsync(command);
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

        private static SaveOluCommand ValidCommand() => new SaveOluCommand
        {
            Id = 0,
            Nimi = "Valid text",
            Kirjeldus = "Valid text",
            Tuup = "Valid text",
            Alkoholiprotsent = 1.25m,
        };
    }
}
