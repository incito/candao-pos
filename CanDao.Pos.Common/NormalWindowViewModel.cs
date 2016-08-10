using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CanDao.Pos.Common
{
    public class NormalWindowViewModel : BaseViewModel
    {
        public NormalWindowViewModel()
        {
            IsEnterKey2Confirm = true;
            InitCommand();
        }

        /// <summary>
        /// 所属窗口。
        /// </summary>
        public Window OwnerWindow { get; set; }

        /// <summary>
        /// 是否回车键代表执行确认命令。
        /// </summary>
        public bool IsEnterKey2Confirm { get; set; }

        /// <summary>
        /// 确定命令。
        /// </summary>
        public ICommand ConfirmCmd { get; private set; }

        /// <summary>
        /// 取消命令。
        /// </summary>
        public ICommand CancelCmd { get; private set; }

        /// <summary>
        /// 窗口加载事件命令。
        /// </summary>
        public ICommand WindowLoadCmd { get; private set; }

        /// <summary>
        /// 窗口关闭事件命令。
        /// </summary>
        public ICommand WindowClosedCmd { get; private set; }

        /// <summary>
        /// 窗口正在关闭事件命令。
        /// </summary>
        public ICommand WindowClosingCmd { get; private set; }

        /// <summary>
        /// 按键按下命令。
        /// </summary>
        public ICommand PreviewKeyDownCmd { get; private set; }

        /// <summary>
        /// 操作命令。
        /// </summary>
        public ICommand OperCmd { get; private set; }

        /// <summary>
        /// 分组命令。
        /// </summary>
        public ICommand GroupCmd { get; private set; }

        /// <summary>
        /// 初始化命令。
        /// </summary>
        protected virtual void InitCommand()
        {
            ConfirmCmd = CreateDelegateCommand(Confirm, CanConfirm);
            CancelCmd = CreateDelegateCommand(Cancel, CanCancel);
            WindowLoadCmd = CreateDelegateCommand(OnWindowLoaded);
            WindowClosedCmd = CreateDelegateCommand(OnWindowClosed);
            WindowClosingCmd = CreateDelegateCommand(WindowClosing);
            PreviewKeyDownCmd = CreateDelegateCommand(PreviewKeyDown);
            OperCmd = CreateDelegateCommand(OperMethod, CanOperMethod);
            GroupCmd = CreateDelegateCommand(GroupMethod, CanGroupMethod);
        }

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
        /// 窗体加载事件命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        protected virtual void OnWindowLoaded(object param)
        {

        }

        /// <summary>
        /// 按键按下事件命令的执行方法。
        /// </summary>
        /// <param name="arg">键盘按下事件参数。</param>
        protected virtual void OnPreviewKeyDown(KeyEventArgs arg)
        {
            if (arg.Key == Key.Enter && IsEnterKey2Confirm)
            {
                arg.Handled = true;
                ConfirmCmd.Execute(null);
            }
        }

        /// <summary>
        /// 键盘按下事件的执行方法。
        /// </summary>
        /// <param name="param">参数。</param>
        private void PreviewKeyDown(object param)
        {
            if (!(param is ExCommandParameter))
                return;

            var args = ((ExCommandParameter)param).EventArgs as KeyEventArgs;
            if (args == null)
                return;

            OnPreviewKeyDown(args);
        }

        /// <summary>
        /// 窗口关闭后的方法。
        /// </summary>
        /// <param name="param">参数。</param>
        protected virtual void OnWindowClosed(object param)
        {

        }

        /// <summary>
        /// 窗口正在关闭命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void WindowClosing(object param)
        {
            ExCommandParameter cmdParam = (ExCommandParameter)param;
            OnWindowClosing(((CancelEventArgs)cmdParam.EventArgs));
        }

        /// <summary>
        /// 窗口正在关闭时执行的方法。
        /// </summary>
        /// <param name="arg">取消事件参数。</param>
        protected virtual void OnWindowClosing(CancelEventArgs arg)
        {

        }

        /// <summary>
        /// 操作命令的可执行方法。
        /// </summary>
        /// <param name="param">参数。</param>
        protected virtual void OperMethod(object param)
        {

        }

        /// <summary>
        /// 操作命令是否可用的判断方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual bool CanOperMethod(object param)
        {
            return true;
        }

        /// <summary>
        /// 分组命令的执行方法。
        /// </summary>
        /// <param name="param">参数。</param>
        protected virtual void GroupMethod(object param)
        {

        }

        /// <summary>
        /// 分组命令是否可用的判断方法。
        /// </summary>
        /// <param name="param">参数。</param>
        /// <returns></returns>
        protected virtual bool CanGroupMethod(object param)
        {
            return true;
        }

        /// <summary>
        /// 关闭窗口。
        /// </summary>
        /// <param name="result"></param>
        protected void CloseWindow(bool result)
        {
            try
            {
                if (OwnerWindow != null)
                    OwnerWindow.DialogResult = result;
            }
            catch (Exception ex)
            {
                MessageDialog.Warning(ex.MyMessage());
            }
        }
    }
}