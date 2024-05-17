using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Wrappers
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string[] Errors { get; set; }
        public string Message { get; set; } = string.Empty;

        public Response(): this(default(T))
        {

        }

        public Response(T data)
        {
            Data = data;
        }
    }
}
