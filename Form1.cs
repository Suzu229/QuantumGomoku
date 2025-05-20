using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuantumGomoku
{
    public partial class Form1 : Form
    {
        private Label lblTurnInfo;
        private Label lblNextStone;
        private Label lblObservationLeft;

        private int player1Observations = 5;
        private int player2Observations = 5;

        private GomokuBoardControl gomokuBoard;
        private Button btnObserve;
        private Button btnCancelObservation;

        private Label lblMessage;
        private Timer fadeTimer;
        private float messageOpacity = 1.0f;

        public Form1()
        {
            InitializeComponent();

            // ───────── 盤面の作成 ─────────
            gomokuBoard = new GomokuBoardControl();
            gomokuBoard.Location = new Point(20, 20);
            this.Controls.Add(gomokuBoard);

            // ───────── 右側パネルの状態表示 ─────────
            lblTurnInfo = new Label();
            lblTurnInfo.Text = "Player 1's turn (Black)";
            lblTurnInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTurnInfo.AutoSize = true;
            lblTurnInfo.Location = new Point(gomokuBoard.Right + 30, 20);
            this.Controls.Add(lblTurnInfo);

            lblNextStone = new Label();
            lblNextStone.Text = "Next stone: 90%";
            lblNextStone.Font = new Font("Arial", 10);
            lblNextStone.AutoSize = true;
            lblNextStone.Location = new Point(lblTurnInfo.Left, lblTurnInfo.Bottom + 10);
            this.Controls.Add(lblNextStone);

            lblObservationLeft = new Label();
            lblObservationLeft.Text = $"Observations left: {player1Observations}";
            lblObservationLeft.Font = new Font("Arial", 10);
            lblObservationLeft.AutoSize = true;
            lblObservationLeft.Location = new Point(lblTurnInfo.Left, lblNextStone.Bottom + 10);
            this.Controls.Add(lblObservationLeft);

            // ───────── 観測ボタン配置 ─────────
            btnObserve = new Button();
            btnObserve.Text = "Observe";
            btnObserve.Size = new Size(120, 30);
            btnObserve.Location = new Point(lblTurnInfo.Left, lblObservationLeft.Bottom + 20);
            btnObserve.Click += btnObserve_Click;
            this.Controls.Add(btnObserve);

            btnCancelObservation = new Button();
            btnCancelObservation.Text = "Cancel Observation";
            btnCancelObservation.Size = new Size(120, 30);
            btnCancelObservation.Location = new Point(btnObserve.Left, btnObserve.Bottom + 10);
            btnCancelObservation.Click += btnCancelObservation_Click;
            this.Controls.Add(btnCancelObservation);

            // ───────── ラベルメッセージ（オプション） ─────────
            lblMessage = new Label();
            lblMessage.Text = "";
            lblMessage.Font = new Font("Arial", 10, FontStyle.Bold);
            lblMessage.BackColor = Color.LightYellow;
            lblMessage.AutoSize = true;
            lblMessage.Visible = false;
            lblMessage.Location = new Point(lblTurnInfo.Left, btnCancelObservation.Bottom + 20);
            this.Controls.Add(lblMessage);

            fadeTimer = new Timer();
            fadeTimer.Interval = 100;
            fadeTimer.Tick += FadeTimer_Tick;

            // ───────── フォームサイズを自動調整 ─────────
            int rightPanelRight = btnCancelObservation.Right;
            int totalWidth = rightPanelRight + 50;
            int totalHeight = Math.Max(gomokuBoard.Bottom, lblMessage.Bottom) + 30;
            this.ClientSize = new Size(totalWidth, totalHeight);

            // ───────── フォーム設定 ─────────
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void btnObserve_Click(object sender, EventArgs e)
        {
            bool isPlayer1Turn = gomokuBoard.IsPlayer1Turn;
            int remaining = isPlayer1Turn ? player1Observations : player2Observations;

            if (remaining > 0)
            {
                gomokuBoard.ObserveStones();

                if (isPlayer1Turn) player1Observations--;
                else player2Observations--;

                UpdateStatus(gomokuBoard.IsPlayer1Turn, gomokuBoard.Player1NextIs90, gomokuBoard.Player2NextIs10);
            }
            else
            {
                ShowMessage("No observations remaining for this player!");
            }
        }

        private void btnCancelObservation_Click(object sender, EventArgs e)
        {
            gomokuBoard.CancelObservation();
        }

        public void UpdateStatus(bool isPlayer1Turn, bool player1NextIs90, bool player2NextIs10)
        {
            lblTurnInfo.Text = isPlayer1Turn
                ? "Player 1's turn (Black)"
                : "Player 2's turn (White)";

            lblNextStone.Text = isPlayer1Turn
                ? $"Next stone: {(player1NextIs90 ? 90 : 70)}%"
                : $"Next stone: {(player2NextIs10 ? 10 : 30)}%";

            lblObservationLeft.Text = isPlayer1Turn
                ? $"Observations left: {player1Observations}"
                : $"Observations left: {player2Observations}";
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
            messageOpacity = 1.0f;
            lblMessage.ForeColor = Color.Black;
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            messageOpacity -= 0.1f;
            if (messageOpacity <= 0)
            {
                fadeTimer.Stop();
                lblMessage.Visible = false;
            }
            else
            {
                lblMessage.ForeColor = Color.FromArgb((int)(255 * messageOpacity), Color.Black);
            }
        }
    }
}
