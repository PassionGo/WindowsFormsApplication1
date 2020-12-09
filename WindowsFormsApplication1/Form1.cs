using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Timers;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }
        Int32 line, i; string flag1="  Energy       Flux  ", flag2="MeV          (m**2 sr s MeV)-1";
        Int32 plotflag,extensionnameflag;
        string extensionname;
        public void init()
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.textBox2.Text = "txt";
            this.textBox2.Enabled = false;
            extensionname = ".txt";
            line = 3;
            i = 0;
        }
        public string GetStrFields(string strWords)
        {

            Regex replaceSpace = new Regex(@"\s{1,}", RegexOptions.IgnoreCase);

            return replaceSpace.Replace(strWords, ",").Trim();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\Users\\Administrator\\Desktop";  //选择初始打开文件夹
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "文本文件(*.txt)|*.txt";
            dialog.RestoreDirectory = true;
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label6.Text = "选择文件";
                foreach (string files in dialog.FileNames)
                {
                    string[] lines = File.ReadAllLines(files, Encoding.GetEncoding("gb2312"));
                    string specificLine="";
                    string txtname = System.IO.Path.GetFileName(files);
                    if (checkBox1.Checked == false)
                    {
                        if ((lines[0].Contains(flag1)) & (lines[1].Contains(flag2)))
                        {
                            if (lines.Length < (line - 1))
                            { MessageBox.Show(txtname + "文件有误，请重新选择文件", "错误"); }
                            else{
                                i++;
                                lines[line - 1] = GetStrFields(lines[line - 1]);
                                lines[0] = "*,                ," + lines[0];
                                lines[1] = "*,数据来源文件名," + lines[1];
                                label6.Text = files.Replace(txtname, "");
                                lines[0] = lines[0].Replace(" Flux", ",Flux");
                                lines[0] = lines[0].Replace(" Radius", ",Radius");
                                lines[0] = lines[0].Replace(" Dose", ",Dose");
                                lines[1] = lines[1].Replace(" (m**2 sr s MeV)-1", ",(m**2 sr s MeV)-1");
                                lines[1] = lines[1].Replace(" g/cm2 Al", ",g/cm2 Al");
                                lines[1] = lines[1].Replace(" rad(Si)", ",rad(Si)");
                                lines[1] = lines[1].Replace(" mm Al", ",mm Al");
                                lines[1] = lines[1].Replace("等", ",等");
                                lines[1] = lines[1].Replace(" 剂", ",剂");
                                if (richTextBox1.Text.Contains(lines[0])) {
                                    specificLine = i + "'," + txtname + lines[line - 1];
                                }
                                else{
                                    specificLine = lines[0] + "\n" + lines[1] + "\n" + i + "'," + txtname + lines[line - 1];
                                }
                            }
                        }
                        else
                        { MessageBox.Show(txtname + "文件有误，请重新选择文件", "错误"); }
                    }
                    else if(checkBox1.Checked==true){
                        if (lines.Length < line - 1)
                        { MessageBox.Show("提取"+txtname + "信息错误，请核对后获取数据", "错误");}
                        else
                        {
                            i++;
                            specificLine = i + "'," + txtname+lines[line - 1];
                        }
                    }
                    if (richTextBox1.Text.Length == 0){
                        richTextBox1.Text = specificLine;
                    }
                    else {
                        richTextBox1.Text = richTextBox1.Text + "\n" + specificLine;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string s = textBox1.Text.Trim();
            i = 0;
            int result;
            if (Int32.TryParse(s, out result))
            {
                line = result;
            }
            else
            {
                // 转换失败，提示错误
                MessageBox.Show("输入错误！请输入整型数", "错误");
                // 清空文本框
                textBox1.Text = "1";
            }
        }

        private void button1_Click(object sender, EventArgs e)//所有数据清空，回复原有设置
        {
            i = 0;
            this.comboBox1.SelectedIndex = 0;
            this.textBox1.Text = "3";
            this.richTextBox1.Text=string.Empty;
            this.label6.Text = "请输入文件";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
           // dialog.InitialDirectory = "C:\\Users\\Administrator\\Desktop";
            dialog.Title = "保存到";
            dialog.Filter = " csv文件（*.csv）| *.csv|文本文件（*.txt）| *.txt";
            dialog.RestoreDirectory = true;
            dialog.FilterIndex = 1;
            DateTime d = DateTime.Now;
            String date1 = d.ToShortDateString();
            string date2 = d.ToShortTimeString();
            date1 = date1.Replace("/", "");
            date1 = date1.Remove(0,4);
            if (date1.Length == 3)
            { date1 = date1.Insert(2, "-0"); }
            else { date1 = date1.Insert(2, "-"); }
            date2 = date2.Replace(":", "");
            date1 = date1 + " " + date2;
            string timename = date1;
            dialog.FileName = timename+ ".csv";
            dialog.AddExtension = false;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string localFilePath = dialog.FileName.ToString();
                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                string fname = dialog.FileName;
                System.IO.File.WriteAllText(fname, richTextBox1.Text,Encoding.GetEncoding("gb2312"));
                dialog.Dispose();
            }
            label6.Text = "文件保存完毕";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            i = 0;
            switch (this.comboBox1.SelectedIndex)
            {
                case  0 :  line = 3; flag1= "  Energy       Flux  "; flag2="MeV          (m**2 sr s MeV)-1"; break;
                case  1 :  line = 3; flag1= "  Energy       Flux  "; flag2 = "MeV          (m**2 sr s MeV)-1"; break;
                case  2 :  line = 3; flag1 = "  Energy       Flux  "; flag2 = "MeV          (m**2 sr s MeV)-1"; break;
                case  3 :  line = 3; flag1 = "  Energy       Flux  "; flag2 = "MeV          (m**2 sr s MeV)-1"; break;
                case  4 :  line = 17; flag1 = "Radius     Dose         Radius"; flag2 = "g/cm2 Al     rad(Si)        mm Al"; break ;
                case  5 :  line = 21; flag1 = "Radius     Dose         Radius"; flag2 = "g/cm2 Al     rad(Si)        mm Al"; break;
                case  6 :  line = 17; flag1 = "整个任务周期内累计位移损伤剂量"; flag2 = "屏蔽mm"; break;
                case  7 :  line = 21; flag1 = "整个任务周期内累计位移损伤剂量"; flag2 = "屏蔽mm"; break;

            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            i = 0;
            if (this.checkBox1.Checked == true)
            {
                this.comboBox1.Enabled = false;
                this.textBox1.Enabled  = true;
                this.button4.Enabled   = false;
            }
            else
            {
                this.textBox1.Enabled  = false;
                this.comboBox1.Enabled = true;
                this.button4.Enabled   = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0: plotflag = 0; break;   //高度变化
                case 1: plotflag = 0; break;   //高度变化
                case 2: plotflag = 1; break;   //倾角变化
                case 3: plotflag = 1; break;   //倾角变化
                case 4: plotflag = 0; break;   //高度变化
                case 6: plotflag = 0; break;   //高度变化
                case 7: plotflag = 0; break;   //高度变化
            }
            if (plotflag == 0)
            {

            }

          }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0: extensionname = ".txt";break;
                case 1: extensionname = ".doc"; break;
                case 2: extensionname = ".pdf"; break;
                case 3: extensionname = ".xsl"; break;
                case 4: extensionname = ".jpg"; break;
                case 6: extensionname = ".bmp"; break;
                case 7: extensionname = ".png"; break;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\Users\\Administrator\\Desktop";  //选择初始打开文件夹
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            //dialog.Filter = "文本文件(*.txt)|*.txt";
            dialog.RestoreDirectory = true;
            //dialog.FilterIndex = 1
            if (extensionnameflag == 1)
            { extensionname = "."+textBox2.Text; }
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (MessageBox.Show("转换文件格式可能导致文件无法打开\n是否继续转换?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    label6.Text = "选择文件";
                    string path;string opentextname;
                    foreach (string files in dialog.FileNames)
                    {
                        string extensionname1 = System.IO.Path.GetExtension(files);
                        System.IO.File.Move(files, files.Replace(extensionname1, extensionname));
                    }
                  //  System.Diagnostics.Process.Start("Explorer", "C:\\Users\\10583\\Desktop\\Shadowsocks-4.1.10.0");
                    if (MessageBox.Show("转换完成", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        path = System.IO.Path.GetDirectoryName(dialog.FileName);
                       // System.Diagnostics.Process.Start("Explorer", "/select,"+path);
                        foreach (string file in dialog.FileNames)
                        {
                            opentextname = System.IO.Path.GetFileName(file);
                            System.Diagnostics.Process.Start("Explorer", "/select," + path+"\\" + opentextname);
                        }
                       label6.Text = "完成转换";
                    }
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                textBox2.Enabled = false;
                comboBox2.Enabled = true;
                extensionnameflag = 0;
            }
            else if (checkBox2.Checked == true)
            {
                textBox2.Enabled = true;
                comboBox2.Enabled = false;
                extensionnameflag = 1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            label5.Text = dt.ToString();
        }

    }
}
