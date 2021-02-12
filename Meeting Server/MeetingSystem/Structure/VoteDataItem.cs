using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeetingSystem
{
    public class VoteDataItem
    {
        private int id = -1;
        private List<VoteData> candidateList = new List<VoteData>();
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public List<VoteData> CandidateList
        {
            get { return candidateList; }
            set { candidateList = value; }
        }
        public class VoteData
        {
            private String name;
            private Boolean orderBy;
            private Boolean agreeChoose;
            private Boolean rejectChoose;
            private Boolean invalidateChoose;
            private String department;
            private String unit;
            private String rank;

            public String Rank
            {
                get { return rank; }
                set { rank = value; }
            }
            public String Department
            {
                get { return department; }
                set { department = value; }
            }
            public String Unit
            {
                get { return unit; }
                set { unit = value; }
            }
            public Boolean InvalidateChoose
            {
                get { return invalidateChoose; }
                set { invalidateChoose = value; }
            }
            public String Name
            {
                get { return name; }
                set { name = value; }
            }
            public Boolean OrderBy
            {
                get { return orderBy; }
                set { orderBy = value; }
            }
            public Boolean AgreeChoose
            {
                get { return agreeChoose; }
                set { agreeChoose = value; }
            }
            public Boolean RejectChoose
            {
                get { return rejectChoose; }
                set { rejectChoose = value; }
            }
            public void print()
            {
                Console.Write(name + "/" + agreeChoose + "/" + rejectChoose + "/" + invalidateChoose);
            }
        }

        public void print()
        {
            Console.Write("id:" + Id);
            Console.Write("   candidate:");
            for(int i =0; i < CandidateList.Count(); i++)
            {
                CandidateList.ElementAt(i).print();
                Console.Write("   ");
            }
            Console.WriteLine("");
        }
    }
}
