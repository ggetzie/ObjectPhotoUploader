using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public int Progress { get; set; }
        //{
        //    get
        //    {
        //        return Progress;
        //    }
        //    set
        //    {
        //        Progress = value;
        //        OnPropertyChanged();
        //    }
        //}
        public Visibility ProgressVisibility { get; set; }
        //{
        //    get
        //    {
        //        return ProgressVisibility;
        //    }
        //    set
        //    {
        //        ProgressVisibility = value;
        //        OnPropertyChanged();
        //    }
        //}
        public string ProgressStatus { get; set; }
        //{
        //    get
        //    {
        //        return ProgressStatus;
        //    }
        //    set
        //    {
        //        ProgressStatus = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
