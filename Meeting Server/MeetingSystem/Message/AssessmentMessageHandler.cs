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
using MeetingSystem.Assessment;

namespace MeetingSystem.Message
{
    class AssessmentMessageHandler : Message
    {
        private Hashtable assessmentDataItems = null;
        private BindingList<AssessmentDataPair> assessmentDataPairs = new BindingList<AssessmentDataPair>();

        public enum AssessmentMessageMode { AssessmentRequest, AssessmentResponse };
        public ArrayList collection = new ArrayList(); // save the ip and assessment which rearrangement
        public Random random = new Random();

        public BindingList<AssessmentSettingDataItem> SettingDataItems
        {
            get { return (BindingList<AssessmentSettingDataItem>)parm; }
            set { parm = value; }
        }

        public Hashtable AssessmentDataItems
        {
            get { return assessmentDataItems; }
            set { assessmentDataItems = value; }
        }

        public BindingList<AssessmentDataPair> AssessmentDataPairs
        {
            get { return assessmentDataPairs; }
            set { assessmentDataPairs = value; }
        }

        private AssessmentMessageHandler()
        {
            parm = new BindingList<AssessmentSettingDataItem>();
            SettingDataItems.ListChanged += new ListChangedEventHandler(AssessmentSettingItems_ListChanged);
        }

        /// <summary>
        /// AssessmentSettingItems_ListChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssessmentSettingItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                return;
            }
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                AssessmentSettingItem_Rmove(e.NewIndex);
                return;
            }
            AssessmentSettingItem_Add();
        }

        /// <summary>
        /// AssessmentSettingItem_Add
        /// </summary>
        private void AssessmentSettingItem_Add()
        {
            foreach (AssessmentSettingDataItem settingDataItem in SettingDataItems)
            {
                AssessmentDataPair targetDataPair = assessmentDataPairs.FirstOrDefault(pair => pair.SettingDataItem == settingDataItem);
                bool isExist = (targetDataPair != null);
                if (!isExist) //不存在所以新增
                {
                    AssessmentDataPair dataPair = new AssessmentDataPair(settingDataItem);
                    assessmentDataPairs.Add(dataPair);
                }
            }
        }

        /// <summary>
        /// OpenResultExcel
        /// </summary>
        /// <param name="id"></param>
        internal void OpenResultExcel(int id)
        {
            AssessmentDataPair dataPair = assessmentDataPairs.FirstOrDefault(pair => pair.AssessmentId == id);
            string path = dataPair.SettingDataItem.AssessmentResultPath;
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// AssessmentSettingItem_Rmove
        /// </summary>
        /// <param name="affectIndex"></param>
        private void AssessmentSettingItem_Rmove(int affectIndex)
        {
            if (affectIndex < assessmentDataPairs.Count)
            {
                AssessmentSettingDataItem assessmentSettingDataItem = assessmentDataPairs[affectIndex].SettingDataItem;
                if (!SettingDataItems.Contains(assessmentSettingDataItem))
                {
                    assessmentDataPairs.RemoveAt(affectIndex);
                }
            }
        }
        
        /// <summary>
        /// Initialize
        /// </summary>
        public static void Init()
        {
            MessagePool.registerMessage(ClientMessage.AssessmentMessage, new AssessmentMessageHandler());
        }

        /// <summary>
        /// CalcVoteResult
        /// </summary>
        /// <param name="id"></param>
        public void CalcVoteResult(int id)
        {
            lock (assessmentDataPairs)
            {
                try
                {
                    AssessmentDataPair dataPair = assessmentDataPairs.FirstOrDefault(pair => pair.SettingDataItem.AssessmentId == id);
                    if (dataPair.DataItems.Count <= 0)
                    {
                        return;
                    }
                    DateTime time_start = DateTime.Now;
                    DateTime time_saveBallot = DateTime.Now;
                    SaveAssessmentResult(dataPair);//存投票結果
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
        /// save assessment result
        /// </summary>
        /// <param name="dataPair"></param>
        private void SaveAssessmentResult(AssessmentDataPair dataPair) // 結果檔
        {
            AssessmentResultWriter writer = new AssessmentResultWriter(dataPair.SettingDataItem.AssessmentSourcePath);
            writer = new AssessmentResultWriter(dataPair.SettingDataItem.AssessmentResultPath);

            int committeeCount = dataPair.DataItems.Count; // 評委數量
            int candidateCount = ((AssessmentDataItem)dataPair.DataItems[0][1]).CandidateList.Count; // 候選人數量

            double[,] candidateScores = new double[candidateCount, committeeCount]; // 選票明細陣列, 一維為候選人, 二維為評委
            int[] candidateRanks = new int[candidateCount]; // 候選人排名
            
            List<double> candidateAverages = new List<double>(); // 候選人平均得分
            List<string> departments = new List<string>();
            List<string> names = new List<string>();
            List<string> ips = new List<string>();

            for (int i = 0; i < committeeCount; i++) // 哪幾位評委
            {
                AssessmentDataItem item = (AssessmentDataItem)dataPair.DataItems[i][1];
                string fullIp = ((System.Net.IPAddress)(dataPair.DataItems[i][0])).ToString();
                ips.Add(fullIp.Substring(fullIp.LastIndexOf('.') + 1)); // 只取 ip 末三碼

                for (int j = 0; j < candidateCount; j++) // 哪幾位候選人
                {
                    if (i == 0)
                    {
                        departments.Add(item.CandidateList[j].Department);
                        names.Add(item.CandidateList[j].Name);
                    }

                    candidateScores[j, i] = Convert.ToDouble(item.CandidateList[j].Scores); // 存選票明細
                }
            }

            for (int i = 0; i < candidateScores.GetLength(0); i++)
            {
                double candidateTotal = 0;

                for (int j = 0; j < candidateScores.GetLength(1); j++)
                {
                    candidateTotal += candidateScores[i, j];
                }

                candidateAverages.Add(Math.Round(candidateTotal / committeeCount, 2));
            }

            int rank = 1;
            List<double> candidateSortedAverages = candidateAverages.OrderByDescending(c => c).Distinct().Cast<double>().ToList();

            for (int i = 0; i < candidateSortedAverages.Count; i++)
            {
                int times = 0;

                for (int j = 0; j < candidateAverages.Count; j++)
                {
                    if (candidateSortedAverages.ElementAt(i) == candidateAverages.ElementAt(j))
                    {
                        candidateRanks[j] = rank;
                        times++;
                    }
                }

                rank += times;
            }

            writer.setSheetIndex(1);
            writer.WriteHeader(5, 5, ips); // E5
            writer.WriteDepartmentsAndNames(6, 5, departments, names); // E6
            writer.WriteScores(6, 7, candidateScores); // G6
            writer.WriteAverages(6, committeeCount + 7, candidateAverages);
            writer.WriteRanks(6, committeeCount + 8, candidateRanks);

            writer.setSheetIndex(2);
            writer.WriteAveragesNoStyle(4, 13, candidateAverages); // M4
            writer.WriteRanksNoStyle(4, 1, candidateCount); // A4
            writer.WriteDetailsNoStyle(4, 1, writer.GetDetails(4, 1, candidateCount * 2)); // A4

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
                string msg = "{\"cmd\":7," + "\"msg\":" + jsonStr + "}\n"; // Assessment 7 <---------
                writer.Write(msg);
                Console.WriteLine("AssessmentMessageHandler.sendMessage(): " + msg);
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
                sendMessage(client, writer, GetNonAssessmentSettingItem(client));
            }
            else
            {   //儲存使用者的評分結果
                if (obj.GetType() == typeof(AssessmentDataItem))
                {
                    SaveDataItem(client, (AssessmentDataItem)obj);
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
            EventLog.Write(" Assessment receive cmdval: " + cmdVal);
            
            if (Enum.IsDefined(typeof(AssessmentMessageMode), cmdVal))
            {
                AssessmentMessageMode assessmentMode = (AssessmentMessageMode)cmdVal;
                switch (assessmentMode)
                {
                    case AssessmentMessageMode.AssessmentResponse:
                        EventLog.Write(" Assessment Mode Response");
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
                        AssessmentDataItem assessmentDataItem = (AssessmentDataItem)JsonTool.getInstance().Deserialize(jsonStr, typeof(AssessmentDataItem));
                        return assessmentDataItem;
                    case AssessmentMessageMode.AssessmentRequest:
                        return null;
                }
            }
            else
            {
                throw new Exception("Assessment Message Mode is not defined.");
            }
            return null;
        }

        /// <summary>
        /// SaveDataItem
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dataItem"></param>
        private void SaveDataItem(MeetingClient client, AssessmentDataItem dataItem)
        {
            lock (assessmentDataPairs)
            {
                AssessmentDataPair dataPair = assessmentDataPairs.FirstOrDefault(pair => pair.SettingDataItem.AssessmentId == dataItem.Id);
                if (dataPair != null && dataPair.AssessmentSettingStatus == AssessmentSettingDataItem.AssessmentSettingStatus.ACTIVE)
                {
                    dataPair.SaveDataItem(client, dataItem);
                }
                else
                {
                    throw new Exception(String.Format("Assessment Id {0} : Missing AssessmentDataPair", dataItem.Id));
                }
            }
        }

        /// <summary>
        /// GetNonScoreSettingItem
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private object GetNonAssessmentSettingItem(MeetingClient client)
        {
            List<AssessmentSettingDataItem> nonAssessmentSettingItems = new List<AssessmentSettingDataItem>(SettingDataItems);
            nonAssessmentSettingItems.RemoveAll(item => IsExist(client, item.AssessmentId));
            return nonAssessmentSettingItems;
        }

        /// <summary>
        /// IsExist
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExist(MeetingClient client, int id)
        {
            AssessmentDataPair dataPair = assessmentDataPairs.FirstOrDefault(pair => pair.SettingDataItem.AssessmentId == id);
            List<object[]> assessmentDataItems = dataPair == null ? null : (List<object[]>)dataPair.DataItems;
            return (assessmentDataItems != null && assessmentDataItems.Any(item => item[0].Equals(client.IpAddr))) || (dataPair.AssessmentSettingStatus != AssessmentSettingDataItem.AssessmentSettingStatus.ACTIVE);
        }

        /// <summary>
        /// SetToCompleteDataItem
        /// </summary>
        /// <param name="id"></param>
        public void SetToCompleteDataItem(int id)
        {
            AssessmentDataPair dataPair = assessmentDataPairs.FirstOrDefault(pair => pair.SettingDataItem.AssessmentId == id);
            dataPair.SettingDataItem.Status = AssessmentSettingDataItem.AssessmentSettingStatus.COMPLETE;
        }
    }
}
