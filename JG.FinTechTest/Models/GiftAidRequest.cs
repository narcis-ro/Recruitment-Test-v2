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
        /// <inheritdoc />
        public GiftAidRequestValidator(DonationConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(config.MinDonationAmount)
                .LessThan(config.MaxDonationAmount ?? decimal.MaxValue);
        }
    }
}
