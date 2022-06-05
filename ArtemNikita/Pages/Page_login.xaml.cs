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

namespace ArtemNikita.Pages
{
    /// <summary>
    /// Interaction logic for Page_login.xaml
    /// </summary>
    public partial class Page_login : Page
    {
        public Page_login()
        {
            InitializeComponent();
        }

        //здесь два списка с данными из БД
        //User и Role в угловых скопках грубо говоря, это название таблицы
        List<User> users = Classes.Manager.DBContext.User.ToList();
        List<Role> rols = Classes.Manager.DBContext.Role.ToList();

        //функция нажатия на кнопку, она создаёться автоматически, при написании 
        //click, или выборе из списка в правой части окна в панели свойств, если нажать дважды
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            //строки логина и пароля из поля ввода пароля,
            //чтобы получить данные нужно вместо text прописать password 
            string login = tb_login.Text;
            string password = tb_password.Password;

            //если пользователь ничего не ввёл, то появляеться сообщение
            if (login == "" || password == "")
                //отображение окна с сообщением
                MessageBox.Show("Заполните поля");
            else
            {
                //иначе(тобишь если пользователь чтото ввёл)
                //две переменные с типом bool(это тип у которого два значения true и false)
                //это нужно для того, чтобы понять, что пользователь ввёл верно, что не верно
                bool login_find = false;
                bool password_find = false;
                //переменные которые будут хранить роль пользователя и его имя
                string fullname = "";
                string role_user = "";
                //foreach это цикл, который длиться пока есть элементы в списке
                //переменная item - это курсор или элемент, на котором находиться цикл 
                foreach (var item in users) 
                {
                    // если у элемента списка логин совпадает с введёным логином и паролем
                    //то login_find становиться true что значит, что логин введён правильно
                    //(тоже самое и с паролем)
                    //так же мы получаем рол пользователя и его имя
                    //UserLogin это название столбца в таблице БД с логином
                    if (item.UserLogin == login && item.UserPassword == password)
                    {
                        login_find = true;
                        password_find = true;
                        fullname = item.UserName;
                        role_user = item.UserRole.ToString();
                    }
                        
                }
                //если пароль и логин были найдены
                if (login_find == true && password_find == true) 
                {
                    //здесь мы так же вытаскиваем данные из таблицы с ролями
                    //так как у нас только id роли
                    foreach (var item in rols) 
                    {
                        //если роль которую мы получили из пользователя == айдироли из таблицы роли
                        //то переменныя роли принимает нормальное значение, например "Администратор"
                        if (item.RoleID.ToString() == role_user) 
                            role_user = item.RoleName;
                    }
                    //переход на главную страницу, тут мы передаём этой странице имя и роль пользователя
                    //переходи в Page_main.xaml.cs
                    NavigationService.Navigate(new Pages.Page_main(fullname, role_user));
                }
            }
        }

        private void btn_login_guest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //если нажали на "войти как гость" то переход на главное окно с именем гость и ролью гость
            NavigationService.Navigate(new Pages.Page_main("Гость", "Гость"));
        }
    }
}
