using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Client;
using System.IO;
using MeetingSystem.Utils;
using System.ComponentModel;
using System.Collections;
using MeetingSystem.Vote;
using MeetingSystem.Structure;
using MeetingSystem.Server;
using System.Globalization;

namespace MeetingSystem.Message
{
    public class VoteMessageHandler : Message
    {
        public enum VoteMessageMode { VoteRequest, VoteResponse };
        private BindingList<VoteDataPair> voteDataPairs = new BindingList<VoteDataPair>();

        public BindingList<VoteDataPair> VoteDataPairs
        {
            get
            {
                return voteDataPairs;
            }
        }

        private VoteMessageHandler()
        {
            parm = new BindingList<VoteSettingDataItem>();
            VoteSettingDataItems.ListChanged += new ListChangedEventHandler(VoteSettingItems_ListChanged);
        }

        private void VoteSettingItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                return;
            }
                
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                voteSettingItem_Rmove(e.NewIndex);
                return;
            }

            voteSettingItem_Add();
        }

        private void voteSettingItem_Rmove(int affectIndex)
        {
            if (affectIndex < VoteDataPairs.Count)
            {
                VoteSettingDataItem voteSettingDataItem = VoteDataPairs[affectIndex].VoteSettingItem;

                if (!VoteSettingDataItems.Contains(voteSettingDataItem))
                {
                    VoteDataPairs.RemoveAt(affectIndex);
                }
            }
        }

        private void voteSettingItem_Add()
        {
            foreach (VoteSettingDataItem settingDataItem in VoteSettingDataItems)
            {
                if (VoteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem == settingDataItem) == null)
                {
                    VoteDataPair dataPair = new VoteDataPair(settingDataItem);

                    VoteDataPairs.Add(dataPair);
                }
            }
        }

        public static void init()
        {
            MessagePool.registerMessage(ClientMessage.VoteMessage, new VoteMessageHandler());
        }

        public override void sendMessage(MeetingClient client, BinaryWriter writer, Object obj)
        {
            String jsonStr = JsonTool.getInstance().Serialize(obj);
            String msg = "{\"cmd\":1," + "\"msg\":" + jsonStr + "}\n";

            writer.Write(msg);
            Console.WriteLine("VoteMessageHandler.sendMessage(): " + msg);
        }

        public override void handlerMessage(MeetingClient client, BinaryWriter writer, Object obj)
        {
            if (obj == null)
            {
                sendMessage(client, writer, getNonVoteSettingItem(client));
            }
            else if (obj.GetType() == typeof(VoteDataItem))
            {
                saveDataItem(client, (VoteDataItem)obj);
            }
        }

        private void saveDataItem(MeetingClient client, VoteDataItem voteDataItem)
        {
            lock (VoteDataPairs)
            {
                VoteDataPair dataPair = VoteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem.VoteId == voteDataItem.Id);

                if (dataPair != null && dataPair.VoteSettingStatus == VoteSettingDataItem.VoteSettingStatus.ACTIVE)
                {
                    dataPair.saveDataItem(client, voteDataItem);
                }
                else
                {
                    throw new Exception(String.Format("Vote Id {0}:Missing VoteDataPair", voteDataItem.Id));
                }
            }
        }

        private Object getNonVoteSettingItem(MeetingClient client)
        {
            List<VoteSettingDataItem> nonVoteSettingItems = new List<VoteSettingDataItem>(VoteSettingDataItems);

            nonVoteSettingItems.RemoveAll(item => isExist(client, item.VoteId));
            return nonVoteSettingItems;
        }

        private bool isExist(MeetingClient client, int id)
        {
            VoteDataPair dataPair = VoteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem.VoteId == id);
            List<Object[]> scoreDataItems = dataPair == null ? null : (List<Object[]>)dataPair.DataItems;

            return (scoreDataItems != null && scoreDataItems.Any(item => item[0].Equals(client.IpAddr))) || (dataPair.VoteSettingStatus != VoteSettingDataItem.VoteSettingStatus.ACTIVE);
        }

        public override Object receivedMessage(MeetingClient client, BinaryReader reader)
        {
            int cmdVal = reader.ReadInt32();
            EventLog.Write(" Vote receive cmdVal: " + cmdVal);

            if (Enum.IsDefined(typeof(VoteMessageMode), cmdVal))
            {
                VoteMessageMode scoreMode = (VoteMessageMode)cmdVal;

                switch (scoreMode)
                {
                    case VoteMessageMode.VoteResponse:
                        EventLog.Write(" Vote Mode Response");

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
                        return (VoteDataItem)JsonTool.getInstance().Deserialize(jsonStr, typeof(VoteDataItem));
                    case VoteMessageMode.VoteRequest:
                        return null;
                }
            }
            else
            {
                throw new Exception("Vote Message Mode is not defined.");
            }
            return null;
        }

        public BindingList<VoteSettingDataItem> VoteSettingDataItems
        {
            get
            {
                return (BindingList<VoteSettingDataItem>)parm;
            }
            set
            {
                parm = value;
            }
        }

        public void calcVoteResult(int id)
        {
            lock (voteDataPairs)
            {
                VoteDataPair dataPair = voteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem.VoteId == id);

                if (dataPair.DataItems.Count <= 0)
                {
                    return;
                }
                    
                saveVoteBallot(dataPair); // 每一張選票
            }
        }

        public void setToCompleteVoteDataItem(int id)
        {
            VoteDataPair dataPair = voteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem.VoteId == id);

            dataPair.VoteSettingItem.Status = VoteSettingDataItem.VoteSettingStatus.COMPLETE;
        }

        public void calcAllVoteResult()
        {
            foreach (VoteDataPair pair in voteDataPairs)
            {
                if (pair.VoteSettingStatus == VoteSettingDataItem.VoteSettingStatus.ACTIVE)
                {
                    calcVoteResult(pair.VoteId);
                }
            }
        }

        public void openResultExcel(int id)
        {
            VoteDataPair dataPair = voteDataPairs.FirstOrDefault(pair => pair.VoteSettingItem.VoteId == id);
            string path = dataPair.DataResult;

            System.Diagnostics.Process.Start(path);
        }

        private IList<string> GetIpList(VoteDataPair dataPair)
        {
            IList<string> ips = new List<string>();

            for (int i = 1; i <= dataPair.DataItems.Count; i++)
            {
                string ip = ((System.Net.IPAddress)(dataPair.DataItems[i - 1][0])).ToString();
                string[] ids = ip.Split('.');

                ips.Add(ids[3]);
            }

            return ips;
        }

        private void saveVoteBallot(VoteDataPair dataPair)
        {
            String path = dataPair.VoteSettingItem.VoteReslutPath;
            VoteResultWriter writer = new VoteResultWriter(path);

            const int IP_INDEX = 0;
            const int DATA_INDEX = 1;

            VoteDataItem firstItem = (VoteDataItem)dataPair.DataItems[0][DATA_INDEX];
            VoteDataItem tempItem;

            int dataItemsCount = dataPair.DataItems.Count;
            int candidateListCount = firstItem.CandidateList.Count;
            bool isOrder = firstItem.CandidateList[0].OrderBy;
            string votedNumber = Math.Ceiling((decimal)(dataItemsCount * 2) / 3).ToString();
            IList<string> ips = GetIpList(dataPair);

            if (isOrder) // 同意及排序
            {
                const int ALPHABETS = 26;
                const int BLANK_ROWS = 1;
                const int CLEAN_END_ROW = 49;
                const int CLEAN_START_ROW = 6;
                const int RESERVED_TOP_ROWS = 6;
                const int RESERVED_BOTTOM_ROWS = 6;
                const int RESERVED_LEFT_COLUMNS = 5;
                const int RESERVED_RIGHT_COLUMNS = 2;

                int signatureRow = candidateListCount + RESERVED_TOP_ROWS + BLANK_ROWS + 1;
                int totalCol = dataItemsCount + RESERVED_LEFT_COLUMNS + RESERVED_RIGHT_COLUMNS - 1;
                char endCol = (char)('A' + (totalCol < ALPHABETS ? totalCol : totalCol - ALPHABETS)); // A~Z, AA~AZ
                string endRow = (candidateListCount + RESERVED_TOP_ROWS + RESERVED_BOTTOM_ROWS).ToString();
                string printAreaRange = totalCol < ALPHABETS ? ("$A$1:$" + endCol + "$" + endRow) : ("$A$1:$A" + endCol + "$" + endRow);
                printAreaRange = dataItemsCount < 4 ? ("$A$1:$K$" + endRow) : printAreaRange; // default set "print area" to column 'K' for signature field

                /****************************************************************************************
                 * 選票檔A
                 ****************************************************************************************/

                writer.setSheetIndex(1);
                writer.cleanBallotRow(CLEAN_START_ROW, CLEAN_END_ROW);
                writer.writeVotedNumAndPassThershold(dataItemsCount.ToString(), votedNumber); // 寫入投票人數與門檻
                writer.writeGridHeader();
                writer.writeVotesAndSignature(signatureRow, 6); // 'C' + 6 = 'I'
                writer.setPrintArea(printAreaRange);

                for (int i = 1; i <= dataItemsCount; i++)
                {
                    tempItem = (VoteDataItem)dataPair.DataItems[i - 1][DATA_INDEX];

                    if (i == 1)
                    {
                        writer.writeUnitAndName(tempItem.CandidateList);
                    }

                    writer.writeVoteStatistics(tempItem.CandidateList, i, ips[i - 1]);
                }

                List<string> agreeResult = writer.computeVoteTotal(candidateListCount, dataItemsCount, true);
                List<string> rejectResult = writer.computeVoteTotal(candidateListCount, dataItemsCount, false);

                /****************************************************************************************
                 * 結果檔A
                 ****************************************************************************************/

                writer.setSheetIndex(2);
                writer.cleanBallotRow(CLEAN_START_ROW, CLEAN_END_ROW);
                writer.writeVotedNumAndPassThershold(dataItemsCount.ToString(), votedNumber); // 寫入投票人數與門檻
                writer.writeGridHeader();
                writer.writeVotesAndSignature(signatureRow, 4); // 'C' + 4 = 'G'
                writer.setPrintArea("$A$1:$J$" + endRow);

                List<int> groupEndIndexes = writer.writeUnitAndName(firstItem.CandidateList);
                writer.writeVoteTotal(candidateListCount, agreeResult, rejectResult, votedNumber);

                /****************************************************************************************
                 * 選票檔B
                 ****************************************************************************************/

                writer.setSheetIndex(3);
                writer.cleanBallotRow(CLEAN_START_ROW, CLEAN_END_ROW);
                writer.writeVotedNumAndPassThershold(dataItemsCount.ToString(), votedNumber); // 寫入投票人數與門檻
                writer.writeGridHeader();
                writer.writeVotesAndSignature(signatureRow, 6); // 'C' + 6 = 'I'
                writer.setPrintArea(printAreaRange);

                for (int i = 1; i <= dataItemsCount; i++)
                {
                    tempItem = (VoteDataItem)dataPair.DataItems[i - 1][DATA_INDEX];
                    
                    if (i == 1)
                    {
                        writer.writeUnitAndName(tempItem.CandidateList);
                    }

                    writer.writeRankStatistics(tempItem.CandidateList, i, ips[i - 1]);
                }

                List<string> rankTotal = writer.computeRankTotal(candidateListCount, dataItemsCount, groupEndIndexes, false);
                List<string> rankResult = writer.computeRankTotal(candidateListCount, dataItemsCount, groupEndIndexes, true);

                /****************************************************************************************
                 * 結果檔B
                 ****************************************************************************************/

                writer.setSheetIndex(4);
                writer.cleanBallotRow(CLEAN_START_ROW, CLEAN_END_ROW);
                writer.writeVotedNumAndPassThershold(dataItemsCount.ToString(), votedNumber); // 寫入投票人數與門檻
                writer.writeGridHeader();
                writer.writeVotesAndSignature(signatureRow, 4); // 'C' + 4 = 'G'
                writer.setPrintArea("$A$1:$I$" + endRow);

                writer.writeUnitAndName(firstItem.CandidateList);
                writer.writeRankSubTotal(candidateListCount, rankTotal, agreeResult, votedNumber);
                writer.writeRankTotal(candidateListCount, rankResult, agreeResult, votedNumber);
            }
            else // 同意與不同意
            {
                int titleColumn = 4; // title init column

                List<int> resultList = new List<int>();
                List<int> indexList = new List<int>();

                /****************************************************************************************
                 * 選票檔
                 ****************************************************************************************/

                writer.setSheetIndex(1);
                writer.cleanBallotRow(7, 49);
                writer.writeVotedNumAndPassThershold(dataItemsCount.ToString(), votedNumber); // 寫入投票人數與門檻

                for (int i = 1; i <= dataItemsCount; i++)
                {
                    indexList.Add(i);
                    tempItem = (VoteDataItem)dataPair.DataItems[i - 1][DATA_INDEX];

                    if (i == 1)
                    {
                        for (int j = 0; j < tempItem.CandidateList.Count; j++)
                        {
                            writer.writeTitle(tempItem.CandidateList[j].Name, titleColumn); // title only write once
                            resultList.Add(0);
                            titleColumn += 2;
                        }
                    }

                    for (int j = 0; j < tempItem.CandidateList.Count; j++)
                    {
                        if (writer.isAgree(tempItem.CandidateList[j]))
                        {
                            resultList[j]++;
                        }

                        if (j == 0)
                        {
                            writer.writeIndex(indexList, ips);
                        }
                    }

                    writer.writeRankStatistics(tempItem, i);
                }

                /****************************************************************************************
                 * 結果檔
                 ****************************************************************************************/

                writer.setSheetIndex(2);
                writer.cleanBallotRow(7, 49);
                writer.writeVotedNumAndPassThershold(dataItemsCount.ToString(), votedNumber); // 寫入投票人數與門檻
                
                for (int i = 1; i <= dataItemsCount; i++)
                {
                    titleColumn = 4;
                    tempItem = (VoteDataItem)dataPair.DataItems[i - 1][DATA_INDEX];

                    for (int j = 0; j < tempItem.CandidateList.Count; j++)
                    {
                        writer.writeTitleTextOnly(tempItem.CandidateList[j].Name, titleColumn++); // title only write once
                    }

                    break;
                }

                writer.writeResultTitle();
                writer.writeResultList(resultList);
            }

            writer.save();
            writer.close();
        }
    }
}
