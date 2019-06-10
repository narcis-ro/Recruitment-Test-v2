using FluentValidation.TestHelper;
using JG.FinTechTest.Api.Models;
using JG.FinTechTest.Domain.Config;
using NUnit.Framework;

// ReSharper disable NUnit.RedundantArgumentInTestCaseAttribute

namespace JG.FinTechTest.UnitTests.Validators
{
    [TestFixture]
    public class DonationRequestValidatorTests
    {
        [Test]
        [TestCase(1, 2, 100)]
        [TestCase(100, 1, 100)]
        [TestCase(1, 2, 100)]
        [TestCase(0, 0, 100)]
        public void Amount_IsWithin_MinMaxRange(decimal amount, decimal min, decimal max)
        {
            var validator = Arrange(min, max);

            validator.ShouldHaveValidationErrorFor(r => r.Amount, amount);
        }


        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void FirstName_Is_Required(string firstName)
        {
            var validator = Arrange();

            validator.ShouldHaveValidationErrorFor(r => r.FirstName, firstName);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void LastName_Is_Required(string lastName)
        {
            var validator = Arrange();

            validator.ShouldHaveValidationErrorFor(r => r.LastName, lastName);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void PostCode_Is_Required(string postCode)
        {
            var validator = Arrange();

            validator.ShouldHaveValidationErrorFor(r => r.PostCode, postCode);
        }

        private DonationRequestValidator Arrange(decimal min = 0, decimal max = 0)
        {
           return new DonationRequestValidator(new DonationConfig
            {
                MinDonationAmount = min,
                MaxDonationAmount = max
            });
        }
    }
}
