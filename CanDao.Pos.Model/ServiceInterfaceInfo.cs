namespace CanDao.Pos.Model
{
    public class ServiceInterfaceInfo
    {
        /// <summary>
        /// 接口名。
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 实现接口的Dll名。
        /// </summary>
        public string DllName { get; private set; }
        /// <summary>
        /// 实现接口的类名。
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// 实例化接口信息。
        /// </summary>
        /// <param name="name">接口名。</param>
        /// <param name="dllName">实现接口的Dll名。</param>
        /// <param name="className">实现接口的类名。</param>
        public ServiceInterfaceInfo(string name, string dllName, string className)
        {
            Name = name;
            DllName = dllName;
            ClassName = className;
        }
    }
}