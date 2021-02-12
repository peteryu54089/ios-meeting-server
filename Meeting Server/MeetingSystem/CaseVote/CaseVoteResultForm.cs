using MeetingSystem.Client;
using MeetingSystem.Message;
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
    public partial class CaseVoteResultForm : Form
    {
        bool isCompute = false;

        public CaseVoteResultForm()
        {
            InitializeComponent();
        }

        private void CaseVoteResultForm_Load(object sender, EventArgs e)
        {
            caseVoteBindingSource.DataSource = MessagePool.getMessage(ClientMessage.CaseVoteMessage);
        }

        private void showAllButton_Click(object sender, EventArgs e)
        {
            CaseVoteMessageHandler caseVoteMessageHandler = (CaseVoteMessageHandler)MessagePool.getMessage(ClientMessage.CaseVoteMessage);
            caseVoteMessageHandler.calcAllVoteResult();
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lock (this)
            {
                caseVoteResultToolStripStatusLabel.Text = "結算中.....";
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (this)
            {
                int id = 0;
                CaseVoteMessageHandler caseVoteMessageHandler = (CaseVoteMessageHandler)MessagePool.getMessage(ClientMessage.CaseVoteMessage);
                if (e.Result == null)
                {
                    caseVoteResultToolStripStatusLabel.Text = "";
                    Console.WriteLine("Case vote result EventArgs NULL");
                    return;
                }
                else
                {
                    id = (int)e.Result;
                }
                caseVoteMessageHandler.setToCompleteVoteDataItem(id);//需不需要修改方法名稱?
                caseVoteResultToolStripStatusLabel.Text = "";
            }
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (this)
            {
                CaseVoteMessageHandler caseVoteMessageHandler = (CaseVoteMessageHandler)MessagePool.getMessage(ClientMessage.CaseVoteMessage);
                int id = (int)e.Argument;
                caseVoteMessageHandler.calcCaseVoteResult(id);
                e.Result = id;
            }
        }

        private void caseVoteResultDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //ButtonName
            if (e.ColumnIndex == 4)
            {
                String buttonName = caseVoteResultDataGridView.CurrentRow.Cells["SettleName"].Value.ToString();
                int id = Convert.ToInt32(caseVoteResultDataGridView.CurrentRow.Cells["CaseVoteId"].Value);
                CaseVoteMessageHandler caseVoteMessageHandler = (CaseVoteMessageHandler)MessagePool.getMessage(ClientMessage.CaseVoteMessage);

                if (buttonName.Equals("結算") && !isCompute)
                {
                    isCompute = true;
                    caseVoteResultToolStripStatusLabel.Text = "結算中.....";
                    BackgroundWorker tempBackgroundWorker = new BackgroundWorker();
                    tempBackgroundWorker.WorkerReportsProgress = true;
                    tempBackgroundWorker.WorkerSupportsCancellation = true;
                    tempBackgroundWorker.DoWork += BackgroundWorker_DoWork;
                    tempBackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
                    tempBackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                    tempBackgroundWorker.RunWorkerAsync(id);
                }
                else if (buttonName.Equals("開票"))
                {
                    isCompute = false;
                    caseVoteMessageHandler.openResultExcel(id);
                }
            }
        }
    }
}
