using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement
{
    public class User
    {
        public int? Code { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public UserType? UserType { get; set; }
        public string? DeleteTime { get; set; }
    }

    /// <summary>
    /// 삭제할 수 없는 기존 10명의 사용자와
    /// 삭제할 수 있는 직접 추가한 사용자를 구분
    /// </summary>
    public enum UserType
    {
        //고정 10명의 사용자
        [Description("defaultUser")]
        기본사용자,

        //직접 추가한 사용자
        [Description("addUser")]
        신규사용자
    }
}
