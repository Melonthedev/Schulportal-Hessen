using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.Services;
public class ErrorService
{
    public event Action<string, string> OnErrorOccurred;

    public void TriggerError(string message, string title = "Error")
    {
        OnErrorOccurred?.Invoke(title, message);
    }
}
