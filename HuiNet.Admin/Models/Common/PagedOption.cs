using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace HuiNet.Admin.Models.Common
{
    public class PagedOption
    {
        public PagedOption(IPagedList model, string targetId = "page-data", string pageIndexParameterName = "PageIndex")
        {
            Model = model;
            TargetId = targetId;
            PageIndexParameterName = pageIndexParameterName;
        }

        public string TargetId { get; set; }

        public string PageIndexParameterName { get; set; }

        public IPagedList Model { get; set; }
    }
}