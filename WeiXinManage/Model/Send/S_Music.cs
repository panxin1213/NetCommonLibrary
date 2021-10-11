using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 音乐信息
    /// </summary>
    public class S_Music : SendBase
    {
        public S_Music()
        {
            base.MsgType = "music";
        }

        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicURL { get; set; }

        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }

        /// <summary>
        /// 缩略图的媒体id，通过上传多媒体文件，得到的id
        /// </summary>
        public string ThumbMediaId { get; set; }
    }
}
