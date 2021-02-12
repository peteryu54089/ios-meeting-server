using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Score;
using MeetingSystem.Structure;

namespace MeetingSystem.Rank
{
    public class RankResultWriter : DataWriter
    {
        public RankResultWriter(string filePath)
        {
            this.filePath = filePath;
            excelUtil = new ExcelUtil(filePath, false, 1);
        }

        public RankResultWriter(string filePath,bool isVisible)
        {
            this.filePath = filePath;
            excelUtil = new ExcelUtil(filePath, isVisible, 1);
        }

        public void writeRankTicketWriter(string rankStartValue, int offset,List<Structure.RankDataItem.RankData> list)
        {
            int startCol, startrRow;
            gridToPos(rankStartValue, out startCol, out startrRow);

            int row = startrRow;

            foreach (RankDataItem.RankData data in list)
            {
                string name = data.Name;

                excelUtil.write(row, startCol, name.Remove(name.LastIndexOf(":")));
                excelUtil.write(row, startCol + 1, name.Substring(name.LastIndexOf(":") + 1));
                excelUtil.write(row, startCol + 2, data.Rank);
                row++;
            }
        }

        public void writeRankResult(string startValue, Dictionary<string, int> returnTable, string title)
        {
            int startCol, startrRow;
            gridToPos(startValue, out startCol, out startrRow);

            int row = startrRow + 2;
            excelUtil.write(3, 1, title);

            foreach (KeyValuePair<string, int> pair in returnTable)
            {
                string name = pair.Key;

                excelUtil.write(row, startCol, name.Remove(name.LastIndexOf(":")));
                excelUtil.write(row, startCol + 1, name.Substring(name.LastIndexOf(":") + 1));
                excelUtil.write(row++, startCol + 2, pair.Value.ToString());
            }
        }

        internal void writeRankStatistics(string startValue, int offset, string ip, List<RankDataItem.RankData> list)
        {
            int startCol, startrRow;
            gridToPos(startValue, out startCol, out startrRow);

            int col = startCol;
            excelUtil.write(startrRow, startCol, ip); 
            col++;

            foreach (RankDataItem.RankData data in list)
            {
                string name = data.Name;

                if (excelUtil.read(1, col).Equals(name))
                {
                    excelUtil.write(startrRow, col, data.Rank);
                }
                else
                {
                    for(int i = 1; i <= list.Count; i++)
                    {
                        if (excelUtil.read(1, i).Equals(name))
                        {
                            excelUtil.write(startrRow, i, data.Rank);
                        }
                    }
                }
               
                col++;
            }
        }

        public void writeRankStatisticsTitle(string startValue, int p, List<RankDataItem.RankData> list)
        {
            int startCol, startrRow;
            gridToPos(startValue, out startCol, out startrRow);

            int col = startCol;
            excelUtil.write(startrRow, col++, "委員編號");

            foreach (RankDataItem.RankData data in list)
            {
                string name = data.Name;
                excelUtil.write(startrRow, col++, name);
            }
        }

        public string readTitle()
        {
            return excelUtil.read(1, 1);
        }

        public string readColName1()
        {
            return excelUtil.read(3, 1);
        }

        public string readColName2()
        {
            return excelUtil.read(3, 2);
        }
        
        public void cleanTableRow(int startRow, int endRow)
        {
            excelUtil.deleteRows(startRow, endRow);
        }

        public void setCell(int row, int col)
        {
            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 14);
            excelUtil.setCellAlignCenter(row, col);
            excelUtil.setCellBorder(row, col);
        }

        public void writeTitle(int end, string title)
        {
            char endLetter = (char)('A' + end + 2);

            for (int i = 1; i <= end; i++)
            {
                setCell(3, i);
            }
            
            excelUtil.mergeColumn("A3:" + endLetter + "3");
            excelUtil.write(3, 1, title);
        }

        public void writeColumnName(int end, string colName1, string colName2, List<string> ips)
        {
            for (int i = 1; i <= end + 3; i++) // 加三為額外欄位 系所 姓名 總分
            {
                setCell(4, i);
            }

            excelUtil.setColumnWidth("A4:B4", 20);
            excelUtil.write(4, 1, colName1);
            excelUtil.write(4, 2, colName2);

            for (int i = 3; i < end + 3; i++) // 加三為額外欄位 系所 姓名 總分
            {
                excelUtil.write(4, i, "(" + (i - 2).ToString() + ") " + ips[i - 3]);
            }

            excelUtil.write(4, end + 3, "總分");
        }

        public void writeRankSingle(string startValue, Dictionary<string, int> returnTable, int end)
        {
            int startCol, startrRow;
            gridToPos(startValue, out startCol, out startrRow);

            int row = startrRow + 2;

            foreach (KeyValuePair<string, int> pair in returnTable)
            {
                string name = pair.Key;

                setCell(row, startCol);
                excelUtil.write(row, startCol, name.Remove(name.LastIndexOf(":")));
                setCell(row, startCol + 1);
                excelUtil.write(row, startCol + 1, name.Substring(name.LastIndexOf(":") + 1));
                setCell(row, end + 3);
                excelUtil.write(row++, end + 3, pair.Value.ToString());
            }
        }

        internal void writeRankMultiple(List<RankDataItem.RankData> list, int col)
        {
            int row = 5;

            foreach (RankDataItem.RankData data in list)
            {
                setCell(row, col + 3);
                excelUtil.write(row++, col + 3, data.Rank);
            }
        }
    }
}
