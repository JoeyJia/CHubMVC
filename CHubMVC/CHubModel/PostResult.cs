using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class RequestResult
    {
        public RequestResult()
        {
            this.Success = true;
        }

        public RequestResult(bool success, string msg=null,object data=null)
        {
            Success = success;
            Msg = msg;
            Data = data;
        }
        public RequestResult(bool success)
        {
            Success = success;
            if (success)
                Msg = "Success";
            else
                Msg = "Fail";
            Data = null;
        }

        public RequestResult( object data)
        {
            Success = true;
            Msg = null;
            Data = data;
        }

        public bool Success { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }
}
