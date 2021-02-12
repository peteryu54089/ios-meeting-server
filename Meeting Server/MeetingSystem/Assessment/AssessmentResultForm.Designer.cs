namespace MeetingSystem.Assessment
{
    partial class AssessmentResultForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.AssessmentResultDataGridView = new System.Windows.Forms.DataGridView();
            this.AssessmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assessmentDataItemCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssessmentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ButtonName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.assessmentResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssessmentResultDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assessmentResultBindingSource)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.AssessmentResultDataGridView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1458, 570);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // AssessmentResultDataGridView
            // 
            this.AssessmentResultDataGridView.AllowUserToAddRows = false;
            this.AssessmentResultDataGridView.AllowUserToDeleteRows = false;
            this.AssessmentResultDataGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AssessmentResultDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.AssessmentResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssessmentResultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AssessmentId,
            this.tableNameDataGridViewTextBoxColumn,
            this.assessmentDataItemCountDataGridViewTextBoxColumn,
            this.AssessmentStatus,
            this.ButtonName});
            this.AssessmentResultDataGridView.DataSource = this.assessmentResultBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AssessmentResultDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.AssessmentResultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssessmentResultDataGridView.Location = new System.Drawing.Point(7, 6);
            this.AssessmentResultDataGridView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AssessmentResultDataGridView.Name = "AssessmentResultDataGridView";
            this.AssessmentResultDataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AssessmentResultDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.AssessmentResultDataGridView.RowHeadersVisible = false;
            this.AssessmentResultDataGridView.RowTemplate.Height = 24;
            this.AssessmentResultDataGridView.Size = new System.Drawing.Size(1444, 518);
            this.AssessmentResultDataGridView.TabIndex = 1;
            this.AssessmentResultDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AssessmentResultDataGridView_CellContentClick);
            // 
            // AssessmentId
            // 
            this.AssessmentId.DataPropertyName = "AssessmentId";
            this.AssessmentId.HeaderText = "AssessmentId";
            this.AssessmentId.Name = "AssessmentId";
            this.AssessmentId.ReadOnly = true;
            this.AssessmentId.Visible = false;
            // 
            // tableNameDataGridViewTextBoxColumn
            // 
            this.tableNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tableNameDataGridViewTextBoxColumn.DataPropertyName = "TableName";
            this.tableNameDataGridViewTextBoxColumn.FillWeight = 65F;
            this.tableNameDataGridViewTextBoxColumn.HeaderText = "評分表名稱";
            this.tableNameDataGridViewTextBoxColumn.Name = "tableNameDataGridViewTextBoxColumn";
            this.tableNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // assessmentDataItemCountDataGridViewTextBoxColumn
            // 
            this.assessmentDataItemCountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.assessmentDataItemCountDataGridViewTextBoxColumn.DataPropertyName = "AssessmentDataItemCount";
            this.assessmentDataItemCountDataGridViewTextBoxColumn.FillWeight = 20F;
            this.assessmentDataItemCountDataGridViewTextBoxColumn.HeaderText = "已投票人數";
            this.assessmentDataItemCountDataGridViewTextBoxColumn.Name = "assessmentDataItemCountDataGridViewTextBoxColumn";
            this.assessmentDataItemCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // AssessmentStatus
            // 
            this.AssessmentStatus.DataPropertyName = "AssessmentStatus";
            this.AssessmentStatus.HeaderText = "狀態";
            this.AssessmentStatus.Name = "AssessmentStatus";
            this.AssessmentStatus.ReadOnly = true;
            // 
            // ButtonName
            // 
            this.ButtonName.DataPropertyName = "ButtonName";
            this.ButtonName.FillWeight = 15F;
            this.ButtonName.HeaderText = "";
            this.ButtonName.Name = "ButtonName";
            this.ButtonName.ReadOnly = true;
            this.ButtonName.Text = "";
            // 
            // assessmentResultBindingSource
            // 
            this.assessmentResultBindingSource.DataMember = "AssessmentDataPairs";
            this.assessmentResultBindingSource.DataSource = typeof(MeetingSystem.Message.AssessmentMessageHandler);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 548);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 30, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1458, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // AssessmentResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1458, 570);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "AssessmentResultForm";
            this.Text = "AssessmentResultForm";
            this.Load += new System.EventHandler(this.AssessmentResultForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssessmentResultDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assessmentResultBindingSource)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource assessmentResultBindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView AssessmentResultDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn AssessmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assessmentDataItemCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AssessmentStatus;
        private System.Windows.Forms.DataGridViewButtonColumn ButtonName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}