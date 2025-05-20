using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuantumGomoku
{
    public partial class GomokuBoardControl : UserControl
    {
        private List<QuantumStone> originalStonesBackup = new List<QuantumStone>();
        private bool isObserved = false;
        private Random random = new Random();

        public bool IsPlayer1Turn => isPlayer1Turn;
        public bool Player1NextIs90 => player1NextIs90;
        public bool Player2NextIs10 => player2NextIs10;


        private bool isPlayer1Turn = true;
        private bool player1NextIs90 = true;
        private bool player2NextIs10 = true;

        private List<QuantumStone> stones = new List<QuantumStone>();

        private const int GridSize = 19;        // 縦横のマス数
        private const int CellSize = 30;        // 1マスのサイズ（ピクセル）
        private const int Margin = 30;          // 碁盤の外枠マージン

        private bool isLocked = false;

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
                    g.FillEllipse(Brushes.Black, cx - 3, cy - 3, 6, 6);
                }
            }

            foreach (var stone in stones)
            {
                int cx = Margin + stone.Col * CellSize;
                int cy = Margin + stone.Row * CellSize;

                // 色の決定
                Color stoneColor;
                Brush textBrush;

                if (stone.IsObserved)
                {
                    stoneColor = (stone.Color == "black") ? Color.Black : Color.White;
                    textBrush = (stone.Color == "black") ? Brushes.White : Brushes.Black;
                }
                else
                {
                    if (stone.Color == "black")
                    {
                        stoneColor = stone.Probability == 90 ? Color.Black :
                                     stone.Probability == 70 ? Color.DimGray : Color.DarkGray;
                        textBrush = Brushes.White;
                    }
                    else
                    {
                        stoneColor = stone.Probability == 30 ? Color.LightGray :
                                     stone.Probability == 10 ? Color.WhiteSmoke : Color.White;
                        textBrush = Brushes.Black;
                    }
                }

                // 石を塗る
                Brush stoneBrush = new SolidBrush(stoneColor);
                g.FillEllipse(stoneBrush, cx - 12, cy - 12, 24, 24);

                // 枠線（勝利石は赤）
                var borderPen = stone.IsWinningStone ? Pens.Red : Pens.Black;
                g.DrawEllipse(borderPen, cx - 12, cy - 12, 24, 24);

                // 数字（未観測時のみ表示）
                if (!stone.IsObserved)
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(stone.Probability.ToString(), new Font("Arial", 10, FontStyle.Bold), textBrush, cx, cy, sf);
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isLocked) return;

            int row = (e.Y - Margin + CellSize / 2) / CellSize;
            int col = (e.X - Margin + CellSize / 2) / CellSize;

            // 範囲外なら無視
            if (row < 0 || row >= GridSize || col < 0 || col >= GridSize)
                return;

            // 既に石が置かれている場所は無効
            if (stones.Any(s => s.Row == row && s.Col == col))
                return;

            // すでに今ターンに置いていたら無効（Form1側で制御）
            var form = this.FindForm() as Form1;
            if (form != null && form.hasPlacedStone)
                return;

            // 石を置く
            if (isPlayer1Turn)
            {
                int prob = player1NextIs90 ? 90 : 70;
                stones.Add(new QuantumStone(row, col, "black", prob));
                player1NextIs90 = !player1NextIs90;
            }
            else
            {
                int prob = player2NextIs10 ? 10 : 30;
                stones.Add(new QuantumStone(row, col, "white", prob));
                player2NextIs10 = !player2NextIs10;
            }

            if (form != null)
            {
                form.hasPlacedStone = true;
                form.EnableObservationButtons(); // ボタンを有効にする！
            }

            // 表示更新
            Invalidate();
        }

        // 観測を実行する
        public void ObserveStones()
        {
            if (isObserved) return;

            // 元の状態を保存
            originalStonesBackup = stones
                .Select(s => new QuantumStone(s.Row, s.Col, s.Color, s.Probability))
                .ToList();

            // 色を確率で仮確定（色だけ変える）
            foreach (var stone in stones)
            {
                int roll = random.Next(1, 101); // 1〜100
                bool becomesBlack = roll <= stone.Probability;
                stone.Color = becomesBlack ? "black" : "white";
                stone.Probability = 100;
                stone.IsObserved = true;
            }

            isObserved = true;
            Invalidate();
        }

        // 数字に戻す
        public void CancelObservation()
        {
            if (!isObserved) return;

            stones = originalStonesBackup
                .Select(s => new QuantumStone(s.Row, s.Col, s.Color, s.Probability))
                .ToList();

            isObserved = false;
            Invalidate();
        }

        // 5つ並び判定ロジック
        public bool HasFiveInARow(string color)
        {
            int[,] directions = new int[,] { { 1, 0 }, { 0, 1 }, { 1, 1 }, { 1, -1 } }; // 横, 縦, 斜め右下, 斜め左下

            foreach (var stone in stones.Where(s => s.Probability == 100 && s.Color == color))
            {
                foreach (int d in Enumerable.Range(0, 4))
                {
                    int dx = directions[d, 0], dy = directions[d, 1];
                    List<QuantumStone> matched = new List<QuantumStone> { stone };

                    for (int step = 1; step < 5; step++)
                    {
                        int nx = stone.Col + dx * step;
                        int ny = stone.Row + dy * step;

                        var nextStone = stones.FirstOrDefault(s =>
                            s.Col == nx && s.Row == ny && s.Color == color && s.Probability == 100);

                        if (nextStone != null)
                            matched.Add(nextStone);
                        else
                            break;
                    }

                    if (matched.Count >= 5)
                    {
                        foreach (var s in matched)
                            s.IsWinningStone = true;  // 赤枠表示用

                        return true;
                    }
                }
            }

            return false;
        }


        public void LockBoard()
        {
            isLocked = true;
        }

        // 勝利判定ロジック内で「5個並びの石」にフラグを立てる
        public void MarkWinningStones(string color)
        {
            foreach (var stone in stones)
                stone.IsWinningStone = false; // 一旦すべてリセット

            int[,] directions = new int[,] { { 1, 0 }, { 0, 1 }, { 1, 1 }, { 1, -1 } };

            foreach (var stone in stones.Where(s => s.Probability == 100 && s.Color == color))
            {
                for (int d = 0; d < 4; d++)
                {
                    int dx = directions[d, 0], dy = directions[d, 1];
                    var matched = new List<QuantumStone> { stone };

                    for (int step = 1; step < 5; step++)
                    {
                        int nx = stone.Col + dx * step;
                        int ny = stone.Row + dy * step;
                        var match = stones.FirstOrDefault(s =>
                            s.Col == nx && s.Row == ny && s.Color == color && s.Probability == 100);
                        if (match != null)
                            matched.Add(match);
                        else
                            break;
                    }

                    if (matched.Count >= 5)
                    {
                        foreach (var s in matched)
                            s.IsWinningStone = true;
                        return; // 最初の5連だけ赤くする
                    }
                }
            }
        }

        public void ResetBoard()
        {
            stones.Clear();
            isPlayer1Turn = true;
            player1NextIs90 = true;
            player2NextIs10 = true;
            isLocked = false;
            isObserved = false;
            Invalidate();
        }

        public void TogglePlayerTurn()
        {
            isPlayer1Turn = !isPlayer1Turn;
        }

        public void UnlockBoard()
        {
            isLocked = false;
        }

    }
}
