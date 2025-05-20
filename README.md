# 📘 Quantum Gomoku — Rulebook

## 🎮 Objective

Be the first player to align **five or more same-colored stones** in a row — horizontally, vertically, or diagonally — *after observation*.

---

## 👥 Players

* 2 players: **Player 1 (Black)** and **Player 2 (White)**

---

## 🔁 Turn Order

1. On your turn, place a quantum stone on an empty grid cell.
2. Choose whether to:

   * **Observe** the board (to collapse probabilities into actual colors), or
   * **Skip** and pass the turn without observing.

---

## 🧱 Quantum Stones

Each stone is placed with a probability value depending on the player’s turn:

| Player           | First Stone             | Second Stone            | Alternates            |
| ---------------- | ----------------------- | ----------------------- | --------------------- |
| Player 1 (Black) | 90% Black               | 70% Black               | Alternates every turn |
| Player 2 (White) | 10% Black (→ 90% White) | 30% Black (→ 70% White) | Alternates every turn |

> Note: Probability values are **displayed on the stone** before observation.

---

## 🔎 Observation

* You can **observe the board up to 5 times per player** during the game.
* When observing:

  * Each stone's color is randomly determined based on its probability.
  * The probability number disappears, and only black or white remains.
  * After observation, **you can cancel** to undo the result and let the opponent play.
  * If you **do not cancel**, the turn ends and the opponent plays.

---

## 🏆 Winning

* If **five or more stones of the same color are lined up** after observation:

  * That player wins.
* If **both players complete a five-in-a-row during observation**, the player who pressed **Observe** wins.
* After a win, the board is **locked**, and no further moves can be made.

---

## 🔁 Restart

* After a win, press the **Restart** button to begin a new game.

---

## 📝 Notes

* Only one stone can be placed per turn.
* Skip or Observe must be selected after placing a stone.
* You cannot place another stone until your next turn.


# 📘 Quantum Gomoku（量子五目並べ）ルールブック

## 🎯 勝利条件

**観測後に、同じ色の石が縦・横・斜めいずれかに5個以上並んだプレイヤーが勝利**します。

---

## 👥 プレイ人数

* 2人対戦
* **プレイヤー1（黒）** と **プレイヤー2（白）**

---

## 🔁 ターンの流れ

1. 自分のターンで、空いているマスに **量子石** を1つ置きます。
2. その後、以下のいずれかを選択します：

   * **観測（Observe）** → 石の色を確定させる
   * **スキップ（Skip）** → 観測せずに相手にターンを渡す

---

## 🪨 量子石の仕様

各プレイヤーは交互に異なる確率の石を置きます：

| プレイヤー     | 1手目        | 2手目        | 以降の手    |
| --------- | ---------- | ---------- | ------- |
| プレイヤー1（黒） | 黒90%       | 黒70%       | 交互に繰り返す |
| プレイヤー2（白） | 白90%（黒10%） | 白70%（黒30%） | 交互に繰り返す |

> 石には「90」「70」などの確率が表示され、**観測前はまだ色が確定していません**。

---

## 🔎 観測（Observe）

* 観測を行うと、全ての石の色が **それぞれの確率に基づいて** ランダムに決定されます。
* 石の数字は消え、白 or 黒として描画されます。
* 各プレイヤーは **最大5回まで観測可能**。
* 観測後は「キャンセル（Cancel Observation）」で取り消すことができます。
* キャンセルせずにそのまま確定するとターン終了になります。

---

## 🏆 勝利の判定

* **観測の結果として5つ以上同じ色が一直線に並んだ場合、そのプレイヤーが勝利します**。
* **両プレイヤーとも5つ揃っていた場合、観測ボタンを押したプレイヤーが勝者**です。
* 勝敗が決まると、盤面はロックされ、操作できなくなります。

---

## 🔁 再戦（Restart）

* 勝敗が決まると **「Restart」ボタン** が表示されます。
* これを押すことで盤面・石・回数などがリセットされ、ゲームを最初から始めることができます。

---

## 💡補足ルール

* 1ターンで置ける石は1つだけです。
* 石を置いた後、必ず **「観測」または「スキップ」** を選択してください。
* キャンセルすると **相手のターンに移ります**。
