namespace ExpertVideoToolbox
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.removeAllBtn = new System.Windows.Forms.Button();
            this.removeFileBtn = new System.Windows.Forms.Button();
            this.addFolderBtn = new System.Windows.Forms.Button();
            this.addFileBtn = new System.Windows.Forms.Button();
            this.videoSetgB = new System.Windows.Forms.GroupBox();
            this.cmdCodeTB = new System.Windows.Forms.TextBox();
            this.encoderCB = new System.Windows.Forms.ComboBox();
            this.encoderLabel = new System.Windows.Forms.Label();
            this.brORqLabel = new System.Windows.Forms.Label();
            this.fileFormatCB = new System.Windows.Forms.ComboBox();
            this.fileFormatLabel = new System.Windows.Forms.Label();
            this.removeSettingBtn = new System.Windows.Forms.Button();
            this.addSettingBtn = new System.Windows.Forms.Button();
            this.encodeSettinglabel = new System.Windows.Forms.Label();
            this.encodeSettingCB = new System.Windows.Forms.ComboBox();
            this.set2SelectedBtn = new System.Windows.Forms.Button();
            this.set2AllBtn = new System.Windows.Forms.Button();
            this.StartBtn = new System.Windows.Forms.Button();
            this.fileListView = new System.Windows.Forms.ListView();
            this.outputFilePathlabel = new System.Windows.Forms.Label();
            this.outputFilePathTB = new System.Windows.Forms.TextBox();
            this.chooseOutputPathBtn = new System.Windows.Forms.Button();
            this.audioSetgB = new System.Windows.Forms.GroupBox();
            this.profileCB = new System.Windows.Forms.ComboBox();
            this.profileLabel = new System.Windows.Forms.Label();
            this.bitrateOrQualityCB = new System.Windows.Forms.ComboBox();
            this.bitrateOrQualityLabel = new System.Windows.Forms.Label();
            this.codecModeCB = new System.Windows.Forms.ComboBox();
            this.codecModeLabel = new System.Windows.Forms.Label();
            this.audioEncoderCB = new System.Windows.Forms.ComboBox();
            this.audioEncoder = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.videoSetgB.SuspendLayout();
            this.audioSetgB.SuspendLayout();
            this.SuspendLayout();
            // 
            // removeAllBtn
            // 
            this.removeAllBtn.Location = new System.Drawing.Point(395, 178);
            this.removeAllBtn.Name = "removeAllBtn";
            this.removeAllBtn.Size = new System.Drawing.Size(75, 23);
            this.removeAllBtn.TabIndex = 8;
            this.removeAllBtn.Text = "移除所有";
            this.removeAllBtn.UseVisualStyleBackColor = true;
            this.removeAllBtn.Click += new System.EventHandler(this.removeAllBtn_Click);
            // 
            // removeFileBtn
            // 
            this.removeFileBtn.Location = new System.Drawing.Point(314, 178);
            this.removeFileBtn.Name = "removeFileBtn";
            this.removeFileBtn.Size = new System.Drawing.Size(75, 23);
            this.removeFileBtn.TabIndex = 7;
            this.removeFileBtn.Text = "移除文件";
            this.removeFileBtn.UseVisualStyleBackColor = true;
            this.removeFileBtn.Click += new System.EventHandler(this.removeFileBtn_Click);
            // 
            // addFolderBtn
            // 
            this.addFolderBtn.Location = new System.Drawing.Point(93, 178);
            this.addFolderBtn.Name = "addFolderBtn";
            this.addFolderBtn.Size = new System.Drawing.Size(75, 23);
            this.addFolderBtn.TabIndex = 6;
            this.addFolderBtn.Text = "添加文件夹";
            this.addFolderBtn.UseVisualStyleBackColor = true;
            this.addFolderBtn.Click += new System.EventHandler(this.addFolderBtn_Click);
            // 
            // addFileBtn
            // 
            this.addFileBtn.Location = new System.Drawing.Point(12, 178);
            this.addFileBtn.Name = "addFileBtn";
            this.addFileBtn.Size = new System.Drawing.Size(75, 23);
            this.addFileBtn.TabIndex = 5;
            this.addFileBtn.Text = "添加文件";
            this.addFileBtn.UseVisualStyleBackColor = true;
            this.addFileBtn.Click += new System.EventHandler(this.addFileBtn_Click);
            // 
            // videoSetgB
            // 
            this.videoSetgB.Controls.Add(this.cmdCodeTB);
            this.videoSetgB.Controls.Add(this.encoderCB);
            this.videoSetgB.Controls.Add(this.encoderLabel);
            this.videoSetgB.Controls.Add(this.brORqLabel);
            this.videoSetgB.Controls.Add(this.fileFormatCB);
            this.videoSetgB.Controls.Add(this.fileFormatLabel);
            this.videoSetgB.Location = new System.Drawing.Point(14, 279);
            this.videoSetgB.Name = "videoSetgB";
            this.videoSetgB.Size = new System.Drawing.Size(377, 143);
            this.videoSetgB.TabIndex = 9;
            this.videoSetgB.TabStop = false;
            this.videoSetgB.Text = "视频流设置";
            // 
            // cmdCodeTB
            // 
            this.cmdCodeTB.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmdCodeTB.Location = new System.Drawing.Point(6, 46);
            this.cmdCodeTB.Multiline = true;
            this.cmdCodeTB.Name = "cmdCodeTB";
            this.cmdCodeTB.Size = new System.Drawing.Size(357, 91);
            this.cmdCodeTB.TabIndex = 11;
            this.cmdCodeTB.Text = "--crf 20 --level 4.1 --bframes 7 --b-adapt 2 --ref 4 --aq-mode 1 --aq-strength 0." +
    "7 --merange 32 --me umh --subme 10 --partitions all --trellis 2 --psy-rd 0.7:0 -" +
    "-deblock 0:0";
            // 
            // encoderCB
            // 
            this.encoderCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encoderCB.FormattingEnabled = true;
            this.encoderCB.Items.AddRange(new object[] {
            "x264_64-8bit.exe",
            "x264_64-10bit.exe",
            "x265-10bit.exe"});
            this.encoderCB.Location = new System.Drawing.Point(76, 20);
            this.encoderCB.Name = "encoderCB";
            this.encoderCB.Size = new System.Drawing.Size(152, 20);
            this.encoderCB.TabIndex = 6;
            // 
            // encoderLabel
            // 
            this.encoderLabel.AutoSize = true;
            this.encoderLabel.Location = new System.Drawing.Point(7, 23);
            this.encoderLabel.Name = "encoderLabel";
            this.encoderLabel.Size = new System.Drawing.Size(53, 12);
            this.encoderLabel.TabIndex = 5;
            this.encoderLabel.Text = "编码程序";
            // 
            // brORqLabel
            // 
            this.brORqLabel.AutoSize = true;
            this.brORqLabel.Location = new System.Drawing.Point(6, 55);
            this.brORqLabel.Name = "brORqLabel";
            this.brORqLabel.Size = new System.Drawing.Size(0, 12);
            this.brORqLabel.TabIndex = 3;
            // 
            // fileFormatCB
            // 
            this.fileFormatCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileFormatCB.FormattingEnabled = true;
            this.fileFormatCB.Items.AddRange(new object[] {
            "mp4",
            "mkv"});
            this.fileFormatCB.Location = new System.Drawing.Point(289, 20);
            this.fileFormatCB.Name = "fileFormatCB";
            this.fileFormatCB.Size = new System.Drawing.Size(73, 20);
            this.fileFormatCB.TabIndex = 1;
            // 
            // fileFormatLabel
            // 
            this.fileFormatLabel.AutoSize = true;
            this.fileFormatLabel.Location = new System.Drawing.Point(231, 23);
            this.fileFormatLabel.Name = "fileFormatLabel";
            this.fileFormatLabel.Size = new System.Drawing.Size(53, 12);
            this.fileFormatLabel.TabIndex = 0;
            this.fileFormatLabel.Text = "文件格式";
            // 
            // removeSettingBtn
            // 
            this.removeSettingBtn.Location = new System.Drawing.Point(314, 250);
            this.removeSettingBtn.Name = "removeSettingBtn";
            this.removeSettingBtn.Size = new System.Drawing.Size(75, 23);
            this.removeSettingBtn.TabIndex = 10;
            this.removeSettingBtn.Text = "移除预设";
            this.removeSettingBtn.UseVisualStyleBackColor = true;
            this.removeSettingBtn.Click += new System.EventHandler(this.removeSettingBtn_Click);
            // 
            // addSettingBtn
            // 
            this.addSettingBtn.Location = new System.Drawing.Point(223, 250);
            this.addSettingBtn.Name = "addSettingBtn";
            this.addSettingBtn.Size = new System.Drawing.Size(85, 23);
            this.addSettingBtn.TabIndex = 9;
            this.addSettingBtn.Text = "添加预设";
            this.addSettingBtn.UseVisualStyleBackColor = true;
            this.addSettingBtn.Click += new System.EventHandler(this.addSettingBtn_Click);
            // 
            // encodeSettinglabel
            // 
            this.encodeSettinglabel.AutoSize = true;
            this.encodeSettinglabel.Location = new System.Drawing.Point(21, 255);
            this.encodeSettinglabel.Name = "encodeSettinglabel";
            this.encodeSettinglabel.Size = new System.Drawing.Size(53, 12);
            this.encodeSettinglabel.TabIndex = 8;
            this.encodeSettinglabel.Text = "编码预设";
            // 
            // encodeSettingCB
            // 
            this.encodeSettingCB.FormattingEnabled = true;
            this.encodeSettingCB.Location = new System.Drawing.Point(90, 252);
            this.encodeSettingCB.Name = "encodeSettingCB";
            this.encodeSettingCB.Size = new System.Drawing.Size(122, 20);
            this.encodeSettingCB.TabIndex = 7;
            this.encodeSettingCB.SelectedIndexChanged += new System.EventHandler(this.encodeSettingCB_SelectedIndexChanged);
            // 
            // set2SelectedBtn
            // 
            this.set2SelectedBtn.Location = new System.Drawing.Point(395, 250);
            this.set2SelectedBtn.Name = "set2SelectedBtn";
            this.set2SelectedBtn.Size = new System.Drawing.Size(74, 23);
            this.set2SelectedBtn.TabIndex = 10;
            this.set2SelectedBtn.Text = "应用到选中";
            this.set2SelectedBtn.UseVisualStyleBackColor = true;
            this.set2SelectedBtn.Click += new System.EventHandler(this.set2SelectedBtn_Click);
            // 
            // set2AllBtn
            // 
            this.set2AllBtn.Location = new System.Drawing.Point(394, 281);
            this.set2AllBtn.Name = "set2AllBtn";
            this.set2AllBtn.Size = new System.Drawing.Size(75, 23);
            this.set2AllBtn.TabIndex = 11;
            this.set2AllBtn.Text = "全部应用";
            this.set2AllBtn.UseVisualStyleBackColor = true;
            this.set2AllBtn.Click += new System.EventHandler(this.set2AllBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(397, 457);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(74, 53);
            this.StartBtn.TabIndex = 12;
            this.StartBtn.Text = "开始任务";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // fileListView
            // 
            this.fileListView.AllowDrop = true;
            this.fileListView.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.fileListView.Location = new System.Drawing.Point(12, 12);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(457, 160);
            this.fileListView.TabIndex = 13;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.fileListView_DragDrop);
            this.fileListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.fileListView_DragEnter);
            // 
            // outputFilePathlabel
            // 
            this.outputFilePathlabel.AutoSize = true;
            this.outputFilePathlabel.Location = new System.Drawing.Point(21, 225);
            this.outputFilePathlabel.Name = "outputFilePathlabel";
            this.outputFilePathlabel.Size = new System.Drawing.Size(53, 12);
            this.outputFilePathlabel.TabIndex = 14;
            this.outputFilePathlabel.Text = "输出路径";
            // 
            // outputFilePathTB
            // 
            this.outputFilePathTB.Location = new System.Drawing.Point(90, 222);
            this.outputFilePathTB.Name = "outputFilePathTB";
            this.outputFilePathTB.Size = new System.Drawing.Size(299, 21);
            this.outputFilePathTB.TabIndex = 15;
            // 
            // chooseOutputPathBtn
            // 
            this.chooseOutputPathBtn.Location = new System.Drawing.Point(395, 222);
            this.chooseOutputPathBtn.Name = "chooseOutputPathBtn";
            this.chooseOutputPathBtn.Size = new System.Drawing.Size(75, 23);
            this.chooseOutputPathBtn.TabIndex = 16;
            this.chooseOutputPathBtn.Text = "浏览";
            this.chooseOutputPathBtn.UseVisualStyleBackColor = true;
            this.chooseOutputPathBtn.Click += new System.EventHandler(this.chooseOutputPathBtn_Click);
            // 
            // audioSetgB
            // 
            this.audioSetgB.Controls.Add(this.profileCB);
            this.audioSetgB.Controls.Add(this.profileLabel);
            this.audioSetgB.Controls.Add(this.bitrateOrQualityCB);
            this.audioSetgB.Controls.Add(this.bitrateOrQualityLabel);
            this.audioSetgB.Controls.Add(this.codecModeCB);
            this.audioSetgB.Controls.Add(this.codecModeLabel);
            this.audioSetgB.Controls.Add(this.audioEncoderCB);
            this.audioSetgB.Controls.Add(this.audioEncoder);
            this.audioSetgB.Controls.Add(this.label3);
            this.audioSetgB.Location = new System.Drawing.Point(14, 428);
            this.audioSetgB.Name = "audioSetgB";
            this.audioSetgB.Size = new System.Drawing.Size(377, 82);
            this.audioSetgB.TabIndex = 17;
            this.audioSetgB.TabStop = false;
            this.audioSetgB.Text = "音频流设置";
            // 
            // profileCB
            // 
            this.profileCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileCB.FormattingEnabled = true;
            this.profileCB.Location = new System.Drawing.Point(289, 24);
            this.profileCB.Name = "profileCB";
            this.profileCB.Size = new System.Drawing.Size(75, 20);
            this.profileCB.TabIndex = 12;
            this.profileCB.SelectedIndexChanged += new System.EventHandler(this.profileCB_SelectedIndexChanged);
            // 
            // profileLabel
            // 
            this.profileLabel.AutoSize = true;
            this.profileLabel.Location = new System.Drawing.Point(236, 27);
            this.profileLabel.Name = "profileLabel";
            this.profileLabel.Size = new System.Drawing.Size(47, 12);
            this.profileLabel.TabIndex = 11;
            this.profileLabel.Text = "Profile";
            // 
            // bitrateOrQualityCB
            // 
            this.bitrateOrQualityCB.FormattingEnabled = true;
            this.bitrateOrQualityCB.Items.AddRange(new object[] {
            "0",
            "9",
            "18",
            "27",
            "36",
            "45",
            "54",
            "63",
            "73",
            "82",
            "91",
            "100",
            "109",
            "118",
            "127"});
            this.bitrateOrQualityCB.Location = new System.Drawing.Point(255, 52);
            this.bitrateOrQualityCB.Name = "bitrateOrQualityCB";
            this.bitrateOrQualityCB.Size = new System.Drawing.Size(110, 20);
            this.bitrateOrQualityCB.TabIndex = 10;
            // 
            // bitrateOrQualityLabel
            // 
            this.bitrateOrQualityLabel.AutoSize = true;
            this.bitrateOrQualityLabel.Location = new System.Drawing.Point(220, 55);
            this.bitrateOrQualityLabel.Name = "bitrateOrQualityLabel";
            this.bitrateOrQualityLabel.Size = new System.Drawing.Size(29, 12);
            this.bitrateOrQualityLabel.TabIndex = 9;
            this.bitrateOrQualityLabel.Text = "质量";
            // 
            // codecModeCB
            // 
            this.codecModeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codecModeCB.FormattingEnabled = true;
            this.codecModeCB.Location = new System.Drawing.Point(75, 52);
            this.codecModeCB.Name = "codecModeCB";
            this.codecModeCB.Size = new System.Drawing.Size(121, 20);
            this.codecModeCB.TabIndex = 8;
            this.codecModeCB.SelectedIndexChanged += new System.EventHandler(this.codecModeCB_SelectedIndexChanged);
            // 
            // codecModeLabel
            // 
            this.codecModeLabel.AutoSize = true;
            this.codecModeLabel.Location = new System.Drawing.Point(5, 55);
            this.codecModeLabel.Name = "codecModeLabel";
            this.codecModeLabel.Size = new System.Drawing.Size(53, 12);
            this.codecModeLabel.TabIndex = 7;
            this.codecModeLabel.Text = "编码模式";
            // 
            // audioEncoderCB
            // 
            this.audioEncoderCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioEncoderCB.FormattingEnabled = true;
            this.audioEncoderCB.Items.AddRange(new object[] {
            "复制音频流",
            "无音频流",
            "qaac"});
            this.audioEncoderCB.Location = new System.Drawing.Point(75, 24);
            this.audioEncoderCB.Name = "audioEncoderCB";
            this.audioEncoderCB.Size = new System.Drawing.Size(151, 20);
            this.audioEncoderCB.TabIndex = 6;
            this.audioEncoderCB.SelectedIndexChanged += new System.EventHandler(this.audioEncoderCB_SelectedIndexChanged);
            // 
            // audioEncoder
            // 
            this.audioEncoder.AutoSize = true;
            this.audioEncoder.Location = new System.Drawing.Point(6, 27);
            this.audioEncoder.Name = "audioEncoder";
            this.audioEncoder.Size = new System.Drawing.Size(53, 12);
            this.audioEncoder.TabIndex = 5;
            this.audioEncoder.Text = "编码程序";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 521);
            this.Controls.Add(this.audioSetgB);
            this.Controls.Add(this.removeSettingBtn);
            this.Controls.Add(this.chooseOutputPathBtn);
            this.Controls.Add(this.addSettingBtn);
            this.Controls.Add(this.outputFilePathTB);
            this.Controls.Add(this.encodeSettinglabel);
            this.Controls.Add(this.encodeSettingCB);
            this.Controls.Add(this.outputFilePathlabel);
            this.Controls.Add(this.fileListView);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.set2AllBtn);
            this.Controls.Add(this.set2SelectedBtn);
            this.Controls.Add(this.videoSetgB);
            this.Controls.Add(this.removeAllBtn);
            this.Controls.Add(this.removeFileBtn);
            this.Controls.Add(this.addFolderBtn);
            this.Controls.Add(this.addFileBtn);
            this.Name = "MainForm";
            this.Text = "Expert Video Toolbox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.videoSetgB.ResumeLayout(false);
            this.videoSetgB.PerformLayout();
            this.audioSetgB.ResumeLayout(false);
            this.audioSetgB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button removeAllBtn;
        private System.Windows.Forms.Button removeFileBtn;
        private System.Windows.Forms.Button addFolderBtn;
        private System.Windows.Forms.Button addFileBtn;
        private System.Windows.Forms.GroupBox videoSetgB;
        private System.Windows.Forms.ComboBox encoderCB;
        private System.Windows.Forms.Label encoderLabel;
        private System.Windows.Forms.Label brORqLabel;
        private System.Windows.Forms.ComboBox fileFormatCB;
        private System.Windows.Forms.Label fileFormatLabel;
        private System.Windows.Forms.ComboBox encodeSettingCB;
        private System.Windows.Forms.TextBox cmdCodeTB;
        private System.Windows.Forms.Button removeSettingBtn;
        private System.Windows.Forms.Button addSettingBtn;
        private System.Windows.Forms.Label encodeSettinglabel;
        private System.Windows.Forms.Button set2SelectedBtn;
        private System.Windows.Forms.Button set2AllBtn;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.Label outputFilePathlabel;
        private System.Windows.Forms.TextBox outputFilePathTB;
        private System.Windows.Forms.Button chooseOutputPathBtn;
        private System.Windows.Forms.GroupBox audioSetgB;
        private System.Windows.Forms.ComboBox profileCB;
        private System.Windows.Forms.Label profileLabel;
        private System.Windows.Forms.ComboBox bitrateOrQualityCB;
        private System.Windows.Forms.Label bitrateOrQualityLabel;
        private System.Windows.Forms.ComboBox codecModeCB;
        private System.Windows.Forms.Label codecModeLabel;
        private System.Windows.Forms.ComboBox audioEncoderCB;
        private System.Windows.Forms.Label audioEncoder;
        private System.Windows.Forms.Label label3;
    }
}

