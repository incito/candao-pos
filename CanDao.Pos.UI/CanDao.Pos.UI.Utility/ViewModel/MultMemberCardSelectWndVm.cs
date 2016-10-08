using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 多会员卡选择窗口的Vm。
    /// </summary>
    public class MultMemberCardSelectWndVm : NormalWindowViewModel<MultMemberCardSelectWindow>
    {
        public MultMemberCardSelectWndVm(List<string> memberCardList, string defaultSelectCard)
        {
            MemberCards = new ObservableCollection<string>();
            if (memberCardList != null)
            {
                memberCardList.ForEach(MemberCards.Add);
                SelectedCard = !string.IsNullOrEmpty(defaultSelectCard)
                    ? defaultSelectCard
                    : MemberCards.FirstOrDefault();
            }
        }

        /// <summary>
        /// 会员卡集合。
        /// </summary>
        public ObservableCollection<string> MemberCards { get; set; }

        /// <summary>
        /// 选择的会员卡。
        /// </summary>
        public string SelectedCard { get; set; }
    }
}