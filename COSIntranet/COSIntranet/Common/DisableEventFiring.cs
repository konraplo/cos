namespace Change.Intranet.Common
{
    using Microsoft.SharePoint;
    using System;

    /// <summary>
    /// Class used for disable events firing
    /// </summary>
    public sealed class DisableEventFiring : SPItemEventReceiver, IDisposable
    {
        bool _originalValue;

        public DisableEventFiring()
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
