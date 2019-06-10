using System;
using JG.FinTechTest.Common.Models;
using JG.FinTechTest.Domain.Requests;

namespace JG.FinTechTest.Domain.UnitTests.Handlers.Donation.Builders
{
    internal class FileGiftAidDeclarationResponseBuilder
    {
        private FileGiftAidDeclarationResponse _response;

        public FileGiftAidDeclarationResponseBuilder()
        {
            _response = new FileGiftAidDeclarationResponse
            {
                Declaration = new GiftAidDeclarationBuilderBuilder().Build()
            };
        }

        public FileGiftAidDeclarationResponseBuilder With(FileGiftAidDeclarationResponse response)
        {
            _response = response;

            return this;
        }

        public FileGiftAidDeclarationResponseBuilder With(Action<FileGiftAidDeclarationResponse> props)
        {
            props?.Invoke(_response);

            return this;
        }

        public FileGiftAidDeclarationResponse Build()
        {
            return _response;
        }
    }
}