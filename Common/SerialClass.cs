using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Globalization;
namespace Common
{
    public class SerialClass
    {
        SerialPort _serialPort = null;
        //定义委托
        public delegate void SerialPortDataReceiveEventArgs(object sender, SerialDataReceivedEventArgs e, byte[] bits);
        //定义接收数据事件
        public event SerialPortDataReceiveEventArgs DataReceived;
        //定义接收错误事件
        //public event SerialErrorReceivedEventHandler Error;
        //接收事件是否有效 false表示有效
        public bool ReceiveEventFlag = false;
        #region 获取串口名
        private string protName;
        public string PortName
        {
            get { return _serialPort.PortName; }
            set
            {
                _serialPort.PortName = value;
                protName = value;
            }
        }
        #endregion
        #region 获取比特率
        private int baudRate;
        public int BaudRate
        {
            get { return _serialPort.BaudRate; }
            set
            {
                _serialPort.BaudRate = value;
                baudRate = value;
            }
        }
        #endregion
        #region 默认构造函数
        /// &lt;summary&gt;
        /// 默认构造函数，操作COM1，速度为9600，没有奇偶校验，8位字节，停止位为1 &quot;COM1&quot;, 9600, Parity.None, 8, StopBits.One
        /// &lt;/summary&gt;
        public SerialClass()
        {
            _serialPort = new SerialPort();
        }
        #endregion
        #region 构造函数
        /// &lt;summary&gt;
        /// 构造函数,
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;comPortName&quot;&gt;&lt;/param&gt;
        public SerialClass(string comPortName)
        {
            _serialPort = new SerialPort(comPortName);
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.Even;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.RtsEnable = true;
            _serialPort.ReadTimeout = 2000;
            setSerialPort();
        }
        #endregion
        #region 构造函数,可以自定义串口的初始化参数
        /// &lt;summary&gt;
        /// 构造函数,可以自定义串口的初始化参数
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;comPortName&quot;&gt;需要操作的COM口名称&lt;/param&gt;
        /// &lt;param name=&quot;baudRate&quot;&gt;COM的速度&lt;/param&gt;
        /// &lt;param name=&quot;parity&quot;&gt;奇偶校验位&lt;/param&gt;
        /// &lt;param name=&quot;dataBits&quot;&gt;数据长度&lt;/param&gt;
        /// &lt;param name=&quot;stopBits&quot;&gt;停止位&lt;/param&gt;
        public SerialClass(string comPortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort = new SerialPort(comPortName, baudRate, parity, dataBits, stopBits);
            _serialPort.RtsEnable = true;  //自动请求
            _serialPort.ReadTimeout = 3000;//超时
            setSerialPort();
        }
        #endregion
        #region 析构函数
        /// &lt;summary&gt;
        /// 析构函数，关闭串口
        /// &lt;/summary&gt;
        ~SerialClass()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }
        #endregion
        #region 设置串口参数
        /// &lt;summary&gt;
        /// 设置串口参数
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;comPortName&quot;&gt;需要操作的COM口名称&lt;/param&gt;
        /// &lt;param name=&quot;baudRate&quot;&gt;COM的速度&lt;/param&gt;
        /// &lt;param name=&quot;dataBits&quot;&gt;数据长度&lt;/param&gt;
        /// &lt;param name=&quot;stopBits&quot;&gt;停止位&lt;/param&gt;
        public void setSerialPort(string comPortName, int baudRate, int dataBits, int stopBits)
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
            _serialPort.PortName = comPortName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = (StopBits)stopBits;
            _serialPort.Handshake = Handshake.None;
            _serialPort.RtsEnable = false;
            _serialPort.ReadTimeout = 3000;
            _serialPort.NewLine = "/r/n";
            setSerialPort();
        }
        #endregion
        #region 设置接收函数
        /// &lt;summary&gt;
        /// 设置串口资源,还需重载多个设置串口的函数
        /// &lt;/summary&gt;
        void setSerialPort()
        {
            if (_serialPort != null)
            {
                //设置触发DataReceived事件的字节数为1
                _serialPort.ReceivedBytesThreshold = 1;
                //接收到一个字节时，也会触发DataReceived事件
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                //接收数据出错,触发事件
                _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(_serialPort_ErrorReceived);
                //打开串口
                //openPort();
            }
        }
        #endregion
        #region 打开串口资源
        /// &lt;summary&gt;
        /// 打开串口资源
        /// &lt;returns&gt;返回bool类型&lt;/returns&gt;
        /// &lt;/summary&gt;
        public bool openPort()
        {
            bool ok = false;
            //如果串口是打开的，先关闭
            if (_serialPort.IsOpen)
                _serialPort.Close();
            try
            {
                //打开串口
                _serialPort.Open();
                ok = true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ok;
        }
        #endregion
        #region 关闭串口
        /// &lt;summary&gt;
        /// 关闭串口资源,操作完成后,一定要关闭串口
        /// &lt;/summary&gt;
        public void closePort()
        {
            //如果串口处于打开状态,则关闭
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }
        #endregion
        #region 接收串口数据事件
        /// &lt;summary&gt;
        /// 接收串口数据事件
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;sender&quot;&gt;&lt;/param&gt;
        /// &lt;param name=&quot;e&quot;&gt;&lt;/param&gt;
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            try
            {
                System.Threading.Thread.Sleep(20);
                byte[] _data = new byte[_serialPort.BytesToRead];
                _serialPort.Read(_data, 0, _data.Length);
                if (_data.Length == 0) { return; }
                if (DataReceived != null)
                {
                    DataReceived(sender, e, _data);
                }
                //_serialPort.DiscardInBuffer();  //清空接收缓冲区   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 接收数据出错事件
        /// &lt;summary&gt;
        /// 接收数据出错事件
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;sender&quot;&gt;&lt;/param&gt;
        /// &lt;param name=&quot;e&quot;&gt;&lt;/param&gt;
        void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
        }
        #endregion
        #region 发送数据string类型
        public void SendData(string data)
        {
            //发送数据
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(data);
            }
        }
        #endregion
        #region 发送数据byte类型
        /// &lt;summary&gt;
        /// 数据发送
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;data&quot;&gt;要发送的数据字节&lt;/param&gt;
        public void SendData(byte[] data, int offset, int count)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            try
            {
                if (_serialPort.IsOpen)
                {
                    //_serialPort.DiscardInBuffer();//清空接收缓冲区
                    _serialPort.Write(data, offset, count);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 发送命令
        /// &lt;summary&gt;
        /// 发送命令
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;SendData&quot;&gt;发送数据&lt;/param&gt;
        /// &lt;param name=&quot;ReceiveData&quot;&gt;接收数据&lt;/param&gt;
        /// &lt;param name=&quot;Overtime&quot;&gt;超时时间&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public int SendCommand(byte[] SendData, ref  byte[] ReceiveData, int Overtime)
        {

            if (_serialPort.IsOpen)
            {
                try
                {
                    ReceiveEventFlag = true;        //关闭接收事件 
                    _serialPort.DiscardInBuffer();  //清空接收缓冲区                
                    _serialPort.Write(SendData, 0, SendData.Length);
                    int num = 0, ret = 0;
                    System.Threading.Thread.Sleep(10);
                    ReceiveEventFlag = false;      //打开事件
                    while (num++ < Overtime)
                    {
                        if (_serialPort.BytesToRead >= ReceiveData.Length)
                            break;
                        System.Threading.Thread.Sleep(10);
                    }

                    if (_serialPort.BytesToRead >= ReceiveData.Length)
                    {
                        ret = _serialPort.Read(ReceiveData, 0, ReceiveData.Length);
                    }
                    else
                    { 
                        ret = _serialPort.Read(ReceiveData, 0, _serialPort.BytesToRead);
                    }
                    ReceiveEventFlag = false;      //打开事件 
                    return ret;
                }
                catch (Exception ex)
                {
                    ReceiveEventFlag = false;
                    throw ex;
                }
            }
            return -1;
        }
        #endregion
        #region 获取串口
        /// &lt;summary&gt;
        /// 获取所有已连接短信猫设备的串口
        /// &lt;/summary&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public string[] serialsIsConnected()
        {
             List<string> lists = new List<string>();
            string[] seriallist = getSerials();
            foreach (string s in seriallist)
            {
            }
            return lists.ToArray();
        }
        #endregion
        #region 获取当前全部串口资源
        /// &lt;summary&gt;
        /// 获得当前电脑上的所有串口资源
        /// &lt;/summary&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public string[] getSerials()
        {
            return SerialPort.GetPortNames();
        }
        #endregion
        #region 字节型转换16
        /// &lt;summary&gt;
        /// 把字节型转换成十六进制字符串
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;InBytes&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InByte);
            }
            return StringOut;
        }
        #endregion
        #region 十六进制字符串转字节型
        /// &lt;summary&gt;
        /// 把十六进制字符串转换成字节型(方法1)
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;InString&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(" ".ToCharArray());
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length-1 ; i++)
            {
                //ByteOut[i] = System.Text.Encoding.ASCII.GetBytes(ByteStrings[i]);
                ByteOut[i] = Byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);
                //ByteOut[i] =Convert.ToByte("0x" + ByteStrings[i]);
            }
            return ByteOut;
        }
        #endregion
        #region 十六进制字符串转字节型
        /// &lt;summary&gt;
        /// 字符串转16进制字节数组(方法2)
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;hexString&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion
        #region 字节型转十六进制字符串
        /// &lt;summary&gt;
        /// 字节数组转16进制字符串
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;bytes&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion
    }
}

/*
static SerialClass sc = new SerialClass();
static void Main(string[] Args)
{
   sc.DataReceived += new SerialClass.SerialPortDataReceiveEventArgs(sc_DataReceived);
   sc.writeData("at");
   Console.ReadLine();
   sc.closePort();
   }
static void sc_DataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits)
{
   Console.WriteLine(Encoding.Default.GetString(bits));

}

*/