using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pyatnashki
{
    public partial class Form1 : Form
    {
        // где сейчас пустая клетка (столбец и строка в tableLayoutPanel1)
        int emptyColumn = 3;
        int emptyRow = 3;

        int countShag = 0; // сколько ходов сделал игрок
        bool gameOver = false; // чтобы после победы нельзя было двигать фишку

        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        // функция нажатия на фишку
        private void Click(object sender, EventArgs e)
        {
            if (gameOver) return; // после победы двигать нельзя

            Move((Button)sender);
            IsWin();
        }

        // пробуем подвинуть фишку в пустую клетку
        private void Move(Button b)
        {
            TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(b);

            // фишка должна стоять вплотную к пустой клетке (слева, справа, сверху или снизу)
            // если сумма разниц по столбцу и строке равна 1, значит она рядом
            if (Math.Abs(pos.Column - emptyColumn) + Math.Abs(pos.Row - emptyRow) == 1)
            {
                // ставим фишку на место пустой клетки
                tableLayoutPanel1.SetCellPosition(b, new TableLayoutPanelCellPosition(emptyColumn, emptyRow));

                // переставляем клетку туда, где стояла фишка
                emptyColumn = pos.Column;
                emptyRow = pos.Row;

                countShag++;
                labelHody.Text = "Ходы: " + countShag;
            }
        }

        // перемешиваем фишки
        private void RandState()
        {
            gameOver = false;

            // чтобы позиция была решаемой - перемешиваем от собранной игры, а не рандомно
            for (int i = 0; i < 150; i++)
            {
                // собираем все фишки, которые стоят рядом с пустой клеткой
                List<Button> sosed = new List<Button>();
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(c);
                    if (Math.Abs(pos.Column - emptyColumn) + Math.Abs(pos.Row - emptyRow) == 1)
                    {
                        sosed.Add((Button)c);
                    }
                }

                // двигаем случайную из них
                Move(sosed[rand.Next(sosed.Count)]);
            }

            // обнуляем счет
            countShag = 0;
            labelHody.Text = "Ходы: 0";
        }

        // проверка выигрыша
        private void IsWin()
        {
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                // из имени кнопки "button7" получаем номер 7
                int nomer = Convert.ToInt32(c.Name.Substring(6));

                TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(c);

                // фишка с номером n должна стоять в клетке ((n-1)%4, (n-1)/4)
                if (pos.Column != (nomer - 1) % 4 || pos.Row != (nomer - 1) / 4)
                {
                    return; // какая-то фишка не на своём месте, выходим
                }
            }

            // все фишки на местах
            gameOver = true;
            MessageBox.Show("Победа! Ходов: " + countShag, "Пятнашки");

            if (MessageBox.Show("Сыграть ещё раз?", "Пятнашки", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RandState();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RandState();
        }

        private void buttonZanovo_Click(object sender, EventArgs e)
        {
            RandState();
        }
    }
}
