namespace TestConsole
{
    using Microsoft.SharePoint;
    using System;

    /// <summary>
    /// Class used
    /// </summary>
    public sealed class DisableEventFiringScope : SPItemEventReceiver, IDisposable
    {
        bool _originalValue;

        public DisableEventFiringScope()
        {
            // Save off the original value of EventFiringEnabled 
            _originalValue = base.EventFiringEnabled;

            // Set EventFiringEnabled to false to disable it 
            base.EventFiringEnabled = false;
        }

        public void Dispose()
        {
            // Set EventFiringEnabled back to its original value 
            base.EventFiringEnabled = _originalValue;
        }
    }
}
