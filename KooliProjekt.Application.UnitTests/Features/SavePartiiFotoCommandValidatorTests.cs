using KooliProjekt.Application.Features.PartiiFotod;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 06.02: isolated validator tests. No database or repository is required.
    public class SavePartiiFotoCommandValidatorTests
    {
        [Fact]
        public async Task Valid_command_has_no_errors()
        {
            var result = await new SavePartiiFotoCommandValidator().ValidateAsync(ValidCommand());
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
        public async Task FailiTee_checks_empty_values(string value, bool valid)
        {
            var command = ValidCommand();
            command.FailiTee = value;
            await AssertValidation(command, "FailiTee", valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(499, true)]
        [InlineData(500, true)]
        [InlineData(501, false)]
        public async Task FailiTee_checks_length_boundary(int length, bool valid)
        {
            var command = ValidCommand();
            command.FailiTee = new string('x', length);
            await AssertValidation(command, "FailiTee", valid);
        }

        private static async Task AssertValidation(SavePartiiFotoCommand command, string property, bool valid)
        {
            var result = await new SavePartiiFotoCommandValidator().ValidateAsync(command);
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

        private static SavePartiiFotoCommand ValidCommand() => new SavePartiiFotoCommand
        {
            Id = 0,
            FailiTee = "Valid text",
            PartiiId = 1,
        };
    }
}
