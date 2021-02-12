namespace MeetingSystem.CaseVote
{
  partial class CaseVoteResultForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.caseVoteStatusStrip = new System.Windows.Forms.StatusStrip();
            this.caseVoteResultToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.caseVoteResultDataGridView = new System.Windows.Forms.DataGridView();
            this.caseVoteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CaseVoteId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSourceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.caseVoteDataItemCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.caseVoteSettingStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SettleName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.caseVoteStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteResultDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.caseVoteStatusStrip, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.caseVoteResultDataGridView, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1733, 900);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // caseVoteStatusStrip
            // 
            this.caseVoteStatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.caseVoteStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.caseVoteResultToolStripStatusLabel});
            this.caseVoteStatusStrip.Location = new System.Drawing.Point(0, 878);
            this.caseVoteStatusStrip.Name = "caseVoteStatusStrip";
            this.caseVoteStatusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 30, 0);
            this.caseVoteStatusStrip.Size = new System.Drawing.Size(1733, 22);
            this.caseVoteStatusStrip.TabIndex = 0;
            this.caseVoteStatusStrip.Text = "statusStrip1";
            // 
            // caseVoteResultToolStripStatusLabel
            // 
            this.caseVoteResultToolStripStatusLabel.Name = "caseVoteResultToolStripStatusLabel";
            this.caseVoteResultToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // caseVoteResultDataGridView
            // 
            this.caseVoteResultDataGridView.AutoGenerateColumns = false;
            this.caseVoteResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.caseVoteResultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CaseVoteId,
            this.dataSourceNameDataGridViewTextBoxColumn,
            this.caseVoteDataItemCountDataGridViewTextBoxColumn,
            this.caseVoteSettingStatusDataGridViewTextBoxColumn,
            this.SettleName});
            this.caseVoteResultDataGridView.DataSource = this.caseVoteBindingSource;
            this.caseVoteResultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.caseVoteResultDataGridView.Location = new System.Drawing.Point(7, 6);
            this.caseVoteResultDataGridView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.caseVoteResultDataGridView.Name = "caseVoteResultDataGridView";
            this.caseVoteResultDataGridView.RowTemplate.Height = 24;
            this.caseVoteResultDataGridView.Size = new System.Drawing.Size(1719, 848);
            this.caseVoteResultDataGridView.TabIndex = 1;
            this.caseVoteResultDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.caseVoteResultDataGridView_CellClick);
            // 
            // caseVoteBindingSource
            // 
            this.caseVoteBindingSource.DataMember = "CaseVoteDataPairs";
            this.caseVoteBindingSource.DataSource = typeof(MeetingSystem.Message.CaseVoteMessageHandler);
            // 
            // CaseVoteId
            // 
            this.CaseVoteId.DataPropertyName = "CaseVoteId";
            this.CaseVoteId.HeaderText = "CaseVoteId";
            this.CaseVoteId.Name = "CaseVoteId";
            this.CaseVoteId.ReadOnly = true;
            this.CaseVoteId.Visible = false;
            // 
            // dataSourceNameDataGridViewTextBoxColumn
            // 
            this.dataSourceNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataSourceNameDataGridViewTextBoxColumn.DataPropertyName = "DataSourceName";
            this.dataSourceNameDataGridViewTextBoxColumn.HeaderText = "單記投票單名稱";
            this.dataSourceNameDataGridViewTextBoxColumn.Name = "dataSourceNameDataGridViewTextBoxColumn";
            this.dataSourceNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // caseVoteDataItemCountDataGridViewTextBoxColumn
            // 
            this.caseVoteDataItemCountDataGridViewTextBoxColumn.DataPropertyName = "CaseVoteDataItemCount";
            this.caseVoteDataItemCountDataGridViewTextBoxColumn.HeaderText = "人數";
            this.caseVoteDataItemCountDataGridViewTextBoxColumn.Name = "caseVoteDataItemCountDataGridViewTextBoxColumn";
            this.caseVoteDataItemCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // caseVoteSettingStatusDataGridViewTextBoxColumn
            // 
            this.caseVoteSettingStatusDataGridViewTextBoxColumn.DataPropertyName = "CaseVoteSettingStatus";
            this.caseVoteSettingStatusDataGridViewTextBoxColumn.HeaderText = "狀態";
            this.caseVoteSettingStatusDataGridViewTextBoxColumn.Name = "caseVoteSettingStatusDataGridViewTextBoxColumn";
            this.caseVoteSettingStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // SettleName
            // 
            this.SettleName.DataPropertyName = "SettleName";
            this.SettleName.HeaderText = "";
            this.SettleName.Name = "SettleName";
            this.SettleName.ReadOnly = true;
            // 
            // CaseVoteResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1733, 900);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "CaseVoteResultForm";
            this.Text = "CaseVoteResultForm";
            this.Load += new System.EventHandler(this.CaseVoteResultForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.caseVoteStatusStrip.ResumeLayout(false);
            this.caseVoteStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteResultDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteBindingSource)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.StatusStrip caseVoteStatusStrip;
    private System.Windows.Forms.DataGridView caseVoteResultDataGridView;
    private System.Windows.Forms.BindingSource caseVoteBindingSource;
    private System.Windows.Forms.ToolStripStatusLabel caseVoteResultToolStripStatusLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaseVoteId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSourceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn caseVoteDataItemCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn caseVoteSettingStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn SettleName;
    }
}