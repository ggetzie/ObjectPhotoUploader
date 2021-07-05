using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace ObjectPhotoUploader
{
    public class FindPhotoFile : INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ObjectFind Find { get; set; }
        public StorageFile LocalFile { get; set; }
        public bool IsUploaded { get; set; }

        private int _progress { get; set; }
        public int Progress 
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                OnPropertyChanged();
}
        }

        private Visibility _progressVisibility { get; set; }
        public Visibility ProgressVisibility
        {
            get
            {
                return _progressVisibility;
            }
            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }
        private string _progressStatus { get; set; }
        public string ProgressStatus
        {
            get
            {
                return _progressStatus;
            }
            set
            {
                _progressStatus = value;
                OnPropertyChanged();
            }
        }

        public FindPhotoFile(ObjectFind find, StorageFile localFile, bool isUploaded, int progress,
            Visibility visibility, string status)
        {
            this.Find = find;
            this.LocalFile = localFile;
            this.IsUploaded = isUploaded;
            this.Progress = progress;
            this.ProgressVisibility = visibility;
            this.ProgressStatus = status;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
