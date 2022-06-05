using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtemNikita.Classes
{
    //Класс элемента списка
    public class Item_Posuda
    {
        //переменые, которые в него входят
        //построение переменной, уровень доступа(тут она видимая во всех файлах),
        //тип переменной (string - текст и цифры, int - только числа, byte[] - массив данных, в котором храниться картинка)
        //и название переменной, у первой переменной это ProductPhoto
        public byte[] ProductPhoto { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductManufacturer { get; set; }
        public string ProductCost { get; set; }
        public string ProductQuantityInStock { get; set; }
        public string ProductArticulNumber{ get; set; }

        //"конструктор" класса, другими словами, добавляем возможность классу принимать значения
        public Item_Posuda(
            //в скопках то что принимает класс
         byte[] ProductPhoto,
         string ProductName,
         string ProductDescription,
         string ProductManufacturer,
         string ProductCost,
         string ProductQuantityInStock,
         string ProductArticulNumber
            )
        {
            //здесь мы переменные класса заполняем данными, которые мы передаём
            this.ProductPhoto = ProductPhoto;
            this.ProductName = ProductName;
            this.ProductDescription = ProductDescription;
            this.ProductManufacturer = ProductManufacturer;
            this.ProductCost = ProductCost;
            this.ProductQuantityInStock = ProductQuantityInStock;
            this.ProductArticulNumber = ProductArticulNumber;
        }
    }
}
