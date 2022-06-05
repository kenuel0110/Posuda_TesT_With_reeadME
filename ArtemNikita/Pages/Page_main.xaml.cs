using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Page_main.xaml
    /// </summary>
    public partial class Page_main : Page
    {
        //Все списки, которые мы используем
        List<Product> posuda_DB = Classes.Manager.DBContext.Product.ToList();
        List<Manufacturers> manufacturers_list = Classes.Manager.DBContext.Manufacturers.ToList();
        //Здесь исапользуеться класс, который мы писали сами, он храниться в Classes.Item_Product
        //Он содержит перемнные, которые мы будем отображать
        List<Classes.Item_Posuda> posuda_list = new List<Classes.Item_Posuda>();
        //переменнвя роли
        string role ="";

        //ЗДесь в скобках прописываем значеия, которые мы хотим получить на эту страницу
        public Page_main(string fullname, string role)
        {
            InitializeComponent();
            //если роль "Администратор" отображаем кнопку добавления посуды
            if (role == "Администратор")
                btn_add_posuda.Visibility = Visibility.Visible;

            //передаём роль в переменную, которая наверху, чтобыиспользовать её в дальнейшем
            this.role = role;

            //отображаем имя в надпись
            tb_fullname.Text = fullname;
            //Функция отображения и фильтрации данных
            init(null, null);
            //создаём список состоящий из string
            List<String> manuf = new List<String>();
            //в этот список добавляем элемент "все производители"
            manuf.Add("Все производители");
            //заполняем список данными из БД
            foreach (var item in this.manufacturers_list) 
            {
                manuf.Add(item.Name);
            }
            //заполняем выпадающий список списком, который у нас получился
            lb_category.ItemsSource = manuf;
            //выбираем первый элемент списка  
            lb_category.SelectedIndex = 0;
        }
        //функция заполнения списка посуды и его фильтрации, лн принимает два значения
        //фильтер и объект который выхвал эту функцию
        private void init(string filter, string _object)
        {
            //пытаться, чтобы программа не вылетила при ошибке
            try
            {
                //очищаем список, который будем заполнять
                this.posuda_list.Clear();
                //пока элемени в списке
                foreach (var item in this.posuda_DB)
                {
                    //строки, которые мы получаем из БД
                    string name = item.ProductName;
                    string description = item.ProductDescription;
                    string manufacturer_name = "";
                    string coast = item.ProductCost.ToString();
                    string instock;
                    string articule = item.ProductArticleNumber;
                    byte[] image = item.ProductPhoto;

                    //пока элемент в списке
                    foreach (var manufacturer in this.manufacturers_list)
                    {
                        //если id совпадает с тем, который присущ элементу
                        if (manufacturer.id == item.IDProductManufacturer)
                            //Мы аолучаем название производителя
                            manufacturer_name = manufacturer.Name;
                    }

                    //если на складе больше 0
                    if (item.ProductQuantityInStock > 0)
                        //тогда просто вставляем данные
                        instock = item.ProductQuantityInStock.ToString();
                    else
                        //иначе пишем нет в наличии
                        instock = "Нет в наличии";

                    //добавляем эдемент в список, это делаеться обращаясь у нашему классу, который мы создали
                    //и передаём ему в правильном порядке данные, которые мы полкчили
                    this.posuda_list.Add(new Classes.Item_Posuda(image, name, description, manufacturer_name, coast, instock, articule));
                }
                //это начало фильтрации, если мы получили null и null
                //то росто передаём наш список listview
                if (filter == null && _object == null)
                {
                    lv_posuda.ItemsSource = this.posuda_list;
                    //информацию о фильтрации скрываем
                    tb_filter_count.Visibility = Visibility.Hidden;
                }
                //Если есть какой-то фильтр и объект аыпадающий список
                else if (filter != null && _object == "combobox")
                {
                    //делаем сортировку с помощью FindAll где x производитель равен фильтру
                    var sort_list = this.posuda_list.FindAll(x => x.ProductManufacturer == filter);
                    //информацию о филтре делаем видимой
                    tb_filter_count.Visibility = Visibility.Visible;
                    //отображаем количество найденного их колическтва всех элементов
                    tb_filter_count.Text = $"{sort_list.Count} из {this.posuda_list.Count}";
                    //очищаем список
                    this.posuda_list.Clear();
                    // и заполняем его отсортированным списком
                    this.posuda_list = sort_list.ToList();
                    //заполняем listview
                    lv_posuda.ItemsSource = this.posuda_list;
                }
                //если есть фильтр и фильтруем по поиску
                else if (filter != null && _object == "search")
                {
                    //сортируем благодаря where гле переводим данные в нижний регистр и в нём есть то что мы написали
                    var sort_list = this.posuda_list.Where(x => x.ProductName.ToLower().Contains(filter.ToLower()));
                    //информацию о филтре делаем видимой
                    tb_filter_count.Visibility = Visibility.Visible;
                    //отображаем количество найденного их колическтва всех элементов
                    tb_filter_count.Text = $"{sort_list.ToList().Count} из {this.posuda_list.Count}";
                    //очищаем список
                    this.posuda_list.Clear();
                    // и заполняем его отсортированным списком
                    this.posuda_list = sort_list.ToList();
                    //заполняем listview
                    lv_posuda.ItemsSource = this.posuda_list;
                }
            }
            //если что-то сломалось во время этого получаем ошибку
            catch (Exception ex) 
            {
                // и выводим её пользователю
                MessageBox.Show(ex.ToString());
            }
        }

        //при изменении выбранного элемента
        private void lb_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //если выбран первый элемент то всё возращаеться в норму
            if (lb_category.SelectedIndex == 0)
                this.init(null, null);
            else
                //иначе передаём, то что получаем из выпадающего списка и _object - выпадающий список
                this.init(lb_category.SelectedItem.ToString(), "combobox");
        }
        // событие при изменении текста
        private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //делаем выбранный элемент первым
            lb_category.SelectedIndex = 0;
            //если поиск пустой то всё возращаем в норму
            if (tb_search.Text.ToString() == "")
                this.init(null, null);
            else
                //иначе передаём, то что получаем из выпадающего списка и _object - поиск
                this.init(tb_search.Text.ToString(), "search");
        }
        //нсли нажали на элемент списка
        private void lv_posuda_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //если выбран какой либо элемен
            if (lv_posuda.SelectedIndex != -1)
            {
                //получаем объект, который мы выбрали (получить данные из списка - название_списка[порядковый номер элеменита].то что мы хотим получить)
                string aricule_item = this.posuda_list[lv_posuda.SelectedIndex].ProductArticulNumber;
                //запускаем страницу для отображения элемента, передаём имя, роль, и индивидуальный номер посуды
                NavigationService.Navigate(new Pages.Page_Item(tb_fullname.Text, this.role, aricule_item));
            }
        }

        //кнопка добавления посуды
        private void btn_add_posuda_Click(object sender, RoutedEventArgs e)
        {
            //запускаем страницу добавления посуды передаём ей имя и роль
            NavigationService.Navigate(new Pages.Page_add_posuda(tb_fullname.Text, this.role));
        }

    }
}
