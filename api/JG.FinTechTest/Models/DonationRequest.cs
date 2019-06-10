using System;
using FluentValidation;
using JG.FinTechTest.Domain.Config;

namespace JG.FinTechTest.Api.Models
{
    /// <summary>
    /// Request to calculate Gift-Aid tax amount
    /// </summary>
    public class DonationRequest
    {
        /// <summary>
        /// Donation amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Donor first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Donor last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Donor address postal code
        /// </summary>
        public string PostCode { get; set; }
    }

    // ReSharper disable once UnusedMember.Global
    /// <inheritdoc />
    public class DonationRequestValidator : AbstractValidator<DonationRequest>
    {
        /// <inheritdoc />
        public DonationRequestValidator(DonationConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            RuleFor(x => x.Amount)
                .NotEmpty().GreaterThan(config.MinDonationAmount).LessThan(config.MaxDonationAmount ?? decimal.MaxValue);

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.PostCode).NotEmpty();

            // TODO: Do proper postal code validation
        }
    }
}
