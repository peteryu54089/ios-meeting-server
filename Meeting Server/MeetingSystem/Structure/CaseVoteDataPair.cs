using MeetingSystem.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MeetingSystem
{
  public class CaseVoteDataPair : INotifyPropertyChanged
  {
    private CaseVoteSettingDataItem caseVoteSettingItem = null;
    private List<Object[]> dataItems = new List<Object[]>();
    public CaseVoteDataPair(CaseVoteSettingDataItem settingDataItem)
    {
      this.caseVoteSettingItem = settingDataItem;
      caseVoteSettingItem.PropertyChanged += OnPropertyChanged;
    }
    public String SettleName
    {
      get
      {
        return CaseVoteSettingStatus == (CaseVoteSettingDataItem.CaseVoteSettingStatus.COMPLETE) ? "開票" : "結算";
      }
    }
    public CaseVoteSettingDataItem.CaseVoteSettingStatus CaseVoteSettingStatus
    {
      get { return caseVoteSettingItem.Status; }
    }
    public int CaseVoteId
    {
      get { return caseVoteSettingItem.CaseVoteId; }
    }
    public String DataSource
    {
      get
      {
        String path = CaseVoteSettingItem == null ? "" : CaseVoteSettingItem.CaseVoteSourcePath;
        return path;
      }
    }
    public String DataResult
    {
      get
      {
        String path = CaseVoteSettingItem == null ? "" : CaseVoteSettingItem.CaseVoteResultPath;
        return path;
      }
    }
    public String DataSourceName
    {
      get
      {
        String path = CaseVoteSettingItem == null ? "" : CaseVoteSettingItem.CaseVoteSourcePath;
        String name = path;
        if (name.Contains("."))
          name = path.Substring(path.LastIndexOf("\\") + 1);
        if (name.Contains("."))
          name = name.Remove(name.LastIndexOf("."));
        return name;
      }
    }
    public List<Object[]> DataItems //ip + CaseVoteDataItem
    {
      get { return dataItems; }
      set
      {
        dataItems = value;
        OnPropertyChanged("DataItems");
      }
    }
    public int CaseVoteDataItemCount
    {
      get { return DataItems.Count; }
    }
    public void saveDataItem(MeetingClient client, CaseVoteDataItem caseVoteDataItem)
    {
      Console.Write(client.IpAddr + "   ");
      caseVoteDataItem.print();
      Object[] existData = findByIp(client.IpAddr);
      if (existData != null)
      {
        Console.WriteLine("####Error#### Repeat Case Vote data");
        caseVoteDataItem.print();
        existData[1] = caseVoteDataItem;
      }
      else
      {
        DataItems.Add(new Object[] { client.IpAddr, caseVoteDataItem });
      }
      OnPropertyChanged("CaseVoteDataItemCount");
    }
    public Object[] findByIp(Object ip)
    {
      for (int i = 0; i < DataItems.Count; i++)
      {
        Object[] data = DataItems.ElementAt(i);
        if (data[0].Equals(ip))
          return data;
      }
      return null;
    }
    public CaseVoteSettingDataItem CaseVoteSettingItem
    {
      get { return caseVoteSettingItem; }
      set
      {
        caseVoteSettingItem = value;
        OnPropertyChanged("CaseVoteSettingItem");
      }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private VoteSettingDataItem settingDataItem;
    // Create the OnPropertyChanged method to raise the event 
    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }

    protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(sender, e);
      }
    }
  }
}
