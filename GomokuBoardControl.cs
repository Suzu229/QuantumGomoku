using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuantumGomoku
{
    public partial class GomokuBoardControl : UserControl
    {
        private bool isPlayer1Turn = true;
        private bool player1NextIs90 = true;
        private bool player2NextIs10 = true;

        private List<QuantumStone> stones = new List<QuantumStone>();

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

            foreach (var stone in stones)
            {
                int cx = Margin + stone.Col * CellSize;
                int cy = Margin + stone.Row * CellSize;

                Color fill = stone.Color == "black" ? Color.Black : Color.White;
                Brush brush = new SolidBrush(fill);
                Brush textBrush = stone.Color == "black" ? Brushes.White : Brushes.Black;

                // 円（石）を描く
                g.FillEllipse(brush, cx - 12, cy - 12, 24, 24);
                g.DrawEllipse(Pens.Black, cx - 12, cy - 12, 24, 24);

                // 数字を中央に描く
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(stone.Probability.ToString(), new Font("Arial", 10, FontStyle.Bold), textBrush, cx, cy, sf);
            }

            foreach (var stone in stones)
            {
                int cx = Margin + stone.Col * CellSize;
                int cy = Margin + stone.Row * CellSize;

                // 石の色を確率に応じて設定
                Color stoneColor;
                Brush textBrush;

                if (stone.Color == "black")
                {
                    stoneColor = stone.Probability == 90 ? Color.Black :
                                 stone.Probability == 70 ? Color.DimGray : Color.DarkGray;
                    textBrush = Brushes.White;
                }
                else // white
                {
                    stoneColor = stone.Probability == 30 ? Color.LightGray :
                                 stone.Probability == 10 ? Color.WhiteSmoke : Color.White;
                    textBrush = Brushes.Black;
                }

                Brush stoneBrush = new SolidBrush(stoneColor);

                // 円（石）を描画
                g.FillEllipse(stoneBrush, cx - 12, cy - 12, 24, 24);
                g.DrawEllipse(Pens.Black, cx - 12, cy - 12, 24, 24);

                // 数字を描画
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(stone.Probability.ToString(), new Font("Arial", 10, FontStyle.Bold), textBrush, cx, cy, sf);
            }


        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int row = (e.Y - Margin + CellSize / 2) / CellSize;
            int col = (e.X - Margin + CellSize / 2) / CellSize;

            if (row >= 0 && row < GridSize && col >= 0 && col < GridSize)
            {
                // すでに石があるかを確認（簡易版）
                if (stones.Any(s => s.Row == row && s.Col == col))
                    return;

                if (isPlayer1Turn)
                {
                    int prob = player1NextIs90 ? 90 : 70;
                    stones.Add(new QuantumStone(row, col, "black", prob));
                    player1NextIs90 = !player1NextIs90; // 次は逆の値に
                }
                else
                {
                    int prob = player2NextIs10 ? 10 : 30;
                    stones.Add(new QuantumStone(row, col, "white", prob));
                    player2NextIs10 = !player2NextIs10;
                }

                isPlayer1Turn = !isPlayer1Turn;
                Invalidate();

            }
        }


    }
}
