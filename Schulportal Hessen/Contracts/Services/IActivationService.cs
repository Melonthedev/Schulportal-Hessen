namespace Schulportal_Hessen.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
