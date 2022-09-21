using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    /// <summary>
    /// Кнопка с загружаемым фоном
    /// </summary>
    public class XButton : Control
    {
        private bool m_bIsPressed = false;
        private Font m_Font = new Font("Times New Roman", 10);
        private string m_Text;
        private EventHandler m_onClick;

        public string sText { get => m_Text; set => m_Text = value; }
        public XButton()
        {

        }
        public XButton(String text)
        {
            m_Text = text;
        }

        public event EventHandler OnClicked
        {
            add { m_onClick += value; }
            remove { m_onClick -= value; }
        }
        protected void DoClick()
        {
            m_onClick?.Invoke(this, EventArgs.Empty);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            DoClick();
            Refresh();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.DrawRectangle(Pens.Blue, 0, 0, Width - 1, Height - 1);
            float tw = g.MeasureString(sText, m_Font).Width;
            float th = g.MeasureString(sText, m_Font).Height;
            int x = (int)Math.Round((double)(Width / 2 - tw / 2));
            int y = (int)Math.Round((double)(Height / 2 - th / 2));
            g.DrawString(sText, m_Font, new SolidBrush(Color.Black), x, y);
        }
    }
}
