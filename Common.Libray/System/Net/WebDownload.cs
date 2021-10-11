namespace System.Net
{
    /// <summary>
    /// 扩展WebClient类，加入超时时间(Timeout)设置
    /// </summary>
    public class WebDownload : WebClient
    {
        public int Timeout { get; set; }

        public WebDownload() : this(60000)
        {
        }

        public WebDownload(int timeout)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = Timeout;
            }
            return request;
        }
    }
}
