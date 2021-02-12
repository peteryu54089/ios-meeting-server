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

namespace MeetingSystem.Promotion
{
    public partial class PromotionResultForm : Form, IBackgroundWorker
    {
        bool isCompute = false;

        public PromotionResultForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// PromotionResultForm_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PromotionResultForm_Load(object sender, EventArgs e)
        {
            promotionResultBindingSource.DataSource = MessagePool.getMessage(ClientMessage.PromotionMessage);
        }

        /// <summary>
        /// PromotionResultDataGridView_CellContentClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PromotionResultDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                string buttonName = PromotionResultDataGridView.CurrentRow.Cells["ButtonName"].Value.ToString();
                int id = Convert.ToInt32(PromotionResultDataGridView.CurrentRow.Cells["PromotionId"].Value);
                PromotionMessageHandler promotionMessageHandler = (PromotionMessageHandler)MessagePool.getMessage(ClientMessage.PromotionMessage);

                if (buttonName.Equals("結算") && !isCompute)
                {
                    isCompute = true;
                    toolStripStatusLabel.Text = "結算中.....";
                    BackgroundWorker tempBackGroundWorker = new BackgroundWorker();
                    tempBackGroundWorker.WorkerReportsProgress = true;
                    tempBackGroundWorker.WorkerSupportsCancellation = true;
                    tempBackGroundWorker.DoWork += BackgroundWorker_DoWork;
                    tempBackGroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
                    tempBackGroundWorker.RunWorkerCompleted += BackkgroundWorker_RunWorkerCompleted;
                    tempBackGroundWorker.RunWorkerAsync(id);
                }
                else if (buttonName.Equals("開票"))
                {
                    isCompute = false;
                    promotionMessageHandler.OpenResultExcel(id);
                }
            }
        }

        /// <summary>
        /// BackgroundWorker_DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (this)
            {
                PromotionMessageHandler promotionMessageHandler = (PromotionMessageHandler)MessagePool.getMessage(ClientMessage.PromotionMessage);
                int id = (int)e.Argument;
                promotionMessageHandler.CalcVoteResult(id);
                e.Result = id;
            }
        }

        /// <summary>
        /// BackkgroundWorker_RunWorkerCompleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BackkgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (this)
            {
                PromotionMessageHandler promotionMessageHandler = (PromotionMessageHandler)MessagePool.getMessage(ClientMessage.PromotionMessage);
                int id = 0;
                if (e.Result == null)
                {
                    toolStripStatusLabel.Text = "";
                    Console.WriteLine("promotion result EventArgs NULL");
                    return;
                }
                else
                {
                    id = (int)e.Result;
                }
                promotionMessageHandler.SetToCompleteDataItem(id);
                toolStripStatusLabel.Text = "";
            }
        }

        /// <summary>
        /// BackgroundWorker_ProgressChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lock (this)
            {
                toolStripStatusLabel.Text = "結算中.....";
            }
        }
    }
}
