using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postalcode
{
    class Program
    {
        static void Main(string[] args)
        {
//            string file_path = @"C:\47OKINAW.CSV";
            string file_path;
            string encode = "shift_jis";

            if (args.Length <= 0)
            {
                while (true)
                {
                    Console.WriteLine("ファイルパスがありません再度入力してください\n終了する場合はexitと入力してください");
                     file_path = Console.ReadLine();
                    if (file_path == "exit")
                    {
                        return;
                    }
                    else if (System.IO.File.Exists(file_path) == true)
                    {
                        searchpostal(file_path, encode);
                        break;
                    }
                }
            }
            else
            {
                file_path = args[0];

                searchpostal(file_path, encode);
            }
        }

        static void searchpostal(string file_path, string encode)
        {
            using (StreamReader sr = new StreamReader(file_path, Encoding.GetEncoding(encode)))
            {
                Dictionary<string, string> postal_dic = new Dictionary<string, string>();
                while (true)
                {
                    string csvdata = sr.ReadLine();
                    if (csvdata == null)
                    {
                        break;
                    }
                    string[] token_list = csvdata.Split(',');
                    if (token_list.Length <= 2)
                    {
                        continue;
                    }

                    // C列を取出し頭に〒、最後尾に半角スペースを追加
                    string c_elem = token_list[2].Trim('"');     //.Insert(0, "〒").Insert(8, " ");
                    // 以下G, H, I列を"を取ってそれぞれの変数に代入
                    string g_elem = token_list[6].Trim('"');
                    string h_elem = token_list[7].Trim('"');
                    string i_elem = token_list[8].Trim('"');
                    string ghi_elem = g_elem + h_elem + i_elem;

                    // postalリストに結合していれる。
                    if (!postal_dic.ContainsKey(c_elem))
                    {
                        postal_dic.Add(c_elem, ghi_elem);
                    }
                }

                Console.WriteLine("郵便番号を入力してください");
                string user_input = Console.ReadLine();
                user_input.Replace("-", "");

                if (postal_dic.ContainsKey(user_input) == true)
                {
                    foreach (var dic_elem in postal_dic)
                    {
                        if (dic_elem.Key == user_input)
                        {
                            Console.WriteLine("〒{0} {1}", dic_elem.Key, dic_elem.Value);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("該当する郵便番号はありません");
                }
            }
        }
    }
}