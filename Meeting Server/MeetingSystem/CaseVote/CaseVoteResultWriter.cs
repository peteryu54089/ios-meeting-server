using MeetingSystem.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeetingSystem.CaseVote
{
    public class CaseVoteResultWriter : DataWriter
    {
        public bool isSetTitleHeight;

        public CaseVoteResultWriter(String path)
        {
            filePath = path;
            excelUtil = new ExcelUtil(path, false, 1);
        }

        /// <summary>
        /// clear ballot row
        /// </summary>
        public void clearBallotRow(int startRow, int endRow)
        {
            excelUtil.deleteRows(startRow, endRow);
        }

        /// <summary>
        /// 寫入投票人數與門檻
        /// </summary>
        /// <param name="votedNum"></param>
        /// <param name="passThershold"></param>
        public void writeVotedNumAndPassThershold(string votedNum, string passThershold)
        {
            writeVotedNumAndPassThersholdStyle();

            excelUtil.write(3, 1, "本次投票人數"); // A3
            excelUtil.write(3, 2, votedNum); // B3
            excelUtil.write(4, 1, "全部人數2/3"); // A4
            excelUtil.write(4, 2, passThershold); // B4
        }

        /// <summary>
        /// 寫入 "評選事項" 字樣
        /// </summary>
        /// <param name="totalCol"></param>
        public void writeOptionTitle(int totalCol)
        {
            int row = 7;
            int col = 4; // D

            writeOptionTitleStyle(row, col, totalCol);

            excelUtil.write(row, col, "評選事項");
        }

        /// <summary>
        /// 寫入所有評選事項標題
        /// </summary>
        /// <param name="titleName"></param>
        /// <param name="startCol"></param>
        public void writeTitle(string titleName, int startCol)
        {
            int row = 8;

            writeTitleStyle(row, startCol, titleName);

            excelUtil.write(row, startCol, titleName);
        }

        public bool isAgree(Object obj)
        {
            CaseVoteDataItem.CaseVoteData caseVoteData = (CaseVoteDataItem.CaseVoteData)obj;
            return caseVoteData.AgreeChoose;
        }

        public void writeIndex(List<int> indexList, List<string> ipList)
        {
            int row = 9;
            int col = 3; // C
            int indexListCount = indexList.Count;

            writeIndexStyle(row, col, indexListCount);

            for (int i = 0; i < indexListCount; i++, row++)
            {
                excelUtil.write(row, col, "(" + indexList[i].ToString() + ") " + ipList[i]);
            }
        }

        public void writeRankStatistics(CaseVoteDataItem data, int no)
        {
            int row = 8;
            int col = 4;
            int candidateListCount = data.CandidateList.Count;

            writeRankStatisticsStyle(row, col, candidateListCount, no);

            for (int i = 0; i < candidateListCount; i++, col++)
            {
                excelUtil.write(row + no, col, isAgree(data.CandidateList[i]) ? "O" : "X");
            }
        }

        /// <summary>
        /// 寫入 "票數" 字樣
        /// </summary>
        public void writeTotalBallot()
        {
            int row = 9;
            int col = 3; // C

            writeTotalBallotStyle(row, col);

            excelUtil.write(row, col, "票數");
        }

        public void writeResultList(List<int> resultList)
        {
            int row = 9;
            int col = 4; // D
            int resultListCount = resultList.Count;

            writeResultListStyle(row, col, resultListCount);

            for (int i = 0; i < resultListCount; i++, col++)
            {
                excelUtil.write(row, col, resultList[i].ToString());
            }
        }

        /****************************************************************************************
         * Style
         ****************************************************************************************/

        /// <summary>
        /// 寫入投票人數與門檻 Style
        /// </summary>
        public void writeVotedNumAndPassThersholdStyle()
        {
            int startRow = 3, endRow = 4; // A3:B4
            int startCol = 1, endCol = 2;

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    excelUtil.fontSize(i, j, 20);
                    excelUtil.setCellBorder(i, j);
                }
            }

            excelUtil.setColumnWidth("A3:B3", 30);
        }

        /// <summary>
        /// 寫入 "評選事項" 字樣 Style
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="totalCol"></param>
        public void writeOptionTitleStyle(int row, int col, int totalCol)
        {
            char endCol = (char)('A' + (totalCol + 2)); // 起始從 D 行
            string range = "D7:" + endCol + "7";

            for (int i = col; i <= totalCol + 3; i++)
            {
                excelUtil.font(row, i);
                excelUtil.fontSize(row, i, 18);
                excelUtil.setCellAlignDistributed(row, i);
                excelUtil.setCellBorder(row, i);
            }

            excelUtil.setRowHeight(range, 40);
            excelUtil.mergeColumn(range);
        }

        /// <summary>
        /// 寫入所有評選事項標題 Style
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="titleName"></param>
        public void writeTitleStyle(int row, int col, string titleName)
        {
            char startCol = (char)('A' + col - 1);
            string range = startCol + row.ToString() + ":" + startCol + row.ToString();

            if (titleName.Length < 10 && !isSetTitleHeight)
            {
                excelUtil.setRowHeight(range, 50);
                excelUtil.setCellAlignCenter(row, col);
            }
            else
            {
                excelUtil.setRowHeight(range, 250);
                excelUtil.setCellAlignLeft(row, col);
                isSetTitleHeight = true;
            }
            
            excelUtil.setColumnWidth(range, 80);
            excelUtil.setWrapText(range, true);

            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 18);
            excelUtil.setCellBorder(row, col);
        }

        public void writeIndexStyle(int row, int col, int indexListCount)
        {
            char startCol = (char)('A' + col - 1);
            string range = startCol + row.ToString() + ":" + startCol + row.ToString();

            excelUtil.setColumnWidth(range, 20);

            for (int i = 0; i < indexListCount; i++, row++)
            {
                excelUtil.font(row, col);
                excelUtil.fontSize(row, col, 18);
                excelUtil.setCellAlignLeft(row, col);
            }
        }

        public void writeRankStatisticsStyle(int row, int col, int candidateListCount, int no)
        {
            for (int i = 0; i < candidateListCount; i++, col++)
            {
                excelUtil.font(row + no, col);
                excelUtil.fontSize(row + no, col, 18);
                excelUtil.setCellAlignCenter(row + no, col);
                excelUtil.setCellBorder(row + no, col);
            }
        }

        /// <summary>
        /// 寫入 "票數" 字樣 Style
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void writeTotalBallotStyle(int row, int col)
        {
            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 18);
            excelUtil.setCellAlignCenter(row, col);
        }

        public void writeResultListStyle(int row, int col, int resultListCount)
        {
            for (int i = 0; i < resultListCount; i++, col++)
            {
                excelUtil.font(row, col);
                excelUtil.fontSize(row, col, 18);
                excelUtil.setCellBorder(row, col);
                excelUtil.setCellAlignCenter(row, col);
            }
        }

        /****************************************************************************************
         ****************************************************************************************/
    }
}
