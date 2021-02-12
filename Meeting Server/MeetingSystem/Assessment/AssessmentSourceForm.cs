using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeetingSystem.Message;
using MeetingSystem.Client;
using MeetingSystem.Server;
using Spire.Xls;
using System.IO;
using MeetingSystem.Utils;
using MeetingSystem.Structure;
using System.Diagnostics;

namespace MeetingSystem.Assessment
{
    public partial class AssessmentSourceForm : Form
    {
        private static int id = 0;
        private Random random = null;
        private string WEB_PATH;

        public AssessmentSourceForm(string WEB_PATH)
        {
            InitializeComponent();
            random = new Random();
            this.WEB_PATH = WEB_PATH;
            assessmentTableBindingSource.DataSource = MessagePool.getMessage(ClientMessage.AssessmentMessage);
        }

        /// <summary>
        /// AssessmentTableTextBox_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssessmentTableTextBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel(.xls,.xlsx)|*.xls;*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((TextBox)sender).Text = dialog.FileName;
            }
        }

        /// <summary>
        /// AssessmentDataGridView_CellClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssessmentDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex > -1)
            {
                object parm = MessagePool.getMessage(ClientMessage.AssessmentMessage).getParm();
                BindingList<AssessmentSettingDataItem> list = (BindingList<AssessmentSettingDataItem>)parm;
                DataGridViewButtonCell button = (DataGridViewButtonCell)assessmentDataGridView.CurrentRow.Cells["deleteButton"];
                list.RemoveAt(e.RowIndex);
            }
        }

        /// <summary>
        /// AddButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            const string HTML_EXT = ".html";
            string assessmentTable = assessmentTableTextBox.Text;
            string assessmentResult = assessmentResultTextBox.Text;

            if (assessmentTable == "" || assessmentResult == "")
            {
                return;
            }

            object parm = MessagePool.getMessage(ClientMessage.AssessmentMessage).getParm();
            if (parm != null)
            {
                BindingList<AssessmentSettingDataItem> list = (BindingList<AssessmentSettingDataItem>)parm;
                string webSourePath = DateTime.Now.Ticks.ToString() + random.Next(100) + HTML_EXT;
                AssessmentSettingDataItem item = new AssessmentSettingDataItem(id++, assessmentTable, assessmentResult, webSourePath);
                item.AssessmentWebIp = string.Format("http://{0}/Assessment.html", MeetingServer.getInstance().IP);
                list.Add(item);
                assessmentTableTextBox.Text = string.Empty;
                assessmentResultTextBox.Text = string.Empty;
            }
            else
            {
                Console.WriteLine("AssessmentTableForm : parm is null.");
            }
        }

        /// <summary>
        /// OkButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            object parm = MessagePool.getMessage(ClientMessage.AssessmentMessage).getParm();

            if (parm != null)
            {
                BindingList<AssessmentSettingDataItem> list = (BindingList<AssessmentSettingDataItem>)parm;
                foreach (AssessmentSettingDataItem dataItem in list)
                {
                    if (dataItem.Status == AssessmentSettingDataItem.AssessmentSettingStatus.NONACTIVE)
                    {
                        string webPathDir = WEB_PATH + dataItem.WebFileName;
                        Xls2Html.xlsToHtml(dataItem.AssessmentSourcePath, webPathDir);
                        dataItem.Status = AssessmentSettingDataItem.AssessmentSettingStatus.ACTIVE;
                    }
                }
            }

            Hide();
        }
    }
}
