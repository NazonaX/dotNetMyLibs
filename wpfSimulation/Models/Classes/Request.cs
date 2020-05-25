using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Classes
{
    public class Request
    {
        public enum RequestType { SET_IN, TAKE_OUT}

        //differentiate the request from set in and take out
        public RequestType Type { get; set; }
        //-->for store request, the target goods will be set at once
        //and then be send into Storage Manager to slice into some other child requests
        //-->for take out request, the target goods will not be set at once
        //and then the requseted order name/ good name/ good specification will be send into Storage Manager
        //to ensure the storages which will be taken out and to slice into some other child requests
        public Goods TargetGoods { get; set; }
        public Position TargetPosition { get; set; }

    }
}
