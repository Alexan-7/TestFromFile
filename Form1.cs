// Программа тестирует студента по какому-либо предмету обучения
using System;
using System.Windows.Forms;
// Другие директивы using удалены, поскольку они не используются
// в данной программе

namespace TestFromFile
{
    public partial class Form1 : Form
    {
        int СчетВопросов = 0; // Счет вопросов
        int ПравилОтветов = 0; // Количество правильных ответов
        int НеПравилОтветов = 0; // Количество неправильных ответов
        string[] НеПравилОтветы; // Массив вопросов,
                                 // на которые даны неправильные ответы
        int НомерПравОтвета; // Номер правильного ответа
        int ВыбранОтвет; // Номер ответа, выбранный студентом
                         // Чтобы русские буквы читались корректно, объявляем объект Кодировка:
        System.Text.Encoding Кодировка =
        System.Text.Encoding.GetEncoding("UTF-8");
        System.IO.StreamReader Читатель;
        public Form1()
        {
            InitializeComponent();
            button1.Text = "Следующий вопрос";
            button2.Text = "Выход";
            // Подписка на событие изменение состояния
            // переключателей RadioButton:
            radioButton1.CheckedChanged +=
            new System.EventHandler(ИзмСостПерекл);
            radioButton2.CheckedChanged +=
            new System.EventHandler(ИзмСостПерекл);
            radioButton3.CheckedChanged +=
            new System.EventHandler(ИзмСостПерекл);
            НачалоТеста();
        }
        void НачалоТеста()
        {
            try
            { // Создание экземпляра StreamReader для чтения из файла
                Читатель = new System.IO.
                    StreamReader(System.IO.Directory.GetCurrentDirectory()
                                + @"\test.qst", Кодировка);
                this.Text = Читатель.ReadLine(); // Название предмета
                                                 // Обнуление всех счетчиков:
                СчетВопросов = 0; ПравилОтветов = 0; НеПравилОтветов = 0;
                НеПравилОтветы = new string[100];
            }
            catch (Exception Ситуация)
            { // Отчет о всех ошибках
                MessageBox.Show(Ситуация.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            ЧитатьСледВопрос();
        }
        void ЧитатьСледВопрос()
        {
            label1.Text = Читатель.ReadLine();
            // Считывание вариантов ответа:
            radioButton1.Text = Читатель.ReadLine();
            radioButton2.Text = Читатель.ReadLine();
            radioButton3.Text = Читатель.ReadLine();
            // Выясняем, какой ответ - правильный:
            НомерПравОтвета = int.Parse(Читатель.ReadLine());
            // Переводим все переключатели в состояние "выключено":
            radioButton1.Checked = false; radioButton2.Checked = false;
            radioButton3.Checked = false;
            // Первая кнопка не активна, пока студент не выберет вариант ответа
            button1.Enabled = false;
            СчетВопросов = СчетВопросов + 1; // Проверка, конец ли файла:
            if (Читатель.EndOfStream == true) button1.Text = "Завершить";
        }
        private void ИзмСостПерекл(object sender, EventArgs e)
        { // Кнопка "Следующий вопрос" становится активной, и ей передаем фокус
            button1.Enabled = true; button1.Focus();
            RadioButton Переключатель = (RadioButton)sender;
            string tmp = Переключатель.Name;
            // Выясняем номер ответа, выбранный студентом:
            ВыбранОтвет = int.Parse(tmp.Substring(11));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Щелчок на кнопке
            // "Следующий вопрос/Завершить/Начать тестирование сначала".
            // Счет правильных ответов:
            if (ВыбранОтвет == НомерПравОтвета)
                ПравилОтветов = ПравилОтветов + 1;
            if (ВыбранОтвет != НомерПравОтвета)
            { // Счет неправильных ответов:
                НеПравилОтветов = НеПравилОтветов + 1;
                // Запоминаем вопросы с неправильными ответами:
                НеПравилОтветы[НеПравилОтветов] = label1.Text;
            }
            if (button1.Text == "Начать тестирование сначала")
            {
                button1.Text = "Следующий вопрос";
                // Переключатели становятся видимыми, доступными для выбора:
                radioButton1.Visible = true; radioButton2.Visible = true;
                radioButton3.Visible = true;
                // Переход к началу файла
                НачалоТеста(); return;
            }
            if (button1.Text == "Завершить")
            {
                Читатель.Close(); // Закрываем текстовый файл
                                  // Переключатели делаем невидимыми:
                radioButton1.Visible = false; radioButton2.Visible = false;
                radioButton3.Visible = false;
                // Формируем оценку за тест:
                label1.Text = string.Format("Тестирование завершено.\n" +
                "Правильных ответов: {0} из {1}.\n" +
                "Оценка в пятибалльной системе: {2:F2}.", ПравилОтветов,
                СчетВопросов, (ПравилОтветов * 5F) / СчетВопросов);
                // 5F - это максимальная оценка
                button1.Text = "Начать тестирование сначала";
                // Вывод вопросов, на которые Вы дали неправильный ответ
                string Str = "СПИСОК ВОПРОСОВ, НА КОТОРЫЕ ВЫ ДАЛИ " +
                "НЕПРАВИЛЬНЫЙ ОТВЕТ:\n\n";
                for (int i = 1; i <= НеПравилОтветов; i++)
                    Str = Str + НеПравилОтветы[i] + "\n";
                // Если есть неправильные ответы, то вывести через MessageBox
                // список соответствующих вопросов:
                if (НеПравилОтветов != 0)
                    MessageBox.Show(Str, "Тестирование завершено");
            }
            if (button1.Text == "Следующий вопрос") ЧитатьСледВопрос();
        }

            private void button2_Click(object sender, EventArgs e)
        {
            // Щелчок на кнопке "Выход"
            this.Close();
        }
    }
}
