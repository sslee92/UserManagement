using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.MainWindowVM _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            SqLiteClass.CreateSqLite(); // 내부 DB 파일 생성

            _dataContext = new ViewModels.MainWindowVM();
            this.DataContext = _dataContext;
        }


    }
}
