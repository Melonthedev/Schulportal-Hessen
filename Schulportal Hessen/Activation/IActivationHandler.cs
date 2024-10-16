namespace Schulportal_Hessen.Activation;

public interface IActivationHandler {
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
