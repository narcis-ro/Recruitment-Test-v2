using System;
using FluentValidation;
using JG.FinTechTest.Domain.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JG.FinTechTest.Models
{
    /// <summary>
    /// Request to calculate Gift-Aid tax amount
    /// </summary>
    public class GiftAidRequest
    {
        /// <summary>
        /// Donation amount
        /// </summary>
        [BindRequired] [FromQuery] public decimal Amount { get; set; }
    }

    // ReSharper disable once UnusedMember.Global
    /// <inheritdoc />
    public class GiftAidRequestValidator : AbstractValidator<GiftAidRequest>
    {
        private readonly DonationConfig _config;

        /// <inheritdoc />
        public GiftAidRequestValidator(DonationConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(_config.MinDonationAmount)
                .LessThan(_config.MaxDonationAmount ?? decimal.MaxValue);
        }
    }
}
