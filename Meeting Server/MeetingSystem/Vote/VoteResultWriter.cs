using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Score;

namespace MeetingSystem.Vote
{
    public class VoteResultWriter:DataWriter
    {
        public VoteResultWriter(String path)
        {
            filePath = path;
            excelUtil = new ExcelUtil(path, false, 1);
        }

        /// <summary>
        /// clean ballot row
        /// </summary>
        public void cleanBallotRow(int startRow, int endRow)
        {
            excelUtil.deleteRows(startRow, endRow);
        }

        /// <summary>
        /// 寫入投票人數與門檻
        /// </summary>
        /// <param name="votedNum"></param>
        /// <param name="passThreshold"></param>
        public void writeVotedNumAndPassThershold(string votedNum, string passThershold) {
            excelUtil.write(3, 2, votedNum); // B3
            excelUtil.write(4, 2, passThershold); // B4
        }

        public bool isAgree(Object obj)
        {
            VoteDataItem.VoteData voteData = (VoteDataItem.VoteData)obj;
            return voteData.AgreeChoose;
        }

        /****************************************************************************************
         * 同意及排序
         ****************************************************************************************/

        public void writeGridHeader()
        {
            int row = 6;
            int col = 3; // C
            writeGridHeaderStyle(row, col);
            excelUtil.write(row, col, "學院");
            
            col = 4; // D
            writeGridHeaderStyle(row, col);
            excelUtil.write(row, col, "系(所、科)");
            
            col = 5; // E
            writeGridHeaderStyle(row, col);
            excelUtil.write(row, col, "擬聘人員");
        }

        public void writeVotesAndSignature(int row, int signatureCols)
        {
            int col = 3; // C
            writeVotesAndSignatureStyle(row, col, signatureCols);
            excelUtil.write(row, col, "總投票數：　　　  　　票，至少三分之二以上同意之票數為　　　  　　票");

            row++;
            writeVotesAndSignatureStyle(row, col, 1);
            excelUtil.write(row, col, "計票人：            (簽名)");

            row++;
            writeVotesAndSignatureStyle(row, col, 1);
            excelUtil.write(row, col, "監票人：            (簽名)");
        }

        public void setPrintArea(string range)
        {
            excelUtil.setPrintArea(range);
        }

        public List<int> writeUnitAndName(List<VoteDataItem.VoteData> list)
        {
            int row = 7;
            int col = 3; // C
            int listCount = list.Count;

            writeUnitAndNameStyle(row - 1, col, listCount);

            for (int i = 0; i < listCount; i++)
            {
                excelUtil.write(row + i, col, list[i].Department);
                excelUtil.write(row + i, col + 1, list[i].Unit);
                excelUtil.write(row + i, col + 2, list[i].Name);
            }

            return mergeUnit(list);
        }

        public List<int> mergeUnit(List<VoteDataItem.VoteData> list)
        {
            bool isChanged = true;
            int listCount = list.Count;
            List<int> groupEndIndexes = new List<int>();

            for (int i = 0; i < listCount; i++)
            {
                groupEndIndexes.Add(0);
            }

            for (int i = 0; i < listCount; i++)
            {
                string department = list[i].Department;
                string unit = list[i].Unit;

                for (int j = i + 1; j <= listCount; j++)
                {
                    bool isEnd = j == listCount;

                    if (isEnd)
                    {
                        groupEndIndexes[i] = j - 1;
                        mergeUnitStyle(i, j, isChanged);
                        isChanged = !isChanged;
                        i = j - 1;
                        break;
                    }
                    else if (department != list[j].Department || unit != list[j].Unit)
                    {
                        groupEndIndexes[i] = j - 1;
                        mergeUnitStyle(i, j, isChanged);
                        isChanged = !isChanged;
                        i = j - 1;
                        break;
                    }
                }
            }

            return groupEndIndexes;
        }

        public void writeVoteStatistics(List<VoteDataItem.VoteData> list, int no, string ip)
        {
            int row = 6;
            int col = 5 + no; // no >= 1, Column >= F
            int listCount = list.Count;

            writeVoteStatisticsStyle(row, col, listCount);
            excelUtil.write(row - 1, col, "[" + no.ToString() + "]");
            excelUtil.write(row, col, ip);

            for (int i = 0; i < listCount; i++)
            {
                row = 7 + i;
                excelUtil.write(row, col, isAgree(list[i]) ? "O" : "X");
            }
        }

        public List<string> computeVoteTotal(int totalRow, int totalCol, bool isAgree)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < totalRow; i++)
            {
                int tempTotal = 0;
                int row = 7 + i;

                for (int j = 0; j < totalCol; j++)
                {
                    if (isAgree && excelUtil.read(row, 6 + j).Contains("O")) // Column >= F
                    {
                        tempTotal++;
                    }
                    else if (!isAgree && !excelUtil.read(row, 6 + j).Contains("O")) // Column >= F
                    {
                        tempTotal++;
                    }
                }

                result.Add(tempTotal.ToString());
            }

            return result;
        }

        public void writeVoteTotal(int totalRow, List<string> agreeResult, List<string> rejectResult, string votedNumber)
        {
            int row = 6;
            int col = 6; // F

            writeVoteTitleStyle(row, col);
            writeVoteTotalStyle(row, col, totalRow, agreeResult, votedNumber);

            excelUtil.write(row, col, "同意票數");
            excelUtil.write(row, col + 1, "不同意票數");
            excelUtil.write(row, col + 2, "是否達出席委員2/3以上門檻");

            for (int i = 0; i < totalRow; i++)
            {
                row = 7 + i;
                excelUtil.write(row, col, agreeResult[i]);
                excelUtil.write(row, col + 1, rejectResult[i]);
                excelUtil.write(row, col + 2, Convert.ToInt32(agreeResult[i]) < Convert.ToInt32(votedNumber) ? "否" : "是");
            }
        }

        public void writeRankStatistics(List<VoteDataItem.VoteData> list, int no, string ip)
        {
            int row = 6;
            int col = 5 + no; // no >= 1, Column >= F
            int listCount = list.Count;

            writeVoteStatisticsStyle(row, col, listCount);
            excelUtil.write(row - 1, col, "[" + no.ToString() + "]");
            excelUtil.write(row, col, ip);

            for (int i = 0; i < listCount; i++)
            {
                row = 7 + i;
                excelUtil.write(row, col, list[i].Rank.Contains("X") ? "1" : list[i].Rank);
            }
        }

        public List<string> computeRankTotal(int totalRow, int totalCol, List<int> groupEndIndexes, bool isHandle)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < totalRow; i++)
            {
                int tempTotal = 0;
                int row = 7 + i;

                for (int j = 0; j < totalCol; j++)
                {
                    string val = excelUtil.read(row, 6 + j); // Column >= F

                    tempTotal += Convert.ToInt32(val);
                }

                result.Add(tempTotal.ToString());
            }

            return isHandle ? handleRankTotal(result, groupEndIndexes) : result;
        }

        public List<string> handleRankTotal(List<string> result, List<int> groupEndIndexes)
        {
            for (int i = 0; i < result.Count; i++)
            {
                int endIndex = groupEndIndexes[i]; // 每一組的結尾 index

                List<string> tempRank = new List<string>(); // 將排序後的排名加總轉成 1234 名次
                List<string> tempSort = new List<string>(); // 將原有 result 一組內的 排名加總 排序

                for (int j = i; j <= endIndex; j++) // 將 result 內一組的值灌進 tempSort
                {
                    tempSort.Add(result[j]);
                }

                for (int j = 0; j < tempSort.Count; j++)
                {
                    for (int k = 0; k < tempSort.Count - 1 - j; k++)
                    {
                        if (Convert.ToInt32(tempSort[k]) > Convert.ToInt32(tempSort[k + 1])) // 泡沫排序小到大
                        {
                            string temp = tempSort[k + 1];

                            tempSort[k + 1] = tempSort[k];
                            tempSort[k] = temp;
                        }
                    }
                }

                for (int j = 0; j < tempSort.Count; j++) // 將排序後的排名加總轉成 1234 名次
                {
                    if (j == 0)
                    {
                        tempRank.Add("1");
                    }
                    else
                    {
                        if (tempSort[j] == tempSort[j - 1]) // 加總排名一樣
                        {
                            tempRank.Add(tempRank[tempRank.Count - 1]); // 名次與前一個一樣
                        }
                        else
                        {
                            tempRank.Add((j + 1).ToString()); // 名次持續累加
                        }
                    }
                }

                for (int j = i; j <= endIndex; j++) // 將 tempRank 的值灌進 result
                {
                    for (int k = 0; k < tempSort.Count; k++)
                    {
                        if (result[j] == tempSort[k]) // 用 tempSort 跟 result 比對找到該名次位置
                        {
                            result[j] = tempRank[k]; // 將位於 tempRank 該位置的值灌給 result (tempSort/tempRank 長度與值所在位置皆一樣)
                            break;
                        }
                    }
                }

                i = endIndex; // 這組排序完成 接下來從下一組的第一個值開始
            }

            return result;
        }

        public void writeRankSubTotal(int totalRow, List<string> rankTotal, List<string> agreeResult, string votedNumber)
        {
            int row = 6;
            int col = 6; // F

            writeRankTotalStyle(row, col, totalRow, agreeResult, votedNumber);
            excelUtil.write(row, col, "排序小計");

            for (int i = 0; i < totalRow; i++)
            {
                row = 7 + i;
                excelUtil.write(row, col, rankTotal[i]);
            }
        }

        public void writeRankTotal(int totalRow, List<string> rankResult, List<string> agreeResult, string votedNumber)
        {
            int row = 6;
            int col = 7; // G

            writeRankTotalStyle(row, col, totalRow, agreeResult, votedNumber);
            excelUtil.write(row, col, "排序結果");

            for (int i = 0; i < totalRow; i++)
            {
                row = 7 + i;
                excelUtil.write(row, col, rankResult[i]);
            }
        }

        /****************************************************************************************
         * 同意及排序 Style
         ****************************************************************************************/

        public void writeGridHeaderStyle(int row, int col)
        {
            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 16);
            excelUtil.setFontColorBule(row, col);
            excelUtil.setCellBorder(row, col);
        }

        public void writeVotesAndSignatureStyle(int row, int col, int mergeCols)
        {
            char startCol = (char)('A' + col - 1);
            string range = startCol + row.ToString() + ":" + (char)(startCol + mergeCols) + row.ToString();

            excelUtil.mergeColumn(range);
            excelUtil.setRowHeight(row.ToString() + ":" + row.ToString(), 40);

            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 16);
        }

        public void writeUnitAndNameStyle(int row, int col, int listCount)
        {
            excelUtil.setColumnWidth(((char)('A' + col - 1) + ":" + (char)('A' + col)), 20); // C ~ D
            excelUtil.setColumnWidth(((char)('A' + col + 1) + ":" + (char)('A' + col + 1)), 15); // E

            for (int i = 0; i <= listCount; i++)
            {
                for (int j = 0; j < 3; j++) // Department, Unit, Name
                {
                    int tempRow = row + i;
                    int tempCol = col + j;

                    excelUtil.font(tempRow, tempCol);
                    excelUtil.setCellBorder(tempRow, tempCol);
                }
            }
        }

        public void mergeUnitStyle(int i, int j, bool isChanged)
        {
            char startCol = 'C';
            char middleCol = 'D';
            char endCol = 'E';
            string range = startCol + (i + 7).ToString() + ":" + endCol + (j + 6).ToString(); // because j >= i + 1

            if (isChanged)
            {
                excelUtil.setInteriorColorCoral(range);
            }

            excelUtil.mergeColumn(startCol + (i + 7).ToString() + ":" + startCol + (j + 6).ToString());
            excelUtil.mergeColumn(middleCol + (i + 7).ToString() + ":" + middleCol + (j + 6).ToString());
        }

        public void writeVoteStatisticsStyle(int row, int col, int listCount)
        {
            for (int i = 0; i <= listCount; i++)
            {
                row = 6 + i;

                excelUtil.font(row, col);
                excelUtil.fontSize(row, col, 16);
                excelUtil.setCellBorder(row, col);
            }
        }

        public void writeVoteTitleStyle(int row, int col)
        {
            excelUtil.setColumnWidth(((char)('A' + col - 1) + ":" + (char)('A' + col)), 20); // F ~ G
            excelUtil.setColumnWidth(((char)('A' + col + 1) + ":" + (char)('A' + col + 1)), 15); // H

            for (int i = 0; i < 3; i++) // 同意票數, 不同意票數, 是否達出席委員2/3以上門檻
            {
                excelUtil.font(row, col + i);
                excelUtil.setCellBorder(row, col + i);

                if (i == 2) // 是否達出席委員2/3以上門檻
                {
                    excelUtil.fontSize(row, col + i, 12);
                }
                else // 同意票數, 不同意票數
                {
                    excelUtil.fontSize(row, col + i, 16);
                    excelUtil.setFontColorBule(row, col + i);
                }
            }
        }

        public void writeVoteTotalStyle(int row, int col, int totalRow, List<string> agreeResult, string votedNumber)
        {
            for (int i = 0; i < totalRow; i++)
            {
                row = 7 + i;

                for (int j = 0; j < 3; j++) // 同意票數, 不同意票數, 是否達出席委員2/3以上門檻
                {
                    excelUtil.font(row, col + j);
                    excelUtil.fontSize(row, col + j, 16);
                    excelUtil.setCellBorder(row, col + j);

                    if (Convert.ToInt32(agreeResult[i]) < Convert.ToInt32(votedNumber))
                    {
                        excelUtil.setInteriorColorYellow(row, col + j);
                    }
                }
            }
        }

        public void writeRankTotalStyle(int row, int col, int totalRow, List<string> agreeResult, string votedNumber)
        {
            char startCol = (char)('A' + col - 1);
            string range = startCol + row.ToString() + ":" + startCol + row.ToString();

            excelUtil.setColumnWidth(range, 20);

            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 16);
            excelUtil.setFontColorBule(row, col);
            excelUtil.setCellBorder(row, col);

            for (int i = 0; i < totalRow; i++)
            {
                row = 7 + i;

                excelUtil.font(row, col);
                excelUtil.fontSize(row, col, 16);
                excelUtil.setCellBorder(row, col);
                
                if (Convert.ToInt32(agreeResult[i]) >= Convert.ToInt32(votedNumber))
                {
                    excelUtil.setFontColorBule(row, col);
                }
                else
                {
                    excelUtil.setInteriorColorYellow(row, col);
                }
            }
        }

        /****************************************************************************************
         * 同意與不同意
         ****************************************************************************************/

        public void writeTitle(string titleName, int col)
        {
            IList<string> temp = new List<string>() { "同意", "不同意" };
            int tempCount = temp.Count;
            int row = 7;

            writeTitleStyle(row, col, tempCount);
            excelUtil.write(row, col, titleName);
            
            for (int i = col; i < tempCount + col; i++)
            {
                row = 8;

                excelUtil.write(row, i, temp[i - col]);
            }
        }

        public void writeIndex(List<int> indexList, IList<string> ips)
        {
            int row = 7;
            int col = 3; // C
            int indexListCount = indexList.Count;

            writeIndexStyle(row, col, indexListCount);

            for (int i = 0; i < indexListCount + 2; i++, row++)
            {
                excelUtil.write(row, col, row < 9 ? string.Empty : "(" + indexList[i - 2].ToString() + ") " + ips[i - 2]);
            }
        }

        public void writeRankStatistics(VoteDataItem data, int no)
        {
            int row = 8;
            int col = 4; // D
            int candidateListCount = data.CandidateList.Count;

            writeRankStatisticsStyle(row, col, no, candidateListCount);

            for (int i = 0; i < candidateListCount; i++, col += 2)
            {
                excelUtil.write(row + no, isAgree(data.CandidateList[i]) ? col : col + 1, "O");
            }
        }

        public void writeTitleTextOnly(string titleName, int col)
        {
            int row = 7;

            writeTitleTextOnlyStyle(row, col);

            excelUtil.write(row, col, titleName);
        }

        public void writeResultTitle()
        {
            int row = 7;
            int col = 3;

            writeResultTitleStyle(row, col);

            for (int i = row; i < row + 2; i++) // one empty cell
            {
                excelUtil.write(i, col, i == 7 ? string.Empty : "同意票數");
            }
        }

        public void writeResultList(List<int> resultList)
        {
            int row = 8;
            int col = 4; // D
            int resultListCount = resultList.Count;

            for (int i = 0; i < resultListCount; i++, col++)
            {
                writeResultListStyle(row, col);
                excelUtil.write(row, col, resultList[i].ToString());
            }
        }

        /****************************************************************************************
         * 同意與不同意 Style
         ****************************************************************************************/

        public void writeTitleStyle(int row, int col, int tempCount)
        {
            char startCol = (char)('A' + col - 1);
            char endCol = (char)('A' + col);
            string range = startCol + ":" + endCol;

            for (int i = col; i < col + 2; i++)
            {
                excelUtil.font(row, i);
                excelUtil.fontSize(row, i, 18);
                excelUtil.setCellBorder(row, i);
                excelUtil.setCellAlignCenter(row, i);
            }

            excelUtil.setColumnWidth(range, 25);
            excelUtil.setRowHeight(row.ToString() + ":" + row.ToString(), 80);

            range = startCol + row.ToString() + ":" + endCol + row.ToString();
            excelUtil.mergeColumn(range);
            row = 8;

            for (int i = col; i < tempCount + col; i++)
            {
                excelUtil.font(row, i);
                excelUtil.fontSize(row, i, 18);
                excelUtil.setCellBorder(row, i);
                excelUtil.setCellAlignCenter(row, i);
            }
        }

        public void writeIndexStyle(int row, int col, int indexListCount)
        {
            for (int i = 0; i < indexListCount + 2; i++, row++)
            {
                excelUtil.font(row, col);
                excelUtil.fontSize(row, col, 18);
                excelUtil.setCellBorder(row, col);
                excelUtil.setCellAlignCenter(row, col);
            }
        }

        public void writeRankStatisticsStyle(int row, int col, int no, int candidateListCount)
        {
            for (int i = 0; i < candidateListCount; i++, col += 2)
            {
                for (int j = 0; j < 2; j++) // 同意, 不同意
                {
                    excelUtil.font(row + no, col + j);
                    excelUtil.fontSize(row + no, col + j, 18);
                    excelUtil.setCellBorder(row + no, col + j);
                    excelUtil.setCellAlignCenter(row + no, col + j);
                    excelUtil.setInteriorColorYellow(row + no, col + j);
                }
            }
        }

        public void writeTitleTextOnlyStyle(int row, int col)
        {
            char startCol = (char)('A' + col - 1);
            string range = startCol + ":" + startCol;

            excelUtil.setColumnWidth(range, 50);
            excelUtil.setRowHeight(row.ToString() + ":" + row.ToString(), 80);

            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 18);
            excelUtil.setCellBorder(row, col);
            excelUtil.setCellAlignCenter(row, col);
        }

        public void writeResultTitleStyle(int row, int col)
        {
            for (int i = row; i < row + 2; i++) // one empty cell
            {
                excelUtil.font(i, col);
                excelUtil.fontSize(i, col, 18);
                excelUtil.setCellBorder(i, col);
                excelUtil.setCellAlignCenter(i, col);
            }
        }

        public void writeResultListStyle(int row, int col)
        {
            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 18);
            excelUtil.setCellBorder(row, col);
            excelUtil.setCellAlignCenter(row, col);
            excelUtil.setInteriorColorYellow(row, col);
        }

        /****************************************************************************************
         ****************************************************************************************/
    }
}
