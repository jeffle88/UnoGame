using System;
using System.Collections.Generic;

namespace Uno
{
    public class Deck
    {
        public List<Card> cards { get; set; }
        
        public Deck()
        {
            CreateDeck();
            Shuffle();
        }

        public Deck(bool discard)
        {
            this.cards = new List<Card>();
        }

        public void CreateDeck()
        {
            cards = new List<Card>();
            string[] colors = {"Red", "Yellow", "Blue", "Green"};
            foreach(string c in colors)
            {
                for(int i = 1; i < 13; i++){
                    cards.Add(new Card(c, i));
                    cards.Add(new Card(c, i));
                }
                cards.Add(new Card(c, 0));
            }
            for(int i = 0; i < 4; i++){
                cards.Add(new Card("Wild", 13));
                cards.Add(new Card("Wild", 14));
            }
        }

        public void Shuffle()
        {
            Random rand = new Random();
            for(int i = 0; i < cards.Count; i++)
            {
                int otherIndex = rand.Next(0, cards.Count);
                Card temp = cards[otherIndex];
                cards[otherIndex] = cards[i];
                cards[i] = temp;
            }
        }
        public Card Deal()
        {
            Card dealCard = cards[0];
            cards.RemoveAt(0);
            return dealCard;
        }

        public override string ToString()
        {
            foreach(Card c in cards)
            {
                System.Console.WriteLine(c);
            }
            return $"{cards.Count} cards in the deck";
        }
    }
}
