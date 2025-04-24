using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}