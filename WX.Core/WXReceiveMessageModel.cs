using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Core
{

    /// <summary>
    /// 接收微信消息实体对象
    /// </summary>
    public class WXReceiveMessageModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 事件key
        /// </summary>
        public string EventKey { get; set; }
    }
}
