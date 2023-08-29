using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.JwtAuth.Client.Common.Classes;

public class CustomValidation : ComponentBase
{
    private ValidationMessageStore? _messageStore;

    [CascadingParameter] private EditContext? CurrentEditContext { get; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
            throw new InvalidOperationException(
                $"{nameof(CustomValidation)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. " +
                $"For example, you can use {nameof(CustomValidation)} " +
                $"inside an {nameof(EditForm)}.");

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (_, _) => _messageStore?.Clear();
        CurrentEditContext.OnFieldChanged += (_, e) => _messageStore?.Clear(e.FieldIdentifier);
    }

    public void DisplayErrors(Dictionary<string, List<string>> errors)
    {
        if (CurrentEditContext is null) return;
        foreach (var err in errors) _messageStore?.Add(CurrentEditContext.Field(err.Key), err.Value);

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public void ClearErrors()
    {
        _messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }
}
