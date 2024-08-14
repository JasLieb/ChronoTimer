using CommunityToolkit.Mvvm.ComponentModel;

namespace ChronoTimer.Core.ViewModels.Validation;

public partial class ValidatableObject<T> : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<string> _errors;
    
    [ObservableProperty]
    private bool _isValid;

    [ObservableProperty]
    private T? _value = default;

    private readonly IEnumerable<IValidationRule<T>> _validationsRules;
    
    public ValidatableObject(IEnumerable<IValidationRule<T>>? validationRules = null)
    {
        _validationsRules = validationRules ?? [];
        _isValid = true;
        _errors = [];
    }
    
    public bool Validate()
    {
        Errors = _validationsRules
            ?.Where(v => Value is not null && !v.Check(Value))
            ?.Select(v => v.ValidationMessage)
            ?.ToArray()
            ?? Enumerable.Empty<string>();
        IsValid = !Errors.Any();
        return IsValid;
    }
}
