using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Connect3.Models;


namespace Connect3
{
    public partial class MainWindow : Window
    {
        #region Variables

        private Player player1;
        private Player player2;

        int turnCounter = 0;
        List<string> myWinChecks = new List<string>();

        string myWinCheckOne;
        string myWinCheckTwo;
        string myWinCheckThree;
        string myWinCheckFour;
        string myWinCheckFive;
        string myWinCheckSix;
        string myWinCheckSeven;
        string myWinCheckEight;
            
        List<string> lines = new List<string>();

        List<Button> childrenList;

        int currentGame = 1;

        bool isWon = false;

        #endregion


        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            player1 = new Player("Adam");
            player2 = new Player("Miriam");

            childrenList = mainWindowGrid.Children.OfType<Button>().ToList();

            CurrentTurn();
        }

        #endregion

        #region Checkers

        /// <summary>
        /// Checks whose name to show in textblock "Turn player:".
        /// </summary>
        public void CurrentTurn()
        {
            AlternativeEndChecker();
            
            if (turnCounter % 2 == 0)
            {
                turnPlayerTextblock.Text = $"Turn player: {player1.Name}";
            }

            else
            {
                turnPlayerTextblock.Text = $"Turn player: {player2.Name}";
            }
        }

        /// <summary>
        /// Check if it is player 2 move.
        /// </summary>
        public void TurnChecker()
        {
            if (playVersusAiCheckbox.IsChecked == true)
            {
                player2.Name = "AI";
            }

            else
            {
                player2.Name = "Miriam";
            }
        }

        /// <summary>
        /// Check if there is already winning combination.
        /// </summary>

        public void AlternativeEndChecker()
        {
            myWinChecks = new List<string>();
            
            myWinCheckOne = (string)childrenList[0].Content + (string)childrenList[1].Content + (string)childrenList[2].Content;
            myWinCheckTwo = (string)childrenList[0].Content + (string)childrenList[3].Content + (string)childrenList[6].Content;
            myWinCheckThree = (string)childrenList[6].Content + (string)childrenList[7].Content + (string)childrenList[8].Content;
            myWinCheckFour = (string)childrenList[2].Content + (string)childrenList[5].Content + (string)childrenList[8].Content;
            myWinCheckFive = (string)childrenList[1].Content + (string)childrenList[4].Content + (string)childrenList[7].Content;
            myWinCheckSix = (string)childrenList[3].Content + (string)childrenList[4].Content + (string)childrenList[5].Content;
            myWinCheckSeven = (string)childrenList[2].Content + (string)childrenList[4].Content + (string)childrenList[6].Content;
            myWinCheckEight = (string)childrenList[0].Content + (string)childrenList[4].Content + (string)childrenList[8].Content;

            myWinChecks.AddRange(new List<string> { myWinCheckOne, myWinCheckTwo, myWinCheckThree, myWinCheckFour, myWinCheckFive, myWinCheckSix, myWinCheckSeven, myWinCheckEight });

            foreach (string check in myWinChecks)
            {
                if (check == "XXX")
                {
                    player1.WinCount += 1;

                    scoreFirstPlayerTextblock.Text = $"Score First Player: {player1.WinCount}";

                    MessageBox.Show($"{player1.Name} has won!", "Game has ended!", MessageBoxButton.OK);

                    ResetGame();

                    break;
                }

                else if (check == "OOO")
                {
                    player2.WinCount += 1;

                    scoreSecondPlayerTextblock.Text = $"Score Second Player: {player2.WinCount}";

                    MessageBox.Show($"{player2.Name} has won!", "Game has ended!", MessageBoxButton.OK);

                    ResetGame();

                    break;
                }               
            }

            if (turnCounter == 9 || turnCounter == 10)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (myWinChecks[i] == "XXX")
                    {
                        player1.WinCount += 1;

                        scoreFirstPlayerTextblock.Text = $"Score First Player: {player1.WinCount}";

                        MessageBox.Show($"{player1.Name} has won!", "Game has ended!", MessageBoxButton.OK);

                        ResetGame();

                        break;
                    }

                    else if (myWinChecks[i] == "OOO")
                    {
                        player2.WinCount += 1;

                        scoreSecondPlayerTextblock.Text = $"Score Second Player: {player2.WinCount}";

                        MessageBox.Show($"{player2.Name} has won!", "Game has ended!", MessageBoxButton.OK);

                        ResetGame();

                        break;
                    }
                }

                MessageBox.Show("It is a draw!", "Game has ended!", MessageBoxButton.OK);

                ResetGame();
            }
        }

        #endregion

        #region Endgame

        /// <summary>
        /// Saves history of every turn to text file.
        /// </summary>

        public void SaveTurnHistory()
        {
            string oneLine = string.Join('|', lines);
            File.WriteAllText($"{ Environment.SpecialFolder.LocalApplicationData}\\TurnHistory{currentGame}.csv", oneLine);

            //$"C:\\data\\TurnHistory{currentGame}.csv"



            lines = new List<string>();
            
            currentGame += 1;
        }

        /// <summary>
        /// Resets the game.
        /// </summary>

        public void ResetGame()
        {
            SaveTurnHistory();

            turnCounter = 0;

            MessageBox.Show("A new game has begun!", "New game started", MessageBoxButton.OK);

            List<Button> myRestartList = new List<Button>();
            myRestartList = mainWindowGrid.Children.OfType<Button>().ToList();
            foreach (Button button in myRestartList)
            {
                button.Content = string.Empty;
            }
           
            playVersusAiCheckbox.IsEnabled = true;

            TurnChecker();
        }

        #endregion 

        #region Checkbox check

        /// <summary>
        /// Decides if there will be a hot seat game or vs. AI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void playVersusAiCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (turnCounter == 0)
            {
                player2 = new Player("AI");

                MessageBox.Show("You turned on AI!", "AI activated!", MessageBoxButton.OK);
            }

            else
            {
                playVersusAiCheckbox.IsEnabled = false;

                MessageBox.Show("Next game will be against AI!", "Game is not finished yet!", MessageBoxButton.OK);
            }

        }

        private void playVersusAiCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (turnCounter == 0)
            {
                player2 = new Player("Miriam");

                MessageBox.Show("You turned off AI!", "AI deactivated!", MessageBoxButton.OK);
            }

            else
            {
                playVersusAiCheckbox.IsEnabled = false;

                MessageBox.Show("Next game will be againt a human player!", "Game is still on!", MessageBoxButton.OK);
            }
        }

        #endregion 

        #region AI Implementation

        /// <summary>
        /// Easy "AI" move. Basically a random move.
        /// </summary>

        public void TurnOfAI()
        {
            if (turnCounter != 8)
            {
                List<Button> randomizedList = childrenList.OrderBy(x => Guid.NewGuid()).ToList();

                RandomButtoner(randomizedList);

                turnCounter += 1;

                AlternativeEndChecker();
            }
        }

        /// <summary>
        /// Looks for a random free spot and fills it with "O".
        /// </summary>
        /// <param name="model"></param>

        public void RandomButtoner(List<Button> model)
        {          
            for (int i = 0; i < 9; i++)
            {                                              
                Button randomButton = model[i];

                if ((string)randomButton.Content == "")
                {
                    randomButton.Content = "O";
                    randomButton.Foreground = Brushes.Red;
                    lines.Add(randomButton.Name);

                    break;
                }
            }            
        }

        #endregion

        #region Button events

        /// <summary>
        /// Click event which files clicked button. Also pops a warning if the tile is not free.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if ((string)button.Content == "")
            {               
                if (turnCounter % 2 == 0)
                {
                    button.Content = "X";
                    button.Foreground = Brushes.Blue;
                    lines.Add(button.Name);
                    turnCounter += 1;

                    if (player2.Name == "AI" && isWon == false)
                    {
                        TurnOfAI();
                    }
                }

                else
                {
                    button.Content = "O";
                    button.Foreground = Brushes.Red;
                    lines.Add(button.Name);
                    turnCounter += 1;
                }

                CurrentTurn();
            }
            else
            {
                MessageBox.Show("Select an empty position!", "Invalid posiiton", MessageBoxButton.OK);
            }          
        }
        #endregion
    }
}