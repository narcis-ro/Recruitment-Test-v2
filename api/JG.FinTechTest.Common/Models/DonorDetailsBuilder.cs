using System;
using JG.FinTechTest.Domain.Data.Model;

namespace JG.FinTechTest.Common.Models
{
    internal class DonorDetailsBuilder
    {
        private DonorDetails _model;

        public DonorDetailsBuilder()
        {
            _model = new DonorDetails
            {
                FirstName = "FirstName",
                LastName = "LastName",
                PostCode = "12345"
            };
        }

        public DonorDetailsBuilder With(DonorDetails model)
        {
            _model = model;
            return this;
        }

        public DonorDetailsBuilder With(Action<DonorDetails> props)
        {
            props?.Invoke(_model);

            return this;
        }

        public DonorDetails Build()
        {
            return _model;
        }
    }
}
