using System.Collections.Specialized;

namespace Schulportal_Hessen.Contracts.Services;

public interface IAppNotificationService {
    void Initialize();

    bool Show(string payload);

    NameValueCollection ParseArguments(string arguments);

    void Unregister();
}
