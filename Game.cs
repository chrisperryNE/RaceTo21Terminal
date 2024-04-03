using System;
using System.Collections.Generic;
using System.Reflection;

namespace RaceTo21
{
    public class Game
    {
        int numberOfPlayers; 
        List<Player> players = new List<Player>(); 
        CardTable cardTable; 
        Deck deck = new Deck(); 
        int currentPlayer = 0; 
        int round = 1; /// Added this but didn't use it yet.  Would need it to create cumulative score.
        int playerPot = 0;  /// Added this and the int below but didn't use them yet.  I wanted my RaceTo21 to start by choosing a win condition (amount of money in pot or number of rounds won)
        int roundsWon = 0;
        public string nextTask; 
        private bool cheating = false; 
        
       

        public Game(CardTable c)
        {
            
            cardTable = c;
            deck.Shuffle();
            ///deck.ShowAllCards();
            nextTask = "GetNumberOfPlayers";
            
        }

        
        public void AddPlayer(string n)
        {
            players.Add(new Player(n));
        }

        
        public void DoNextTask()
        {
            Console.WriteLine("================================"); // this line should be elsewhere right?
            if (nextTask == "GetNumberOfPlayers")
            {
                numberOfPlayers = cardTable.GetNumberOfPlayers();
                nextTask = "GetNames";
            }
            else if (nextTask == "GetNames")
            {
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    var name = cardTable.GetPlayerName(count);
                    AddPlayer(name); 
                }
                nextTask = "IntroducePlayers";
            }
            else if (nextTask == "IntroducePlayers")
            {
                cardTable.ShowPlayers(players);
                nextTask = "PlayerTurn";
            }
            else if (nextTask == "PlayerTurn")
            {
                cardTable.ShowHands(players);
                Player player = players[currentPlayer];
                if (player.status == PlayerStatus.active)
                {
                    if (cardTable.OfferACard(player))
                    {
                        Card card = deck.DealTopCard();
                        player.cards.Add(card);
                        player.score = ScoreHand(player);
                        if (player.score > 21)
                        {
                            player.status = PlayerStatus.bust;
                        }
                        else if (player.score == 21)
                        {
                            player.status = PlayerStatus.win;
                            Player winner = DoFinalScoring();
                            cardTable.AnnounceWinner(winner);
                            nextTask = "GameOver";
                        }
                    }
                    else
                    {
                        player.status = PlayerStatus.stay;
                    }
                }
                cardTable.ShowHand(player);
                nextTask = "CheckForEnd";
            }
            else if (nextTask == "CheckForEnd")
            {
                if (!CheckActivePlayers())
                {
                    Player winner = DoFinalScoring();
                   
                    cardTable.AnnounceWinner(winner);
                    /// I added in the decision below
                    var decision = cardTable.IsGameOver(winner);
                    if (decision == true)
                    {
                        Console.WriteLine("The group has decided to play again!" +
                            "/n Shuffling cards...");
                        
                        deck.Shuffle();
                        deck.ShowAllCards();
                        /// The line below is where things get messy.  It gets the number of players but does not forget the previous players.  If I write nextTask = "IntroducePlayers", I get stuck in a loop and start an ew round.  I should likely be writing a new task / method.
                        nextTask = "GetNumberOfPlayers";
                    }
                    else if (decision == false)
                    {
                        Console.Write("Press <Enter> to exit...");
                        while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    }
                    
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer > players.Count - 1)
                    {
                        currentPlayer = 0; 
                    }
                    nextTask = "PlayerTurn";
                }
            }
            else if (nextTask == "IsGameOver")
            {
                
                deck.Shuffle();
                deck.ShowAllCards();
                nextTask = "PlayerTurn";
            }


            else 
            {
                Console.WriteLine("I'm sorry, I don't know what to do now!");
                nextTask = "GameOver";
            }
        }

        public int ScoreHand(Player player)
        {
            int score = 0;
            if (cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.name + "'s score be?");
                    response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    char faceValue = card.ID[0];
                    switch (faceValue)
                    {
                        case 'K':
                        case 'Q':
                        case 'J':
                        case 'T':
                            score = score + 10;
                            break;
                        case 'A':
                            score = score + 1;
                            break;
                        default:
                            score = score + (faceValue - '0');
                            break;
                    }
                }
            }
            return score;
        }

        public bool CheckActivePlayers()
        {
            int numberOfOtherPlayers = (players.Count - 1);
            if (numberOfPlayers > 1)
            {

                foreach (var player in players)
                {
                    if (player.status == PlayerStatus.active)
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }

        public Player DoFinalScoring()
        {
            int highScore = 0;
            foreach (var player in players)
            {
                cardTable.ShowHand(player);
                if (player.status == PlayerStatus.win) 
                {
                    return player;
                }
                if (player.status == PlayerStatus.stay) 
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                
            }
            if (highScore > 0) 
            {
                
                return players.Find(player => player.score == highScore);
            }
            return null; 
        }

        
    }
}
