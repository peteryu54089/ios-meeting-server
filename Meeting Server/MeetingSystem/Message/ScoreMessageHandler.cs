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

namespace MeetingSystem.Message
{
    class ScoreMessageHandler : Message
    {
        public enum ScoreMessageMode { ScoreRequest, ScoreResponse };
        public string teacherInfo, score, average;
        public ArrayList collection = new ArrayList(); // save the ip and score which rearrangement
        private Hashtable scoreDataItems = null;
        private BindingList<ScoreDataPair> scoreDataPairs = new BindingList<ScoreDataPair>();
        Random random = new Random();

        public BindingList<ScoreSettingDataItem> SettingDataItems
        {
            get { return (BindingList<ScoreSettingDataItem>)parm; }
            set { parm = value; }
        }

        public Hashtable ScoreDataItems
        {
            get { return scoreDataItems; }
            set { scoreDataItems = value; }
        }

        public BindingList<ScoreDataPair> ScoreDataPairs
        {
            get { return scoreDataPairs; }
            set { scoreDataPairs = value; }
        }

        private ScoreMessageHandler()
        {
            parm = new BindingList<ScoreSettingDataItem>(); 
            SettingDataItems.ListChanged += new ListChangedEventHandler(ScoreSettingItems_ListChanged);
        }

        /// <summary>
        /// ScoreSettingItems_ListChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ScoreSettingItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                return;
            }
                
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                scoreSettingItem_Rmove(e.NewIndex);
                return;
            }

            scoreSettingItem_Add();
        }

        /// <summary>
        /// scoreSettingItem_Add
        /// </summary>
        private void scoreSettingItem_Add()
        {
            foreach (ScoreSettingDataItem settingDataItem in SettingDataItems)
            {
                if (scoreDataPairs.FirstOrDefault(pair => pair.SettingDataItem == settingDataItem) == null) // 不存在所以新增
                {
                    ScoreDataPair dataPair = new ScoreDataPair(settingDataItem);
                    scoreDataPairs.Add(dataPair);
                }
            }
        }

        /// <summary>
        /// scoreSettingItem_Rmove
        /// </summary>
        /// <param name="affectIndex"></param>
        private void scoreSettingItem_Rmove(int affectIndex)
        {
            if (affectIndex < scoreDataPairs.Count)
            {
                ScoreSettingDataItem scoreSettingDataItem = scoreDataPairs[affectIndex].SettingDataItem;

                if (!SettingDataItems.Contains(scoreSettingDataItem))
                {
                    scoreDataPairs.RemoveAt(affectIndex);
                }
            }
        }

        /// <summary>
        /// init
        /// </summary>
        public static void init()
        {
            MessagePool.registerMessage(ClientMessage.ScoreMessage, new ScoreMessageHandler());
        }

        /// <summary>
        /// calcVoteResult
        /// </summary>
        /// <param name="id"></param>
        public void calcVoteResult(int id)
        {
            lock (scoreDataPairs)
            {
                try
                {
                    ScoreDataPair dataPair = scoreDataPairs.FirstOrDefault(pair => pair.SettingDataItem.ScoreId == id);

                    if (dataPair.DataItems.Count <= 0)
                    {
                        return;
                    }
                        
                    DateTime time_start = DateTime.Now;
                    saveTeacherInfo(dataPair);
                    saveScoreBallot(dataPair); // 存每一張票

                    DateTime time_saveBallot = DateTime.Now;
                    saveScoreResult(dataPair); // 存投票結果

                    DateTime time_saveResult = DateTime.Now;
                    EventLog.Write("存每一張票的時間 " + (time_saveBallot - time_start).TotalMilliseconds.ToString() + " 存投票結果時間 " + (time_saveResult - time_saveBallot).TotalMilliseconds.ToString());
                    //setToCompleteDataItem(dataPair); 
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
        /// date
        /// </summary>
        /// <returns></returns>
        public string date() {
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            return string.Format("{0}.{1}.{2}", taiwanCalendar.GetYear(DateTime.Now), DateTime.Now.Month, DateTime.Now.Day);
        }

        /// <summary>
        /// 升等評分檔
        /// </summary>
        /// <param name="dataPair"></param>
        private void saveTeacherInfo(ScoreDataPair dataPair) {
            const string name = "C7";
            const string averageScore = "H12";
            const string startScore = "I15";
            const string endScore = "I16";
      
            string path = dataPair.SettingDataItem.ScoreSourcePath; // rank file
            ScoreResultWriter reader = new ScoreResultWriter(path); // read teacher info 
           
            teacherInfo = reader.getTeacherInfo(name);
            score = reader.getRangeScore(startScore, endScore);
            average = reader.getAverageScore(averageScore);

            reader.close();
        }

        /// <summary>
        /// 結果檔
        /// </summary>
        /// <param name="dataPair"></param>
        private void saveScoreResult(ScoreDataPair dataPair)
        {
            const string startValue = "C7";
            const string endValue = "E7";
            const string indexValue = "A7";
            const string resultPos = "A3";
            const string startPrintScore = "G3";
            const string endPrintScore = "H4";
            const string averageScore = "B7";

            const int ipIndex = 0;
            const int scoreListIndex = 1;
            int startRow = Convert.ToInt32(startValue.Substring(1));
            int startIndex = 7;
            int people = dataPair.DataItems.Count;

            string ipCount = Convert.ToString(dataPair.DataItems.Count);
            string path = dataPair.SettingDataItem.ScoreResultPath;

            Hashtable calResult = new Hashtable();
            ScoreResultWriter writer = new ScoreResultWriter(path);

            for (int i = 0; i < dataPair.DataItems.Count; i++)
            {
                string fullIp = ((System.Net.IPAddress)(dataPair.DataItems[i][ipIndex])).ToString(); // dataItem
                string ip = fullIp.Substring(fullIp.LastIndexOf('.') + 1);
                ScoreDataItem item = (ScoreDataItem)(dataPair.DataItems[i][scoreListIndex]); // 分數 list

                //writer.writeValue(indexValue[0] + startRow.ToString(), 1, ip.Substring(ip.LastIndexOf('.') + 1));
                //writer.writeScore(startValue[0]+startRow.ToString(), endValue[0]+startRow.ToString(), 1, item.Scores); // 將分數寫入
                //writer.writeValue(averageScore[0] + startRow.ToString(), 1, average);

                combineDataToBeAList("(" + (i + 1).ToString() + ") " + ip.Substring(ip.LastIndexOf('.') + 1), average, item.Scores);
                startRow++;

                calcReasonResult(item.Reasons, Convert.ToInt32(ip.Substring(ip.LastIndexOf('.') + 1)), calResult); // 統計未通過的理由數量
            }
            
            for (int i = 0; i < collection.Count; i++) // 重新排列
            {
                int j = random.Next(0, people);
                object temp = collection[i];
                collection[i] = collection[j];
                collection[j] = temp;         
            }
            
            for (int i = 0; i < dataPair.DataItems.Count; i++) // 排列後寫入
            {
                writer.writeRearrangement(indexValue[0] + startIndex.ToString(), endValue[0] + startIndex.ToString(), collection[i]);
                startIndex++;
            }
            
            EventLog.Write(ipCount + " ip has voted " + path); // Count how many people have voted.
            writer.writeIpCount(ipCount);
            writer.printTeacherInfo(resultPos, teacherInfo, date());
            writer.printRangeScore(startPrintScore, endPrintScore, score);
            writer.delete(people + 7, 36);
            collection.Clear();
            
            if(calResult.Count != 0) // 將未通過的理由數量寫入投票結果 excel 中的下一頁
            {
                writeResultForResult(calResult, writer);
            }

            writer.save();
            writer.close();
        }

        /// <summary>
        /// combineDataToBeAList
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="average"></param>
        /// <param name="scores"></param>
        public void combineDataToBeAList(string ip, string average, float[] scores)
        {
            float sum = 0;
            float[] userScores = { 0, 0, 0, 0 };
            string[] tempScores = score.Split(',');

            for (int i = 0; i < 4; i++) {
                userScores[i] = float.Parse(tempScores[i]);
            }

            if (scores[0] < userScores[0] || scores[0] > userScores[1] || scores[1] < userScores[2] || scores[1] > userScores[3])
            {
                EventLog.Write("Score Fail");
            }
            sum = float.Parse(average) + scores[0] + scores[1];

            string str = ip + "," + average + "," + scores[0] + "," + scores[1] + "," + sum.ToString() + "," + scores[2];
            collection.Add(str);
        }

        /// <summary>
        /// writeResultForResult
        /// </summary>
        /// <param name="calResult"></param>
        /// <param name="writer"></param>
        private static void writeResultForResult(Hashtable calResult, ScoreResultWriter writer)
        {
            int index = 1;

            writer.setSheetIndex(2);
            writer.setRangeColumnWidth("A:A", 40);
            writer.setRangeColumnWidth("B:B", 80);
            
            foreach (object key in calResult.Keys)
            {
                List<string> str = new List<string>();
                List<int> reasonId = (List<int>)calResult[key];
                string reason = (string)key;
                int count = reasonId.Count;

                str.Add(reasonsIdToString(reasonId));
                str.Add(reason);
                str.Add(count.ToString());

                writer.writeScore("A" + index.ToString(), "C" + index.ToString(), 1, str.ToArray());
                index++;
            }
        }

        /// <summary>
        /// 存每張票的選票檔
        /// </summary>
        /// <param name="dataPair"></param>
        private void saveScoreBallot(ScoreDataPair dataPair)
        {
            const string scoreStartValue = "I15";
            const string scoreEndValue = "I16";
            const string reasonStartValue = "B18";
            const string reasonEndValue = "E22";
            const int ipIndex = 0;
            const int scoreListIndex = 1; // 分數資料

            string path = dataPair.SettingDataItem.ScoreSourcePath;
            string dir = path.Remove(path.LastIndexOf("."));
            string ext = path.Substring(path.LastIndexOf("."));
            string newTable = dir + "_選票檔" + ext;

            File.Copy(path, newTable, true);
            ScoreResultWriter writer = new ScoreResultWriter(newTable);
            writer.clone(dataPair.DataItems.Count);

            for (int i = 1; i <= dataPair.DataItems.Count; i++)
            {
                string ip = ((System.Net.IPAddress)(dataPair.DataItems[i-1][ipIndex])).ToString(); // dataItem
                ScoreDataItem item = (ScoreDataItem)(dataPair.DataItems[i-1][scoreListIndex]); // dataItem

                writer.setSheetIndex(i);
                writer.reNameSheet(ip.Substring(ip.LastIndexOf('.') + 1)); 
                writer.writeScore(scoreStartValue, scoreEndValue, 1, item.Scores);
                writer.writeReasons(reasonStartValue, reasonEndValue, 1, item.Reasons);
            }

            writer.save();
            writer.close();
            EventLog.Write(newTable + "選票檔完成"); 
        }

        /// <summary>
        /// calcReasonResult
        /// </summary>
        /// <param name="reasons"></param>
        /// <param name="id"></param>
        /// <param name="calcResult"></param>
        private void calcReasonResult(string[] reasons, int id, Hashtable calcResult)
        {
            foreach (string reason in reasons)
            {
                if (calcResult[reason] == null)
                {
                    calcResult[reason] = new List<int>();
                }

                ((List<int>)calcResult[reason]).Add(id);
                //calcResult[reason] = calcResult[reason] == null ? 1 : (int)calcResult[reason] + 1;
            }
        }

        /// <summary>
        /// reasonsIdToString
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        private static string reasonsIdToString(List<int> reasonId)
        {
            string ids = "委員編號:";

            foreach (int id in reasonId)
            {
                ids += " (" + id + ")";
            }

            return ids;
        }

        /// <summary>
        /// setToCompleteDataItem
        /// </summary>
        /// <param name="id"></param>
        public void setToCompleteDataItem(int id)
        {
            ScoreDataPair dataPair = scoreDataPairs.FirstOrDefault(pair => pair.SettingDataItem.ScoreId == id);
            dataPair.SettingDataItem.Status = ScoreSettingDataItem.ScoreSettingStatus.COMPLETE;
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
                string msg = "{\"cmd\":0," + "\"msg\":" + jsonStr + "}\n";

                //writer.Write((int)ClientMessage.ScoreMessage + jsonStr + "\n");
                writer.Write(msg);
                //Console.WriteLine(msg);
                //writer.Write(jsonStr);
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
            if (obj == null) // 送出該使用者尚未進行評分的列表
            {
                sendMessage(client, writer, getNonScoreSettingItem(client));
            }
            else
            {   
                if (obj.GetType() == typeof(ScoreDataItem)) // 儲存使用者的評分結果
                {
                    saveDataItem(client, (ScoreDataItem)obj);
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
            EventLog.Write(" Score receive cmdval: " + cmdVal);

            if (Enum.IsDefined(typeof(ScoreMessageMode), cmdVal))
            {
                ScoreMessageMode scoreMode = (ScoreMessageMode)cmdVal;

                switch (scoreMode)
                {
                    case ScoreMessageMode.ScoreResponse:
                        EventLog.Write(" Score Mode Response");
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
                        ScoreDataItem scoreDataItem = (ScoreDataItem)JsonTool.getInstance().Deserialize(jsonStr, typeof(ScoreDataItem));

                        return scoreDataItem;
                    case ScoreMessageMode.ScoreRequest:
                        return null;
                }
            }
            else
            {
                throw new Exception("Score Message Mode is not defined.");
            }

            return null;
        }

        /// <summary>
        /// saveDataItem
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dataItem"></param>
        private void saveDataItem(MeetingClient client, ScoreDataItem dataItem)
        {
            lock (scoreDataPairs)
            {
                ScoreDataPair dataPair = scoreDataPairs.FirstOrDefault(pair => pair.SettingDataItem.ScoreId == dataItem.Id);

                if (dataPair != null && dataPair.ScoreSettingStatus == ScoreSettingDataItem.ScoreSettingStatus.ACTIVE)
                {
                    dataPair.saveDataItem(client, dataItem);
                }
                else
                {
                    throw new Exception(string.Format("Score Id {0} : Missing ScoreDataPair", dataItem.Id));
                }
            }
        }

        /// <summary>
        /// getNonScoreSettingItem
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private object getNonScoreSettingItem(MeetingClient client)
        {
            List<ScoreSettingDataItem> nonScoreSettingItems = new List<ScoreSettingDataItem>(SettingDataItems);
            nonScoreSettingItems.RemoveAll(item => isExist(client, item.ScoreId));
            return nonScoreSettingItems;
        }

        /// <summary>
        /// isExist
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool isExist(MeetingClient client, int id)
        {
            ScoreDataPair dataPair = scoreDataPairs.FirstOrDefault(pair => pair.SettingDataItem.ScoreId == id);
            List<object[]> scoreDataItems = dataPair == null ? null : dataPair.DataItems;
            return (scoreDataItems != null && scoreDataItems.Any(item => item[0].Equals(client.IpAddr))) || (dataPair.ScoreSettingStatus != ScoreSettingDataItem.ScoreSettingStatus.ACTIVE);
        }

        /// <summary>
        /// calcCurrentAllResult
        /// </summary>
        public void calcCurrentAllResult()
        {
            foreach (ScoreDataPair dataPair in scoreDataPairs)
            {
                if (dataPair.ScoreSettingStatus == ScoreSettingDataItem.ScoreSettingStatus.ACTIVE)
                {
                    calcVoteResult(dataPair.ScoreId);
                }
            }
        }

        /// <summary>
        /// openResultExcel
        /// </summary>
        /// <param name="id"></param>
        public void openResultExcel(int id)
        {
            //VoteDataPair dataPair = voteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem.VoteId == id);
            //string path = dataPair.DataResult;
            //VoteResultWriter writer = new VoteResultWriter(path, true);

            ScoreDataPair dataPair = scoreDataPairs.FirstOrDefault(pair => pair.ScoreId == id);
            string path = dataPair.SettingDataItem.ScoreResultPath;

            //ScoreResultWriter writer = new ScoreResultWriter(path, true);
            System.Diagnostics.Process.Start(path);
        }
    }
}
