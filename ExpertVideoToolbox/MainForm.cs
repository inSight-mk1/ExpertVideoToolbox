using ExpertVideoToolbox.cmdCodeGenerator;
using ExpertVideoToolbox.taskManager;
using ExpertVideoToolbox.TaskManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertVideoToolbox
{
    public partial class MainForm : Form
    {
        taskSetting[] settings;
        Job j;
        
        public MainForm()
        {
            InitializeComponent();
            
            CheckForIllegalCrossThreadCalls = false;
            
            string fileNameColumnText = "文件名";
            string settingColumnText = "编码预设";
            
            this.fileListView.View = View.Details;

            int settingColumnWidth = 100;
            int reservedWidth = 5;
            
            // 显示的文件名列，优化了省略号的位置
            ColumnHeader visualFileNameCH = new ColumnHeader();
            visualFileNameCH.Text = fileNameColumnText;
            visualFileNameCH.Width = this.fileListView.Width - settingColumnWidth - reservedWidth;

            ColumnHeader settingCH = new ColumnHeader();
            settingCH.Text = settingColumnText;
            settingCH.Width = settingColumnWidth;

            // 真实文件名列，不显示在UI上，宽度为0
            ColumnHeader realFileNameCH = new ColumnHeader();
            realFileNameCH.Text = "";
            realFileNameCH.Width = 0;

            this.fileListView.Columns.Add(visualFileNameCH);
            this.fileListView.Columns.Add(settingCH);
            this.fileListView.Columns.Add(realFileNameCH);

            //this.outputFilePathTB.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\trans";
           
            // 从文件中读取已存储的预设
            int i = readAllSettingsFromFiles();
            
            // 将读到的预设加载到combobox中
            for (int j = 0; j < i; j++)
            {               
                this.encodeSettingCB.Items.Add(settings[j].name);
            }

            this.encodeSettingCB.SelectedIndex = 0;

            //ListViewItem gg = new ListViewItem(new string[] { "test01", "test" }); //这是至关重要的一点。
            //this.fileListView.Items.Add(gg);
        }

        // ！！点击开始转换
        private void StartBtn_Click(object sender, EventArgs e)
        {           
            if (this.fileListView.Items.Count == 0)
            {
                string noFileMsg = "未添加任何文件";
                MessageBox.Show(noFileMsg);
            }
            else
            {
                if (checkSettingChange() == -1)
                {
                    MessageBox.Show("一个或多个任务的预设已被删除，请重新添加预设！");
                    return;
                }
                
                if (checkSettingChange() == 1) // 1为真，表示有变化
                {
                    string note = "检测到预设已修改，是否覆盖当前选中的预设？\n点击“确定”将覆盖并应用该预设到全部任务。点击“取消”将中止任务且不做改动。";
                    DialogResult dr = MessageBox.Show(note, "检测到预设修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        //点确定的代码
                        saveSetting2File(this.encodeSettingCB.Text);
                        readAllSettingsFromFiles();

                        for (int i = 0; i < this.fileListView.Items.Count; i++)
                        {                           
                            fileListView.Items[i].SubItems[1].Text = this.encodeSettingCB.Text;
                        }
                    }
                    else
                    {
                        //点取消的代码 
                        return;
                    }
                }

                int count = fileListView.Items.Count;
                filePathStorage q = new filePathStorage(count);
                for (int i = 0; i < count; i++)
                {
                    if (!CheckSettingExist(fileListView.Items[i].SubItems[1].Text))
                    {
                        MessageBox.Show("一个或多个任务的预设已被删除，请检查编码设置！");
                        return;
                    }
                    
                    // subitems[2]中保存着完整文件名
                    videoTask t = new videoTask(fileListView.Items[i].SubItems[2].Text, fileListView.Items[i].SubItems[1].Text);
                    q.add(t);
                }

                this.j = new Job(q);
                //this.j.ErrorEvent += DisposeJob;
                this.j.runJob(this.outputFilePathTB.Text);
            }
        }

        private void fileListView_DragDrop(object sender, DragEventArgs e)
        {
            Array files = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
            int count = files.Length;
            for (int i = 0; i < count; i++)
            {
                string fp = files.GetValue(i).ToString();
                string setting = this.encodeSettingCB.Text;

                addItem2FileListView(fp, setting);
            }
        }

        // 必须添加这一事件才可以拖动进来
        private void fileListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Process p = Process.GetCurrentProcess();
            p.Kill();
        }

        private void addFileBtn_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);  // 默认打开显示我的文档
            openFileDialog1.Filter = "mp4视频文件 (*.mp4)|*.mp4|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            string fp = openFileDialog1.FileName;
                            string setting = this.encodeSettingCB.Text;
                            
                            ListViewItem item = new ListViewItem(new string[] { fp, setting });
                            this.fileListView.Items.Add(item);
                      }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void addFolderBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;

                string[] files = System.IO.Directory.GetFiles(folderPath);
                int filesCount = files.Length;
                int videoFilesCount = 0;
                for (int i = 0; i < filesCount; i++)
                {
                    string fp = files[i];
                    string[] s = fp.Split(new char[] { '.' });
                    string ext = s[s.Length - 1];
                    if (videoFileCheckByExtension(ext))
                    {
                        videoFilesCount++;

                        string setting = this.encodeSettingCB.Text;

                        ListViewItem item = new ListViewItem(new string[] { fp, setting });
                        this.fileListView.Items.Add(item);
                    }

                }
                string msg = "目录搜索完成，找到" + videoFilesCount.ToString() + "个支持的文件";
                MessageBox.Show(msg);
            }
        }

        private bool videoFileCheckByExtension(string extension)
        {
            string[] videoExt = { "mp4", "wmv", "avi", "mkv", "rm", "rmvb", "vob", "dat", "asf", "mpg", "ts", "mpeg", "mpe", "mov", "flv", "f4v", "3gp" };
            return this.multiStringEqualCheck(videoExt, extension);
        }

        private bool multiStringEqualCheck(string[] targets, string source)
        {
            int len = targets.Length;
            for (int i = 0; i < len; i++)
            {
                if (string.Equals(targets[i], source))
                {
                    return true;
                }
            }
            return false;
        }

        private void removeFileBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexesREADONLY = fileListView.SelectedIndices;
            
            int count = indexesREADONLY.Count;
            int[] indexes = new int[count];
            for (int i = 0; i < count; i++)
            {
                indexes[i] = indexesREADONLY[i];
            }
        
            if (count == 0)
            {
                MessageBox.Show("未选择任何任务！");
                return;
            }
            for (int i = 0; i < count; )
            {
                int removeIndex = indexes[i++];
                fileListView.Items.RemoveAt(removeIndex);

                // 此时i是刚刚处理的indexes数组元素索引值 +1 ，遍历还未处理的indexes数组
                for (int j = i; j < count; j++)
                {
                    // 如果此索引值大于刚刚删除的item的索引值，说明该位置的元素要前移一位
                    // 如果不大于就不用管，对其位置没有影响
                    if (indexes[j] > removeIndex)
                    {
                        indexes[j]--;
                    }  
                }
            }
        }

        // 编码预设改变时更新所有信息
        private void encodeSettingCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (taskSetting ts in settings)
            {             
                if (ts != null && String.Equals(this.encodeSettingCB.Text, ts.name))
                {
                    // Items集合中有此项值才能通过这种方法添加（DropList模式）
                    this.encoderCB.Text = ts.encoder;
                    this.audioEncoderCB.Text = ts.audioEncoder;
                    this.fileFormatCB.Text = ts.outputFormat;
                    this.cmdCodeTB.Text = ts.encoderSetting;

                    this.profileCB.Text = ts.audioProfile;
                    this.codecModeCB.Text = ts.audioCodecMode;
                    if (String.Equals(this.bitrateOrQualityLabel, "质量"))
                    {
                        this.bitrateOrQualityCB.Text = ts.audioQuality;
                    }
                    else
                    {
                        this.bitrateOrQualityCB.Text = ts.audioKbps;
                    }
                
                    break;
                }
            }
        }

        // 点击添加预设按钮
        private void addSettingBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.encodeSettingCB.Items.Count; i++)
            {
                // 如果要添加的预设名称和已有的任意一个相同
                if (String.Equals(this.encodeSettingCB.Text, this.encodeSettingCB.Items[i].ToString()))
                {
                    DialogResult dr = MessageBox.Show("确定要覆盖预设" + this.encodeSettingCB.Text + "吗？", "确认覆盖", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        //点确定的代码
                        saveSetting2File(this.encodeSettingCB.Text);
                        readAllSettingsFromFiles();
                        return;
                    }
                    else
                    {
                        //点取消的代码 
                        return;
                    }
                }
            }

            // 如果没有重复不仅要存储预设，还要更新UI
            saveSetting2File(this.encodeSettingCB.Text);
            this.encodeSettingCB.Items.Add(this.encodeSettingCB.Text);
            readAllSettingsFromFiles();
        }

        private void saveSetting2File(string name)
        {
            taskSetting ts = new taskSetting(name, this.encoderCB.Text, this.fileFormatCB.Text, this.cmdCodeTB.Text,
                this.audioEncoderCB.Text, this.profileCB.Text, this.codecModeCB.Text, this.bitrateOrQualityCB.Text, 
                this.bitrateOrQualityCB.Text);
            string fp = System.Windows.Forms.Application.StartupPath + "\\taskSettings\\" + name + ".json";
            File.WriteAllText(fp, JsonConvert.SerializeObject(ts));
        }

        private int readAllSettingsFromFiles()
        {
            DirectoryInfo folder = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\taskSettings");
            FileInfo[] fis = folder.GetFiles();
            
            settings = new taskSetting[fis.Length];
            int i = 0;
            foreach (FileInfo fi in fis)
            {
                if (String.Equals(fi.Extension, ".json"))  // 文件的扩展名要包括.（点）
                {
                    settings[i++] = JsonConvert.DeserializeObject<taskSetting>(File.ReadAllText(fi.FullName));
                }
            }
            return i;
        }

        // 音频编码器选项变化时
        private void audioEncoderCB_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (this.audioEncoderCB.SelectedIndex == 0 || this.audioEncoderCB.SelectedIndex == 1)
           {
               this.profileCB.Enabled = false;
               this.codecModeCB.Enabled = false;
               this.bitrateOrQualityCB.Enabled = false;              
           }
           else
           {
               this.profileCB.Enabled = true;
               this.addProfileItems();
               this.codecModeCB.Enabled = true;
               this.bitrateOrQualityCB.Enabled = true;
           }
        }

        // 点击移除预设按钮
        private void removeSettingBtn_Click(object sender, EventArgs e)
        {
            // 需要做的事: 
            // 1.删除保存的json文件(必须先删除，不然文件名就会错误)
            // 2.从combobox中删除，并显示另一个预设的参数
            // 不需要从setting数组中删除对应的元素，因为不可能选到被删除的预设了，如果再次添加同名的预设，会重新读取到setting数组（详见添加预设按钮的代码）
            DialogResult dr = MessageBox.Show("确定要删除预设" + this.encodeSettingCB.Text + "吗？", "确认删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //点确定的代码               
                string fp = System.Windows.Forms.Application.StartupPath + "\\taskSettings\\" + this.encodeSettingCB.Text + ".json";
                File.Delete(fp);
                this.encodeSettingCB.Items.Remove(this.encodeSettingCB.Text);
                this.encodeSettingCB.SelectedIndex = 0;
            }
            else
            {
                //点取消的代码 
            }
        }

        // 点击移除所有按钮
        private void removeAllBtn_Click(object sender, EventArgs e)
        {
            fileListView.Items.Clear();
        }

        // 选择输出文件夹
        private void chooseOutputPathBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;
                this.outputFilePathTB.Text = folderPath;
            }
        }

        // 点击应用到选中
        private void set2SelectedBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = fileListView.SelectedIndices;
            int count = indexes.Count;
            if (count == 0)
            {
                MessageBox.Show("未选择任何任务!");
                return;
            }
            for (int i = 0; i < count; i++)
            {
                fileListView.Items[indexes[i]].SubItems[1].Text = this.encodeSettingCB.Text;
            }
        }

        // 点击全部应用
        private void set2AllBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.fileListView.Items.Count; i++)
            {
                fileListView.Items[i].SubItems[1].Text = this.encodeSettingCB.Text;
            }
        }

        // 检查预设与UI显示的有无不同, 结果1和0分别表示真和假，-1表示预设不存在
        private int checkSettingChange()
        {            
            string fp = System.Windows.Forms.Application.StartupPath + "\\taskSettings\\" + this.encodeSettingCB.Text + ".json";
            try
            {
                taskSetting ts = JsonConvert.DeserializeObject<taskSetting>(File.ReadAllText(fp));
                if (String.Equals(this.encoderCB.Text, ts.encoder)
                    && String.Equals(this.fileFormatCB.Text, ts.outputFormat)
                    && String.Equals(this.cmdCodeTB.Text, ts.encoderSetting)
                    && String.Equals(this.audioEncoderCB.Text, ts.audioEncoder)
                    && (String.Equals(this.profileCB.Text, ts.audioProfile) || this.profileCB.Enabled == false)
                    && (String.Equals(this.codecModeCB.Text, ts.audioCodecMode) || this.codecModeCB.Enabled == false)
                    && (String.Equals(this.bitrateOrQualityCB.Text, ts.audioQuality) || this.bitrateOrQualityCB.Enabled == false))
                {
                    return 0;  // false表示没有变化
                }
                return 1;
            }
            catch (System.IO.IOException)
            {                
                return -1;
            }
        }

        // 检查预设是否存在
        private bool CheckSettingExist(string s)
        {
            string fp = System.Windows.Forms.Application.StartupPath + "\\taskSettings\\" + s + ".json";
            return File.Exists(fp);
        }

        private void DisposeJob()
        {
            this.j = null;
            GC.Collect();        
        }

        private void profileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (string.Equals(comboBox.Text, "HE-AAC"))
            {
                //this.bitrateOrQualityCB.Enabled = true;
                //this.codecModeCB.Enabled = true;
                this.addCodecModeItems();
                this.codecModeCB.SelectedIndex = 0;
                this.brORqLabel.Text = "码率";
                this.addBitrateItems();
                this.bitrateOrQualityCB.Text = "48";     // 设置默认值
            }
            else if (string.Equals(comboBox.Text, "LC-AAC"))
            {
                //this.bitrateOrQualityCB.Enabled = true;
                //this.codecModeCB.Enabled = true;
                this.addCodecModeItems();
                this.codecModeCB.SelectedIndex = 3;
                this.brORqLabel.Text = "质量";
                this.addQualityItems();
                this.bitrateOrQualityCB.Text = "91";      // 设置默认值
            } else
            {
                this.bitrateOrQualityCB.Enabled = false;
                this.codecModeCB.Enabled = false;
            }
        }

        private void addQualityItems()
        {
            this.bitrateOrQualityCB.Items.Clear();
            this.bitrateOrQualityCB.Items.Add("0");
            this.bitrateOrQualityCB.Items.Add("9");
            this.bitrateOrQualityCB.Items.Add("18");
            this.bitrateOrQualityCB.Items.Add("27");
            this.bitrateOrQualityCB.Items.Add("36");
            this.bitrateOrQualityCB.Items.Add("45");
            this.bitrateOrQualityCB.Items.Add("54");
            this.bitrateOrQualityCB.Items.Add("63");
            this.bitrateOrQualityCB.Items.Add("73");
            this.bitrateOrQualityCB.Items.Add("82");
            this.bitrateOrQualityCB.Items.Add("91");
            this.bitrateOrQualityCB.Items.Add("100");
            this.bitrateOrQualityCB.Items.Add("109");
            this.bitrateOrQualityCB.Items.Add("118");
            this.bitrateOrQualityCB.Items.Add("127");
        }

        private void addBitrateItems()
        {
            this.bitrateOrQualityCB.Items.Clear();
            this.bitrateOrQualityCB.Items.Add("32");
            this.bitrateOrQualityCB.Items.Add("64");
            this.bitrateOrQualityCB.Items.Add("96");
            this.bitrateOrQualityCB.Items.Add("128");
            this.bitrateOrQualityCB.Items.Add("192");
            this.bitrateOrQualityCB.Items.Add("256");
            this.bitrateOrQualityCB.Items.Add("320");
        }

        private void addCodecModeItems()
        {
            this.codecModeCB.Items.Clear();
            this.codecModeCB.Items.Add("CBR");
            this.codecModeCB.Items.Add("ABR");
            this.codecModeCB.Items.Add("CVBR");
            if (this.profileCB.SelectedIndex == 0)
            {
                this.codecModeCB.Items.Add("TVBR");
            }
        }

        private void addProfileItems()
        {
            if (this.audioEncoderCB.SelectedIndex == 2)
            {
                this.profileCB.Items.Clear();
                this.profileCB.Items.Add("LC-AAC");
                this.profileCB.Items.Add("HE-AAC");
                this.profileCB.Items.Add("ALAC");
                this.profileCB.SelectedIndex = 0;
            }
        }

        private void codecModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (string.Equals(comboBox.Text, "TVBR"))
            {
                this.brORqLabel.Text = "质量";
                this.addQualityItems();
                this.bitrateOrQualityCB.Text = "91";      // 设置默认值
            }
            else
            {
                if (string.Equals(this.brORqLabel.Text, "质量"))
                {
                    this.brORqLabel.Text = "码率";
                    this.addBitrateItems();

                    this.bitrateOrQualityCB.Text = "256";     // 设置默认值
                }
            }  
        }

        // 将文件名和预设数据添加到UI，并优化显示模式
        private void addItem2FileListView(string fp, string setting)
        {
            // 生成显示在UI上的字符串
            string dot = ".";
            
            var fpFullWidth = this.CreateGraphics().MeasureString(fp, this.fileListView.Font).Width;  // 获取文本的显示宽度
            var dotWidth = this.CreateGraphics().MeasureString(dot, this.fileListView.Font).Width;
            
            var columnWidth = this.fileListView.Columns[0].Width - (int)dotWidth;

            string visualfp = "";
            if (fpFullWidth <= columnWidth)  // 文本不大于列宽度，则全部显示，不做处理
            {
                visualfp = fp;
            }
            else
            {
                int length = fp.Length;                      // 文字字符长度
                double averageWidth = fpFullWidth / length;  // 平均每个字符的显示宽度

                double frontRatio = 0.2;                     // 文本前部占有效信息的比例，0.5表示前后各一半
                int dotCount = 6;
                double frontTextWidth = (columnWidth - dotWidth * dotCount) * frontRatio;
                int frontTextLength = (int)Math.Round(frontTextWidth / averageWidth);
                frontTextLength = frontTextLength >= 3 ? frontTextLength : 3;  // 文本前部长度最小为3，至少要能显示盘符
                int rearTextLength = (int)Math.Round(frontTextLength * (1 - frontRatio) / frontRatio);

                int rearStartIndex = length - rearTextLength;
                visualfp = fp.Substring(0, frontTextLength) + ("").PadLeft(dotCount, '0').Replace("0", dot) + fp.Substring(rearStartIndex, rearTextLength);
                
                // 以上为初步生成，如果不符合要求，则继续修剪
                var tempWidth = this.CreateGraphics().MeasureString(visualfp, this.fileListView.Font).Width;
                bool trimTooMuch = false;

                // 两种情况下要修剪，1.超出宽度 2.宽度少一个点以上
                while( tempWidth > columnWidth || (trimTooMuch = (columnWidth - tempWidth > dotWidth)) )  
                {
                    if (trimTooMuch == false) // 第一种情况
                    {
                        if (frontTextLength <= 3) // 文本前部即将小于最小宽度，只能修建文本后部 
                        {
                            visualfp = fp.Substring(0, frontTextLength) + ("").PadLeft(dotCount, '0').Replace("0", dot) + fp.Substring(++rearStartIndex, rearTextLength);
                        }
                        else // 否则修剪文本前部
                        {
                            visualfp = fp.Substring(0, --frontTextLength) + ("").PadLeft(dotCount, '0').Replace("0", dot) + fp.Substring(rearStartIndex, rearTextLength);
                        }              
                    }
                    else  // 第二种情况，直接前移文本后部的起始索引
                    {
                        visualfp = fp.Substring(0, frontTextLength) + ("").PadLeft(dotCount, '0').Replace("0", dot) + fp.Substring(--rearStartIndex, ++rearTextLength);
                        trimTooMuch = false;  // 重置判断状态
                    }
                    tempWidth = this.CreateGraphics().MeasureString(visualfp, this.fileListView.Font).Width;
                }
            }
            ListViewItem item = new ListViewItem(new string[] { visualfp, setting, fp });
            this.fileListView.Items.Add(item);
        }
    }
}