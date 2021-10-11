using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace 域名查询.Code
{

    public class TaskMain2
    {
        public class TaskParms
        {
            /// <summary>
            /// listview控件
            /// </summary>
            public ListView List;
            /// <summary>
            /// 外链数大于此数才进一步查询
            /// </summary>
            //public int LinkCount = 0;

            /// <summary>
            /// 外链域名数大于此数才进一步查询
            /// </summary>
            //public int LinkDomainCount =0;

            /// <summary>
            /// 首页是否第一
            /// </summary>
            //public bool IsIndexFirst = false;

            /// <summary>
            /// 导出时，趋势平均值大于此数才导出
            /// </summary>
            public int MaxTrend = 0;

            /// <summary>
            /// 导出时，权重大于此数时才导出
            /// </summary>
            public int MaxWeight = 0;


            public ToolStripProgressBar StatusBar;

            public ToolStripStatusLabel StatusLable;
        }
        public static int threadCount = 5;
        public static int index =0;
        public static object locker=new object();
        public static int tcount = 0;
        private static void _go(object o)
        {
            TaskParms parms = (TaskParms)o;
            ListView list = parms.List;

            ToolStripProgressBar StatusBar = parms.StatusBar;
            ToolStripStatusLabel StatusLable = parms.StatusLable;
            int i = -1;
            while (true)
            {
                string domain = "";
                lock (locker)
                {
                   
                    i = index++;


                    if (i >= list.Items.Count)
                    {
                        break;
                    }
                }
                list.Invoke(new DelegateComm(() =>
                    {
                        StatusBar.Maximum = list.Items.Count - 1;
                        StatusBar.Value = i;
                        StatusLable.Text = String.Format("{0}/{1}", i + 1, list.Items.Count);
                    }
                ));
                try
                {
                    list.Invoke(new DelegateComm(() => 
                        {
                            domain = list.Items[i].Text;
                            if (!"1".Equals(list.Tag))
                                list.Items[i].EnsureVisible();//选中
                        
                        }));
                }
                catch (Exception)
                {
                    continue;
                }
                Console.WriteLine(String.Format("t:{0}={1}={2}", Thread.CurrentThread.ManagedThreadId, tcount++, domain));
                int trend = 0;

                new TaskSogouTrend(domain, a =>
                {
                    list.Invoke(new DelegateComm(() =>
                    {
                        Int32.TryParse(a,out trend);
                        list.Items[i].SubItems[1].Text = a;
                    }));
                });
                new TaskWhoisBeiAn(domain, a =>
                    {
                        list.Invoke(new DelegateComm(() =>
                                list.Items[i].SubItems[8].Text = a
                            ));
                    });
                if (trend >= parms.MaxTrend)
                {
                    int weight = 0;
                    new TaskSogouWeight(domain, a =>
                    {
                        list.Invoke(new DelegateComm(() =>
                        {
                            Int32.TryParse(a, out weight);
                            list.Items[i].SubItems[2].Text = a;
                        }));
                    });
                    new TaskLinkCount(domain,
                        (a, b, c) => //a = 外链，b=域名数，c=首页第一
                        {
                            list.Invoke(new DelegateComm(() =>
                            {
                                list.Items[i].SubItems[5].Text = a;
                                list.Items[i].SubItems[6].Text = b;
                                list.Items[i].SubItems[7].Text = c;
                            }
                          ));

                        });
                    if (weight >= parms.MaxWeight)
                    {
                        //查whois
                        new TaskWhois(domain,
                            a =>
                            {
                                list.Invoke(new DelegateComm(() =>
                                    list.Items[i].SubItems[3].Text = a
                                    ));
                            }
                            );
                        new TaskGFW().Check(domain, a =>
                        {
                            list.Invoke(new DelegateComm(() =>
                                    list.Items[i].SubItems[4].Text = a
                                    ));
                        });
                    }
                    else
                    {
                        list.Invoke(new DelegateComm(() =>
                                  list.Items[i].SubItems[3].Text = "跳过"
                      ));

                        list.Invoke(new DelegateComm(() =>
                            list.Items[i].SubItems[4].Text = "跳过"));
                    }
                }
                else
                {
                    list.Invoke(new DelegateComm(() =>
                                    list.Items[i].SubItems[2].Text = "跳过"
                        ));
                    list.Invoke(new DelegateComm(() =>
                                    list.Items[i].SubItems[3].Text = "跳过"
                        ));
                    list.Invoke(new DelegateComm(() =>
                        list.Items[i].SubItems[4].Text = "跳过"));
                    list.Invoke(new DelegateComm(() =>
                        list.Items[i].SubItems[5].Text = "跳过"));
                    list.Invoke(new DelegateComm(() =>
                        list.Items[i].SubItems[6].Text = "跳过"));
                    list.Invoke(new DelegateComm(() =>
                        list.Items[i].SubItems[7].Text = "跳过"));
                }
                ////查whois
                //new TaskWhois(domain,
                //    a =>
                //    {
                //        list.Invoke(new DelegateComm(() =>
                //            list.Items[i].SubItems[3].Text = a
                //            ));
                //    }
                //);


                //int linkcount =0, linkdomaincount = 0;
                //bool isIndexFirst=false;
                //new TaskLinkCount(domain,
                //    (a,b,c) => //a = 外链，b=域名数，c=首页第一
                //    {
                //        list.Invoke(new DelegateComm(() =>
                //        {
                //            list.Items[i].SubItems[2].Text = a;
                //        }
                //        ));

                //    }
                //);
                //if (linkcount >= parms.LinkCount && linkdomaincount >= parms.LinkDomainCount && (!parms.IsIndexFirst || isIndexFirst))
                //{
                //    //查br
                //    new TaskBR(domain,
                //        a =>
                //        {
                //            list.Invoke(new DelegateComm(() =>
                //                list.Items[i].SubItems[4].Text = a
                //                ));
                //        }
                //    );
                //    //查备案
                //    new TaskWhoisBeiAn(domain, a =>
                //    {
                //        list.Invoke(new DelegateComm(()=>
                //                list.Items[i].SubItems[5].Text = a
                //            ));
                //    });
                //    //查whois
                //    new TaskWhois(domain,
                //        a =>
                //        {
                //            list.Invoke(new DelegateComm(() =>
                //                list.Items[i].SubItems[6].Text = a
                //                ));
                //        }
                //    );
                    
                //}
                //else {
                //    try
                //    {
                //        list.Invoke(new DelegateComm(() =>
                //                    list.Items[i].SubItems[4].Text = "跳过"
                //        ));
                //        list.Invoke(new DelegateComm(() =>
                //                    list.Items[i].SubItems[5].Text = "跳过"
                //        ));
                //        list.Invoke(new DelegateComm(() =>
                //                    list.Items[i].SubItems[6].Text = "跳过"
                //        ));
                //    }
                //    catch (Exception)
                //    { }
                //}
               
            }
        }
        public static void go(object parms)
        {
            for (int i = 0; i < TaskMain2.threadCount; i++)
            {
                var s = new Thread(new ParameterizedThreadStart(_go));

                s.Start(parms);
            }
        }


    }
}

