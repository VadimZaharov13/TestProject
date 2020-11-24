using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;


namespace ConsoleApp3
{
    [Table(Name = "Table_1")]
    public class Table_1
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "Title")]
        public string Title { get; set; }
        [Column(Name = "Category")]
        public string Category { get; set; }
        [Column(Name = "Weight")]
        public double Weight { get; set; }
        [Column(Name = "Price")]
        public double Price { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["str"].ConnectionString;

            SqlCommand com = new SqlCommand();
            com.Connection = conn;

            DataContext db = new DataContext(conn.ConnectionString);
            Table<Table_1> TableOne = db.GetTable<Table_1>();



            foreach (var table in TableOne)
            {
                Console.WriteLine("{0} \t{1} \t{2} \t{3} \t{4}", table.Id, table.Title, table.Category, table.Weight, table.Price);     //// 1
            }
            Console.WriteLine("---------------------------------");   

            var Number2 = db.GetTable<Table_1>().Count(g => g.Weight > 1);
            Console.WriteLine("Quantity - " + Number2);                          //// 2

            Console.WriteLine("---------------------------------");   

            var Number3 = db.GetTable<Table_1>().GroupBy(g => g.Category).Select(j => new { Name = j.Key, Count = j.Count() });      //// 3
            foreach (var item in Number3) Console.WriteLine(item.Name + " (Quantity : " + item.Count + ") ");

            Console.WriteLine("---------------------------------");

            var Number4 = db.GetTable<Table_1>().Where(g => g.Title.ToUpper().StartsWith("S") && g.Title.Length == 5);               //// 4

            foreach (var item in Number4) Console.WriteLine(item.Title);

            Console.WriteLine("---------------------------------");

            var Number5 = db.GetTable<Table_1>().Where(g => g.Price < 400).OrderBy(f => f.Title).ToList();                          //// 5
            foreach (var item in Number5)
            {
                Console.WriteLine(item.Title);
            }
            Console.WriteLine("---------------------------------"); 

            var Number6 = db.GetTable<Table_1>().FirstOrDefault(t => t.Price == db.GetTable<Table_1>().Max(g => g.Price));          //// 6
            Console.WriteLine("Category - " + Number6.Category);

            Console.WriteLine("---------------------------------");  

                 //-------------------------------------- ДОПОЛНИТЕЛЬНО -----------------------------------------------//

            Table_1 table1 = db.GetTable<Table_1>().FirstOrDefault(g => g.Title == "Shoes");
            table1.Price = 999.99;                                                                  
            db.SubmitChanges();                                                                                          //// 1

            Console.WriteLine("---------------------------------");   

            Table_1 table2 = new Table_1 { Title = "Bus", Category = "Toys", Weight = 1.0, Price = 249.99 };                     //// 2
            db.GetTable<Table_1>().InsertOnSubmit(table2);
            db.SubmitChanges();

            //Console.WriteLine("---------------------------------"); 

            var table3 = db.GetTable<Table_1>().Where(g => g.Title == "Cat").FirstOrDefault();                       //// 3
            if (table3 != null)
            {
                db.GetTable<Table_1>().DeleteOnSubmit(table3);
                db.SubmitChanges();
            }
        }
    }
}
    