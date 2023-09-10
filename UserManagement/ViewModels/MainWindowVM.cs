using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace UserManagement.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        /// <summary> DataGrid에 보여질 사용자 리스트 </summary>
        public ObservableCollection<User> UserList { get; set; } = new ObservableCollection<User>();

        //선택한 사용자 (DataGrid에서 선택한 Row)
        private User _selectedUser = null;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        public MainWindowVM()
        {
            InitControls();
            SearchUser();
        }

        /// <summary>각 컨트롤 초기화</summary>
        private void InitControls()
        {
            SearchMethodList.Add("이름");
            SearchMethodList.Add("연락처");
            SelectedSearchMethod = "이름";

            InputNameText = string.Empty;
            InputAgeText = string.Empty;
            InputPhoneNoText = string.Empty;
        }

        #region 조회 =====================================================================================================
        /// <summary>조회방식 리스트 (ComboBox에 Binding)</summary>
        public ObservableCollection<string> SearchMethodList { get; private set; } = new ObservableCollection<string>();

        // 선택한 조회방식
        private string _selectedSearchMethod;
        public string SelectedSearchMethod
        {
            get { return _selectedSearchMethod; }
            set { _selectedSearchMethod = value; OnPropertyChanged(nameof(SelectedSearchMethod)); }
        }

        //조회 입력 텍스트
        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }

        private ICommand _searchUserCommand;
        public ICommand SearchUserCommand
        {
            get { return (this._searchUserCommand) ?? (this._searchUserCommand = new DelegateCommand(SearchUser)); }
        }

        private async void SearchUser()
        {
            try
            {
                await Task.Run(() =>
                {
                    Task.Delay(1000);// 1초지연

                    ObservableCollection<User> userList;
                    if(SelectedSearchMethod == "이름")
                    {
                        userList = SqLiteClass.GetUserList(SearchText);
                    }
                    else
                    {
                        userList = SqLiteClass.GetUserList("", SearchText);
                    }
                    
                    // UserList를 업데이트하려면
                    // Dispatcher를 사용하여 UI 스레드로 작업을 보내야함
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UserList.Clear();

                        foreach (User user in userList)
                        {
                            UserList.Add(user);
                        }
                    });
                });
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

        #region 추가 =====================================================================================================
        //이름 입력 텍스트
        private string _inputNameText = string.Empty;
        public string InputNameText
        {
            get { return _inputNameText; }
            set { _inputNameText = value; OnPropertyChanged(nameof(InputNameText)); }
        }

        //나이 입력 텍스트
        private string _inputAgeText = string.Empty;
        public string InputAgeText
        {
            get { return _inputAgeText; }
            set { _inputAgeText = value; OnPropertyChanged(nameof(InputAgeText)); }
        }

        //연락처 입력 텍스트
        private string _inputPhoneNoText = string.Empty;
        public string InputPhoneNoText
        {
            get { return _inputPhoneNoText; }
            set { _inputPhoneNoText = value; OnPropertyChanged(nameof(InputPhoneNoText)); }
        }

        private ICommand _addUserCommand;
        public ICommand AddUserCommand
        {
            get { return (this._addUserCommand) ?? (this._addUserCommand = new DelegateCommand(AddUser)); }
        }

        /// <summary>사용자 추가</summary>
        private async void AddUser()
        {
            try
            {
                if (string.IsNullOrEmpty(InputNameText))
                {
                    MessageBox.Show("추가할 사용자의 이름을 작성해주세요");
                    return;
                }

                if (string.IsNullOrEmpty(InputAgeText))
                {
                    MessageBox.Show("추가할 사용자의 나이를 작성해주세요");
                    return;
                }

                if (string.IsNullOrEmpty(InputPhoneNoText))
                {
                    MessageBox.Show("추가할 사용자의 연락처를 작성해주세요");
                    return;
                }

                if(Regex.Replace(InputPhoneNoText, @"[^\d]", "").Length != 11)
                {
                    MessageBox.Show("연락처 형식이 잘못 되었습니다.");
                    return;
                }

                await Task.Run(() =>
                {
                    Task.Delay(1000);// 1초지연

                    User newUser = new User()
                    {
                        Name = InputNameText,
                        Age = InputAgeText,
                        PhoneNumber = Regex.Replace(InputPhoneNoText, @"[^\d]", ""), //숫자 외 문자 모두 제거
                        UserType = UserType.신규사용자
                    };

                    SqLiteClass.InsertUser(newUser);

                    SearchUser();
                });

                MessageBox.Show("추가 되었습니다");
                InitControls();
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

        #region 삭제 =====================================================================================================
        private ICommand _removeUserCommand;
        public ICommand RemoveUserCommand
        {
            get { return (this._removeUserCommand) ?? (this._removeUserCommand = new DelegateCommand(RemoveUser)); }
        }

        private async void RemoveUser()
        {
            try
            {
                if (SelectedUser == null)
                {
                    MessageBox.Show("삭제 할 사용자를 선택해주세요.");
                    return;
                }
                if(SelectedUser.UserType == UserType.기본사용자)
                {
                    MessageBox.Show("삭제할 수 없는 사용자입니다.");
                    return;
                }

                await Task.Run(() =>
                {
                    Task.Delay(1000);// 1초지연

                    SqLiteClass.DeleteUser(SelectedUser);
                    SearchUser();
                });
            }
            catch (Exception ex)
            {
                //Exception 로그 등 처리
                MessageBox.Show("!!! Exception !!!" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        // INotifyPropertyChanged 구현
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
