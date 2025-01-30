namespace BJ
{
    public class PlayingStrategies
    {
        public static Func<Card, Hand, bool> DoSurrender_2Deck_HS17 = new((upcard, hand) =>
        {
            //you must have hit 0 times, and you dont surrender with aces
            if (hand.IsSoft)
                return false;

            //if your value is 16, surrender on Ace upcard only
            if (hand.Cards.All(c => c.Value == 8))
            {
                return upcard.Rank == "A";
            }

            switch (hand.Value)
            {
                case 15:
                case 16:
                    return upcard.Value >= 10;
                case 17:
                    return upcard.Value == 11;
                default:
                    break;
            }
            return false;
        });
        public static Func<Card, Hand, bool> DoSplit_2Deck_HS17_DAS = new((upcard, hand) =>
        {
            if (hand.Cards[0].Rank == "A" && hand.Cards[0].Rank == hand.Cards[1].Rank)
                return true;

            if (hand.Cards[0].Value != hand.Cards[1].Value)
                return false;

            switch (hand.Cards[0].Value)
            {
                case 8:
                    return true;
                case 2:
                case 3:
                case 7:
                    return upcard.Value <= 7;
                case 6:
                    return upcard.Value <= 6;
                case 4:
                    return upcard.Value == 5 || upcard.Value == 6;
                case 9:
                    return upcard.Value <= 9 && upcard.Value != 7;
            }
            return false;
        });
        public static Func<Card, Hand, bool> DoHit_2Deck_HS17 = new((upcard, hand) =>
        {
            if (hand.IsSoft == false)
            {
                if (hand.Value >= 17)
                    return false;
                if (hand.Value <= 11)
                    return true;
                if (hand.Value >= 12 && upcard.Value >= 7)
                    return true;
                if (hand.Value == 12 && upcard.Value <= 3)
                    return true;
            }
            if (hand.IsSoft)
            {
                if (hand.Value >= 19)
                    return false;
                if (hand.Value <= 17)
                    return true;
                if (hand.Value == 18 && upcard.Value != 7 && upcard.Value != 8)
                    return true;
                if (hand.Value == 19 && upcard.Value == 6)
                    return true;
            }

            return false;
        });
        public static Func<Card, Hand, bool> DoDoubleDown_2Deck_HS17 = new((upcard, hand) =>
        {
            if (!hand.IsSoft)
            {
                if (hand.Value == 11)
                    return true;
                if (hand.Value == 10 && upcard.Value <= 9)
                    return true;
                if (hand.Value == 9 && upcard.Value <= 6)
                    return true;
            }
            else
            {
                switch (upcard.Value)
                {
                    case 6: return hand.Value <= 19;
                    case 5: return hand.Value <= 18;
                    case 4: return hand.Value <= 18 && hand.Value >= 14;
                    case 3: return hand.Value == 17 || hand.Value == 18;
                    case 2: return hand.Value == 18;
                }
            }
            return false;
        });

        /// <summary>
        /// Strategy from https://wizardofodds.com/games/blackjack/strategy/calculator/
        /// 2 decks, Hit soft 17, DAS allowed, Surrender allowed, Dealer Peeks
        /// </summary>
        public static readonly Action<GameState>[][] BasicStrategy_HardHand_2D_H17 =
        [
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //5
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //6
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //7
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //8
            [   Dh, Dh, Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //9
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],  //10
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh  ],  //11
            [   H,  H,  S,  S,  S,  H,  H,  H,  H,  H   ],  //12
            [   S,  S,  S,  S,  S,  H,  H,  H,  H,  H   ],  //13
            [   S,  S,  S,  S,  S,  H,  H,  H,  H,  H   ],  //14
            [   S,  S,  S,  S,  S,  H,  H,  H,  Rh, Rh  ],  //15
            [   S,  S,  S,  S,  S,  H,  H,  Rh, Rh, Rh  ],  //16
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  Rs  ],  //17
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21
        ];

        public static readonly Action<GameState>[][] BasicStrategy_SoftHand_2D_H17 =
        [
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  Dh, Dh, H,  H,  H,  H,  H   ],  //13 A2
            [   H,  H,  H,  Dh, Dh, H,  H,  H,  H,  H   ],  //14 A3
            [   H,  H,  Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //15 A4
            [   H,  H,  Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //16 A5
            [   H,  Dh, Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //17 A6
            [   Dh, Dh, Dh, Dh, Dh, S,  S,  H,  H,  H   ],  //18 A7
            [   S,  S,  S,  S,  Ds, S,  S,  S,  S,  S   ],  //19 A8
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20 A9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21 A10
        ];

        public static readonly Action<GameState>[][] BasiStrategy_Pairs_2D_H17_DAS =
        [
            //  2   3   4   5   6   7   8   9   10  A
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //2,2
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //3,3
            [   H,  H,  H,  P,  P,  H,  H,  H,  H,  H   ],   //4,4
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],   //5,5
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //6,6
            [   P,  P,  P,  P,  P,  P,  P,  H,  H,  H   ],   //7,7
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  Rp  ],   //8,8
            [   P,  P,  P,  P,  P,  H,  P,  P,  S,  S   ],   //9,9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //10,10
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  P   ],   //A,A
        ];

        public static void H(GameState gamestate)
        {
        }
        public static void Dh(GameState gamestate) { }
        public static void Ds(GameState gamestate) { }
        public static void S(GameState gamestate) { }
        public static void Rh(GameState gamestate) { }
        public static void Rs(GameState gamestate) { }
        public static void P(GameState gamestate) { }
        public static void Rp(GameState gamestate) { }
    }

    public class PlayingStrategy
    {
        public Func<GameState>[][] Strategy { get; set; }
    }
}
