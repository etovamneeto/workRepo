using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;

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

            //Работа с выводом в Excel-файл; Всю логику работы по выделению данных пиши выше этого места.
            Excel.Application excelAppication = new Excel.Application();//открываем приложение Excel
            excelAppication.Visible = true;//Отображаем окно Excel-приложения
            excelAppication.DisplayAlerts = true;//Запрос на сохранение при изменении
            excelAppication.StandardFont = "Times-New-Roman";//Задаем шрифт и его параметры
            excelAppication.StandardFontSize = 12;

            excelAppication.SheetsInNewWorkbook = 1;//Указываем количество листов в новой рабочей книге
            excelAppication.Workbooks.Add(Type.Missing);//Добавили рабочую книгу в приложение
            Excel.Workbook excelAppWorkbook = excelAppication.Workbooks[1];//Получили ссылку на рабочую книгу
            Excel.Worksheet excelAppWorksheet = (Excel.Worksheet)excelAppWorkbook.Worksheets.get_Item(1);//Получили ссылку на лист рабочей книги

            Excel.Range excelAppCells = excelAppWorksheet.get_Range("A1","J1");//Переменная для работы с ячейками; пока что здесь ячейки под надпись SIR Full (A1-J1)
            excelAppCells.VerticalAlignment = Excel.Constants.xlCenter;//Выравнивание по центру
            excelAppCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelAppCells.Merge();//Слияние ячеек
            excelAppCells.Value2 = "SIR Full";//Выводим в объединенные ячейки текст
            excelAppCells.get_Characters(1, 4).Font.Bold = true;

            //начинаем работу по выводу значений в таблицу; привязка сделана к годам (содержимое years)
            excelAppCells = excelAppWorksheet.get_Range("A2",Type.Missing);//А2 - вывод заголовка столбца А
            excelAppCells.Value2 = "year";
            excelAppCells = excelAppWorksheet.get_Range("B2", Type.Missing);//В2 - вывод заголовка столбца В
            excelAppCells.Value2 = "RUS";

            for (int i = 0; i < years[0]-1986; i++)//заполнение тех строк и столбцов, которые не участвуют в построении графика (в данном случае строки 3-8)
            {
                excelAppCells = (Excel.Range)excelAppWorksheet.Cells[i + 3, 1];//Выводим в столбец А (years)
                excelAppCells.Value2 = 1986 + i;

                excelAppCells = (Excel.Range)excelAppWorksheet.Cells[i + 3, 2];//Выводим в столбец В (RUS)
                excelAppCells.Value2 = 1;
            }
            for (int i = 0; i < years.Count; i++)//вывод строк, которые участвую в построении графика
            {
                excelAppCells = (Excel.Range)excelAppWorksheet.Cells[i + 3 + years[0] - 1986, 1];//выводим в столбец А (years)
                foreach (int val in years)
                {
                    excelAppCells.Value2 = years[i];
                    excelAppCells.Font.ColorIndex = 5;
                }

                excelAppCells = (Excel.Range)excelAppWorksheet.Cells[i + 3 + years[0] - 1986, 2];//Выводим в столбец B (RUS)
                excelAppCells.Value2 = 1;
                excelAppCells.Font.ColorIndex = 5;      
            }

            Console.WriteLine(years[years.Count-1]);
            Console.WriteLine(years[years.Count - 1]-years[0]);
            Console.ReadKey();
            excelAppication.Workbooks.Close();
            excelAppication.Quit();
        }
    }
}
