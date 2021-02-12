using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Structure;
using MeetingSystem.Score;
using System.Collections;

namespace MeetingSystem.Promotion
{
    class PromotionResultWriter : DataWriter
    {
        public PromotionResultWriter(string path)
        {
            filePath = path;
            excelUtil = new ExcelUtil(path, false, 1);
        }

        /// <summary>
        /// read title from C3
        /// </summary>
        /// <returns></returns>
        public string ReadTitle()
        {
            return excelUtil.read(3, 3);
        }

        /// <summary>
        /// read date from V2
        /// </summary>
        /// <returns></returns>
        public string ReadDate()
        {
            return excelUtil.read(2, 22);
        }

        /// <summary>
        /// write text
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="text"></param>
        public void WriteText(int row, int col, string text)
        {
            excelUtil.write(row, col, text);
        }

        /// <summary>
        /// WriteDepartmentAndName
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="names"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void WriteDepartmentAndName(List<string> departments, List<string> names, int row, int col)
        {
            for (int i = 0; i < departments.Count; i++)
            {
                WriteText(i + row, col, departments[i]);
                WriteText(i + row, col + 1, names[i]);
            }
        }

        /// <summary>
        /// WriteScores
        /// </summary>
        /// <param name="scoreArray"></param>
        /// <param name="scoreTemp"></param>
        /// <param name="scoreB"></param>
        /// <param name="scoreC"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void WriteScores(double[,] scoreArray, double[] scoreTemp, List<double> scoreB, List<double> scoreC, int row, int col)
        {
            for (int i = 0; i < scoreArray.GetLength(0); i++)
            {
                for (int j = 0; j < scoreArray.GetLength(1); j++)
                {
                    WriteText(i + row, j + col, (scoreArray[i, j]).ToString());
                }
            }

            for (int i = 0; i < scoreTemp.Count(); i++)
            {
                WriteText(i + row, 11 + col, (scoreTemp[i]).ToString());
            }

            for (int i = 0; i < scoreB.Count; i++)
            {
                WriteText(i + row, 12 + col, (scoreB.ElementAt(i)).ToString());
            }

            for (int i = 0; i < scoreC.Count; i++)
            {
                WriteText(i + row, 13 + col, (scoreC.ElementAt(i)).ToString());
            }
        }

        /// <summary>
        /// WriteCommitteeIps
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="ips"></param>
        public void WriteCommitteeIps(int row, int col, List<string> ips)
        {
            for (int i = 0; i < ips.Count; i++)
            {
                WriteText(row, col + i, "(" + (i + 1) + ") " + ips.ElementAt(i));
                SetDetailSheetStyle(row, col + i);
            }
        }

        /// <summary>
        /// WriteCandidateScoreDetail
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="detailArray"></param>
        public void WriteCandidateScoreDetail(int row, int col, double[,] detailArray)
        {
            for (int i = 0; i < detailArray.GetLength(0); i++)
            {
                for (int j = 0; j < detailArray.GetLength(1); j++)
                {
                    WriteText(row + i, col + j, detailArray[i, j].ToString());
                    SetDetailSheetStyle(row + i, col + j);
                }
            }
        }

        /// <summary>
        /// WriteCandidateAveragee
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="candidateAverage"></param>
        public void WriteCandidateAverage(int row, int col, List<double> candidateAverage)
        {
            for (int i = 0; i < candidateAverage.Count; i++)
            {
                WriteText(row + i, col, candidateAverage.ElementAt(i).ToString());
                SetDetailSheetStyle(row + i, col);
            }
        }

        /// <summary>
        /// InitDetailSheet
        /// </summary>
        /// <param name="names"></param>
        /// <param name="averageCol"></param>
        public void InitDetailSheet(List<string> names, int averageCol)
        {
            excelUtil.deleteRows(4, 50);

            WriteText(4, 1, "編號"); // A4
            WriteText(4, 2, "姓名"); // B4
            WriteText(4, averageCol, "平均");
            WriteText(names.Count + 6, 1, "監票人簽名");
            WriteText(names.Count + 9, 1, "計票人簽名");

            SetDetailSheetStyle(4, 1); // A4
            SetDetailSheetStyle(4, 2); // B4
            SetDetailSheetStyle(4, averageCol);

            for (int i = 0; i < names.Count; i++)
            {
                WriteText(i + 5, 1, (i + 1).ToString());
                WriteText(i + 5, 2, names.ElementAt(i));

                SetDetailSheetStyle(i + 5, 1);
                SetDetailSheetStyle(i + 5, 2);
            }
        }

        /// <summary>
        /// SetDetailSheetStyle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void SetDetailSheetStyle(int row, int col)
        {
            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 14);
            excelUtil.setCellAlignCenter(row, col);
            excelUtil.setCellBorder(row, col);
        }
    }
}
