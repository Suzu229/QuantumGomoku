using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantumGomoku
{
    public class QuantumStone
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Color { get; set; } // "black" or "white"
        public int Probability { get; set; } // 10〜90など
        public bool IsWinningStone { get; set; } = false;
        public bool IsObserved { get; set; } = false;

        public QuantumStone(int row, int col, string color, int probability)
        {
            Row = row;
            Col = col;
            Color = color;
            Probability = probability;
        }
    }
}
