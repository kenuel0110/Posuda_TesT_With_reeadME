using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Page_add_posuda.xaml
    /// </summary>
    public partial class Page_add_posuda : Page
    {
        //переменная пути к картинке, которую выбрал пользователь
        string image_path = "";

        //списки с данными из БД
        List<Manufacturers> manufacturers_db = Classes.Manager.DBContext.Manufacturers.ToList();
        List<Categorys> categorys_db = Classes.Manager.DBContext.Categorys.ToList();
        List<Deployers> deployers_db = Classes.Manager.DBContext.Deployers.ToList();
        //переменные роли и имени
        string role;
        string fullname;
        public Page_add_posuda(string fullname, string role)
        {
            InitializeComponent();
            //передаём имя и роль в переменные объявили ввыше
            this.fullname = fullname;
            this.role = role;
            //функция добавления данных в выпадающие списки
            init();
        }

        private void init()
        {
            //пока элемент в производителях
            foreach (var item in manufacturers_db) 
            {
                //добавляем в выпадающий список
                cb_manufacturer.Items.Add(item.Name);
            }
            //пока элемент в категории
            foreach (var item in categorys_db)
            {
                //добавлячем в выпадающий список
                cb_category.Items.Add(item.Name);
            }
            //пока элемент в производителях
            foreach (var item in deployers_db)
            {
                //добавляем в выпадающее список
                cb_deployer.Items.Add(item.Name);
            }
            //выставляем выпадающие списки на первый элемент
            cb_manufacturer.SelectedIndex = 0;
            cb_category.SelectedIndex = 0;
            cb_deployer.SelectedIndex = 0;
        }

        //кнопка добавления картинки
        private void btn_changePhoto_Click(object sender, RoutedEventArgs e)
        {
            //создаём диалог, который спрашивает какой файл выбрать
            OpenFileDialog dialog = new OpenFileDialog();
            //добавляем фильтр на картинку .jpg
            dialog.Filter = "Картинка (*.jpg)|*.jpg";
            //если что-то было выбранно
            if (dialog.ShowDialog() == true) 
            {
                //изменяем изображение в кнопке
                image_changePhoto.Source = new BitmapImage(new Uri(dialog.FileName.ToString()));
                //передаём путь в переменную пути к картинке
                this.image_path = dialog.FileName.ToString();
            }
        }

        //кнопка добавления данных
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            //создаём переменные и заполняем их (в картинку добавляем пустой объект)
            byte[] image = Enumerable.Empty<byte>().ToArray();
            string name = tb_name.Text;
            string aticul = tb_articul.Text;
            string description = tb_description.Text;
            string cost = tb_cost.Text;
            string disount = tb_discount.Text;
            string instock = tb_inStock.Text;

            //здесь обязательно добавляем едениться, так как index начинаеться с 0, а id с 1
            int category = cb_category.SelectedIndex + 1;
            int deployer = cb_deployer.SelectedIndex + 1;
            int manufacturer = cb_manufacturer.SelectedIndex + 1;
            //если что-то не заполненно
            if (
                name == "" ||
                aticul == "" ||
                description == "" ||
                cost == "" ||
                disount == "" ||
                instock == ""
                )
            {
                //выводим сообщение о заполнении поля
                MessageBox.Show("Заполните поля!");
            }
            //инче
            else 
            {
                //если путь к картинке не пустой
                if (this.image_path != "")
                    //читаем данные из файла и добавляем их в переменную картинки
                    image = File.ReadAllBytes(this.image_path);
                //пишем название таблици в какую хотим добавить данные
                //называем эту переменную как-нибудь
                //пишем new и снова название таблицы
                //открываем фигурные скопки и в них прописываем, переменные, которые храняться
                //в таблице и заполняем их теми, которые мы получили от пользователя
                Product product = new Product()
                {
                    ProductArticleNumber = aticul,
                    ProductName = name,
                    IDProductCategory = category,
                    ProductDescription = description,
                    ProductPhoto = image,
                    IDProductManufacturer = manufacturer,
                    ProductCost = Convert.ToInt32(cost),
                    ProductDiscountAmount = Convert.ToInt32(disount),
                    ProductQuantityInStock = Convert.ToInt32(instock),
                    IDProductDeployer = deployer
                };
                //добавляем элемент, который мы сделали в БД
                Classes.Manager.DBContext.Product.Add(product);
                //сохраняем данные
                Classes.Manager.DBContext.SaveChanges();
                //уведомляем пользователя о том что всё сохраненно
                MessageBox.Show("Данные сохранены");
                //переходим на главное окно, передавая имя и роль
                NavigationService.Navigate(new Pages.Page_main(this.fullname, this.role));
            }
        }

        //кнопка назад
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            //если можно идти назад
            if (NavigationService.CanGoBack == true)
                //переходим назад
                NavigationService.GoBack();
        }

        //событие текст при вводе текста
        private void tb_discount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //создаём "фильтр" вводимого текста и ограничиваем его цифрами
            Regex regex = new Regex("[^0-9]+");
            //применяем его к полю вода
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_inStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //тоже самое что и выше
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_cost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //тоже самое что и выше
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
