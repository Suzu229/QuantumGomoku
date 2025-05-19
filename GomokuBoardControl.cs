using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuantumGomoku
{
    public partial class GomokuBoardControl : UserControl
    {
        private const int GridSize = 19;        // 縦横のマス数
        private const int CellSize = 30;        // 1マスのサイズ（ピクセル）
        private const int Margin = 30;          // 碁盤の外枠マージン

        public int TotalSize => Margin * 2 + CellSize * (GridSize - 1);

        public GomokuBoardControl()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.LightGreen;

            // 自動サイズ調整
            this.Width = TotalSize;
            this.Height = TotalSize;

            this.MinimumSize = new Size(TotalSize, TotalSize);
            this.MaximumSize = new Size(TotalSize, TotalSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen gridPen = new Pen(Color.Black);
            Font labelFont = new Font("Arial", 10, FontStyle.Bold);
            Brush labelBrush = Brushes.Black;

            // グリッドの線
            for (int i = 0; i < GridSize; i++)
            {
                int x = Margin + i * CellSize;
                int y = Margin + i * CellSize;

                // 縦線
                g.DrawLine(gridPen, x, Margin, x, Margin + CellSize * (GridSize - 1));
                // 横線
                g.DrawLine(gridPen, Margin, y, Margin + CellSize * (GridSize - 1), y);

                // 横のA〜S
                char letter = (char)('A' + i);
                g.DrawString(letter.ToString(), labelFont, labelBrush, x - 6, Margin - 25);

                // 縦の0〜18
                string number = i.ToString();
                g.DrawString(number, labelFont, labelBrush, Margin - 25, y - 6);
            }

            // 星（●）の描画（3, 9, 15 の交点）
            int[] starPoints = { 3, 9, 15 };
            foreach (int row in starPoints)
            {
                foreach (int col in starPoints)
                {
                    int cx = Margin + col * CellSize;
                    int cy = Margin + row * CellSize;
                    g.FillEllipse(Brushes.Black, cx - 3, cy - 3, 6, 6); // 半径3px
                }
            }
        }
    }
}
