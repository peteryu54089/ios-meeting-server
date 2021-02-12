namespace MeetingSystem.Assessment
{
    partial class AssessmentSourceForm
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
            this.assessmentDataGridView = new System.Windows.Forms.DataGridView();
            this.deleteButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.okButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.assessmentResultTextBox = new System.Windows.Forms.TextBox();
            this.assessmentTableTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.assessmentSourcePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assessmentResultPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assessmentTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assessmentDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assessmentTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.assessmentDataGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1729, 1062);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // assessmentDataGridView
            // 
            this.assessmentDataGridView.AllowUserToAddRows = false;
            this.assessmentDataGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.assessmentDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.assessmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assessmentDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.assessmentSourcePathDataGridViewTextBoxColumn,
            this.assessmentResultPathDataGridViewTextBoxColumn,
            this.deleteButton});
            this.assessmentDataGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.assessmentDataGridView.DataSource = this.assessmentTableBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.assessmentDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.assessmentDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assessmentDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.assessmentDataGridView.Location = new System.Drawing.Point(7, 80);
            this.assessmentDataGridView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.assessmentDataGridView.MultiSelect = false;
            this.assessmentDataGridView.Name = "assessmentDataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.assessmentDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.assessmentDataGridView.RowHeadersVisible = false;
            this.assessmentDataGridView.RowTemplate.Height = 24;
            this.assessmentDataGridView.Size = new System.Drawing.Size(1715, 890);
            this.assessmentDataGridView.TabIndex = 1;
            this.assessmentDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AssessmentDataGridView_CellClick);
            // 
            // deleteButton
            // 
            this.deleteButton.HeaderText = "動作";
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Text = "刪除";
            this.deleteButton.UseColumnTextForButtonValue = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.okButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(7, 982);
            this.panel1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1715, 74);
            this.panel1.TabIndex = 2;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(0, 6);
            this.okButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(1715, 56);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "確定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(7, 6);
            this.panel2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1715, 62);
            this.panel2.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.332454F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.05013F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.707124F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.54354F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.assessmentResultTextBox, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.assessmentTableTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.addButton, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1715, 62);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(7, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 62);
            this.label1.TabIndex = 0;
            this.label1.Text = "評分表";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(786, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 62);
            this.label2.TabIndex = 1;
            this.label2.Text = "評分結果";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // assessmentResultTextBox
            // 
            this.assessmentResultTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.assessmentResultTextBox.Location = new System.Drawing.Point(935, 13);
            this.assessmentResultTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.assessmentResultTextBox.Name = "assessmentResultTextBox";
            this.assessmentResultTextBox.Size = new System.Drawing.Size(611, 36);
            this.assessmentResultTextBox.TabIndex = 2;
            this.assessmentResultTextBox.Click += new System.EventHandler(this.AssessmentTableTextBox_Click);
            // 
            // assessmentTableTextBox
            // 
            this.assessmentTableTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.assessmentTableTextBox.Location = new System.Drawing.Point(115, 13);
            this.assessmentTableTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.assessmentTableTextBox.Name = "assessmentTableTextBox";
            this.assessmentTableTextBox.Size = new System.Drawing.Size(654, 36);
            this.assessmentTableTextBox.TabIndex = 3;
            this.assessmentTableTextBox.Click += new System.EventHandler(this.AssessmentTableTextBox_Click);
            // 
            // addButton
            // 
            this.addButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addButton.Location = new System.Drawing.Point(1563, 6);
            this.addButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(145, 50);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "新增";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // assessmentSourcePathDataGridViewTextBoxColumn
            // 
            this.assessmentSourcePathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.assessmentSourcePathDataGridViewTextBoxColumn.DataPropertyName = "AssessmentSourcePath";
            this.assessmentSourcePathDataGridViewTextBoxColumn.HeaderText = "評分表";
            this.assessmentSourcePathDataGridViewTextBoxColumn.Name = "assessmentSourcePathDataGridViewTextBoxColumn";
            // 
            // assessmentResultPathDataGridViewTextBoxColumn
            // 
            this.assessmentResultPathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.assessmentResultPathDataGridViewTextBoxColumn.DataPropertyName = "AssessmentResultPath";
            this.assessmentResultPathDataGridViewTextBoxColumn.HeaderText = "評分結果";
            this.assessmentResultPathDataGridViewTextBoxColumn.Name = "assessmentResultPathDataGridViewTextBoxColumn";
            // 
            // assessmentTableBindingSource
            // 
            this.assessmentTableBindingSource.DataMember = "SettingDataItems";
            this.assessmentTableBindingSource.DataSource = typeof(MeetingSystem.Message.AssessmentMessageHandler);
            // 
            // AssessmentSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1729, 1062);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "AssessmentSourceForm";
            this.Text = "AssessmentSourceForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.assessmentDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assessmentTableBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView assessmentDataGridView;
        private System.Windows.Forms.BindingSource assessmentTableBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox assessmentResultTextBox;
        private System.Windows.Forms.TextBox assessmentTableTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn assessmentSourcePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assessmentResultPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn deleteButton;
    }
}