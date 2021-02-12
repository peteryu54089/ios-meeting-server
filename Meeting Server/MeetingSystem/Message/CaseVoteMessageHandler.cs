using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MeetingSystem.CaseVote;
using MeetingSystem.Client;
using MeetingSystem.Server;
using MeetingSystem.Structure;
using MeetingSystem.Utils;

namespace MeetingSystem.Message
{
    class CaseVoteMessageHandler : Message
    {
        public enum CaseVoteMessageMode { CaseVoteRequest, CaseVoteResponse };
        private BindingList<CaseVoteDataPair> caseVoteDataPairs = new BindingList<CaseVoteDataPair>();

        public BindingList<CaseVoteDataPair> CaseVoteDataPairs
        {
            get
            {
                return caseVoteDataPairs;
            }
        }

        private CaseVoteMessageHandler()
        {
            parm = new BindingList<CaseVoteSettingDataItem>();
            CaseVoteSettingDataItems.ListChanged += new ListChangedEventHandler(CaseVoteSettingItems_ListChanged);
        }

        private void CaseVoteSettingItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                return;
            }

            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                caseVoteSettingItem_Rmove(e.NewIndex);
                return;
            }

            caseVoteSettingItem_Add();
        }

        private void caseVoteSettingItem_Rmove(int affectIndex)
        {
            if (affectIndex < CaseVoteDataPairs.Count)
            {
                CaseVoteSettingDataItem caseVoteSettingDataItem = CaseVoteDataPairs[affectIndex].CaseVoteSettingItem;

                if (!CaseVoteSettingDataItems.Contains(caseVoteSettingDataItem))
                {
                    CaseVoteDataPairs.RemoveAt(affectIndex);
                }
            }
        }

        private void caseVoteSettingItem_Add()
        {
            foreach (CaseVoteSettingDataItem settingDataItem in CaseVoteSettingDataItems)
            {
                if (CaseVoteDataPairs.FirstOrDefault(pair => pair.CaseVoteSettingItem == settingDataItem) == null)
                {
                    CaseVoteDataPair dataPair = new CaseVoteDataPair(settingDataItem);

                    CaseVoteDataPairs.Add(dataPair);
                }
            }
        }

        public static void init()
        {
            MessagePool.registerMessage(ClientMessage.CaseVoteMessage, new CaseVoteMessageHandler());
        }

        public override void sendMessage(MeetingClient client, BinaryWriter writer, object obj)
        {
            String jsonStr = JsonTool.getInstance().Serialize(obj);
            String msg = "{\"cmd\":5," + "\"msg\":" + jsonStr + "}\n";

            writer.Write(msg);
            Console.WriteLine("CaseVoteMessageHandler.sendMessage(): " + msg);
        }

        public override void handlerMessage(MeetingClient client, BinaryWriter writer, object obj)
        {
            if (obj == null)
            {
                sendMessage(client, writer, getNonVoteSettingItem(client));
            }
            else if (obj.GetType() == typeof(CaseVoteDataItem))
            {
                saveDataItem(client, (CaseVoteDataItem)obj);
            }
        }

        private void saveDataItem(MeetingClient client, CaseVoteDataItem caseVoteDataItem)
        {
            lock (CaseVoteDataPairs)
            {
                CaseVoteDataPair dataPair = CaseVoteDataPairs.FirstOrDefault(pair => pair.CaseVoteSettingItem.CaseVoteId == caseVoteDataItem.Id);

                if (dataPair != null && dataPair.CaseVoteSettingStatus == CaseVoteSettingDataItem.CaseVoteSettingStatus.ACTIVE)
                {
                    dataPair.saveDataItem(client, caseVoteDataItem);
                }
                else
                {
                    throw new Exception(String.Format("CaseVote Id {0}:Missing VoteDataPair", caseVoteDataItem.Id));
                }
            }
        }

        private Object getNonVoteSettingItem(MeetingClient client)
        {
            List<CaseVoteSettingDataItem> nonVoteSettingItems = new List<CaseVoteSettingDataItem>(CaseVoteSettingDataItems);

            nonVoteSettingItems.RemoveAll(item => isExist(client, item.CaseVoteId));
            return nonVoteSettingItems;
        }

        private bool isExist(MeetingClient client, int id)
        {
            CaseVoteDataPair dataPair = CaseVoteDataPairs.FirstOrDefault(pair => pair.CaseVoteSettingItem.CaseVoteId == id);
            List<Object[]> scoreDataItems = dataPair == null ? null : (List<Object[]>)dataPair.DataItems;

            return (scoreDataItems != null && scoreDataItems.Any(item => item[0].Equals(client.IpAddr))) || (dataPair.CaseVoteSettingStatus != CaseVoteSettingDataItem.CaseVoteSettingStatus.ACTIVE);
        }

        public override object receivedMessage(MeetingClient client, BinaryReader reader)
        {
            int cmdVal = reader.ReadInt32();
            EventLog.Write(" Vote receive cmdval: " + cmdVal);

            if (Enum.IsDefined(typeof(CaseVoteMessageMode), cmdVal))
            {
                CaseVoteMessageMode scoreMode = (CaseVoteMessageMode)cmdVal;
                switch (scoreMode)
                {
                    case CaseVoteMessageMode.CaseVoteResponse:
                        EventLog.Write(" Case Vote Mode Response");

                        var sb = new StringBuilder();
                        char c = ' ';

                        while (c != '\n')
                        {
                            c = reader.ReadChar();
                            sb.Append(c);
                        }

                        String jsonStr = sb.ToString();

                        Console.WriteLine(jsonStr);
                        EventLog.Write(jsonStr);
                        return (CaseVoteDataItem)JsonTool.getInstance().Deserialize(jsonStr, typeof(CaseVoteDataItem));
                    case CaseVoteMessageMode.CaseVoteRequest:
                        return null;
                }
            }
            else
            {
                throw new Exception("Case Vote Message Mode is not defined.");
            }
            return null;
        }

        public BindingList<CaseVoteSettingDataItem> CaseVoteSettingDataItems
        {
            get
            {
                return (BindingList<CaseVoteSettingDataItem>)parm;
            }
            set
            {
                parm = value;
            }
        }

        public void calcCaseVoteResult(int id)
        {
            lock (caseVoteDataPairs)
            {
                CaseVoteDataPair dataPair = caseVoteDataPairs.FirstOrDefault(pair => pair.CaseVoteSettingItem.CaseVoteId == id);

                if (dataPair.DataItems.Count <= 0)
                {
                    return;
                }

                saveCaseVoteBallot(dataPair); // 每一章選票
            }
        }

        public void setToCompleteVoteDataItem(int id)
        {
            CaseVoteDataPair dataPair = caseVoteDataPairs.FirstOrDefault(pair => pair.CaseVoteSettingItem.CaseVoteId == id);

            dataPair.CaseVoteSettingItem.Status = CaseVoteSettingDataItem.CaseVoteSettingStatus.COMPLETE;
        }

        public void calcAllVoteResult()
        {
            foreach (CaseVoteDataPair pair in caseVoteDataPairs)
            {
                if (pair.CaseVoteSettingStatus == CaseVoteSettingDataItem.CaseVoteSettingStatus.ACTIVE)
                {
                    calcCaseVoteResult(pair.CaseVoteId);
                }
            }
        }

        public void openResultExcel(int id)
        {
            CaseVoteDataPair dataPair = caseVoteDataPairs.FirstOrDefault(pair => pair.CaseVoteSettingItem.CaseVoteId == id);
            string path = dataPair.DataResult;

            System.Diagnostics.Process.Start(path);
        }

        private void saveCaseVoteBallot(CaseVoteDataPair dataPair)
        {
            String path = dataPair.CaseVoteSettingItem.caseVoteResultPath;
            CaseVoteResultWriter writer = new CaseVoteResultWriter(path);
            List<int> resultList = new List<int>();
            List<int> indexList = new List<int>();
            List<string> ipList = new List<string>();
            CaseVoteDataItem item;

            int totalItem;
            int titleColumn = 4; // title init column
            const int DATA_INDEX = 1;
            string passThreshold = Math.Ceiling((decimal)(dataPair.DataItems.Count * 2) / 3).ToString();

            /****************************************************************************************
             * 選票檔
             ****************************************************************************************/

            writer.setSheetIndex(1);
            writer.clearBallotRow(7, 49);
            writer.writeVotedNumAndPassThershold(dataPair.DataItems.Count.ToString(), passThreshold); // 寫入投票人數與門檻

            for (int i = 1; i <= dataPair.DataItems.Count; i++)
            {
                string ip = ((System.Net.IPAddress)(dataPair.DataItems[i - 1][0])).ToString();
                ipList.Add(ip.Substring(ip.LastIndexOf('.') + 1));
                indexList.Add(i);

                item = (CaseVoteDataItem)dataPair.DataItems[i - 1][DATA_INDEX];
                totalItem = item.CandidateList.Count;

                if (i == 1) // 評選事項標題 only write once
                {
                    writer.writeOptionTitle(totalItem); // 寫入 "評選事項" 字樣

                    for (int j = 0; j < totalItem; j++)
                    {
                        writer.writeTitle(item.CandidateList[j].Name, titleColumn++); // 寫入所有評選事項標題
                        resultList.Add(0);
                    }
                }

                for (int j = 0; j < totalItem; j++)
                {
                    if (j == 0)
                    {
                        writer.writeIndex(indexList, ipList);
                    }

                    if (writer.isAgree(item.CandidateList[j]))
                    {
                        resultList[j]++;
                    }
                }

                writer.writeRankStatistics(item, i);
            }

            /****************************************************************************************
             * 結果檔
             ****************************************************************************************/

            writer.setSheetIndex(2);
            writer.clearBallotRow(7, 49);
            writer.writeVotedNumAndPassThershold(dataPair.DataItems.Count.ToString(), passThreshold); // 寫入投票人數與門檻

            titleColumn = 4;
            item = (CaseVoteDataItem)dataPair.DataItems[0][DATA_INDEX];

            totalItem = item.CandidateList.Count;
            writer.writeOptionTitle(totalItem); // 寫入 "評選事項" 字樣

            writer.isSetTitleHeight = false;
            for (int i = 0; i < totalItem; i++) // 評選事項標題 only write once
            {
                writer.writeTitle(item.CandidateList[i].Name, titleColumn++);
            }

            writer.writeTotalBallot(); // 寫入 "票數" 字樣
            writer.writeResultList(resultList);

            /****************************************************************************************
             ****************************************************************************************/

            writer.save();
            writer.close();
        }
    }
}
