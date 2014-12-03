using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TestProject
{
    class Program//функция для преобразования файла с DoS-переносом строки в файл с Unix-форматом окончания строки
    {

        private static void WinToUnix(string sourceFile, string distFile)
        {
            var lines = File.ReadAllLines(sourceFile);
            using (var safeFile = new FileStream(distFile, FileMode.Create))
            {
                foreach (var bites in lines.Select(item => Encoding.ASCII.GetBytes(item + "\n")))
                {
                    safeFile.Write(bites, 0, bites.Length);
                }
            }
        }

        private static void YearFromLines(string str, List<int> list)
        {
            Regex regular = new Regex(@"\d+\d+\d+\d", RegexOptions.IgnoreCase);
            MatchCollection mc = regular.Matches(str);
            foreach (Match mat in mc)
            {
                list.Add(Convert.ToInt32(mat.ToString()));
            }
        }

        static void Main(string[] args)
        {
//описание стартовых переменных, которые необходимы
            string path = "C:\\Users\\lovachev\\Desktop\\workRepo\\";//папка с файлами
            string fileName = "Full_C00_C80.log";//имя файла для чтения

            FileStream file = new FileStream(path + fileName, FileMode.OpenOrCreate, FileAccess.Read);//Открытие исходного файла
            StreamReader reader = new StreamReader(file);//Читаем файл в поток
            List<string> lines = new List<string>();//Просто пустой список, в него файл загоним

//Запись файла в список сток "lines"
            using (reader)//перегоняем исходный файл в список
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            reader.Close();

            List<int> years = new List<int>();//список, в котором хранятся годы, выделенные из строк с '!'
            //выделение строк с восклицательными знаками, чтобы из них выделить года
            for (int i = 0; i < lines.Count; i++)
            { 
                if (lines[i] != String.Empty)
                {                   
                    if (lines[i][0].CompareTo('!') == 0)
                    {
                        YearFromLines(lines[i],years);
                    }
                }
            }

            Console.ReadKey();

        }
    }
}
