using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    public class MenuSingleAllDishDataResponse : MenuSingleDishDataResponse
    {

        public List<MenuSingleDishDataResponse> dishes { get; set; }

    }
}