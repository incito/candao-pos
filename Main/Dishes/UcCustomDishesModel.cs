using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Classes.Mvvms;

namespace KYPOS.Dishes
{
   public class UcCustomDishesModel:ViewModelBase
   {
       private string _dishesName;
       private string _price;
       private string _dishesCount;

       /// <summary>
       /// 菜名
       /// </summary>
       public string DishesName
       {
           set
           {
               _dishesName = value;
               RaisePropertyChanged(() => DishesName);
           }
            get { return _dishesName; }
       }

       /// <summary>
       /// 价格
       /// </summary>
       public string Price
       {
           set
           {
               _price = value;
               RaisePropertyChanged(() => Price);
           }
           get { return _price; }
       }

       /// <summary>
       /// 数量
       /// </summary>
       public string DishesCount
       {
           set
           {
               _dishesCount = value;
               RaisePropertyChanged(() => DishesCount);
           }
           get { return _dishesCount; }
       }
   }
}
