using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UserManagement
{
    /// <summary>
    /// 업무하며 내부 DB로 쭉 사용해왔던 SQLite를 사용하였습니다.
    /// </summary>
    public class SqLiteClass
    {
        private static string _strConn = @"Data Source=UserData.dat";

        private static List<User> _userList;

        public static void CreateSqLite()
        {
            string DbFile = "UserData.dat";
            //string ConnectionString = string.Format("Data Source={0};Version=3;", DbFile);

            try
            {
                // UserData 파일이 없다면
                // 파일생성 및 테이블 생성
                if (!System.IO.File.Exists(DbFile))
                {
                    SQLiteConnection.CreateFile(DbFile);
                }
                else
                {
                    return;
                }

                SQLiteConnection sqliteConn = new SQLiteConnection(_strConn);
                sqliteConn.Open();
                SQLiteCommand cmd;

                string sql = string.Empty;

                #region 테이블 생성
                try
                {
                    //시재 테이블
                    sql = "create table user_list(code integer primary key autoincrement, name text, age text, phoneNumber text, type text, deleteTime text)";
                    cmd = new SQLiteCommand(sql, sqliteConn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //Exception 로그 등 처리
                    //로그남기는 로직은 구현하지 않고 로그만 찍었습니다.
                    string excptionMsg = string.Empty;
                    excptionMsg += "!!! Exception !!!\r\n";
                    excptionMsg += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]";
                    excptionMsg += "Function Name :: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    excptionMsg += "Exception Type :: " + ex.GetType().Name;
                    excptionMsg += "Stack Trace :: " + ex.StackTrace;
                    excptionMsg += "Exception Message :: " + ex.Message;
                    MessageBox.Show(excptionMsg);
                }
                #endregion

                sqliteConn.Close();

                InsertBaseUser();
            }
            catch (Exception ex)
            {
                //Exception 로그 등 처리
                //로그남기는 로직은 구현하지 않고 로그만 찍었습니다.
                string excptionMsg = string.Empty;
                excptionMsg += "!!! Exception !!!\r\n";
                excptionMsg += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]";
                excptionMsg += "Function Name :: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                excptionMsg += "Exception Type :: " + ex.GetType().Name;
                excptionMsg += "Stack Trace :: " + ex.StackTrace;
                excptionMsg += "Exception Message :: " + ex.Message;
                MessageBox.Show(excptionMsg);
            }
        }


        #region 삭제할 수 없는 사용자 10명 추가
        /// <summary>10명의 사용자 생성</summary>
        public static void InsertBaseUser()
        {
            try
            {
                List<User> userList = new List<User>()
                {
                    new User {Name = "사용자1", Age = "21", UserType = UserType.기본사용자, PhoneNumber = "01011111111"},
                    new User {Name = "사용자2", Age = "22", UserType = UserType.기본사용자, PhoneNumber = "01022222222"},
                    new User {Name = "사용자3", Age = "23", UserType = UserType.기본사용자, PhoneNumber = "01033333333"},
                    new User {Name = "사용자4", Age = "24", UserType = UserType.기본사용자, PhoneNumber = "01044444444"},
                    new User {Name = "사용자5", Age = "25", UserType = UserType.기본사용자, PhoneNumber = "01055555555"},
                    new User {Name = "사용자6", Age = "26", UserType = UserType.기본사용자, PhoneNumber = "01066666666"},
                    new User {Name = "사용자7", Age = "27", UserType = UserType.기본사용자, PhoneNumber = "01077777777"},
                    new User {Name = "사용자8", Age = "28", UserType = UserType.기본사용자, PhoneNumber = "01088888888"},
                    new User {Name = "사용자9", Age = "29", UserType = UserType.기본사용자, PhoneNumber = "01099999999"},
                    new User {Name = "사용자10", Age = "30", UserType = UserType.기본사용자, PhoneNumber = "01000000000"}
                };

                using (SQLiteConnection conn = new SQLiteConnection(_strConn))
                {
                    conn.Open();

                    foreach(User user in userList)
                    {
                        string sql = "insert into user_list (name, age, phoneNumber, type) values ('" + user .Name + "', '" + user .Age + "', '" + user .PhoneNumber + "', '" + user.UserType + "')";
                        Console.WriteLine(sql);
                        SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //Exception 로그 등 처리
                //로그남기는 로직은 구현하지 않고 로그만 찍었습니다.
                string excptionMsg = string.Empty;
                excptionMsg += "!!! Exception !!!\r\n";
                excptionMsg += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]";
                excptionMsg += "Function Name :: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                excptionMsg += "Exception Type :: " + ex.GetType().Name;
                excptionMsg += "Stack Trace :: " + ex.StackTrace;
                excptionMsg += "Exception Message :: " + ex.Message;
                MessageBox.Show(excptionMsg);
            }
        }
        #endregion

        #region 입력한 사용자 추가
        public static void InsertUser(User user)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_strConn))
                {
                    conn.Open();
                    string sql = "insert into user_list (name, age, phoneNumber, type) values ('" + user.Name + "', '" + user.Age + "', '" + user.PhoneNumber + "', '" + App.GetDescriptionOfEnum(user.UserType!) + "')";
                    Console.WriteLine(sql);
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //Exception 로그 등 처리
                //로그남기는 로직은 구현하지 않고 로그만 찍었습니다.
                string excptionMsg = string.Empty;
                excptionMsg += "!!! Exception !!!\r\n";
                excptionMsg += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]";
                excptionMsg += "Function Name :: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                excptionMsg += "Exception Type :: " + ex.GetType().Name;
                excptionMsg += "Stack Trace :: " + ex.StackTrace;
                excptionMsg += "Exception Message :: " + ex.Message;
                MessageBox.Show(excptionMsg);
            }
        }
        #endregion

        #region 사용자 조회
        /// <summary>
        /// 사용자 조회
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="phoneNo">연락처</param>
        /// <returns>사용자 리스트</returns>
        public static ObservableCollection<User> GetUserList(string name = "", string phoneNo = "")
        {
            try
            {
                ObservableCollection<User> userList = new ObservableCollection<User>();

                using (SQLiteConnection conn = new SQLiteConnection(_strConn))
                {
                    conn.Open();

                    string sql = "select * from user_list where deleteTime is null ";

                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += " and name like '%" + name + "%'";
                    }

                    if (!string.IsNullOrEmpty(phoneNo))
                    {
                        phoneNo = phoneNo.Replace("-", "");
                        sql += " and phoneNumber like '%" + phoneNo + "%'";
                    }

                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    SQLiteDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        User user = new User();

                        string getPhoneNumber = Convert.ToString(rdr["phoneNumber"]);

                        user.Code = Convert.ToInt32(rdr["code"]);
                        user.Name = Convert.ToString(rdr["name"]);
                        user.Age = Convert.ToString(rdr["age"]);
                        user.PhoneNumber = getPhoneNumber.Substring(0, 3) + "-" + getPhoneNumber.Substring(3, 4) + "-" + getPhoneNumber.Substring(7);
                        user.UserType = App.GetEnumFromDescription<UserType>(Convert.ToString(rdr["type"]));

                        userList.Add(user);
                    }

                    rdr.Close();
                    conn.Close();
                }

                return userList;
            }
            catch (Exception ex)
            {
                //Exception 로그 등 처리
                //로그남기는 로직은 구현하지 않고 로그만 찍었습니다.
                string excptionMsg = string.Empty;
                excptionMsg += "!!! Exception !!!\r\n";
                excptionMsg += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]";
                excptionMsg += "Function Name :: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                excptionMsg += "Exception Type :: " + ex.GetType().Name;
                excptionMsg += "Stack Trace :: " + ex.StackTrace;
                excptionMsg += "Exception Message :: " + ex.Message;
                MessageBox.Show(excptionMsg);

                return new ObservableCollection<User>();
            }
        }
        #endregion

        #region 사용자 삭제
        public static void DeleteUser(User user)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_strConn))
                {
                    conn.Open();
                    string sql = $"update user_list set deleteTime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where code = '{user.Code}'";
                    Console.WriteLine(sql);
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //Exception 로그 등 처리
                //로그남기는 로직은 구현하지 않고 로그만 찍었습니다.
                string excptionMsg = string.Empty;
                excptionMsg += "!!! Exception !!!\r\n";
                excptionMsg += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]";
                excptionMsg += "Function Name :: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                excptionMsg += "Exception Type :: " + ex.GetType().Name;
                excptionMsg += "Stack Trace :: " + ex.StackTrace;
                excptionMsg += "Exception Message :: " + ex.Message;
                MessageBox.Show(excptionMsg);
            }
        }
        #endregion
    }
}
