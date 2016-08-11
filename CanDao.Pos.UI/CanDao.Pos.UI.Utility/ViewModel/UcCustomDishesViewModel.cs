
using System.Windows;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.UI.Utility.Model;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.Common;


namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 自定义菜品编辑
    /// </summary>
    public class UcCustomDishesViewModel : ViewModelBase
    {
        #region 字段

        private Window _myVieWindow;

        #endregion

        #region 属性

        public UcCustomDishesModel Model { set; get; }


        public RelayCommand SureCommand { get; set; }

        public RelayCommand CloseCommand { get; set; }

        #endregion

        #region 构造函数

        public UcCustomDishesViewModel()
        {
            Model = new UcCustomDishesModel();
            SureCommand=new RelayCommand(SureHandel);
            CloseCommand=new RelayCommand(CloseHandel);
        }

        #endregion

        #region 公共方法

        public Window GetWindow()
        {
            _myVieWindow = new UcCustomDishesView();
            _myVieWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _myVieWindow.DataContext = this;
            return _myVieWindow;
        }

        #endregion

        #region 私有方法

        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(this.Model.DishesName))
            {
                MessageDialog.Warning("菜名不能为空，请检查！");
                return false;
            }
            if (string.IsNullOrEmpty(this.Model.Price))
            {
                MessageDialog.Warning("价格（元）不能为空，请检查！");
                return false;
            }
            if (string.IsNullOrEmpty(this.Model.DishesCount))
            {
                MessageDialog.Warning("数量（份）不能为空，请检查！");
                return false;
            }
            decimal outDecimal = 0;
            if (!decimal.TryParse(this.Model.Price, out outDecimal))
            {
                MessageDialog.Warning("价格（元）输入格式不正确，请检查！");
                return false; 
            }
            int outTem = 0;
            if (int.TryParse(this.Model.DishesCount, out outTem))
            {
                if (outTem == 0)
                {
                    MessageDialog.Warning("数量（份）不能为[0]，请检查！");
                    return false;
                }
            }
            else
            {
                MessageDialog.Warning("数量（份）输入格式不正确，请检查！");
                return false; 
            }
        
            return true;
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void SureHandel()
        {
            if (_myVieWindow != null)
            {
                if (CheckInput())
                {
                    _myVieWindow.DialogResult = true; 
                }
               
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void CloseHandel()
        {
            if (_myVieWindow != null)
            {
                _myVieWindow.DialogResult = false;
            }
        }

        #endregion

    }
}
