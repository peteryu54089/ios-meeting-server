using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MeetingSystem.Structure
{
    class PromotionSettingDataItem : INotifyPropertyChanged
    {
        private int promotionId;
        private string promotionSourcePath;
        private string promotionResultPath;
        private string promotionWebIp;
        private string webFileName;
        private PromotionSettingStatus status;

        public enum PromotionSettingStatus { ACTIVE, NONACTIVE, COMPLETE };

        public PromotionSettingDataItem(int promotionId, string promotionSourcePath, string promotionResultPath, string webFileName)
        {
            this.promotionId = promotionId;
            this.promotionSourcePath = promotionSourcePath;
            this.promotionResultPath = promotionResultPath;
            this.webFileName = webFileName;
            this.status = PromotionSettingStatus.NONACTIVE;
        }

        public int PromotionId
        {
            get { return promotionId; }
            set
            {
                promotionId = value;
                OnPropertyChanged("PromotionId");
            }
        }

        public string PromotionSourcePath
        {
            get { return promotionSourcePath; }
            set
            {
                promotionSourcePath = value;
                OnPropertyChanged("PromotionSourcePath");
            }
        }

        public string PromotionResultPath
        {
            get { return promotionResultPath; }
            set
            {
                promotionResultPath = value;
                OnPropertyChanged("PromotionResultPath");
            }
        }

        public string PromotionWebIp
        {
            get { return promotionWebIp; }
            set
            {
                promotionWebIp = value;
                OnPropertyChanged("PromotionWebIp");
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

        public PromotionSettingStatus Status
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
