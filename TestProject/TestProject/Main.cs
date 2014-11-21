using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace TestProject
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = "C:\\Users\\lovachevss\\Desktop\\workRepo\\";
            string fileName = "Full_C00_C80.log";
/*
            FileStream newFile1 = new FileStream(path+fileName, FileMode.OpenOrCreate, FileAccess.Write);
            FileStream newFile2 = new FileStream(path + "Full_C00_C80.log", FileMode.Open, FileAccess.Read);

            StreamWriter writer1 = new StreamWriter(newFile1);
            StreamReader reader2 = new StreamReader(newFile2);

            List<string> list = new List<string>();
            


            writer1.Close();
            reader2.Close();
*/
            FileStream file = new FileStream(path + fileName, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            List<string> lines = new List<string>();

            using (reader)
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            foreach (string str in lines)
            {
                Console.WriteLine(str);
            }
/*
            Console.WriteLine("-------------------------");
            Console.WriteLine(lines.Count);
*/ 

            Console.ReadKey();

        }

    }
}
