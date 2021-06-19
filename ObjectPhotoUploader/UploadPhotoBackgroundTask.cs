using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace ObjectPhotoUploader
{
    public sealed class UploadPhotoBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            System.Diagnostics.Debug.WriteLine("Background " + taskInstance + " starting.");
            _deferral = taskInstance.GetDeferral();
            // Insert code here
            _deferral.Complete();
        }
    }
}
