using System;
using System.Collections.Generic;
using System.Threading;

namespace Uno
{
    public class Game
    {
        public Deck deck;
        public Deck discard { get; set; }
        public Card discarded { get; set; }

        public Player player;
        public Player[] players;
        public Card topCard;
        public bool playing;
        public bool reverse;
        public Player currentPlayer;
        public bool playedACard;
        public int startingTotal;
        public Game(Deck deck, Player player)
        {
            this.deck = deck;
            this.player = player;
            this.discard = new Deck(true);
            Reset();
        }
        public void Run()
        {
            topCard = deck.Deal();
            System.Console.WriteLine("-------------------------------------");
            while(playing)
            {   
                if(deck.cards.Count < 5)
                {
                    for(int i = 0; i < deck.cards.Count; i++)
                    {
                        discard.cards.Add(deck.cards[i]);
                        deck.cards.RemoveAt(i);
                    }
                    discard.Shuffle();
                    deck.cards = discard.cards;
                    discard = new Deck(true);
                }
                if(currentPlayer == player)
                {
                    playerTurn();
                    if(!playing)
                    {
                        if(PlayAgain()) Reset();
                        else return;
                    }
                    if(playedACard) ResolveCard();
                    Thread.Sleep(1000);
                    if(reverse) currentPlayer = currentPlayer.prev;
                    else currentPlayer = currentPlayer.next;
                }
                else
                {
                    System.Console.WriteLine($"{currentPlayer.name} takes their turn.");
                    startingTotal = currentPlayer.hand.Count;
                    discarded = topCard;
                    topCard = currentPlayer.Play(topCard, deck);
                    if(startingTotal == currentPlayer.hand.Count) playedACard = false;
                    else
                    {
                        playedACard = true;
                        discard.cards.Add(discarded);
                    }
                    if(currentPlayer.hand.Count == 0)
                    {
                        System.Console.WriteLine(currentPlayer.name + " wins!");
                        if(!PlayAgain()) return;
                        else Reset();
                    }
                    if(playedACard) ResolveCard();
                    Thread.Sleep(2000);
                    if(reverse) currentPlayer = currentPlayer.prev;
                    else currentPlayer = currentPlayer.next;
                }
                System.Console.WriteLine("-------------------------------------");
            }
        }

        public bool CheckMove(Card playedCard)
        {
            if(playedCard.color == topCard.color || playedCard.faceVal == topCard.faceVal || playedCard.color == "Wild" || topCard.color == "Wild") return true;
            return false;
        }

        public void playerTurn()
        {
            System.Console.WriteLine($"The top card is a {topCard}.");
            System.Console.WriteLine("Your hand:");
            System.Console.WriteLine(player);
            if(CheckLegalMoves(player))
            {
                System.Console.WriteLine("Which card would you like to play?");
                Console.Out.Flush();
                int input;
                while(!(Int32.TryParse(Console.ReadLine(), out input)) || input > player.hand.Count-1 || input < 0 || !CheckMove(player.hand[input]))
                {
                    System.Console.WriteLine("Try again.");
                }
                discarded = topCard;
                topCard = player.Play(input);
                playedACard = true;
                discard.cards.Add(discarded);
                if(player.hand.Count == 0)
                {
                    System.Console.WriteLine("You win!");
                    playing = false;
                }
            }
            else
            {
                System.Console.WriteLine("You have no legal moves! Press enter to draw a card.");
                System.Console.ReadLine();
                player.Draw(deck);
                playedACard = false;
                return;
            }
        }

        public bool CheckLegalMoves(Player p)
        {
            foreach(Card c in p.hand)
            {
                if(CheckMove(c)) return true;
            }
            return false;
        }

        public void ResolveCard()
        {
            if(topCard.faceVal == "Reverse")
            {
                if(reverse) reverse = false;
                else reverse = true;
            }
            if(topCard.faceVal == "Skip")
            {
                if(reverse) currentPlayer = currentPlayer.prev;
                else currentPlayer = currentPlayer.next;
            }
            if(topCard.faceVal == "Draw 2")
            {
                if(reverse)
                {
                    if(currentPlayer.prev == player)
                    {
                        System.Console.WriteLine("Press enter to draw 2 cards.");
                        System.Console.ReadLine();
                    }
                    System.Console.WriteLine(currentPlayer.prev.name + " draws 2 cards.");
                    currentPlayer.prev.Draw(deck, 2);
                }
                else
                {
                    if(currentPlayer.next == player)
                    {
                        System.Console.WriteLine("Press enter to draw 2 cards.");
                        System.Console.ReadLine();
                    }
                    System.Console.WriteLine(currentPlayer.next.name + " draws 2 cards.");
                    currentPlayer.next.Draw(deck, 2);
                }
            }
            if(topCard.faceVal == "Draw 4")
            {
                if(reverse)
                {
                    if(currentPlayer.prev == player)
                    {
                        System.Console.WriteLine("Press enter to draw 4 cards.");
                        System.Console.ReadLine();
                    }
                    System.Console.WriteLine(currentPlayer.prev.name + " draws 4 cards.");
                    currentPlayer.prev.Draw(deck, 4);
                }
                else
                {
                    if(currentPlayer.next == player)
                    {
                        System.Console.WriteLine("Press enter to draw 4 cards.");
                        System.Console.ReadLine();
                    }
                    System.Console.WriteLine(currentPlayer.next.name + " draws 4 cards.");
                    currentPlayer.next.Draw(deck, 4);
                }
            }
        }

        public bool PlayAgain()
        {
            System.Console.WriteLine("Do you want to play again?");
            System.Console.Out.Flush();
            string input = System.Console.ReadLine();
            List<string> yes = new List<string>{"y", "Y", "Yes", "yes", "YES"};
            List<string> no = new List<string>{"n", "N", "no", "No", "NO"};
            while(!yes.Contains(input) && !no.Contains(input))
            {
                System.Console.WriteLine("Please enter Yes/No.");
                input = System.Console.ReadLine();
            }
            if(yes.Contains(input)) return true;
            return false;
        }

        public void Reset()
        {
            this.reverse = false;
            this.playedACard = false;
            System.Console.WriteLine("How many opponents do you want to play against? (1 to 9)");
            int numberOpponents = 0;
            Console.Out.Flush();
            while(!(Int32.TryParse(Console.ReadLine(), out numberOpponents)) || numberOpponents < 1 || numberOpponents > 9)
            {
                System.Console.WriteLine("Please enter a valid number of opponents.");
            }
            this.players = new Player[numberOpponents + 1];
            this.players[0] = this.player;
            for(int i = 1; i <= numberOpponents; i++)
            {
                this.players[i] = new Player("Player " + (i + 1));
            }
            for(int i = 0; i < players.Length; i++)
            {
                if(i == 0)
                {
                    players[i].prev = players[players.Length-1];
                    players[i].next = players[1];
                }
                else if(i == players.Length-1)
                {
                    if(players.Length == 2) players[i].prev = players[0];
                    else players[i].prev = players[players.Length-2];
                    players[i].next = players[0];
                }
                else
                {
                    players[i].prev = players[i-1];
                    players[i].next = players[i+1];
                }
            }
            startingTotal = 0;
            playing = true;
            currentPlayer = player;
            player.hand = new List<Card>();
            foreach(Player p in players)
            {
                for(int i = 0; i < 7; i++)
                {
                    p.Draw(deck);
                }
            }
        }

    }
}
