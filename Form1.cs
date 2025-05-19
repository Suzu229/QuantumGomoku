using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuantumGomoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var board = new GomokuBoardControl();
            board.Location = new Point(0, 0);
            this.Controls.Add(board);

            // フォームのクライアントサイズをぴったり合わせる
            this.ClientSize = board.Size;

            // ウィンドウサイズを固定する（リサイズ禁止）
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
    }
}
