using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using 域名查询.Code;
using System.Diagnostics;

namespace 域名查询
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 80;
            System.Net.ServicePointManager.SetTcpKeepAlive(true,2000,2000);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = true;
            fd.Title = "选择域名列表文件，以“，”或回车分隔";
            fd.Filter = "所有文件(*.*)|*.*";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                listView1.Items.Clear();
                //listView1.Refresh();
                LoadFromFile(fd.FileNames);
            }
        }
        void LoadFromFile(string[] filepath)
        {
            var x = new Thread(new ParameterizedThreadStart(_LoadFromFile));
            x.Priority = ThreadPriority.Highest;
            x.Start(filepath);
        }
        void _LoadFromFile(object file)
        {
            var _file = (String[])file;
            foreach (var fl in _file)
            {
                var f = File.ReadAllText(fl).Split(new char[] { ',', '\n','\r' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    this.Invoke(new DelegateComm(() =>
                    {

                        listView1.Items.Add("loading...");
                        listView1.Refresh();
                    }));
                    this.Invoke(new DelegateComm(() => listView1.BeginUpdate()));

                    for (int i = f.Length - 1; i > -1;i-- )
                    {
                        var d = f[i];
                        if (!String.IsNullOrWhiteSpace(d))
                        {
                            this.Invoke(new DelegateComm<string>(a =>
                                    this.listView1.Items.Add(new ListViewItem(new string[] { d, "", "", "", "","","","","" }))
                                ), new object[] { d });
                        }
                    }

                    this.Invoke(new DelegateComm(() =>
                    {
                        listView1.EndUpdate();
                        this.listView1.Items.RemoveAt(0);
                        this.listView1.Refresh();
                    }));

                }
                catch (Exception) { }
            }
            
            
            
        }
        

        private static int _indexOfDomdain = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if (_indexOfDomdain < listView1.Items.Count)
            {

                var parms = new TaskMain.TaskParms()
                {
                    List = listView1,
                    LinkCount = Int32.Parse(comboBox1.Text),
                    LinkDomainCount = Int32.Parse(comboBox3.Text),
                    StatusBar = toolStripProgressBar1,
                    IsIndexFirst = checkBox2.Checked,
                    StatusLable = toolStripStatusLabel3
                };
                TaskMain.index = _indexOfDomdain++;
                TaskMain.threadCount = Int32.Parse(comboBox4.Text);

                TaskMain.go(parms);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            SortedList<string, DomainModel> list = new SortedList<string,DomainModel>();
            var c_linkcount = Int32.Parse( comboBox1.Text);
            var c_br = Int32.Parse(comboBox2.Text);
            var c_linkdomaincout = Int32.Parse(comboBox3.Text);
            foreach (ListViewItem x in listView1.Items)
            {
                int linkCount, linkDomainCount, br;
                if (!Int32.TryParse( x.SubItems[1].Text,out linkCount))
                    linkCount = -1;
                if (!Int32.TryParse(x.SubItems[2].Text, out linkDomainCount))
                    linkDomainCount = -1;
                if (!Int32.TryParse( x.SubItems[4].Text,out br))
                    br = -1;
                var available = x.SubItems[6].Text.Equals("可注");
                var indexIsFirst = x.SubItems[3].Text.Equals("T");

                if (linkDomainCount >= c_linkdomaincout && linkCount >= c_linkcount && br >= c_br && (checkBox1.Checked ? available : true) && (checkBox2.Checked ? indexIsFirst : true))
                {
                    var d = new DomainModel()
                    {
                        domain = x.SubItems[0].Text,
                        linkDomainCount = linkDomainCount,
                        linkCount = linkCount,
                        br = br,
                        available = available,
                        beian = x.SubItems[4].Text.Equals("被墙")
                    };
                    list.Add(String.Format("{0},{1},{2},{3},{4}", d.br.ToString("D2"), d.linkDomainCount.ToString("D4"), d.linkCount.ToString("D8"), d.available ? 0 : 1, list.Count.ToString("D8")), d);
                }
            }

            /*保存文件*/
            SaveFileDialog saveFile1 = new SaveFileDialog();      
            saveFile1.Filter = "文本文件(.html)|*.html";     
            saveFile1.FilterIndex = 1;
            saveFile1.FileName = System.DateTime.Now.ToString("域名结果：yyyyMMdd");
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)            {

                try
                {
                    var sb = new StringBuilder("<html><head>");
                    sb.Append("<meta http-equiv=\"content-type\" content=\"text/html;charset=utf-8\">");
                    sb.Append("<style>");
                    sb.Append("body,table{font-size:14px;text-align:center}");
                    sb.Append("td,th{border:solid #add9c0; border-width:0px 1px 1px 0px; padding:5px 5px;}");
                    sb.Append("th{background:#eee}");
                    sb.Append("table{border:solid #add9c0; border-width:1px 0px 0px 1px;}");
                    sb.Append("a:hover,a:active{color:#F00;text-decoration:underline}");
                    sb.Append("</style>");
                    sb.Append("</head><body>");
                    sb.Append("<table>");
                    sb.Append("<thead><th>域名</th><th>百度权重</th><th>外链域名数</th><th>外链数</th><th>GFW</th><th>能否注册</th></thead>");
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        var x = list.Values[i];
                        sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td><a href=\"http://www.baidu.com/s?wd=%22www.{0}%22&rn=100\" target=\"_blank\">{2}</a></td><td><a href=\"http://www.baidu.com/s?wd=%22www.{0}%22&rn=100\" target=\"_blank\">{3}</a></td><td><a href=\"http://icp.aizhan.com/{0}/\" target=\"_blank\">{4}</a><td><a href=\"http://whois.chinaz.com/{0}\" target=\"_blank\">{5}</a></td></tr>", x.domain, x.br, x.linkDomainCount, x.linkCount,x.beian?"<b style='color:red'>被墙</b>":"正常", x.available?"可注":"-");
                    }
                    sb.Append("</table></html>");
                    File.WriteAllText(saveFile1.FileName,sb.ToString());
                    Process.Start(saveFile1.FileName);
                }
                catch
                {
                    throw;
                }
                finally
                {

                }         
            }
        }

        private class DomainModel {
            public string domain;
            public int linkCount = -1;
            public int linkDomainCount = -1;
            public int br = -1;
            public bool available;
            public bool beian;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //用来切换是否滚动
            listView1.Tag = "1".Equals(listView1.Tag) ? "0" : "1";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        
    }

}
