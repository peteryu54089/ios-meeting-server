using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeetingSystem.Structure
{
    class PromotionDataItem
    {
        private int id;
        private List<PromotionData> candidateList = new List<PromotionData>();

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<PromotionData> CandidateList
        {
            get { return candidateList; }
            set { candidateList = value; }
        }

        public class PromotionData
        {
            private string department;
            private string name;
            private string scores;

            public string Department
            {
                get { return department; }
                set { department = value; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public string Scores
            {
                get { return scores; }
                set { scores = value; }
            }

            /// <summary>
            /// 打印 Promotion 相關資訊
            /// </summary>
            public void Print()
            {
                Console.Write("(" + Department + "/" + Name + "/" + Scores);
                Console.Write(")");
            }
        }

        /// <summary>
        /// 打印 Promotion 相關資訊
        /// </summary>
        public void Print()
        {
            Console.WriteLine("id:" + Id);
            Console.Write("candidate:");
            for (int i = 0; i < CandidateList.Count(); i++)
            {
                CandidateList.ElementAt(i).Print();
            }
            Console.WriteLine("");
        }
    }
}
