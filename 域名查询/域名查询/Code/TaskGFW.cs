using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace 域名查询.Code
{

    public class GfwIpAddress
    {
        /// <summary>
        /// 调用它实现static的构造函数调用
        /// </summary>
        public static bool Open;
        public class IpAndTime
        {
            public IPAddress Ip;
            public DateTime BlockTime;
        }
        /// <summary>
        /// GFW屏蔽时间
        /// </summary>
        private const int BlockMinute = 10;
        private const string Hosts = "www.Live.com,Linkedin.com,resolutioncenter.ebay.com,login.live.com,announcements.ebay.com,help.linkedin.com,business.linkedin.com,www.Ebay.com,deals.ebay.com,pages.ebay.com,ocsnext.ebay.com,www.alexa.com";
        /// <summary>
        /// 启动时加载IP
        /// </summary>
        static GfwIpAddress()
        {
            new Thread(new ThreadStart(Load)).Start();
        }
        /// <summary>
        /// 加载IP
        /// </summary>
        private static void Load()
        {
            var ips = Hosts.Split(',')
                .SelectMany(GetHostAddresses)
                .GroupBy(a => a.ToString())
                .Select(ip => new IpAndTime{ Ip= ip.FirstOrDefault(), BlockTime= DateTime.MinValue});
            IpPool.AddRange(ips);
        }
        /// <summary>
        /// IP池
        /// </summary>
        private static readonly List<IpAndTime> IpPool = new List<IpAndTime>();
        /// <summary>
        /// 获取一个可用IP，被屏蔽2分钟后
        /// </summary>
        /// <returns></returns>
        public static IpAndTime GetIp()
        {
            lock (IpPool)
            {

                var r = IpPool.OrderBy(a => System.Guid.NewGuid()).FirstOrDefault(a => a.BlockTime < DateTime.Now.AddSeconds(-BlockMinute));
                if (r!=null) SetIpInUse(r);
                return r;
            }
        }

        /// <summary>
        /// ip被屏蔽或不可用
        /// </summary>
        /// <param name="ips"></param>
        public static void SetIpInUse(IpAndTime ips)
        {
            ips.BlockTime = DateTime.Now;
        }
        /// <summary>
        /// ip被可用
        /// </summary>
        /// <param name="ips"></param>
        public static void SetIpCanUse(IpAndTime ips)
        {
            ips.BlockTime = DateTime.MinValue;
        }
        private static IPAddress[] GetHostAddresses(string domain)
        {
            var r = new IPAddress[] { };
            try
            {
                r = Dns.GetHostAddresses(domain);
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Dns.GetHostAddresses错误({0}):{1}", domain, e.Message));
            }
            return r;
        }
    }


    /// <summary>
    /// 百度外链
    /// </summary>
    public class TaskGFW
    {
        


        public void Check(string domain, Action<string> ondata,int retry=0)
        {
            var ip = GfwIpAddress.GetIp();
            if (ip == null)
            {
                ondata(string.Format("等待IP池{0}",retry+1));
                Thread.Sleep(1000);
                Check(domain, ondata, ++retry);
                return;
            };
            IPEndPoint ipEnd = new IPEndPoint(ip.Ip, 80);
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.SendTimeout = 20;
                socket.ReceiveTimeout = 20;
                try
                {
                    socket.Connect(ipEnd);
                }
                catch (SocketException e)
                {
                    ondata(string.Format("err:{0}", e.Message));
                    Thread.Sleep(1000);
                    Check(domain, ondata, ++retry);
                    return;
                }
                try
                {
                    StringBuilder buf = new StringBuilder();
                    buf.Append("GET ").Append("http://www.").Append(domain).Append("/").Append(" HTTP/1.1\r\n");
                    buf.Append("Accept: text/html, application/xhtml+xml, */*\r\n");
                    buf.Append("Accept-Language: zh-CN\r\n");
                    buf.Append("User-Agent: ").Append(UserAgent.Get()).Append("\r\n");
                    buf.Append("Accept-Encoding: gzip, deflate").Append("\r\n");
                    buf.Append("Host: www.").Append(domain).Append("\r\n");
                    buf.Append("Connection: Keep-Alive").Append("\r\n");
                    buf.Append("\r\n");

                    byte[] ms = System.Text.Encoding.UTF8.GetBytes(buf.ToString());
                    //发送
                    socket.Send(ms);
                    int recv = 0;
                    byte[] data = new byte[1];
                    //int length = socket.Receive(data);
                    var x = Encoding.Default.GetString(data, 0, 1);
                    ondata("正常");
                    GfwIpAddress.SetIpCanUse(ip); //此ip可用
                }
                catch (Exception e)
                {
                    GfwIpAddress.SetIpInUse(ip); //此ip不可用或屏蔽

                    if (e.Message.IndexOf("强迫", StringComparison.Ordinal) > -1 || e.Message.IndexOf("关闭", StringComparison.Ordinal) > -1 ||
                        e.Message.IndexOf("reset", StringComparison.OrdinalIgnoreCase) > -1 ||
                        e.Message.IndexOf("abort", StringComparison.OrdinalIgnoreCase) > -1)

                        ondata("被墙");

                    else
                    {
                        ondata(string.Format("err:{0}", e.Message));
                        Thread.Sleep(2000);
                        Check(domain, ondata, ++retry);
                    }
                    
                    
                }
                finally
                {
                    //禁用上次的发送和接受
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
            }

        }


    }
}
