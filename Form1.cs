using System;
using System.Drawing;
using System.Runtime.CompilerServices;
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
        private Button btnRestart;

        private Label lblMessage;

        private bool isLocked = false;
        public bool hasPlacedStone { get; set; } = false;

        private Button btnSkip;

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
            btnObserve.Enabled = false;

            btnCancelObservation = new Button();
            btnCancelObservation.Text = "Cancel Observation";
            btnCancelObservation.Size = new Size(120, 30);
            btnCancelObservation.Location = new Point(btnObserve.Left, btnObserve.Bottom + 10);
            btnCancelObservation.Click += btnCancelObservation_Click;
            this.Controls.Add(btnCancelObservation);
            btnCancelObservation.Enabled = false;

            btnSkip = new Button();
            btnSkip.Text = "Skip";
            btnSkip.Size = new Size(120, 30);
            btnSkip.Location = new Point(btnCancelObservation.Left, btnCancelObservation.Bottom + 10);
            btnSkip.Click += btnSkip_Click;
            btnSkip.Enabled = false;
            this.Controls.Add(btnSkip);

            // ───────── メッセージ表示 ─────────
            lblMessage = new Label();
            lblMessage.Text = "";
            lblMessage.Font = new Font("Arial", 10, FontStyle.Bold);
            lblMessage.BackColor = Color.LightYellow;
            lblMessage.AutoSize = true;
            lblMessage.Visible = false;
            lblMessage.Location = new Point(lblTurnInfo.Left, btnSkip.Bottom + 20);
            this.Controls.Add(lblMessage);

            btnRestart = new Button();
            btnRestart.Text = "Restart";
            btnRestart.Size = new Size(120, 30);
            btnRestart.Location = new Point(btnObserve.Left, lblMessage.Bottom + 20);
            btnRestart.Click += BtnRestart_Click;
            btnRestart.Visible = false;
            this.Controls.Add(btnRestart);

            // ───────── フォームサイズを自動調整 ─────────
            int rightPanelRight = btnCancelObservation.Right;
            int totalWidth = rightPanelRight + 50;
            int totalHeight = Math.Max(gomokuBoard.Bottom, btnRestart.Bottom) + 30;
            this.ClientSize = new Size(totalWidth, totalHeight);

            // ───────── フォーム設定 ─────────
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }


        private void btnObserve_Click(object sender, EventArgs e)
        {
            if (!hasPlacedStone) return;

            gomokuBoard.ObserveStones();

            // 観測残数を減らす
            if (gomokuBoard.IsPlayer1Turn)
                player1Observations--;
            else
                player2Observations--;

            // ボタン状態を調整
            btnObserve.Enabled = false;              // 押せないように
            btnCancelObservation.Enabled = true;     // キャンセルだけ有効
            btnSkip.Enabled = false;                 // スキップは無効に

            // 状態更新（ターン交代や勝敗判定はキャンセル後に）
            UpdateStatus(
                gomokuBoard.IsPlayer1Turn,
                gomokuBoard.Player1NextIs90,
                gomokuBoard.Player2NextIs10
            );

            CheckWinner();
        }



        private void btnCancelObservation_Click(object sender, EventArgs e)
        {
            gomokuBoard.CancelObservation(); // 石の状態を戻す

            // ターンを交代する
            gomokuBoard.TogglePlayerTurn();

            // ターンを初期化
            hasPlacedStone = false;
            btnObserve.Enabled = false;
            btnSkip.Enabled = false;

            // 状態を更新
            UpdateStatus(
                gomokuBoard.IsPlayer1Turn,
                gomokuBoard.Player1NextIs90,
                gomokuBoard.Player2NextIs10
            );

            btnCancelObservation.Enabled = false;
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
            lblMessage.ForeColor = Color.Black;
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            // 初期化
            player1Observations = 5;
            player2Observations = 5;
            gomokuBoard.ResetBoard();

            // UI更新
            UpdateStatus(true, true, true); // Player1から再開
            btnObserve.Enabled = false;
            btnCancelObservation.Enabled = false;
            btnRestart.Visible = false;
            lblMessage.Visible = false;

            hasPlacedStone = false;
            gomokuBoard.UnlockBoard(); 
        }

        // Skip ボタンクリック処理
        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (!hasPlacedStone) return;

            EndTurn();
        }

        private void EndTurn()
        {
            hasPlacedStone = false;
            btnObserve.Enabled = false;
            btnSkip.Enabled = false;

            // ターン交代！
            gomokuBoard.TogglePlayerTurn();

            // ステータスを更新（player1NextIs90 なども反映）
            UpdateStatus(
                gomokuBoard.IsPlayer1Turn,
                gomokuBoard.Player1NextIs90,
                gomokuBoard.Player2NextIs10
            );
        }

        public void EnableObservationButtons()
        {
            btnObserve.Enabled = true;
            btnSkip.Enabled = true;
        }

        private void CheckWinner()
        {
            bool blackWin = gomokuBoard.HasFiveInARow("black");
            bool whiteWin = gomokuBoard.HasFiveInARow("white");

            string winner = null;
            if (blackWin && !whiteWin) winner = "Player 1 (Black)";
            else if (whiteWin && !blackWin) winner = "Player 2 (White)";
            else if (blackWin && whiteWin)
                winner = gomokuBoard.IsPlayer1Turn ? "Player 1 (Black)" : "Player 2 (White)";

            if (winner != null)
            {
                ShowMessage($"{winner} wins!");
                gomokuBoard.LockBoard();
                btnObserve.Enabled = false;
                btnCancelObservation.Enabled = false;
                btnSkip.Enabled = false;
                btnRestart.Visible = true;
            }
        }
    }
}
