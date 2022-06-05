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
    /// Interaction logic for Page_Item.xaml
    /// </summary>
    public partial class Page_Item : Page
    {
        //получаем данные из ДБ
        List<Product> posuda_DB = Classes.Manager.DBContext.Product.ToList();
        //переменные имени и роли
        string fullname;
        string role;
        public Page_Item(string fullname, string role, string aricule_item)
        {
            InitializeComponent();
            //если роль "Администратор", то отображаем кнопку удаления
            if (role == "Администратор")
                btn_delete.Visibility = Visibility.Visible;
            //заносим имя и роль в переменные, которые мы объявили выше
            this.role = role;
            this.fullname = fullname;
            //получаем данные о элементе, который мы выбрали на прошлой странице, благодаря переданному артикулу
            var sort_list = this.posuda_DB.FindAll(x => x.ProductArticleNumber == aricule_item);
            //очищаем список
            this.posuda_DB.Clear();
            //запалняем его единственным полученным документом
            this.posuda_DB = sort_list.ToList();
            //заполняем listview нашим элементом
            lv_mainPosuda.ItemsSource = this.posuda_DB;
        }

        //нажатие кнопки назад
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            //если мы можем перейти на прошлую страницу
            if (NavigationService.CanGoBack == true)
                //переходим назад
                NavigationService.GoBack();
        }

        //кнопка удаления
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            //вызываем диалог с кнопками да и нет
            var dialogResult = MessageBox.Show("Вы уверены?\n Действие нельзя изменить", "Удаление", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                //если нажали на кнопку Да
                //получаем единственный элемент из списка
                Product product = this.posuda_DB[0];
                //удаляем его из БД
                Classes.Manager.DBContext.Product.Remove(product);
                //сохраняем изменения в БД
                Classes.Manager.DBContext.SaveChanges();
                //переходим на главное окно, это важно, ему передаём имя и роль, которые мы получили ранее
                NavigationService.Navigate(new Pages.Page_main(fullname, role));
            }
        }
    }
}
