namespace Common.Library.Plugin
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件名称（插件目录名）
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 区域名称
        /// </summary>
        string AreaName { get; }
        /// <summary>
        /// 插件初始化方法
        /// </summary>
        void Initialize();
        /// <summary>
        /// 插件卸载方法
        /// </summary>
        void Unload();
    }

    /// <summary>
    /// 模块标识
    /// </summary>
    public interface IModule : IPlugin {}

    /// <summary>
    /// 站点标识
    /// </summary>
    public interface ISite : IPlugin {}
}
