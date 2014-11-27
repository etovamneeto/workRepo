using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

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

        static void Main(string[] args)
        {
            string path = "C:\\Users\\lovachev\\Desktop\\workRepo\\";//папка с файлами
            string fileName = "Full_C00_C80.log";//имя файла для чтения

            FileStream file = new FileStream(path + fileName, FileMode.OpenOrCreate, FileAccess.Read);//Открытие исходного файла
            StreamReader reader = new StreamReader(file);//Читаем файл в поток
            List<string> lines = new List<string>();//Просто пустой список, в него файл загоним

            FileStream testFile = new FileStream(path + "Test.txt", FileMode.OpenOrCreate, FileAccess.Write);//Тестовый файл, куда переписываем исходный
            StreamWriter testWriter = new StreamWriter(testFile);

            using (reader)//перегоняем исходный файл в список
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            reader.Close();//закрыли чтение потока с исходным файлом

//-------выделение числа из строки, где первый символ восклицательный знак - можно функцию сделать-------//
            char[] buffer = lines[32].ToCharArray();
            char[] newbuf = new char[4];
            string str = "";
            List<int> years = new List<int>();
            Console.WriteLine(buffer);

            for (int i = 0; i < newbuf.Length;)
            {
                for (int k = 0; k < buffer.Length; k++)

                    if (Char.IsNumber(buffer[k]) == true)
                    {
                        newbuf[i] = buffer[k];
                        i++;
                    }              
            }

            if (newbuf.Length > 0)
            {

                str = new string(newbuf);
                years.Add(Convert.ToInt32(str));
            }

            Console.WriteLine("Num of Elements of Years = {0}", years.Count);
            Console.WriteLine("First elem of Years = {0}", years[0]);
//-------------------------------------------------------------------------------------------------------//

            
                /*
                            for (int i = 0; i < lines.Count; i++)//научлся выделять восклицательные знаки, маленькая победа)
                            {
                                if (lines[i] != String.Empty)
                                {
                                    if (lines[i][0].CompareTo('!') == 0)
                                    {
                                        val_good++;
                                    }
                                }
                            }
                */
                /*
                            foreach (string str in lines)//перегоняем имеющийся список в тестовый файл для просмотра изменений
                            {                               //Сейчас он перегоняет в тестовый файл исходный файл без пустых строк
                                if(str != String.Empty)
                                testWriter.WriteLine(str);
                            }
                */
                testWriter.Close();//поток на запись закрываем
            
            Console.WriteLine("Str = {0}", str);
            Console.ReadKey();

        }
    }
}
