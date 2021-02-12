using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Structure;
using MeetingSystem.Score;
using System.Collections;

namespace MeetingSystem.Assessment
{
    class AssessmentResultWriter : DataWriter
    {
        public AssessmentResultWriter(string path)
        {
            filePath = path;
            excelUtil = new ExcelUtil(path, false, 1);
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
        /// SetCellStyle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void SetCellStyle(int row, int col)
        {
            excelUtil.font(row, col);
            excelUtil.fontSize(row, col, 14);
            excelUtil.setCellAlignCenter(row, col);
            excelUtil.setCellBorder(row, col);
        }

        /// <summary>
        /// WriteHeader
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="ips"></param>
        public void WriteHeader(int row, int col, List<string> ips)
        {
            excelUtil.deleteRows(5, 50);

            excelUtil.write(row, col, "服務單位及職稱"); // E5
            SetCellStyle(row, col++);
            excelUtil.write(row, col, "姓名"); // F5
            SetCellStyle(row, col++);

            for (int i = 0; i < ips.Count; i++)
            {
                excelUtil.write(row, col, "(" + (i + 1).ToString() + ") " + ips.ElementAt(i));
                SetCellStyle(row, col++);
            }

            excelUtil.write(row, col, "評分小計");
            SetCellStyle(row, col++);
            excelUtil.write(row, col, "順序");
            SetCellStyle(row, col++);
        }

        /// <summary>
        /// WriteDepartmentsAndNames
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="departments"></param>
        /// <param name="names"></param>
        public void WriteDepartmentsAndNames(int row, int col, List<string> departments, List<string> names)
        {
            for (int i = 0; i < departments.Count; i++)
            {
                excelUtil.write(row + i, col, departments.ElementAt(i));
                SetCellStyle(row + i, col);
            }

            col++;

            for (int i = 0; i < names.Count; i++)
            {
                excelUtil.write(row + i, col, names.ElementAt(i));
                SetCellStyle(row + i, col);
            }
        }

        /// <summary>
        /// WriteScores
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="candidateAverages"></param>
        public void WriteScores(int row, int col, double[,] candidateAverages)
        {
            for (int i = 0; i < candidateAverages.GetLength(0); i++)
            {
                for (int j = 0; j < candidateAverages.GetLength(1); j++)
                {
                    excelUtil.write(row + i, col + j, candidateAverages[i, j].ToString());
                    SetCellStyle(row + i, col + j);
                }
            }
        }

        /// <summary>
        /// WriteAverages
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="candidateAverages"></param>
        public void WriteAverages(int row, int col, List<double> candidateAverages)
        {
            for (int i = 0; i < candidateAverages.Count; i++)
            {
                excelUtil.write(row + i, col, candidateAverages.ElementAt(i).ToString());
                SetCellStyle(row + i, col);
            }
        }

        /// <summary>
        /// WriteRanks
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="candidateRanks"></param>
        public void WriteRanks(int row, int col, int[] candidateRanks)
        {
            for (int i = 0; i < candidateRanks.Length; i++)
            {
                excelUtil.write(row + i, col, candidateRanks[i].ToString());
                SetCellStyle(row + i, col);
            }
        }

        /// <summary>
        /// WriteRanksNoStyle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="candidateCount"></param>
        public void WriteRanksNoStyle(int row, int col, int candidateCount)
        {
            int rank = 1;
            int[] candidateRanks = new int[candidateCount];

            List<double> candidateAverages = new List<double>();
            List<double> candidateSortedAverages = new List<double>();

            for (int i = 0; i < candidateCount; i++)
            {
                candidateAverages.Add(Convert.ToDouble(excelUtil.read(row + (i * 2), 14))); // N
            }
            
            candidateSortedAverages = candidateAverages.OrderByDescending(c => c).Distinct().Cast<double>().ToList();

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

            for (int i = 0; i < candidateRanks.Length; i++)
            {
                excelUtil.write(row + (i * 2), col, candidateRanks[i].ToString());
            }
        }

        /// <summary>
        /// WriteAveragesNoStyle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="candidateAverages"></param>
        public void WriteAveragesNoStyle(int row, int col, List<double> candidateAverages)
        {
            for (int i = 0; i < candidateAverages.Count; i++)
            {
                excelUtil.write(row + (i * 2), col, candidateAverages.ElementAt(i).ToString());
            }
        }

        /// <summary>
        /// GetDetails
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public string[,] GetDetails(int row, int col, int totalRow)
        {
            int detailsX = 0;
            int detailsY = 0;
            string[,] details = new string[(totalRow / 2), 20]; // A ~ N, D ~ I

            for (int i = 0; i < totalRow; i++)
            {
                detailsY = 0;

                for (int j = 0; j < 14; j++) // A ~ N
                {
                    details[detailsX, detailsY] = excelUtil.read(row + i, col + j);
                    detailsY++;
                }

                i++;

                for (int j = 0; j < 6; j++) // D ~ I
                {
                    details[detailsX, detailsY] = excelUtil.read(row + i, (col + 3) + j); // col started from D
                    detailsY++;
                }

                detailsX++;
            }

            return details;
        }

        /// <summary>
        /// WriteDetailsNoStyle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="details"></param>
        public void WriteDetailsNoStyle(int row, int col, string[,] details)
        {
            int detailsX = 0;
            int detailsY = 0;

            Array2DSort comparer = new Array2DSort(details, 0);
            string[,] sortedDetails = comparer.ToSortedArray();

            for (int i = 0; i < (sortedDetails.GetLength(0) * 2); i++)
            {
                detailsY = 0;

                for (int j = 0; j < 14; j++) // A ~ N
                {
                    WriteText(row + i, col + j, sortedDetails[detailsX, detailsY]);
                    detailsY++;
                }

                i++;

                for (int j = 0; j < 6; j++) // D ~ I
                {
                    WriteText(row + i, (col + 3) + j, sortedDetails[detailsX, detailsY]); // col started from D
                    detailsY++;
                }

                detailsX++;
            }
        }
    }

    class Array2DSort : IComparer<int>
    {
        // maintain a reference to the 2-dimensional array being sorted
        string[,] _sortArray;
        int[] _tagArray;
        int _sortIndex;

        protected string[,] SortArray
        {
            get
            {
                return _sortArray;
            }
        }

        /// <summary>
        /// constructor initializes the sortArray reference
        /// </summary>
        /// <param name="theArray"></param>
        /// <param name="sortIndex"></param>
        public Array2DSort(string[,] theArray, int sortIndex)
        {
            _sortArray = theArray;
            _tagArray = new int[_sortArray.GetLength(0)];

            for (int i = 0; i < _sortArray.GetLength(0); ++i)
            {
                _tagArray[i] = i;
            }

            _sortIndex = sortIndex;
        }

        /// <summary>
        /// ToSortedArray
        /// </summary>
        /// <returns></returns>
        public string[,] ToSortedArray()
        {
            Array.Sort(_tagArray, this);
            string[,] result = new string[_sortArray.GetLength(0), _sortArray.GetLength(1)];

            for (int i = 0; i < _sortArray.GetLength(0); i++)
            {
                for (int j = 0; j < _sortArray.GetLength(1); j++)
                {
                    result[i, j] = _sortArray[_tagArray[i], j];
                }
            }

            return result;
        }

        /// <summary>
        /// x and y are integer row numbers into the sortArray
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual int Compare(int x, int y)
        {
            if (_sortIndex < 0)
            {
                return 0;
            }

            return CompareStrings(x, y, _sortIndex);
        }

        /// <summary>
        /// CompareStrings
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected int CompareStrings(int x, int y, int col)
        {
            return _sortArray[x, col].CompareTo(_sortArray[y, col]);
        }
    }
}
