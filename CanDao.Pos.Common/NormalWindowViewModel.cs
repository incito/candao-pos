using System.Windows;
using System.Windows.Input;

namespace CanDao.Pos.Common
{
    public class NormalWindowViewModel : BaseViewModel
    {
        public NormalWindowViewModel()
        {
            InitCommand();
        }

        /// <summary>
        /// 所属窗口。
        /// </summary>
        public Window OwnerWindow { get; set; }

        /// <summary>
        /// 错误信息。
        /// </summary>
        private string _errMsg;
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string ErrMsg
        {
            get { return _errMsg; }
            set
            {
                _errMsg = value;
                RaisePropertyChanged("ErrMsg");
            }
        }

        /// <summary>
        /// 确定命令。
        /// </summary>
        public ICommand ConfirmCmd { get; private set; }

        /// <summary>
        /// 取消命令。
        /// </summary>
        public ICommand CancelCmd { get; private set; }

        /// <summary>
        /// 确定命令的执行方法。
        /// </summary>
        protected virtual void Confirm(object param)
        {
            CloseWindow(true);
        }

        /// <summary>
        /// 确定命令是否可用的判断方法。
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanConfirm(object param)
        {
            return true;
        }

        /// <summary>
        /// 取消命令的执行方法。
        /// </summary>
        protected virtual void Cancel(object param)
        {
            CloseWindow(false);
        }

        /// <summary>
        /// 取消命令是否可用的判断方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual bool CanCancel(object param)
        {
            return true;
        }

        /// <summary>
        /// 初始化命令。
        /// </summary>
        protected virtual void InitCommand()
        {
            ConfirmCmd = CreateDelegateCommand(Confirm, CanConfirm);
            CancelCmd = CreateDelegateCommand(Cancel, CanCancel);
        }

        /// <summary>
        /// 关闭窗口。
        /// </summary>
        /// <param name="result"></param>
        protected void CloseWindow(bool result)
        {
            if (OwnerWindow != null)
                OwnerWindow.DialogResult = result;

        }
    }
}