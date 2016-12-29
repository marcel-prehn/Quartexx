using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace quartett
{
    public partial class QuarteXXX : Form
    {
        DataTable playerCard;
        DataTable computerCard;
        List<int> playerStack;
        List<int> computerStack;
        List<int> drawStack;

        public QuarteXXX()
        {
            InitializeComponent();
        }

        private void QuarteXXX_Load(object sender, EventArgs e)
        {
            startNewGame();
        }

        private void generatePlayerStack()
        {
            Random random = new Random();
            for (int i = 1; i < 17; i++)
            {
                while (true)
                {
                    int next = random.Next(1, 33);
                    if (!playerStack.Contains(next))
                    {
                        playerStack.Add(next);
                        break;
                    }
                }
            }
        }

        private void generateComputerStack()
        {
                Random random = new Random();
                for (int i = 1; i < 17; i++)
                {
                    while (true)
                    {
                        int next = random.Next(1, 33);
                        if (playerStack.Contains(next) == false && computerStack.Contains(next) == false)
                        {
                            computerStack.Add(next);
                            break;
                        }
                    }
                }
        }

        public void getPlayerCard()
        {
            if (playerStack.Count != 0)
            {
                playerCard = sqlite.getCard(playerStack[0]);
                picPlayer.ImageLocation = @"C:\Users\marcel\Documents\Visual Studio 2010\Projects\quartett\res\unknown.jpg";
            }
        }

        private void showPlayerCard()
        {
            picPlayer.ImageLocation = playerCard.Rows[0]["image"].ToString();
            lbName.Text = playerCard.Rows[0]["name"].ToString().ToUpper();
            btBirth.Text = playerCard.Rows[0]["bith"].ToString();
            btHeight.Text = playerCard.Rows[0]["height"].ToString();
            btWeight.Text = playerCard.Rows[0]["weight"].ToString();
            btBrasize.Text = playerCard.Rows[0]["brasize"].ToString();
            btMovies.Text = playerCard.Rows[0]["movies"].ToString();
            btAwards.Text = playerCard.Rows[0]["awards"].ToString();
        }

        private void getComputerCard()
        {
            if (computerStack.Count != 0)
            {
                computerCard = sqlite.getCard(computerStack[0]);
                picComputer.ImageLocation = @"res\unknown.jpg";
                lbName_computer.Text = "????????????";
                btBirth_computer.Text = "????????????";
                btHeight_computer.Text = "????????????";
                btWeight_computer.Text = "????????????";
                btBrasize_computer.Text = "????????????";
                btMovies_computer.Text = "????????????";
                btAwards_computer.Text = "????????????";
            }
        }

        private void showComputerCard()
        {
            picComputer.ImageLocation = computerCard.Rows[0]["image"].ToString();
            lbName_computer.Text = computerCard.Rows[0]["name"].ToString().ToUpper();
            btBirth_computer.Text = computerCard.Rows[0]["bith"].ToString();
            btHeight_computer.Text = computerCard.Rows[0]["height"].ToString();
            btWeight_computer.Text = computerCard.Rows[0]["weight"].ToString();
            btBrasize_computer.Text = computerCard.Rows[0]["brasize"].ToString();
            btMovies_computer.Text = computerCard.Rows[0]["movies"].ToString();
            btAwards_computer.Text = computerCard.Rows[0]["awards"].ToString();
        }

        private void startNewRound()
        {
            moveCard(playerStack);
            moveCard(computerStack);
            lbPlayerStack.Text = playerStack.Count.ToString();
            lbComputerStack.Text = computerStack.Count.ToString();
            resetButtonBackColor();
            getPlayerCard();
            showPlayerCard();
            getComputerCard();
        }

        private void moveCard(List<int> Stack)
        {
            if (Stack.Count != 0)
            {
                int element = Stack[0];
                Stack.RemoveAt(0);
                Stack.Add(element);
            }
            else
            {
                showWinner();
            }
        }

        private void showWinner()
        {
            DialogResult dr = new DialogResult();

            if (playerStack.Count == 0)
            {
                dr = MessageBox.Show("Computer has won the Game!\n\n Start new Game?", "Computer wins!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    startNewGame();
                }
                else
                {
                    Application.Exit();
                }
            }
            if (computerStack.Count == 0)
            {
                dr = MessageBox.Show("You have won the Game!\n\n Start new Game?", "You win!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    startNewGame();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void startNewGame()
        {
            playerStack = new List<int>();
            computerStack = new List<int>();
            drawStack = new List<int>();
            generatePlayerStack();
            generateComputerStack();
            startNewRound();
        }

        private void draw()
        {
            
        }

        private void resetButtonBackColor()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    c.BackColor = Color.FromName("Control");
                }
            }  
        }

        private void btBirth_Click(object sender, EventArgs e)
        {
            compareBirth();
        }

        private void btHeight_Click(object sender, EventArgs e)
        {
            compareHeight();
        }

        private void btWeight_Click(object sender, EventArgs e)
        {
            compareWeight();
        }

        private void btBrasize_Click(object sender, EventArgs e)
        {
            compareBrasize();
        }

        private void btMovies_Click(object sender, EventArgs e)
        {
            compareMovies();
        }

        private void btAwards_Click(object sender, EventArgs e)
        {
            compareAwards();
        }

        private void compareBirth()
        {
            showComputerCard();
            if (DateTime.Parse(playerCard.Rows[0]["bith"].ToString()) > DateTime.Parse(computerCard.Rows[0]["bith"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                playerStack.Add(computerStack[0]);
                computerStack.Remove(computerStack[0]);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btBirth.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else if (DateTime.Parse(playerCard.Rows[0]["bith"].ToString()) < DateTime.Parse(computerCard.Rows[0]["bith"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                computerStack.Add(playerStack[0]);
                playerStack.Remove(playerStack[0]);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btBirth_computer.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else
            {
                drawStack.Add(playerStack[0]);
                drawStack.Add(computerStack[0]);
                playerStack.RemoveAt(0);
                computerStack.RemoveAt(0);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btBirth_computer.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { btBirth.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { lbDraw.Visible = true; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { lbDraw.Text = drawStack.Count.ToString() + " Cards"; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
        }

        private void compareHeight()
        {
            showComputerCard();
            if (int.Parse(playerCard.Rows[0]["height"].ToString()) > int.Parse(computerCard.Rows[0]["height"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                playerStack.Add(computerStack[0]);
                computerStack.Remove(computerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btHeight.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else if (int.Parse(playerCard.Rows[0]["height"].ToString()) < int.Parse(computerCard.Rows[0]["height"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                computerStack.Add(playerStack[0]);
                playerStack.Remove(playerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btHeight_computer.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else
            {
                drawStack.Add(playerStack[0]);
                drawStack.Add(computerStack[0]);
                playerStack.RemoveAt(0);
                computerStack.RemoveAt(0);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btHeight_computer.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { btHeight.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { lbDraw.Visible = true; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { lbDraw.Text = drawStack.Count.ToString() + " Cards"; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
        }
        

        private void compareWeight()
        {
            showComputerCard();
            if (int.Parse(playerCard.Rows[0]["weight"].ToString()) < int.Parse(computerCard.Rows[0]["weight"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                playerStack.Add(computerStack[0]);
                computerStack.Remove(computerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btWeight.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else if (int.Parse(playerCard.Rows[0]["weight"].ToString()) > int.Parse(computerCard.Rows[0]["weight"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                computerStack.Add(playerStack[0]);
                playerStack.Remove(playerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btWeight_computer.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else
            {
                drawStack.Add(playerStack[0]);
                drawStack.Add(computerStack[0]);
                playerStack.RemoveAt(0);
                computerStack.RemoveAt(0);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btWeight_computer.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { btWeight.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { lbDraw.Visible = true; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { lbDraw.Text = drawStack.Count.ToString() + " Cards"; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
        }

        private void compareBrasize()
        {
            showComputerCard();
            string playerBrasize = playerCard.Rows[0]["brasize"].ToString();
            string computerBrasize = computerCard.Rows[0]["brasize"].ToString();
            int playerBrasizeASCII = playerBrasize[0];
            int computerBrasizeASCII = computerBrasize[0];

            if (playerBrasizeASCII > computerBrasizeASCII)
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                playerStack.Add(computerStack[0]);
                computerStack.Remove(computerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btBrasize.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else if (playerBrasizeASCII < computerBrasizeASCII)
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                computerStack.Add(playerStack[0]);
                playerStack.Remove(playerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btBrasize_computer.BackColor = Color.Green; }));
                    Thread.Sleep(1000);
                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else
            {
                drawStack.Add(playerStack[0]);
                drawStack.Add(computerStack[0]);
                playerStack.RemoveAt(0);
                computerStack.RemoveAt(0);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btBrasize_computer.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { btBrasize.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { lbDraw.Visible = true; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { lbDraw.Text = drawStack.Count.ToString() + " Cards"; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
        }

        private void compareMovies()
        {
            showComputerCard();
            if (int.Parse(playerCard.Rows[0]["movies"].ToString()) > int.Parse(computerCard.Rows[0]["movies"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                playerStack.Add(computerStack[0]);
                computerStack.Remove(computerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btMovies.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else if (int.Parse(playerCard.Rows[0]["movies"].ToString()) < int.Parse(computerCard.Rows[0]["movies"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                computerStack.Add(playerStack[0]);
                playerStack.Remove(playerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btMovies_computer.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else
            {
                drawStack.Add(playerStack[0]);
                drawStack.Add(computerStack[0]);
                playerStack.RemoveAt(0);
                computerStack.RemoveAt(0);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btMovies_computer.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { btMovies.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { lbDraw.Visible = true; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { lbDraw.Text = drawStack.Count.ToString() + " Cards"; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
        }

        private void compareAwards()
        {
            showComputerCard();
            if (int.Parse(playerCard.Rows[0]["awards"].ToString()) > int.Parse(computerCard.Rows[0]["awards"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                playerStack.Add(computerStack[0]);
                computerStack.Remove(computerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btAwards.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else if (int.Parse(playerCard.Rows[0]["awards"].ToString()) < int.Parse(computerCard.Rows[0]["awards"].ToString()))
            {
                if (drawStack.Count != 0)
                {
                    computerStack.AddRange(drawStack);
                    drawStack.Clear();
                    lbDraw.Visible = false;
                    lbDraw.Text = "Draw";
                }
                computerStack.Add(playerStack[0]);
                playerStack.Remove(playerStack[0]);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btAwards_computer.BackColor = Color.Green; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
            else
            {
                drawStack.Add(playerStack[0]);
                drawStack.Add(computerStack[0]);
                playerStack.RemoveAt(0);
                computerStack.RemoveAt(0);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Invoke(new Action(delegate { btAwards_computer.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { btAwards.BackColor = Color.Red; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { lbDraw.Visible = true; }));
                    Thread.Sleep(1000);

                    Invoke(new Action(delegate { lbDraw.Text = drawStack.Count.ToString() + " Cards"; }));
                    Thread.Sleep(5);

                    Invoke(new Action(delegate { startNewRound(); }));
                    Thread.Sleep(500);
                });
            }
        }
    }
}
