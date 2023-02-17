using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace LINQ_IO
{
    public class store_product{

        public List<Product> list= new List<Product>();

        //////////////

        //////////////
        public void update(string input)
        {
            string[] input2 = input.Split(' ');

            for(int i=0; i<input2.Length; i++)
            {
                string temp_str = input2[i];
                for(int j = 0; j < temp_str.Length; j++) 
                {
                    if (temp_str[j].ToString() == "_") 
                    {
                        temp_str=temp_str.Remove(j, 1);
                        temp_str=temp_str.Insert(j, " ");
                    }
                }
                input2[i] = temp_str;
            }

            try
            {
                if (input2[0].ToString() == "add")
                {
                    list.Add(new Product(input2[1].ToString(), input2[2].ToString(), input2[3].ToString(), input2[4].ToString(), input2[5].ToString()));
                    Console.WriteLine("Item added !");
                }
                else if (input2[0].ToString() == "insert")
                {
                    list.Insert(int.Parse(input2[1]), new Product(input2[2].ToString(), input2[3].ToString(), input2[4].ToString(), input2[5].ToString(), input2[6].ToString()));
                    Console.WriteLine("Item inserted !");
                }
                else if (input2[0].ToString() == "remove")
                {
                    list.RemoveAt(int.Parse(input2[1]));
                    Console.WriteLine("Item removed !");
                }
                else if (input2[0].ToString() == "print")
                {
                    IEnumerable<Product> k=(from x in list select x);
                    print();
                }
                else if (input2[0].ToString() == "exit")
                {
                    //do nothing
                }
                else if (input2[0].ToString()=="linq")
                {
                    linq();
                }
                else if (input2[0].ToString() == "load")
                {
                    if(input2.Length> 1)
                    {
                        load(input2[1]);
                    }
                    else
                    {
                        load("product.csv");
                    }
                }
                else if (input2[0].ToString() == "save")
                {
                    if (input2.Length > 1)
                    {
                        save(input2[1]);
                    }
                    else
                    {
                        save("product.csv");
                    }
                }
                else if (input2[0].ToString() == "help")
                {
                    Console.WriteLine();
                    Console.WriteLine("=========================================================");
                    Console.WriteLine("@ Modify");
                    Console.WriteLine("add <編號> <名稱> <數量> <價格> <類別>");
                    Console.WriteLine("insert <Index> <編號> <名稱> <數量> <價格> <類別>");
                    Console.WriteLine("remove <Index>");
                    Console.WriteLine();
                    Console.WriteLine("@ Selector");
                    Console.WriteLine("print      (print all item)");
                    Console.WriteLine("linq       (print linq answer)");
                    Console.WriteLine();
                    Console.WriteLine("@ System");
                    Console.WriteLine("load <loactaion\\file.csv>   (or append items)");
                    Console.WriteLine("save <location\\file.csv>");
                    Console.WriteLine("exit");
                    Console.WriteLine("=========================================================");
                }
                else
                {
                    Console.WriteLine("Input format invalid");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Input format invalid");
            }
           
        }
        private void load(string location)
        {
            try
            {
                StreamReader reader = new StreamReader(location);
                try
                {
                    string first_line = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        list.Add(new Product(values[0].ToString(), values[1].ToString(), values[2].ToString(), values[3].ToString(), values[4].ToString()));
                    }
                    Console.WriteLine("File loaded !");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error ! Cannot read the file !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ! File doesnt exist !");
            }
        }
        private void save(string location)
        {      
            try
            {
                string saves_str = "";
                saves_str+="商品編號,商品名稱,商品數量,價格,商品類別\n";
                for (int i = 0; i < list.Count; i++)
                {
                    saves_str+=$"{list[i].id},{list[i].name},{list[i].quantity},{list[i].price},{list[i].groups}\n";
                }

                FileInfo saves= new FileInfo(location);
                using (StreamWriter fs = saves.AppendText())
                {
                    fs.Write(saves_str);
                }

                Console.WriteLine("File saved !");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error ! Cannot save the file !");
                Console.WriteLine(ex.Message); 
            }
            
        }
        private void print()
        {
            try
            {
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Index  ID             \tName                \t\t\tQuantity       \tPrice          \tGroup          ");
                for (int p = 0; p < list.Count; p++)
                {
                    int chineseCharacters = new Regex(@"[^\u4e00-\u9fa5]").Replace(list[p].name, "").Length;
                    string align_name = list[p].name.PadRight(35 - chineseCharacters);
                    Console.WriteLine(
                        $"[{p.ToString().PadLeft(3)}]  {list[p].id.PadRight(15)}\t{align_name.PadRight(20)}\t{list[p].quantity.ToString().PadRight(15)}\t{list[p].price.ToString().PadRight(15)}\t{list[p].groups.PadRight(15)}"
                        );
                };
                Console.WriteLine("===========================================================================================================");
            }
            catch(Exception x)
            {
                Console.WriteLine("Print Error !");
            }
        }
        ///////////
        private void print_ie(IEnumerable<Product> ie)
        {
            try
            {
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Index  ID             \tName                \t\t\tQuantity       \tPrice          \tGroup          ");
                int index = -1;
                foreach(Product p in ie)
                {
                    index++;
                    int chineseCharacters = new Regex(@"[^\u4e00-\u9fa5]").Replace(p.name, "").Length;
                    string align_name = p.name.PadRight(35 - chineseCharacters);
                    Console.WriteLine(
                        $"[{index.ToString().PadLeft(3)}]  {p.id.PadRight(15)}\t{align_name.PadRight(20)}\t{p.quantity.ToString().PadRight(15)}\t{p.price.ToString().PadRight(15)}\t{p.groups.PadRight(15)}"
                        );
                };
                Console.WriteLine("===========================================================================================================");
            }
            catch (Exception x)
            {
                Console.WriteLine("Print Error !");
            }
        }
        private void linq()
        {
            try
            {
                double sum_price = (from x in list select x.price).Sum();
                double average_price = (from x in list select x.price).Average();
                int sum_quantity = (from x in list select x.quantity).Sum();
                double average_quantity = (from x in list select x.quantity).Average();
                double expen_price = (from x in list select x.price).Max();
                double cheap_price = (from x in list select x.price).Min();
                IEnumerable<Product> expen_name = (from x in list where (x.price == expen_price) select x);
                IEnumerable<Product> cheap_name = (from x in list where (x.price == cheap_price) select x);
                double sum_price3C = (from x in list where x.groups == "3C" select x.price).Sum();
                double sum_price_dr_fo = (from x in list where ((x.groups == "飲料") || (x.groups == "食品")) select x.price).Sum();
                Console.WriteLine($"所有商品的總價格 : {sum_price}");
                Console.WriteLine($"所有商品的平均價格 : {average_price}");
                Console.WriteLine($"商品的總數量 : {sum_quantity}");
                Console.WriteLine($"商品的平均數量 :{average_quantity}");
                Console.WriteLine($"商品最貴 :");
                print_ie(expen_name);
                Console.WriteLine($"商品最便宜 :");
                print_ie(cheap_name);
                Console.WriteLine($"3C 的商品總價 : {sum_price3C}");
                Console.WriteLine($"飲料及食品的商品價格 : {sum_price_dr_fo}");
                IEnumerable<Product> k1 = (from x in list where ((x.groups == "食品") && (x.quantity > 100)) select x);
                Console.WriteLine("商品數量大於 100 的商品 :");
                print_ie(k1);
                IEnumerable<Product> k2 = (from x in list where (x.price > 1000) select x);
                Console.WriteLine("價格是大於 1000 的商品 :");
                print_ie(k2);
                double k3 = (from x in list where (x.price > 1000) select x.price).Average();
                Console.WriteLine($"品的價格是大於 1000 + 平均價格 : {k3}");
                Console.WriteLine("高到低排序 : ");
                IEnumerable<Product> sorting = (from x in list select x).OrderByDescending(x => x.price);
                print_ie(sorting);
                Console.WriteLine("低到高排序: ");
                IEnumerable<Product> sorting2 = (from x in list select x).OrderBy(x => x.quantity);
                print_ie(sorting2);
                Console.WriteLine("價格小於等於 10000 的商品 :");
                IEnumerable<Product> k4 = (from x in list where (x.price <= 10000) select x);
                print_ie(k4);

                IEnumerable<IGrouping<string, Product>> result = list.GroupBy(x => x.groups);
                foreach (IGrouping<string, Product> i in result)
                {
                    Console.WriteLine(i.Key);
                    Console.WriteLine($"最貴 {i.Max(x => x.price)}");
                    Console.WriteLine($"最便 {i.Min(x => x.price)}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            store_product base1=new store_product();



            Console.WriteLine("input \"help\" for instructions");
            const bool loop = true;
            while (loop)
            {
                Console.WriteLine();
                Console.WriteLine("help | print | linq | add | insert | remove | load | save | exit | or more...");
                Console.Write(">>>");
                string input=Console.ReadLine();
                base1.update(input);
                if (input == "exit")
                {
                    break;
                }
            }
        }
    }
}
