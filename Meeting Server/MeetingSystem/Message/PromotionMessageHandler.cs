using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Client;
using System.IO;
using MeetingSystem.Utils;
using System.ComponentModel;
using MeetingSystem.Structure;
using System.Collections;
using System.Globalization;
using MeetingSystem.Promotion;

namespace MeetingSystem.Message
{
    class PromotionMessageHandler : Message
    {
        private Hashtable promotionDataItems = null;
        private BindingList<PromotionDataPair> promotionDataPairs = new BindingList<PromotionDataPair>();

        public enum PromotionMessageMode { PromotionRequest, PromotionResponse };
        public ArrayList collection = new ArrayList(); // save the ip and promotion which rearrangement
        public Random random = new Random();

        public BindingList<PromotionSettingDataItem> SettingDataItems
        {
            get { return (BindingList<PromotionSettingDataItem>)parm; }
            set { parm = value; }
        }

        public Hashtable PromotionDataItems
        {
            get { return promotionDataItems; }
            set { promotionDataItems = value; }
        }

        public BindingList<PromotionDataPair> PromotionDataPairs
        {
            get { return promotionDataPairs; }
            set { promotionDataPairs = value; }
        }

        private PromotionMessageHandler()
        {
            parm = new BindingList<PromotionSettingDataItem>();
            SettingDataItems.ListChanged += new ListChangedEventHandler(PromotionSettingItems_ListChanged);
        }

        /// <summary>
        /// PromotionSettingItems_ListChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PromotionSettingItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                return;
            }
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                PromotionSettingItem_Rmove(e.NewIndex);
                return;
            }
            PromotionSettingItem_Add();
        }

        /// <summary>
        /// PromotionSettingItem_Add
        /// </summary>
        private void PromotionSettingItem_Add()
        {
            foreach (PromotionSettingDataItem settingDataItem in SettingDataItems)
            {
                PromotionDataPair targetDataPair = promotionDataPairs.FirstOrDefault(pair => pair.SettingDataItem == settingDataItem);
                bool isExist = (targetDataPair != null);
                if (!isExist) //不存在所以新增
                {
                    PromotionDataPair dataPair = new PromotionDataPair(settingDataItem);
                    promotionDataPairs.Add(dataPair);
                }
            }
        }

        /// <summary>
        /// OpenResultExcel
        /// </summary>
        /// <param name="id"></param>
        internal void OpenResultExcel(int id)
        {
            PromotionDataPair dataPair = promotionDataPairs.FirstOrDefault(pair => pair.PromotionId == id);
            string path = dataPair.SettingDataItem.PromotionResultPath;
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// PromotionSettingItem_Rmove
        /// </summary>
        /// <param name="affectIndex"></param>
        private void PromotionSettingItem_Rmove(int affectIndex)
        {
            if (affectIndex < promotionDataPairs.Count)
            {
                PromotionSettingDataItem promotionSettingDataItem = promotionDataPairs[affectIndex].SettingDataItem;
                if (!SettingDataItems.Contains(promotionSettingDataItem))
                {
                    promotionDataPairs.RemoveAt(affectIndex);
                }
            }
        }
        
        /// <summary>
        /// Initialize
        /// </summary>
        public static void Init()
        {
            MessagePool.registerMessage(ClientMessage.PromotionMessage, new PromotionMessageHandler());
        }

        /// <summary>
        /// CalcVoteResult
        /// </summary>
        /// <param name="id"></param>
        public void CalcVoteResult(int id)
        {
            lock (promotionDataPairs)
            {
                try
                {
                    PromotionDataPair dataPair = promotionDataPairs.FirstOrDefault(pair => pair.SettingDataItem.PromotionId == id);
                    if (dataPair.DataItems.Count <= 0)
                    {
                        return;
                    }
                    DateTime time_start = DateTime.Now;
                    DateTime time_saveBallot = DateTime.Now;
                    SavePromotionResult(dataPair);//存投票結果
                    DateTime time_saveResult = DateTime.Now;
                    EventLog.Write("存每一張票的時間 " + ((TimeSpan)(time_saveBallot - time_start)).TotalMilliseconds.ToString() + " 存投票結果時間 " + ((TimeSpan)(time_saveResult - time_saveBallot)).TotalMilliseconds.ToString());
                    // setToCompleteDataItem(dataPair); 
                }
                catch (Exception e)
                {
                    NotifyStatus(e.Message);
                    Console.WriteLine(e.Message);
                    EventLog.Write(e.Message);
                }
            }
        }

        /// <summary>
        /// Date
        /// </summary>
        /// <returns></returns>
        public string Date()
        {
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            string today = string.Format("{0}.{1}.{2}", taiwanCalendar.GetYear(DateTime.Now), DateTime.Now.Month, DateTime.Now.Day);
            return today;
        }

        /// <summary>
        /// save promotion result
        /// </summary>
        /// <param name="dataPair"></param>
        private void SavePromotionResult(PromotionDataPair dataPair) // 結果檔
        {
            PromotionResultWriter writer = new PromotionResultWriter(dataPair.SettingDataItem.PromotionSourcePath);
            string title = writer.ReadTitle();
            string date = writer.ReadDate();
            writer = new PromotionResultWriter(dataPair.SettingDataItem.PromotionResultPath);

            int committeeCount = dataPair.DataItems.Count; // 評委數量
            int candidateListCount = ((PromotionDataItem)dataPair.DataItems[0][1]).CandidateList.Count; // 候選人數量
            
            double[,] detailArray = new double[candidateListCount, committeeCount]; // 選票明細陣列, 一維為候選人, 二維為評委
            double[,] scoreArray = new double[candidateListCount, 11];
            double[] scoreTemp = new double[candidateListCount];

            List<double> candidateAverage = new List<double>(); // 候選人平均得分
            List<string> departments = new List<string>();
            List<string> names = new List<string>();
            List<double> scoreB = new List<double>();
            List<double> scoreC = new List<double>();
            List<string> ips = new List<string>();

            for (int i = 0; i < committeeCount; i++) // 哪幾位評委
            {
                PromotionDataItem item = (PromotionDataItem)dataPair.DataItems[i][1];
                string fullIp = ((System.Net.IPAddress)(dataPair.DataItems[i][0])).ToString();
                ips.Add(fullIp.Substring(fullIp.LastIndexOf('.') + 1)); // 只取 ip 末三碼

                if (i == 0)
                {
                    for (int j = 0; j < candidateListCount; j++) // 哪幾位候選人
                    {
                        departments.Add(item.CandidateList[j].Department);
                        names.Add(item.CandidateList[j].Name);

                        string[] scores = item.CandidateList[j].Scores.Split(',');
                        for (int k = 0; k <= 10; k++)
                        {
                            scoreArray[j, k] = Convert.ToDouble(scores[k]);
                        }
                    }
                }

                for (int j = 0; j < candidateListCount; j++) // 哪幾位候選人
                {
                    string[] scores = item.CandidateList[j].Scores.Split(',');
                    scoreTemp[j] += Convert.ToDouble(scores[11]);
                    detailArray[j, i] = Convert.ToDouble(scores[11]); // 存選票明細
                }
            }

            for (int i = 0; i < candidateListCount; i++)
            {
                double total = scoreTemp[i];
                double count = committeeCount;
                scoreTemp[i] = Math.Round(total / count, 1);
            }

            for (int i = 0; i < candidateListCount; i++)
            {
                scoreB.Add(scoreTemp[i] + scoreArray[i, 6] + scoreArray[i, 7]);
            }

            for (int i = 0; i < candidateListCount; i++)
            {
                scoreC.Add(scoreB[i] + scoreArray[i, 5]);
            }

            for (int i = 0; i < detailArray.GetLength(0); i++)
            {
                double candidateTotal = 0;
                for (int j = 0; j < detailArray.GetLength(1); j++)
                {
                    candidateTotal += detailArray[i, j]; // 加總每個評委對該候選人的評分
                }
                candidateAverage.Add(Math.Round(candidateTotal / committeeCount, 3)); // 候選人獲得的總分除以評委數取得平均分
            }

            writer.WriteText(3, 3, title); // C3
            writer.WriteText(2, 22, date); // V2
            writer.WriteDepartmentAndName(departments, names, 7, 4); // D7
            writer.WriteScores(scoreArray, scoreTemp, scoreB, scoreC, 7, 6); // F7

            writer.setSheetIndex(2);
            writer.WriteText(3, 1, title); // A3
            writer.WriteText(3, 21, date); // U3
            writer.WriteDepartmentAndName(departments, names, 7, 2); // B7
            writer.WriteScores(scoreArray, scoreTemp, scoreB, scoreC, 7, 4); // D7

            writer.setSheetIndex(3);
            writer.WriteText(3, 1, title); // A3
            writer.WriteText(3, 21, date); // U3
            writer.WriteDepartmentAndName(departments, names, 7, 2); // B7
            writer.WriteScores(scoreArray, scoreTemp, scoreB, scoreC, 7, 4); // D7

            writer.setSheetIndex(4);
            writer.WriteText(3, 1, title); // A3
            writer.InitDetailSheet(names, committeeCount + 3); // 加 3 為前兩欄的 "編號", "姓名" 及最後一欄的 "平均"
            writer.WriteCommitteeIps(4, 3, ips); // C4
            writer.WriteCandidateScoreDetail(5, 3, detailArray); // C5
            writer.WriteCandidateAverage(5, committeeCount + 3, candidateAverage);

            writer.save();
            writer.close();
        }

        /// <summary>
        /// sendMessage
        /// </summary>
        /// <param name="client"></param>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        public override void sendMessage(MeetingClient client, BinaryWriter writer, object obj)
        {
            try
            {
                string jsonStr = JsonTool.getInstance().Serialize(obj);
                string msg = "{\"cmd\":6," + "\"msg\":" + jsonStr + "}\n"; // Promotion 6 <---------
                writer.Write(msg);
                Console.WriteLine("PromotionMessageHandler.sendMessage(): " + msg);
            }
            catch (IOException e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        /// <summary>
        /// handlerMessage
        /// </summary>
        /// <param name="client"></param>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        public override void handlerMessage(MeetingClient client, BinaryWriter writer, object obj)
        {
            if (obj == null) //送出該使用者尚未進行評分的列表
            {
                sendMessage(client, writer, GetNonPromotionSettingItem(client));
            }
            else
            {   //儲存使用者的評分結果
                if (obj.GetType() == typeof(PromotionDataItem))
                {
                    SaveDataItem(client, (PromotionDataItem)obj);
                }
            }
        }

        /// <summary>
        /// receivedMessage
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public override object receivedMessage(MeetingClient client, BinaryReader reader)
        {
            int cmdVal = reader.ReadInt32();
            EventLog.Write(" Promotion receive cmdval: " + cmdVal);
            
            if (Enum.IsDefined(typeof(PromotionMessageMode), cmdVal))
            {
                PromotionMessageMode promotionMode = (PromotionMessageMode)cmdVal;
                switch (promotionMode)
                {
                    case PromotionMessageMode.PromotionResponse:
                        EventLog.Write(" Promotion Mode Response");
                        var sb = new StringBuilder();
                        char c = ' ';

                        while (c != '\n')
                        {
                            c = reader.ReadChar();
                            sb.Append(c);
                        }

                        string jsonStr = sb.ToString();
                        EventLog.Write(jsonStr);
                        Console.WriteLine(jsonStr);
                        PromotionDataItem promotionDataItem = (PromotionDataItem)JsonTool.getInstance().Deserialize(jsonStr, typeof(PromotionDataItem));
                        return promotionDataItem;
                    case PromotionMessageMode.PromotionRequest:
                        return null;
                }
            }
            else
            {
                throw new Exception("Promotion Message Mode is not defined.");
            }
            return null;
        }

        /// <summary>
        /// SaveDataItem
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dataItem"></param>
        private void SaveDataItem(MeetingClient client, PromotionDataItem dataItem)
        {
            lock (promotionDataPairs)
            {
                PromotionDataPair dataPair = promotionDataPairs.FirstOrDefault(pair => pair.SettingDataItem.PromotionId == dataItem.Id);
                if (dataPair != null && dataPair.PromotionSettingStatus == PromotionSettingDataItem.PromotionSettingStatus.ACTIVE)
                {
                    dataPair.SaveDataItem(client, dataItem);
                }
                else
                {
                    throw new Exception(String.Format("Promotion Id {0} : Missing PromotionDataPair", dataItem.Id));
                }
            }
        }

        /// <summary>
        /// GetNonScoreSettingItem
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private object GetNonPromotionSettingItem(MeetingClient client)
        {
            List<PromotionSettingDataItem> nonPromotionSettingItems = new List<PromotionSettingDataItem>(SettingDataItems);
            nonPromotionSettingItems.RemoveAll(item => IsExist(client, item.PromotionId));
            return nonPromotionSettingItems;
        }

        /// <summary>
        /// IsExist
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExist(MeetingClient client, int id)
        {
            PromotionDataPair dataPair = promotionDataPairs.FirstOrDefault(pair => pair.SettingDataItem.PromotionId == id);
            List<object[]> promotionDataItems = dataPair == null ? null : (List<object[]>)dataPair.DataItems;
            return (promotionDataItems != null && promotionDataItems.Any(item => item[0].Equals(client.IpAddr))) || (dataPair.PromotionSettingStatus != PromotionSettingDataItem.PromotionSettingStatus.ACTIVE);
        }

        /// <summary>
        /// SetToCompleteDataItem
        /// </summary>
        /// <param name="id"></param>
        public void SetToCompleteDataItem(int id)
        {
            PromotionDataPair dataPair = promotionDataPairs.FirstOrDefault(pair => pair.SettingDataItem.PromotionId == id);
            dataPair.SettingDataItem.Status = PromotionSettingDataItem.PromotionSettingStatus.COMPLETE;
        }
    }
}
