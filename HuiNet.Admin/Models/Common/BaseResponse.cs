using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Models.Common
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        public IDictionary<string, object> Data { get; set; }

        public void SetSuccess(string message = null)
        {
            this.IsSuccess = true;
            this.Message = message;
        }

        public void SetFail(string message)
        {
            this.IsSuccess = false;
            this.Message = message;
        }

        public void SetData(string key, object data)
        {
            if (Data == null)
                Data = new Dictionary<string, object>();

            if (!Data.ContainsKey(key))
            {
                Data.Add(key, data);
            }
            else
            {
                Data[key] = data;
            }
        }
    }
}