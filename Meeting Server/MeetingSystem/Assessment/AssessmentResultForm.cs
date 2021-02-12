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

namespace MeetingSystem.Assessment
{
    public partial class AssessmentResultForm : Form, IBackgroundWorker
    {
        bool isCompute = false;

        public AssessmentResultForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// AssessmentResultForm_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssessmentResultForm_Load(object sender, EventArgs e)
        {
            assessmentResultBindingSource.DataSource = MessagePool.getMessage(ClientMessage.AssessmentMessage);
        }

        /// <summary>
        /// AssessmentResultDataGridView_CellContentClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssessmentResultDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                string buttonName = AssessmentResultDataGridView.CurrentRow.Cells["ButtonName"].Value.ToString();
                int id = Convert.ToInt32(AssessmentResultDataGridView.CurrentRow.Cells["AssessmentId"].Value);
                AssessmentMessageHandler assessmentMessageHandler = (AssessmentMessageHandler)MessagePool.getMessage(ClientMessage.AssessmentMessage);

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
                    assessmentMessageHandler.OpenResultExcel(id);
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
                AssessmentMessageHandler assessmentMessageHandler = (AssessmentMessageHandler)MessagePool.getMessage(ClientMessage.AssessmentMessage);
                int id = (int)e.Argument;
                assessmentMessageHandler.CalcVoteResult(id);
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
                AssessmentMessageHandler assessmentMessageHandler = (AssessmentMessageHandler)MessagePool.getMessage(ClientMessage.AssessmentMessage);
                int id = 0;
                if (e.Result == null)
                {
                    toolStripStatusLabel.Text = "";
                    Console.WriteLine("assessment result EventArgs NULL");
                    return;
                }
                else
                {
                    id = (int)e.Result;
                }
                assessmentMessageHandler.SetToCompleteDataItem(id);
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
