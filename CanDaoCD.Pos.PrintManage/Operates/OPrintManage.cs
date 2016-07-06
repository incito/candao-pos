using CanDaoCD.Pos.PrintManage.Drives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CanDaoCD.Pos.PrintManage.Structs;
using PrintKs=CanDaoCD.Pos.PrintManage.Drives.DPrintTRW;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.Common.PublicValues;

namespace CanDaoCD.Pos.PrintManage.Operates
{
    /// <summary>
    /// 可视打印(TRW型号)管理类
    /// </summary>
    public class OPrintManage : IPrintServer
    {
        #region 字段
        private int _port;

        private int _bandRate;

        //弹卡模式（0:不完全弹卡;1:完全弹卡）
        private int _popupMode;

        //最大超时时间
        private int _timeOut;

        public bool _isInit;
        #endregion

        #region 属性
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorString
        {
            get;
            set;
        }

        /// <summary>
        /// 状态返回值
        /// </summary>

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)] public byte[] ResState;
        #endregion

        #region 构造函数
        public OPrintManage()
        {
            _isInit = false;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool TestConnect(int port)
        {
            int res = 0;
            _bandRate = 9600;
            _port = port;
            try
            {
                res = PrintKs.OpenCom(_port, 8, 1, 0, _bandRate);
                if (res == 0)
                {
                    Release();
                    return true;
                }
                else
                {
                    ErrorString = OInfoManage.GetErrorInfo(res);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorString = "打印组件错误，请联系管理员："+ex.Message;
                return false;
            }
          
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="port"></param>
        /// <param name="bandRate"></param>
        /// <param name="popupMode"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool Init(int port, int popupMode = 1, int timeOut = 30)
        {
            try
            {
              
                if (!_isInit)
                {
                    _port = port;
                    _bandRate = 9600;
                    _popupMode = popupMode;
                    _timeOut = timeOut;
                    if (PrintKs.OpenCom(_port, 8, 1, 0, _bandRate) == 0)
                    {
                        _isInit = true;
                        return true;
                    }
                    else
                    {
                        //ErrorString = "打印机连接错误，请检查打印机是否连接正常！";
                        //OWindowManage.ShowMessageWindow(
                        //          string.Format("{0}", ErrorString), false);
                        _isInit = false;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                _isInit = false;
                return false;
            }
        }

        /// <summary>
        /// 多次连接
        /// </summary>
        /// <returns></returns>
        private bool ConnectPrint()
        {
            int testCount = 3;//重连3次

            while(testCount>0)
            {
                try
                {
                    if (PrintKs.OpenCom(_port, 8, 1, 0, _bandRate) == 0)
                    {
                        //连接成功
                        _isInit = true;
                        return true;
                    }
                }
                catch
                {

                }
                finally
                {
                    testCount--;
                }
            }

            //连接失败
            _isInit = false;
            ErrorString = "打印机连接错误，请检查打印机是否连接正常！";
            return false;
        }

        /// <summary>
        /// 释放组件
        /// </summary>
        /// <returns></returns>
        public bool Release()
        {
            try
            {
                if (!_isInit)
                {
                    PrintKs.CloseCom(_port);//文档提示总是返回"0"，不用判断
                    return true;
                }
                return true;
            }
            catch(Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 进卡
        /// </summary>
        /// <returns></returns>
        public bool CardInOver()
        {
            try
            {
                if (_isInit)
                {
                    if (CardState())
                    {
                        return true;
                    }

                    int state=0;
                    var res = DPrintTRW.CardRearSide(ref state, _timeOut);
                    if (res != 0)
                    {
                        ErrorString = OInfoManage.GetErrorInfo(res);
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
              
            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 擦除弹出
        /// </summary>
        /// <returns></returns>
        public bool EraseDischarge()
        {
            try
            {
                if (!_isInit)
                {
                    int statu = 0;
                    var res = DPrintTRW.EraseDischarge(_popupMode, ref statu);
                    if (res != 0)
                    {
                        ErrorString = OInfoManage.GetErrorString(res);
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 判断卡是否已插入
        /// </summary>
        /// <returns></returns>
        public bool CardState()
        {
            try
            {
                if (_isInit)
                {
                    var statuRes = new StatuRes();
                    int status = 0;
                    var res = DPrintTRW.RequestStatus(ref statuRes, ref status);
                    if (res !=6)
                    {
                        ErrorString = OInfoManage.GetErrorString(res);

                        if (ErrorString.Equals("Null"))//连接失败
                        {
                            if (ConnectPrint())
                            {
                                return CardState();
                            }
                            else
                            {
                                return false;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        var cardInfo = Encoding.Default.GetString(statuRes.ResBytes);
                        if (cardInfo.Remove(cardInfo.Length - 2).Contains("1") || cardInfo.Remove(cardInfo.Length - 2).Contains("2"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                   if(ConnectPrint())
                   {
                      return CardState();
                   }
                   else
                   {
                       return false;
                   }
                }

            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 设置打印模板
        /// </summary>
        /// <param name="templateList"></param>
        /// <returns></returns>
        public bool SetPrintTemplate(List<string> templateList)
        {
            try
            {
                if (_isInit)
                {
                    int setY = 30;
                    int setX = 10;
                    int statu = 0;
                    foreach (var template in templateList)
                    {
                        var strbuffer = new StringBuilder(1024);
                        strbuffer.Append(template);
                        int res = DPrintTRW.SettingPrintChar(0, setX, setY, strbuffer, ref statu);
                        if (res == 0)
                        {
                            setY += 32;
                        }
                        else
                        {
                            ErrorString = string.Format("错误信息：{0}-详细：{1} 模板设置错误！", OInfoManage.GetErrorString(res), template);
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="templateList"></param>
        /// <returns></returns>
        public bool Print(List<string> templateList)
        {
            try
            {

                if (_isInit)
                {
                    int statu = 0;
                    int res = 0;

                    res = DPrintTRW.ClrPrtExpBuf(ref statu);
                    //if (res != 0)
                    //{
                    //    ErrorString = "打印失败(清除内存）:" + OInfoManage.GetErrorInfo(res);
                    //    return false;
                    //}
                    //res = DPrintTRW.EraseDischarge(0, ref statu);
                    //if (res != 0)
                    //{
                    //    ErrorString = "打印失败(清除信息）:" + OInfoManage.GetErrorString(res);
                    //    return false;
                    //}
                    if (!SetPrintTemplate(templateList)) //设置模板
                    {
                        return false;
                    }

                    res = DPrintTRW.ErasePrintDischarge300(1, ref statu); //打印
                    if (res != 0)
                    {
                        ErrorString = "打印失败:" + OInfoManage.GetErrorString(res);
                        return false;
                    }
                    return true;
                }
                else
                {
                    ErrorString = "打印组件丢失！";
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }

        public bool DischargeCard()
        {
            try
            {
                if (!_isInit)
                {
                    int state = 0;
                    var res = DPrintTRW.DischargeCard(1,ref state, _timeOut);
                    if (res != 0)
                    {
                        ErrorString = OInfoManage.GetErrorInfo(res);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
                return false;
            }
        }
        #endregion

     
    }
}
