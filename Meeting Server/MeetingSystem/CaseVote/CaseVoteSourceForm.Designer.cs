namespace MeetingSystem.CaseVote
{
  partial class CaseVoteSourceForm
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.labelMultiVote = new System.Windows.Forms.Label();
            this.labelVoteResult = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.caseVoteTableTextBox = new System.Windows.Forms.TextBox();
            this.caseVoteResultTextBox = new System.Windows.Forms.TextBox();
            this.caseVoteDataGridView = new System.Windows.Forms.DataGridView();
            this.caseVoteSourcePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaseVoteResultPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.caseVoteTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelTop, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.caseVoteDataGridView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonOK, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1733, 900);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.ColumnCount = 5;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.36508F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.63492F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 459F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 466F));
            this.tableLayoutPanelTop.Controls.Add(this.labelMultiVote, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.labelVoteResult, 2, 0);
            this.tableLayoutPanelTop.Controls.Add(this.buttonAdd, 4, 0);
            this.tableLayoutPanelTop.Controls.Add(this.caseVoteTableTextBox, 1, 0);
            this.tableLayoutPanelTop.Controls.Add(this.caseVoteResultTextBox, 3, 0);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(7, 6);
            this.tableLayoutPanelTop.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 1;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(1719, 51);
            this.tableLayoutPanelTop.TabIndex = 0;
            // 
            // labelMultiVote
            // 
            this.labelMultiVote.AutoSize = true;
            this.labelMultiVote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMultiVote.Location = new System.Drawing.Point(7, 0);
            this.labelMultiVote.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelMultiVote.Name = "labelMultiVote";
            this.labelMultiVote.Size = new System.Drawing.Size(170, 51);
            this.labelMultiVote.TabIndex = 0;
            this.labelMultiVote.Text = "單記投票";
            this.labelMultiVote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVoteResult
            // 
            this.labelVoteResult.AutoSize = true;
            this.labelVoteResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVoteResult.Location = new System.Drawing.Point(633, 0);
            this.labelVoteResult.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelVoteResult.Name = "labelVoteResult";
            this.labelVoteResult.Size = new System.Drawing.Size(153, 51);
            this.labelVoteResult.TabIndex = 1;
            this.labelVoteResult.Text = "投票結果檔";
            this.labelVoteResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAdd.Location = new System.Drawing.Point(1259, 6);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(453, 39);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "新增";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // caseVoteTableTextBox
            // 
            this.caseVoteTableTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.caseVoteTableTextBox.Location = new System.Drawing.Point(191, 7);
            this.caseVoteTableTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.caseVoteTableTextBox.Name = "caseVoteTableTextBox";
            this.caseVoteTableTextBox.Size = new System.Drawing.Size(428, 36);
            this.caseVoteTableTextBox.TabIndex = 3;
            this.caseVoteTableTextBox.Click += new System.EventHandler(this.caseVoteTableTextBox_Click);
            // 
            // caseVoteResultTextBox
            // 
            this.caseVoteResultTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.caseVoteResultTextBox.Location = new System.Drawing.Point(800, 7);
            this.caseVoteResultTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.caseVoteResultTextBox.Name = "caseVoteResultTextBox";
            this.caseVoteResultTextBox.Size = new System.Drawing.Size(442, 36);
            this.caseVoteResultTextBox.TabIndex = 4;
            this.caseVoteResultTextBox.Click += new System.EventHandler(this.caseVoteTableTextBox_Click);
            // 
            // caseVoteDataGridView
            // 
            this.caseVoteDataGridView.AllowUserToAddRows = false;
            this.caseVoteDataGridView.AutoGenerateColumns = false;
            this.caseVoteDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.caseVoteDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.caseVoteSourcePathDataGridViewTextBoxColumn,
            this.CaseVoteResultPath,
            this.DeleteButton});
            this.caseVoteDataGridView.DataSource = this.caseVoteTableBindingSource;
            this.caseVoteDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.caseVoteDataGridView.Location = new System.Drawing.Point(7, 69);
            this.caseVoteDataGridView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.caseVoteDataGridView.Name = "caseVoteDataGridView";
            this.caseVoteDataGridView.RowTemplate.Height = 24;
            this.caseVoteDataGridView.Size = new System.Drawing.Size(1719, 753);
            this.caseVoteDataGridView.TabIndex = 1;
            this.caseVoteDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.caseVoteTableTextBox_CellClick);
            // 
            // caseVoteSourcePathDataGridViewTextBoxColumn
            // 
            this.caseVoteSourcePathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.caseVoteSourcePathDataGridViewTextBoxColumn.DataPropertyName = "CaseVoteSourcePath";
            this.caseVoteSourcePathDataGridViewTextBoxColumn.HeaderText = "單記投票檔";
            this.caseVoteSourcePathDataGridViewTextBoxColumn.Name = "caseVoteSourcePathDataGridViewTextBoxColumn";
            // 
            // CaseVoteResultPath
            // 
            this.CaseVoteResultPath.DataPropertyName = "CaseVoteResultPath";
            this.CaseVoteResultPath.HeaderText = "投票結果檔";
            this.CaseVoteResultPath.Name = "CaseVoteResultPath";
            // 
            // DeleteButton
            // 
            this.DeleteButton.HeaderText = "動作";
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeleteButton.Text = "刪除";
            this.DeleteButton.UseColumnTextForButtonValue = true;
            // 
            // caseVoteTableBindingSource
            // 
            this.caseVoteTableBindingSource.DataMember = "CaseVoteSettingDataItems";
            this.caseVoteTableBindingSource.DataSource = typeof(MeetingSystem.Message.CaseVoteMessageHandler);
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(7, 834);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(1719, 60);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "確定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // CaseVoteSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1733, 900);
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "CaseVoteSourceForm";
            this.Text = "CaseVoteSourceForm";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.caseVoteTableBindingSource)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
    private System.Windows.Forms.Label labelMultiVote;
    private System.Windows.Forms.Label labelVoteResult;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.DataGridView caseVoteDataGridView;
    private System.Windows.Forms.BindingSource caseVoteTableBindingSource;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TextBox caseVoteTableTextBox;
    private System.Windows.Forms.TextBox caseVoteResultTextBox;
    private System.Windows.Forms.DataGridViewTextBoxColumn caseVoteReslutPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn caseVoteSourcePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaseVoteResultPath;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteButton;
    }
}