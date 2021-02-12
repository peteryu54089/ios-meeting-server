using MeetingSystem.Client;
using MeetingSystem.Message;
using MeetingSystem.Server;
using MeetingSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MeetingSystem.CaseVote
{
  public partial class CaseVoteSourceForm : Form
  {
    private Random random = null;
    private static int id = 0;
    private String WEB_PATH;
    public CaseVoteSourceForm(String WEB_PATH)
    {
      random = new Random();
      InitializeComponent();
      this.WEB_PATH = WEB_PATH;
      caseVoteTableBindingSource.DataSource = MessagePool.getMessage(ClientMessage.CaseVoteMessage);
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
      const String HTML_EXT = ".html";
      String caseVoteTable = caseVoteTableTextBox.Text;
      String caseVoteResult = caseVoteResultTextBox.Text;
      //bool order = OrderByCk.Checked;
      if (caseVoteTable.Equals(String.Empty) || caseVoteResult.Equals(String.Empty))
        return;
      Object parm = MessagePool.getMessage(ClientMessage.CaseVoteMessage).getParm();
      if (parm != null)
      {
        BindingList<CaseVoteSettingDataItem> list = (BindingList<CaseVoteSettingDataItem>)parm;
        String webSourcePath = DateTime.Now.Ticks.ToString() + random.Next(100) + HTML_EXT;
        String caseVoteWebIp = String.Format("http://{0}/CasesVote.html", MeetingServer.getInstance().IP);
        CaseVoteSettingDataItem item = new CaseVoteSettingDataItem(id++, caseVoteTable, caseVoteResult, caseVoteWebIp, webSourcePath/*, order*/);
        list.Add(item);
        caseVoteTableTextBox.Text = String.Empty;
        caseVoteResultTextBox.Text = String.Empty;
        //OrderByCk.Checked = false;
      }
      else
      {
        Console.WriteLine("CaseVoteSourceForm : parm is null.");
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      Console.WriteLine("CaseVoteSourceForm.buttonOK_Click()");
      this.DialogResult = System.Windows.Forms.DialogResult.OK;
      Object parm = MessagePool.getMessage(ClientMessage.CaseVoteMessage).getParm();
      if (parm != null)
      {
        BindingList<CaseVoteSettingDataItem> list = (BindingList<CaseVoteSettingDataItem>)parm;
        foreach (CaseVoteSettingDataItem dataItem in list)
        {
          if (dataItem.Status == CaseVoteSettingDataItem.CaseVoteSettingStatus.NONACTIVE)
          {
            String webPathDir = WEB_PATH + dataItem.WebFileName;
            Console.WriteLine("Target dataItem.CaseVoteSourcePath: " + dataItem.CaseVoteSourcePath);
            Xls2Html.xlsToHtml(dataItem.CaseVoteSourcePath, webPathDir);
            dataItem.Status = CaseVoteSettingDataItem.CaseVoteSettingStatus.ACTIVE;
          }
        }
      }
      Hide();
    }

    private void caseVoteTableTextBox_Click(object sender, EventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Excel(.xls,.xlsx)|*.xls;*.xlsx";
      if (dialog.ShowDialog() == DialogResult.OK)
        ((TextBox)sender).Text = dialog.FileName;
    }

    private void caseVoteTableTextBox_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      //Console.WriteLine("e.ColumnIndex = " + e.ColumnIndex);
      if (e.ColumnIndex == 2 && e.RowIndex > -1)
      {
        Object parm = MessagePool.getMessage(ClientMessage.CaseVoteMessage).getParm();
        BindingList<CaseVoteSettingDataItem> list = (BindingList<CaseVoteSettingDataItem>)parm;
        DataGridViewButtonCell button = (DataGridViewButtonCell)caseVoteDataGridView.CurrentRow.Cells["DeleteButton"];
        list.RemoveAt(e.RowIndex);
      }
    }
  }
}
