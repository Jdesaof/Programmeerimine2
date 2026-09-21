using KooliProjekt.Application.Features.Partiid;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 06.02: isolated validator tests. No database or repository is required.
    public class SavePartiiCommandValidatorTests
    {
        [Fact]
        public async Task Valid_command_has_no_errors()
        {
            var result = await new SavePartiiCommandValidator().ValidateAsync(ValidCommand());
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
        public async Task OluId_must_be_positive(int value, bool valid)
        {
            var command = ValidCommand();
            command.OluId = value;
            await AssertValidation(command, "OluId", valid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public async Task Kood_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Kood = value;
            await AssertValidation(command, "Kood", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(49, true)]
        [InlineData(50, true)]
        [InlineData(51, false)]
        public async Task Kood_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Kood = new string('x', length);
            await AssertValidation(command, "Kood", valid);
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
        public async Task Tulemus_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.Tulemus = value;
            await AssertValidation(command, "Tulemus", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(499, true)]
        [InlineData(500, true)]
        [InlineData(501, false)]
        public async Task Tulemus_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.Tulemus = new string('x', length);
            await AssertValidation(command, "Tulemus", valid);
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

        private static async Task AssertValidation(SavePartiiCommand command, string property, bool valid)
        {
            var result = await new SavePartiiCommandValidator().ValidateAsync(command);
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

        private static SavePartiiCommand ValidCommand() => new SavePartiiCommand
        {
            Id = 0,
            Kood = "Valid text",
            Kirjeldus = "Valid text",
            Tulemus = "Valid text",
            OluId = 1,
            Kuupaev = new DateTime(2026, 2, 6),
        };
    }
}
