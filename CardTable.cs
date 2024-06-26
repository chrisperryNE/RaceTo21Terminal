﻿using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class CardTable
    {
        
        public CardTable()
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("===========================================");
            Console.WriteLine("////////// Welcome to Race to 21 //////////");
            Console.WriteLine("===========================================");
            Console.WriteLine("===========================================");
            Console.WriteLine("Setting Up Table...");
        }

        
        public void ShowPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Introduce(i+1); 
            }
        }

       
        public int GetNumberOfPlayers()
        {
            Console.Write("How many players? ");
            string response = Console.ReadLine();
            int numberOfPlayers;
            while (int.TryParse(response, out numberOfPlayers) == false
                || numberOfPlayers < 2 || numberOfPlayers > 8)
            {
                Console.WriteLine("Invalid number of players.");
                Console.Write("How many players?");
                response = Console.ReadLine();
            }
            return numberOfPlayers;
        }

        
        public string GetPlayerName(int playerNum)
        {
            Console.Write("What is the name of player# " + playerNum + "? ");
            string response = Console.ReadLine();
            while (response.Length < 1)
            {
                Console.WriteLine("Invalid name.");
                Console.Write("What is the name of player# " + playerNum + "? ");
                response = Console.ReadLine();
            }
            return response;
        }

        public bool OfferACard(Player player)
        {
            while (true)
            {
                Console.Write(player.name + ", do you want a card? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y"))
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }
        }

        public void ShowHand(Player player)
        {
            int i = 1;
            if (player.cards.Count > 0)
            {
                Console.Write(player.name + " has: ");
              
                foreach (Card card in player.cards)
                {
                    if (i == player.cards.Count)
                    {
                        Console.Write(card.name + " ");
                        i++;
                    }
                    else
                    {
                        Console.Write(card.name + ", ");
                        i++;
                    }
                }
                Console.Write("=" + player.score + "/21 ");
                if (player.status != PlayerStatus.active)
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }

        }

        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }


        public void AnnounceWinner(Player player)
        {
            int nextPlayer = 1;
            if (player != null)
            {
                Console.WriteLine(player.name + " wins!");
            }
            else if (player == null)
            {
                Console.WriteLine("Everyone busted!");              
            }
            


        }
        /// I added the IsGameOver method, which works but only asks the winner whether or not they want to play again.
        public bool IsGameOver(Player player)
        {
            while (true)
            {
                Console.Write(player.name + ", do you want to play another round? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y"))
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }
        }

    }
}