using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Library.SiteMap
{
    /// <summary>
    /// SiteMap Url生成接口
    /// </summary>
    public interface ISiteMapService
    {
        /// <summary>
        /// Url集合生成方法
        /// </summary>
        /// <param name="start">开始条数</param>
        /// <param name="end">结束条数</param>
        /// <param name="ismobile">是否移动端</param>
        /// <param name="uptime">更新时间</param>
        /// <returns></returns>
        List<URL> GetSiteMapList(int start, int end, bool ismobile = false, DateTime? uptime = null);
    }
}
