using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MeetingSystem.Structure
{
    class AssessmentSettingDataItem : INotifyPropertyChanged
    {
        private int assessmentId;
        private string assessmentSourcePath;
        private string assessmentResultPath;
        private string assessmentWebIp;
        private string webFileName;
        private AssessmentSettingStatus status;

        public enum AssessmentSettingStatus { ACTIVE, NONACTIVE, COMPLETE };

        public AssessmentSettingDataItem(int assessmentId, string assessmentSourcePath, string assessmentResultPath, string webFileName)
        {
            this.assessmentId = assessmentId;
            this.assessmentSourcePath = assessmentSourcePath;
            this.assessmentResultPath = assessmentResultPath;
            this.webFileName = webFileName;
            this.status = AssessmentSettingStatus.NONACTIVE;
        }

        public int AssessmentId
        {
            get { return assessmentId; }
            set
            {
                assessmentId = value;
                OnPropertyChanged("AssessmentId");
            }
        }

        public string AssessmentSourcePath
        {
            get { return assessmentSourcePath; }
            set
            {
                assessmentSourcePath = value;
                OnPropertyChanged("AssessmentSourcePath");
            }
        }

        public string AssessmentResultPath
        {
            get { return assessmentResultPath; }
            set
            {
                assessmentResultPath = value;
                OnPropertyChanged("AssessmentResultPath");
            }
        }

        public string AssessmentWebIp
        {
            get { return assessmentWebIp; }
            set
            {
                assessmentWebIp = value;
                OnPropertyChanged("AssessmentWebIp");
            }
        }

        public string WebFileName
        {
            get { return webFileName; }
            set
            {
                webFileName = value;
                OnPropertyChanged("WebFileName");
            }
        }

        public AssessmentSettingStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create the OnPropertyChanged method to raise the event
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
