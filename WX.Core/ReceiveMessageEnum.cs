using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Core
{

    /// <summary>
    /// 接收微信消息枚举
    /// </summary>
    public enum ReceiveMessageTypeEnum
    {
        Text,
        Image,
        Voice,
        Video,
        ShortVideo,
        Location,
        Link,
        Event
    }
}
