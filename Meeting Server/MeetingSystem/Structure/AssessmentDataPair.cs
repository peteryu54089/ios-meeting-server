using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MeetingSystem.Client;

namespace MeetingSystem.Structure
{
    class AssessmentDataPair : INotifyPropertyChanged
    {
        private AssessmentSettingDataItem settingDataItem = null;
        private List<object[]> dataItems = new List<object[]>();

        public AssessmentDataPair(AssessmentSettingDataItem settingDataItem)
        {
            this.settingDataItem = settingDataItem;
            settingDataItem.PropertyChanged += OnPropertyChanged;
        }

        public AssessmentSettingDataItem SettingDataItem
        {
            get { return settingDataItem; }
            set
            {
                settingDataItem = value;
                OnPropertyChanged("AssessmentSettingDataItems");
            }
        }

        public AssessmentSettingDataItem.AssessmentSettingStatus AssessmentSettingStatus
        {
            get { return settingDataItem.Status; }
        }

        public List<object[]> DataItems //ip 分數列表
        {
            get { return dataItems; }
            set
            {
                dataItems = value;
                OnPropertyChanged("AssessmentDataItems");
            }
        }

        public string ButtonName
        {
            get
            {
                return settingDataItem.Status == (AssessmentSettingDataItem.AssessmentSettingStatus.COMPLETE) ? "開票" : "結算";
            }
        }

        public string TableName
        {
            get
            {
                string path = SettingDataItem == null ? "" : SettingDataItem.AssessmentSourcePath;
                string name = path;

                if (name.Contains("."))
                {
                    name = path.Substring(path.LastIndexOf("\\") + 1);
                }
                if (name.Contains("."))
                {
                    name = name.Remove(name.LastIndexOf("."));
                }
                return name;
            }
        }

        public int AssessmentId
        {
            get
            {
                return SettingDataItem.AssessmentId;
            }
        }

        public int AssessmentDataItemCount
        {
            get { return DataItems.Count; }
        }

        public string AssessmentStatus
        {
            get
            {
                switch (settingDataItem.Status)
                {
                    case AssessmentSettingDataItem.AssessmentSettingStatus.ACTIVE:
                        return "投票中";
                    case AssessmentSettingDataItem.AssessmentSettingStatus.NONACTIVE:
                        return "NONACTIVE";
                    case AssessmentSettingDataItem.AssessmentSettingStatus.COMPLETE:
                        return "結算完成";
                }
                return "";
            }
        }

        /// <summary>
        /// 儲存 Assessment DataItem
        /// </summary>
        /// <param name="client"></param>
        /// <param name="assessmentDataItem"></param>
        public void SaveDataItem(MeetingClient client, AssessmentDataItem assessmentDataItem)
        {
            Console.Write(client.IpAddr + "    ");
            assessmentDataItem.Print();
            object[] existData = FindByIp(client.IpAddr);
            if (existData != null)
            {
                Console.WriteLine("####Error#### Repeat Assessment data");
                assessmentDataItem.Print();
                existData[1] = assessmentDataItem;
            }
            else
            {
                DataItems.Add(new object[] { client.IpAddr, assessmentDataItem });
            }
            OnPropertyChanged("AssessmentDataItems");
        }

        /// <summary>
        /// 以 ip 尋找 assessment data
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public object[] FindByIp(object ip)
        {
            for (int i = 0; i < DataItems.Count; i++)
            {
                object[] data = DataItems.ElementAt(i);
                if (data[0].Equals(ip))
                {
                    return data;
                }
            }
            return null;
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

        /// <summary>
        /// Create the OnPropertyChanged method to raise the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
    }
}
