﻿
namespace Infrastructure.Common.Validation
{
    public interface IValidationError
    {
        string Source { get; }
        string Address { get; }
        string Error { get; }
        ValidationErrorLevel ErrorLevel { get; }
        void Navigate();
    }
}