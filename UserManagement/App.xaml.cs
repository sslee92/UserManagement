using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UserManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region 코드 <-> Enum 값 변환 
        /// <summary>
        /// Enum DescriptionAttribute 내용 가져오기
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescriptionOfEnum(Enum en)
        {
            var enumType = en.GetType();
            var members = enumType.GetMember(en.ToString());
            if (members != null && members.Length > 0)
            {
                var attributes = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return en.ToString();
        }

        /// <summary>
        /// DescriptionAttribute 내용으로 Enum 데이터 가져오기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T? GetEnumFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default;
        }
        #endregion
    }

    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">indicate an execute function</param>
        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">execute function </param>
        /// <param name="canExecute">can execute function</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }
        /// <summary>
        /// can executes event handler
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// implement of icommand can execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        /// <returns>can execute or not</returns>
        public bool CanExecute(object o)
        {
            if (this._canExecute == null)
            {
                return true;
            }
            return this._canExecute();
        }

        /// <summary>
        /// implement of icommand interface execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        public void Execute(object o)
        {
            this._execute();
        }

        /// <summary>
        /// raise ca excute changed when property changed
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
