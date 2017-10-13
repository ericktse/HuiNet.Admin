using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace HuiNet.Admin.Models.Common
{
    public class PageModel<TSource, TTarget>
       where TSource : PagedInfo
       where TTarget : class
    {
        public PageModel(TSource condition, PagedList<TTarget> pagedList)
        {
            SearchCondition = condition;
            PagedList = pagedList;
        }

        /// <summary>
        /// 查询条件 
        /// </summary>
        public TSource SearchCondition { get; set; }

        /// <summary>
        /// 页面数据
        /// </summary>
        public PagedList<TTarget> PagedList { get; set; }
    }
}