using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
  
    class XImageGallery : Control
    {
        /// <summary>
        /// список изображений
        /// </summary>
        public List<Image> images = new List<Image>();
        /// <summary>
        /// номер выбранного изображения
        /// </summary>
        public int indShowImage;
        /// <summary>
        /// с какой картинки по номеру показывать снизу
        /// </summary>
        private int thumbStart;
        /// <summary>
        /// на какой закончить
        /// </summary>
        private int thumbEnd;
        /// <summary>
        /// кнопка чтобы штуку снизу прееместить влево
        /// </summary>
        XButton left = new XButton("<");
        /// <summary>
        /// вправо
        /// </summary>
        XButton right = new XButton(">");
        /// <summary>
        /// конструктор с уже заданными картинками
        /// </summary>
        public XImageGallery()
        {
            indShowImage = 2;
            thumbEnd = 2;
            thumbStart = 0;
            for (int i = 1; i <= 3; i++)  
                AddImage($@"C:\Users\448\source\repos\WindowsFormsApp4\WindowsFormsApp4\images\{i}.jpg");
            Width = 521;
            Height = 406;
            left.Width = 10;
            left.Height = 10;
            right.Width = 10;
            right.Height = 10;
            left.Left = 0;
            left.Top = Height - 105 / 2;
            right.Left = Width - 10;
            right.Top = left.Top;
            left.OnClicked += Left_OnClicked;
            right.OnClicked += Right_OnClicked;
            this.Controls.Add(left);
            this.Controls.Add(right);
            
        }
        
        /// <summary>
        /// если нажали кнопку вправо
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Right_OnClicked(object sender, EventArgs e)
        {
            if (thumbEnd != images.Count - 1)
            {
                thumbEnd++;
                thumbStart++;
                Refresh();
            }
        }
        /// <summary>
        /// если нажали влево
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Left_OnClicked(object sender, EventArgs e)
        {
            if (thumbStart != 0)
            {
                thumbEnd--;
                thumbStart--;
                Refresh();
            }
        }
        /// <summary>
        /// метод добавить изображение
        /// </summary>
        /// <param name="filename">путь к файлу</param>
        public void AddImage(string filename)
        {
            images.Add(Image.FromFile(filename));
            Refresh();
        }
        /// <summary>
        /// метод удалить изображение по номеру
        /// </summary>
        /// <param name="index">номер удаляемого изображения</param>
        public void DelImage(int index)
        {
            images.RemoveAt(index);
            if (indShowImage == images.Count)
            {
                indShowImage--;
            }
            Refresh();
        }
        /// <summary>
        /// метод для перемещения изображения с номером k на позицию с номером m
        /// </summary>
        /// <param name="k">номер k</param>
        /// <param name="m">номер m</param>
        public void SwapImage(int k, int m)
        {
            Image tmp = images[m];
            images[m] = images[k];
            images[k] = tmp;
            Refresh();
        }

        protected void Do()
        {
            EventImageSelect?.Invoke(this, EventArgs.Empty);
        }

        private EventHandler EventImageSelect;
        /// <summary>
        /// событие при выборе изображения
        /// </summary>
        public event EventHandler OnImageSelect
        {
            add { EventImageSelect += value; }
            remove { EventImageSelect -= value; }
        }
        
        /// <summary>
        /// нажали мышкой смотрим куда и выбираем изображение
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            int thumbAmount = thumbEnd - thumbStart + 1;
            int count = 5;
            for (int i = thumbStart; i <= thumbEnd; i++)
            {
                if(e.X >= count && e.X <= count + (Width - 10) / thumbAmount - 5 && e.Y >= Height - 105 && e.Y <= Height)
                {
                    indShowImage = i;
                    Do();
                    Refresh();
                }
                count += (Width - 10) / thumbAmount;
            }
        }
        /// <summary>
        /// удаляем выбранную картинку когда нажимаем клавишу Del
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyData == Keys.Delete)
            {
                DelImage(indShowImage);           
            }
        }
        /// <summary>
        /// метод для масштабирования картинки с сохранением пропорций
        /// код не мой взял отсюда
        /// https://stackoverflow.com/a/32593158/14375956
        /// немного преобразовал
        /// </summary>
        /// <param name="img">сама картинка</param>
        /// <returns>Возвращает размеры с сохраненными пропорциями</returns>
        private Size resize(Image img)
        {
            Size original = new Size(img.Width, img.Height);
            int maxSizeH = Height - 110;
            int maxSizeW = Width - 5;

            float percent = (new List<float> { (float)maxSizeW / (float)original.Width, (float)maxSizeH / (float)original.Height }).Min();

            return new Size((int)Math.Floor(original.Width * percent), (int)Math.Floor(original.Height * percent));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            
            if (indShowImage >= 0)
                g.DrawImage(images[indShowImage], new Rectangle(Width/2 - resize(images[indShowImage]).Width/2, 2, resize(images[indShowImage]).Width, resize(images[indShowImage]).Height));
            int thumbAmount = thumbEnd - thumbStart + 1;
            int count = 7;
            for (int i = thumbStart; i <= thumbEnd; i++)
            {
                if (i >= 0 && i < images.Count)
                {
                    Rectangle pos = new Rectangle(count, Height - 105, (Width - 10) / thumbAmount - 5, 103);
                    g.DrawImage(images[i], pos);
                    count += (Width - 10) / thumbAmount;
                    if (i == indShowImage)
                    {
                        g.DrawRectangle(new Pen(Brushes.Black, 3), pos);
                    }
                }
            }
            left.Top = Height - 105 / 2;
            right.Left = Width - 10;
            right.Top = left.Top;
        }

    }
}
