//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

namespace DotNet.Utilities
{
    /// <summary>
    /// DbOperation
    /// 有关数据库操作的定义。
    /// 
    /// 修改纪录
    /// 
    ///		2012.02.04 版本：3.2 JiRiGaLa 修改为 DbOperation。
    ///		2007.07.30 版本：3.1 JiRiGaLa 增加 Truncate 方法。
    ///		2007.04.14 版本：3.0 JiRiGaLa 检查程序格式通过，不再进行修改主键操作。
    ///		2006.04.18 版本：2.0 JiRiGaLa 重新调整主键的规范化。
    ///		
    /// 版本：3.0
    /// 
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2007.04.14</date>
    /// </author> 
    /// </summary>
    public enum DbOperation
    {
        Select,
        Insert,
        Update,
        Delete,
        Truncate
    }
}