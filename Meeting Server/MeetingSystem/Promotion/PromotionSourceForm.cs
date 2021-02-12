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

namespace MeetingSystem.Promotion
{
    public partial class PromotionSourceForm : Form
    {
        private static int id = 0;
        private Random random = null;
        private string WEB_PATH;

        public PromotionSourceForm(string WEB_PATH)
        {
            InitializeComponent();
            random = new Random();
            this.WEB_PATH = WEB_PATH;
            promotionTableBindingSource.DataSource = MessagePool.getMessage(ClientMessage.PromotionMessage);
        }

        /// <summary>
        /// PromotionTableTextBox_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PromotionTableTextBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel(.xls,.xlsx)|*.xls;*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((TextBox)sender).Text = dialog.FileName;
            }
        }

        /// <summary>
        /// PromotionDataGridView_CellClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PromotionDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex > -1)
            {
                object parm = MessagePool.getMessage(ClientMessage.PromotionMessage).getParm();
                BindingList<PromotionSettingDataItem> list = (BindingList<PromotionSettingDataItem>)parm;
                DataGridViewButtonCell button = (DataGridViewButtonCell)promotionDataGridView.CurrentRow.Cells["deleteButton"];
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
            string promotionTable = promotionTableTextBox.Text;
            string promotionResult = promotionResultTextBox.Text;

            if (promotionTable == "" || promotionResult == "")
            {
                return;
            }
                
            object parm = MessagePool.getMessage(ClientMessage.PromotionMessage).getParm();
            if (parm != null)
            {
                BindingList<PromotionSettingDataItem> list = (BindingList<PromotionSettingDataItem>)parm;
                string webSourePath = DateTime.Now.Ticks.ToString() + random.Next(100) + HTML_EXT;
                PromotionSettingDataItem item = new PromotionSettingDataItem(id++, promotionTable, promotionResult, webSourePath);
                item.PromotionWebIp = string.Format("http://{0}/Promotion.html", MeetingServer.getInstance().IP);
                list.Add(item);
                promotionTableTextBox.Text = string.Empty;
                promotionResultTextBox.Text = string.Empty;
            }
            else
            {
                Console.WriteLine("PromotionTableForm : parm is null.");
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
            object parm = MessagePool.getMessage(ClientMessage.PromotionMessage).getParm();

            if (parm != null)
            {
                BindingList<PromotionSettingDataItem> list = (BindingList<PromotionSettingDataItem>)parm;
                foreach (PromotionSettingDataItem dataItem in list)
                {
                    if (dataItem.Status == PromotionSettingDataItem.PromotionSettingStatus.NONACTIVE)
                    {
                        string webPathDir = WEB_PATH + dataItem.WebFileName;
                        Xls2Html.xlsToHtml(dataItem.PromotionSourcePath, webPathDir);
                        dataItem.Status = PromotionSettingDataItem.PromotionSettingStatus.ACTIVE;
                    }
                }
            }

            Hide();
        }
    }
}
