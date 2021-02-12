using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MeetingSystem
{
  public class CaseVoteSettingDataItem : INotifyPropertyChanged
  {
    private int caseVoteId;
    private String caseVoteSourcePath;
    public String caseVoteResultPath;
    private String orderPath;
    private String orderResultPath;
    private bool orderBy;
    private String caseVoteWebIp;
    private String webFileName;
    private CaseVoteSettingStatus status;
    public enum CaseVoteSettingStatus { ACTIVE, NONACTIVE, COMPLETE };
    public String OrderPath
    {
      get { return orderPath; }
      set
      {
        orderPath = value;
        OnPropertyChanged("OrderPath");
      }
    }
    public String OrderResultPath
    {
      get { return orderResultPath; }
      set
      {
        orderResultPath = value;
        OnPropertyChanged("OrderResultPath");
      }
    }
    public bool OrderBy
    {
      get { return orderBy; }
      set
      {
        orderBy = value;
        OnPropertyChanged("OrderBy");
      }
    }
    public CaseVoteSettingStatus Status
    {
      get { return status; }
      set
      {
        status = value;
        OnPropertyChanged("Status");
      }
    }
    public CaseVoteSettingDataItem(int caseVoteId, String caseVoteSourcePath, String caseVoteResultPath, String caseVoteWebIp, String webFileName/*, bool orderBy*/)
    {
      this.caseVoteId = caseVoteId;
      this.caseVoteSourcePath = caseVoteSourcePath;
      this.caseVoteResultPath = caseVoteResultPath;
      this.caseVoteWebIp = caseVoteWebIp;
      this.webFileName = webFileName;
      //this.orderBy = orderBy;
      this.orderPath = "./個案投票.xlsx";
      this.orderResultPath = "./個案投票結果.xlsx";
      Status = CaseVoteSettingStatus.NONACTIVE;
    }
    public String CaseVoteSourcePath
    {
      get { return caseVoteSourcePath; }
      set
      {
        caseVoteSourcePath = value;
        OnPropertyChanged("caseVoteSourcePath");
      }
    }
    public String CaseVoteResultPath
    {
      get { return caseVoteResultPath; }
      set
      {
        caseVoteResultPath = value;
        OnPropertyChanged("caseVoteResultPath");
      }
    }
    public String CaseVoteWebIp
    {
      get { return caseVoteWebIp; }
      set
      {
        caseVoteWebIp = value;
        OnPropertyChanged("caseVoteWebIp");
      }
    }
    public String WebFileName
    {
      get { return webFileName; }
      set
      {
        webFileName = value;
        OnPropertyChanged("WebFileName");
      }
    }
    public int CaseVoteId
    {
      get { return caseVoteId; }
      set { caseVoteId = value; OnPropertyChanged("caseVoteId"); }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }
  }
}
