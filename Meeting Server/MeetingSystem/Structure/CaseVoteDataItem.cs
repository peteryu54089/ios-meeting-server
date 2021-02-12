using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeetingSystem
{
  public class CaseVoteDataItem
  {
    private int id = -1;
    private List<CaseVoteData> candidateList = new List<CaseVoteData>();
    public int Id
    {
      get { return id; }
      set { id = value; }
    }
    public List<CaseVoteData> CandidateList
    {
      get { return candidateList; }
      set { candidateList = value; }
    }
    public class CaseVoteData
    {
      private String name;
      private Boolean agreeChoose;
      private Boolean rejectChoose;
      private Boolean invalidateChoose;

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
      for (int i = 0; i < CandidateList.Count(); i++)
      {
        CandidateList.ElementAt(i).print();
        Console.Write("   ");
      }
      Console.WriteLine("");
    }
  }
}
