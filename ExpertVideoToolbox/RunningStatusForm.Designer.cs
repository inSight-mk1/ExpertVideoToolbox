namespace ExpertVideoToolbox
{
    partial class RunningStatusForm
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
            this.stopBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.filesCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.estETADataTB = new System.Windows.Forms.TextBox();
            this.estETALabel = new System.Windows.Forms.Label();
            this.currentPositionLabel = new System.Windows.Forms.Label();
            this.fpsDataTB = new System.Windows.Forms.TextBox();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.currentPostionDataTB = new System.Windows.Forms.TextBox();
            this.datagB = new System.Windows.Forms.GroupBox();
            this.estKbpsDataTB = new System.Windows.Forms.TextBox();
            this.estKbpsLabel = new System.Windows.Forms.Label();
            this.progressLabel = new System.Windows.Forms.Label();
            this.taskStepsLabel = new System.Windows.Forms.Label();
            this.fullSpeedBtn = new System.Windows.Forms.Button();
            this.lowSpeedBtn = new System.Windows.Forms.Button();
            this.encoderPriority = new System.Windows.Forms.Label();
            this.priorityCB = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.datagB.SuspendLayout();
            this.SuspendLayout();
            // 
            // stopBtn
            // 
            this.stopBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stopBtn.Location = new System.Drawing.Point(12, 217);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(64, 23);
            this.stopBtn.TabIndex = 14;
            this.stopBtn.Text = "中止";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.filesCountLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 249);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(407, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "状态: ";
            // 
            // filesCountLabel
            // 
            this.filesCountLabel.Name = "filesCountLabel";
            this.filesCountLabel.Size = new System.Drawing.Size(27, 17);
            this.filesCountLabel.Text = "0/1";
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.Location = new System.Drawing.Point(12, 186);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(377, 23);
            this.progress.TabIndex = 12;
            // 
            // estETADataTB
            // 
            this.estETADataTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.estETADataTB.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.estETADataTB.Location = new System.Drawing.Point(94, 73);
            this.estETADataTB.Name = "estETADataTB";
            this.estETADataTB.ReadOnly = true;
            this.estETADataTB.Size = new System.Drawing.Size(283, 25);
            this.estETADataTB.TabIndex = 21;
            this.estETADataTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // estETALabel
            // 
            this.estETALabel.AutoSize = true;
            this.estETALabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.estETALabel.Location = new System.Drawing.Point(8, 79);
            this.estETALabel.Name = "estETALabel";
            this.estETALabel.Size = new System.Drawing.Size(80, 17);
            this.estETALabel.TabIndex = 6;
            this.estETALabel.Text = "预计剩余时间";
            // 
            // currentPositionLabel
            // 
            this.currentPositionLabel.AutoSize = true;
            this.currentPositionLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.currentPositionLabel.Location = new System.Drawing.Point(8, 23);
            this.currentPositionLabel.Name = "currentPositionLabel";
            this.currentPositionLabel.Size = new System.Drawing.Size(73, 17);
            this.currentPositionLabel.TabIndex = 0;
            this.currentPositionLabel.Text = "已完成/总计";
            // 
            // fpsDataTB
            // 
            this.fpsDataTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpsDataTB.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fpsDataTB.Location = new System.Drawing.Point(94, 45);
            this.fpsDataTB.Name = "fpsDataTB";
            this.fpsDataTB.ReadOnly = true;
            this.fpsDataTB.Size = new System.Drawing.Size(283, 25);
            this.fpsDataTB.TabIndex = 17;
            this.fpsDataTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fpsLabel.Location = new System.Drawing.Point(8, 50);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(56, 17);
            this.fpsLabel.TabIndex = 2;
            this.fpsLabel.Text = "编码速度";
            // 
            // currentPostionDataTB
            // 
            this.currentPostionDataTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentPostionDataTB.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.currentPostionDataTB.ForeColor = System.Drawing.SystemColors.WindowText;
            this.currentPostionDataTB.Location = new System.Drawing.Point(94, 18);
            this.currentPostionDataTB.Name = "currentPostionDataTB";
            this.currentPostionDataTB.ReadOnly = true;
            this.currentPostionDataTB.Size = new System.Drawing.Size(283, 25);
            this.currentPostionDataTB.TabIndex = 15;
            this.currentPostionDataTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // datagB
            // 
            this.datagB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datagB.Controls.Add(this.estKbpsDataTB);
            this.datagB.Controls.Add(this.estKbpsLabel);
            this.datagB.Controls.Add(this.estETADataTB);
            this.datagB.Controls.Add(this.estETALabel);
            this.datagB.Controls.Add(this.currentPositionLabel);
            this.datagB.Controls.Add(this.fpsDataTB);
            this.datagB.Controls.Add(this.fpsLabel);
            this.datagB.Controls.Add(this.currentPostionDataTB);
            this.datagB.Location = new System.Drawing.Point(12, 36);
            this.datagB.Name = "datagB";
            this.datagB.Size = new System.Drawing.Size(383, 138);
            this.datagB.TabIndex = 11;
            this.datagB.TabStop = false;
            // 
            // estKbpsDataTB
            // 
            this.estKbpsDataTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.estKbpsDataTB.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.estKbpsDataTB.Location = new System.Drawing.Point(94, 102);
            this.estKbpsDataTB.Name = "estKbpsDataTB";
            this.estKbpsDataTB.ReadOnly = true;
            this.estKbpsDataTB.Size = new System.Drawing.Size(283, 25);
            this.estKbpsDataTB.TabIndex = 23;
            this.estKbpsDataTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // estKbpsLabel
            // 
            this.estKbpsLabel.AutoSize = true;
            this.estKbpsLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.estKbpsLabel.Location = new System.Drawing.Point(8, 107);
            this.estKbpsLabel.Name = "estKbpsLabel";
            this.estKbpsLabel.Size = new System.Drawing.Size(56, 17);
            this.estKbpsLabel.TabIndex = 22;
            this.estKbpsLabel.Text = "预计码率";
            // 
            // progressLabel
            // 
            this.progressLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.progressLabel.Location = new System.Drawing.Point(327, 218);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(62, 20);
            this.progressLabel.TabIndex = 15;
            this.progressLabel.Text = "0.00%";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // taskStepsLabel
            // 
            this.taskStepsLabel.AutoSize = true;
            this.taskStepsLabel.Font = new System.Drawing.Font("微软雅黑", 11.25F);
            this.taskStepsLabel.Location = new System.Drawing.Point(12, 13);
            this.taskStepsLabel.Name = "taskStepsLabel";
            this.taskStepsLabel.Size = new System.Drawing.Size(216, 20);
            this.taskStepsLabel.TabIndex = 16;
            this.taskStepsLabel.Text = "正在进行：步骤 1/3 - 复制音频";
            // 
            // fullSpeedBtn
            // 
            this.fullSpeedBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fullSpeedBtn.Location = new System.Drawing.Point(230, 217);
            this.fullSpeedBtn.Name = "fullSpeedBtn";
            this.fullSpeedBtn.Size = new System.Drawing.Size(46, 23);
            this.fullSpeedBtn.TabIndex = 17;
            this.fullSpeedBtn.Text = "正常";
            this.fullSpeedBtn.UseVisualStyleBackColor = true;
            this.fullSpeedBtn.VisibleChanged += new System.EventHandler(this.fullSpeedBtn_VisibleChanged);
            this.fullSpeedBtn.Click += new System.EventHandler(this.fullSpeedBtn_Click);
            // 
            // lowSpeedBtn
            // 
            this.lowSpeedBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lowSpeedBtn.Location = new System.Drawing.Point(274, 217);
            this.lowSpeedBtn.Name = "lowSpeedBtn";
            this.lowSpeedBtn.Size = new System.Drawing.Size(45, 23);
            this.lowSpeedBtn.TabIndex = 18;
            this.lowSpeedBtn.Text = "低速";
            this.lowSpeedBtn.UseVisualStyleBackColor = true;
            this.lowSpeedBtn.Click += new System.EventHandler(this.lowSpeedBtn_Click);
            // 
            // encoderPriority
            // 
            this.encoderPriority.AutoSize = true;
            this.encoderPriority.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.encoderPriority.Location = new System.Drawing.Point(82, 218);
            this.encoderPriority.Name = "encoderPriority";
            this.encoderPriority.Size = new System.Drawing.Size(51, 20);
            this.encoderPriority.TabIndex = 19;
            this.encoderPriority.Text = "优先级";
            // 
            // priorityCB
            // 
            this.priorityCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityCB.FormattingEnabled = true;
            this.priorityCB.Items.AddRange(new object[] {
            "低",
            "低于正常",
            "正常",
            "高于正常",
            "高",
            "实时"});
            this.priorityCB.Location = new System.Drawing.Point(137, 218);
            this.priorityCB.Name = "priorityCB";
            this.priorityCB.Size = new System.Drawing.Size(79, 20);
            this.priorityCB.TabIndex = 20;
            this.priorityCB.SelectedIndexChanged += new System.EventHandler(this.priorityCB_SelectedIndexChanged);
            // 
            // RunningStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 271);
            this.Controls.Add(this.priorityCB);
            this.Controls.Add(this.encoderPriority);
            this.Controls.Add(this.lowSpeedBtn);
            this.Controls.Add(this.fullSpeedBtn);
            this.Controls.Add(this.taskStepsLabel);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.datagB);
            this.Name = "RunningStatusForm";
            this.Text = "RunningStatusForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunningStatusForm_FormClosing);
            this.Shown += new System.EventHandler(this.RunningStatusForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.datagB.ResumeLayout(false);
            this.datagB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel filesCountLabel;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.TextBox estETADataTB;
        private System.Windows.Forms.Label estETALabel;
        private System.Windows.Forms.Label currentPositionLabel;
        private System.Windows.Forms.TextBox fpsDataTB;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.TextBox currentPostionDataTB;
        private System.Windows.Forms.GroupBox datagB;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Label taskStepsLabel;
        private System.Windows.Forms.Button fullSpeedBtn;
        private System.Windows.Forms.Button lowSpeedBtn;
        private System.Windows.Forms.Label encoderPriority;
        private System.Windows.Forms.ComboBox priorityCB;
        private System.Windows.Forms.TextBox estKbpsDataTB;
        private System.Windows.Forms.Label estKbpsLabel;
    }
}