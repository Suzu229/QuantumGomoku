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
        private GomokuBoardControl gomokuBoard;
        private Button btnObserve;
        private Button btnCancelObservation;

        public Form1()
        {
            InitializeComponent();

            // 盤面
            gomokuBoard = new GomokuBoardControl();
            gomokuBoard.Location = new Point(20, 20);
            this.Controls.Add(gomokuBoard);

            // 観測ボタン
            btnObserve = new Button();
            btnObserve.Text = "Observation";
            btnObserve.Location = new Point(20, gomokuBoard.Bottom + 10);
            btnObserve.Click += btnObserve_Click;
            this.Controls.Add(btnObserve);

            // 観測終了ボタン
            btnCancelObservation = new Button();
            btnCancelObservation.Text = "Cancel Observation";
            btnCancelObservation.Location = new Point(btnObserve.Right + 10, btnObserve.Top);
            btnCancelObservation.Click += btnCancelObservation_Click;
            this.Controls.Add(btnCancelObservation);

            // 盤面サイズとボタンの高さを考慮してフォームサイズを設定
            this.ClientSize = new Size(
                gomokuBoard.Right + 20,
                btnObserve.Bottom + 20
            );


            // ウィンドウサイズを固定する（リサイズ禁止）
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void btnObserve_Click(object sender, EventArgs e)
        {
            gomokuBoard.ObserveStones();
        }

        private void btnCancelObservation_Click(object sender, EventArgs e)
        {
            gomokuBoard.CancelObservation();
        }
    }
}
