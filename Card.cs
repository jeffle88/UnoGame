using System;

namespace Uno
{
    public class Card
    {
        public string color { get; set; }
        
        public string faceVal { get; set; }

        public int value { get; set; }
        public Card(string color, int value){
            this.color = color;
            this.value = value;

            switch(value)
            {
                case 10:
                    faceVal = "Skip";
                    break;
                case 11:
                    faceVal = "Reverse";
                    break;
                case 12:
                    faceVal = "Draw 2";
                    break;
                case 13:
                    faceVal = "Card";
                    break;
                case 14:
                    faceVal = "Draw 4";
                    break;
                default:
                    faceVal = value.ToString();
                    break;
            }
        }
        public override string ToString(){
            return $"{color} {faceVal}";
        }
    }
}
