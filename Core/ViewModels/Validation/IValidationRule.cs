using System;

namespace ChronoTimer.Core.ViewModels.Validation;

public interface IValidationRule<T>
{
    string ValidationMessage { get; set; }
    bool Check(T value);
}
