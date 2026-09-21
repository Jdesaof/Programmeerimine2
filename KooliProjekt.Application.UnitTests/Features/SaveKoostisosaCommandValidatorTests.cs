using KooliProjekt.Application.Features.Koostisosad;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 06.02: isolated validator tests. No database or repository is required.
    public class SaveKoostisosaCommandValidatorTests
    {
        [Fact]
        public async Task Valid_command_has_no_errors()
        {
            var result = await new SaveKoostisosaCommandValidator().ValidateAsync(ValidCommand());
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
        public async Task Nimetus_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Nimetus = value;
            await AssertValidation(command, "Nimetus", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(99, true)]
        [InlineData(100, true)]
        [InlineData(101, false)]
        public async Task Nimetus_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Nimetus = new string('x', length);
            await AssertValidation(command, "Nimetus", valid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public async Task Uhik_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Uhik = value;
            await AssertValidation(command, "Uhik", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(19, true)]
        [InlineData(20, true)]
        [InlineData(21, false)]
        public async Task Uhik_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Uhik = new string('x', length);
            await AssertValidation(command, "Uhik", valid);
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
        [InlineData("0", true)]
        [InlineData("-0.0001", false)]
        [InlineData("1.2345", true)]
        [InlineData("1.23456", false)]
        [InlineData("1.23000", true)]
        [InlineData("99999999999999.9999", true)]
        [InlineData("100000000000000", false)]
        public async Task Hind_checks_range_and_precision(string value, bool valid)
        {
            var command = ValidCommand();
            command.Hind = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            await AssertValidation(command, "Hind", valid);
        }

        [Theory]
        [InlineData("0", false)]
        [InlineData("-1", false)]
        [InlineData("0.0001", true)]
        [InlineData("1.2345", true)]
        [InlineData("1.23456", false)]
        [InlineData("1.23000", true)]
        [InlineData("99999999999999.9999", true)]
        [InlineData("100000000000000", false)]
        public async Task Kogus_checks_range_and_precision(string value, bool valid)
        {
            var command = ValidCommand();
            command.Kogus = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            await AssertValidation(command, "Kogus", valid);
        }

        private static async Task AssertValidation(SaveKoostisosaCommand command, string property, bool valid)
        {
            var result = await new SaveKoostisosaCommandValidator().ValidateAsync(command);
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

        private static SaveKoostisosaCommand ValidCommand() => new SaveKoostisosaCommand
        {
            Id = 0,
            Nimetus = "Valid text",
            Uhik = "Valid text",
            Kirjeldus = "Valid text",
            PartiiId = 1,
            Hind = 1.25m,
            Kogus = 1.25m,
        };
    }
}
