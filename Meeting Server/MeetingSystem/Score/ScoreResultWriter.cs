using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Structure;
using MeetingSystem.Score;
using System.Collections;

namespace MeetingSystem
{
    class ScoreResultWriter : DataWriter
    {
        public ScoreResultWriter(string path)
        {
            filePath = path;
            excelUtil = new ExcelUtil(path, false, 1); 
        }

        public ScoreResultWriter(string path, bool isVisible)
        {
            filePath = path;
            excelUtil = new ExcelUtil(path, isVisible, 1);
        }

        /// <summary>
        /// writeScore
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="cellSpan"></param>
        /// <param name="objList"></param>
        public void writeScore(string startPos, string endPos, int cellSpan, Array objList)
        {
            int len = 0;
            int startCol, startrRow;
            int endCol, endRow;

            gridToPos(startPos, out startCol, out startrRow);
            gridToPos(endPos, out endCol, out endRow);
            
            for (int i = startrRow; i <= endRow; i += cellSpan)
            {
                for (int j = startCol; j <= endCol; j += cellSpan)
                {
                    excelUtil.write(i, j, objList.GetValue(len).ToString());
                    len++;
                }
            }
        }

        /// <summary>
        /// read the 升等評分 file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string getTeacherInfo(string data) {
            int startCol, startrRow;
            gridToPos(data, out startCol, out startrRow); // C7 C:col 7:row

            string readTitle = excelUtil.read(2, 2); // B2
            string readName = excelUtil.read(startrRow, startCol); // C7
            string readDepartment = excelUtil.read(startrRow, startCol + 2); // E7
            string readLevel = excelUtil.read(startrRow, startCol + 7); // J7
            string level = "";

            if (string.Compare(readLevel, "副教授") == 0)
            {
                level = "助理教授";
            }
            if (string.Compare(readLevel, "教授") == 0)
            {
                level = "副教授";
            }
            if (string.Compare(readLevel, "教授級專業技術人員") == 0)
            {
                level = ""; // 太長 先不顯示
            }
            if (string.Compare(readLevel, "副教授級專業技術人員") == 0)
            {
                level = ""; // 太長 先不顯示
            }
            if (string.Compare(readLevel, "兼任教授") == 0)
            {
                level = "兼任副教授";
            }
            if (string.Compare(readLevel, "兼任副教授") == 0)
            {
                level = "兼任助理教授";
            }

            return readTitle + readDepartment + level + readName + "-" + readLevel + "升等案";
        }

        /// <summary>
        /// getRangeScore
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public string getRangeScore(string start, string end)
        {
            int startCol, startRow;
            int endCol, endRow;
            string str = "";

            gridToPos(start, out startCol, out startRow); // I15 I:col 15:row
            gridToPos(end, out endCol, out endRow); // I16 I:col 16:row

            for(int i = startRow; i <= endRow; i++){
                for (int j = startCol; j <= endCol; j++) {
                    string score = excelUtil.read(i, j);
                    int startIndex = score.IndexOf('~');
                    
                    str += score.Substring(0, startIndex) + "," + score.Substring(startIndex + 1) + ",";
                }
            }

            return str;
        }

        /// <summary>
        /// getAverageScore
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public string getAverageScore(string pos) {
            int startCol, startRow; // H12
            gridToPos(pos, out startCol, out startRow); // H12 H:col 12:row

            string str = excelUtil.read(startRow, startCol);
            EventLog.Write(str);

            return str;
        }

        /// <summary>
        /// printTeacherInfo
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="info"></param>
        /// <param name="date"></param>
        public void printTeacherInfo(string pos, string info, string date) {
            int startCol, startrRow;
            gridToPos(pos, out startCol, out startrRow); // A2 A:col 2:row
     
            excelUtil.write(startrRow, startCol, info);
            excelUtil.write(startrRow, 5, date);
            excelUtil.font(startrRow, startCol);
            excelUtil.fontSize(startrRow, 5, 10);
        }

        /// <summary>
        /// printRangeScore
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="score"></param>
        public void printRangeScore(string start, string end, string score) {
            int startCol, startRow; // G3
            int endCol, endRow; // H4

            gridToPos(start, out startCol, out startRow); // G3 G:col 3:row
            gridToPos(end, out endCol, out endRow); // H4 H:col 4:row

            string[] scores = score.Split(',');
            int len = 0;

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    excelUtil.write(i, j, scores[len]);
                    len++;
                }
            }     
        }

        /// <summary>
        /// writeRearrangement
        /// </summary>
        /// <param name="indexValue"></param>
        /// <param name="endValue"></param>
        /// <param name="collection"></param>
        public void writeRearrangement(string indexValue, string endValue, object collection)
        {
            int startCol, startRow; // A7
            int endCol, endRow; // E7
            int len = 0, passScore = 0;
            
            string[] str = collection.ToString().Split(',');
            double finalScore = Convert.ToDouble(str[4]);
            double errorCount = Convert.ToDouble(str[5]);

            gridToPos(indexValue, out startCol, out startRow); // A7 A:col 7:row
            gridToPos(endValue, out endCol, out endRow); // E7 E:col 7:row
         
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    if (len == 4 && errorCount != 0)
                    {
                        excelUtil.write(i, j, "廢票");
                    }
                    else
                    {
                        excelUtil.write(i, j, str[len]);
                    }

                    len++;
                }
            }

            if (excelUtil.read(2, 7).Contains(">=80")) // G2
            {
                passScore = 80;
            }
            else if (excelUtil.read(2, 7).Contains(">=78")) // G2
            {
                passScore = 78;
            }

            if (errorCount == 0 && finalScore >= passScore)
            {
                excelUtil.setFontColorBule(startRow, endCol);
            }
            else
            {
                excelUtil.setFontColorRed(startRow, endCol);
            }
        }

        /// <summary>
        /// How many people have voted
        /// </summary>
        /// <param name="ipCount"></param>
        public void writeIpCount(string ipCount) {
            for (int i = 1; i < 100; i++)
            {
                if (excelUtil.read(i, 1) == "參與評分人數：")
                {
                    excelUtil.write(i, 2, ipCount);
                    break;
                }
            }
        }

        /// <summary>
        /// writeReasons
        /// </summary>
        /// <param name="reasonStartValue"></param>
        /// <param name="reasonEndValue"></param>
        /// <param name="cellSpan"></param>
        /// <param name="reasons"></param>
        public void writeReasons(string reasonStartValue, string reasonEndValue, int cellSpan, string[] reasons)
        {
            if (reasons.Length == 0)
            {
                return;
            }
                
            int startCol, startrRow;
            int endCol, endRow;

            gridToPos(reasonStartValue, out startCol, out startrRow);
            gridToPos(reasonEndValue, out endCol, out endRow);

            for (int i = startrRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    string cellStr = excelUtil.read(i, j);

                    if (cellStr.Length > 0)
                    {
                        string reason = cellStr.Substring(cellStr.IndexOf("□") + 1).Replace("___________________", "");
                        cellStr = cellStr.Replace("□", "☑").Replace("___________________", "");

                        if (reasons.Contains(reason))
                        {
                            excelUtil.write(i, j, cellStr);
                        }
                        else if (reasons[reasons.Length - 1].Contains(reason))
                        {
                            excelUtil.write(i, j, reasons[reasons.Length - 1].Replace("其他：（請敘明具體事實）", cellStr));
                        } 
                    }
                }
            } 
        }
    }
}
