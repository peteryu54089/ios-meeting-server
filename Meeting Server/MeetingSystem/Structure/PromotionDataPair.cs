using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MeetingSystem.Client;

namespace MeetingSystem.Structure
{
    class PromotionDataPair : INotifyPropertyChanged
    {
        private PromotionSettingDataItem settingDataItem = null;
        private List<object[]> dataItems = new List<object[]>();

        public PromotionDataPair(PromotionSettingDataItem settingDataItem)
        {
            this.settingDataItem = settingDataItem;
            settingDataItem.PropertyChanged += OnPropertyChanged;
        }

        public PromotionSettingDataItem SettingDataItem
        {
            get { return settingDataItem; }
            set
            {
                settingDataItem = value;
                OnPropertyChanged("PromotionSettingDataItems");
            }
        }

        public PromotionSettingDataItem.PromotionSettingStatus PromotionSettingStatus
        {
            get { return settingDataItem.Status; }
        }

        public List<object[]> DataItems //ip 分數列表
        {
            get { return dataItems; }
            set
            {
                dataItems = value;
                OnPropertyChanged("PromotionDataItems");
            }
        }

        public string ButtonName
        {
            get
            {
                return settingDataItem.Status == (PromotionSettingDataItem.PromotionSettingStatus.COMPLETE) ? "開票" : "結算";
            }
        }

        public string TableName
        {
            get
            {
                string path = SettingDataItem == null ? "" : SettingDataItem.PromotionSourcePath;
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

        public int PromotionId
        {
            get
            {
                return SettingDataItem.PromotionId;
            }
        }

        public int PromotionDataItemCount
        {
            get { return DataItems.Count; }
        }

        public string PromotionStatus
        {
            get
            {
                switch (settingDataItem.Status)
                {
                    case PromotionSettingDataItem.PromotionSettingStatus.ACTIVE:
                        return "投票中";
                    case PromotionSettingDataItem.PromotionSettingStatus.NONACTIVE:
                        return "NONACTIVE";
                    case PromotionSettingDataItem.PromotionSettingStatus.COMPLETE:
                        return "結算完成";
                }
                return "";
            }
        }

        /// <summary>
        /// 儲存 Promotion DataItem
        /// </summary>
        /// <param name="client"></param>
        /// <param name="promotionDataItem"></param>
        public void SaveDataItem(MeetingClient client, PromotionDataItem promotionDataItem)
        {
            Console.Write(client.IpAddr + "    ");
            promotionDataItem.Print();
            object[] existData = FindByIp(client.IpAddr);
            if (existData != null)
            {
                Console.WriteLine("####Error#### Repeat Promotion data");
                promotionDataItem.Print();
                existData[1] = promotionDataItem;
            }
            else
            {
                DataItems.Add(new object[] { client.IpAddr, promotionDataItem });
            }
            OnPropertyChanged("PromotionDataItems");
        }

        /// <summary>
        /// 以 ip 尋找 promotion data
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
