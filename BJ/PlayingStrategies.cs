namespace BJ
{
    public class PlayingStrategies
    {
        /// <summary>
        /// Strategy from https://wizardofodds.com/games/blackjack/strategy/calculator/
        /// 2 decks, Hit soft 17, DAS allowed, Surrender allowed, Dealer Peeks
        /// </summary>
        public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_HardHand_2D_H17 =
        [
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //4    4 is here in case I they cannot split a 4 for whatever reason.
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
            [   S,  S,  S,  S,  S,  H,  H,  H,  Rh, Rh  ],  //16
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  Rs  ],  //17
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //18
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21
        ];
        public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_SoftHand_2D_H17 =
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
        public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_Pairs_2D_H17_DAS =
        [
            //  2   3   4   5   6   7   8   9   10  A
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //2,2
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //3,3
            [   H,  H,  H,  P,  P,  H,  H,  H,  H,  H   ],   //4,4
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],   //5,5
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //6,6
            [   P,  P,  P,  P,  P,  P,  P,  H,  H,  H   ],   //7,7
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  Rp  ],   //8,8
            [   P,  P,  P,  P,  P,  S,  P,  P,  S,  S   ],   //9,9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //10,10
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  P   ],   //A,A
        ];

        public static readonly (int, Func<GameState, ActionEnum>)?[][] Deviations_HardHand_SD =
        [
            //  2       3       4       5       6       7       8       9       10      A
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //4
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //5
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //6
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //7
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //8
            [   (+1,Dh),null,   null,   null,   null,   (+3,Dh),null,   null,   null,   null    ],  //9
            [   null,   null,   null,   null,   null,   null,   null,   null,   (+4,Dh),(+4,Dh) ],  //10
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   (+1,Dh) ],  //11
            [   (+3,S), (+2,S), Hlte0,  (-2,H), (-1,H), null,   null,   null,   null,   null    ],  //12
            [   (-1,H), (-2,H), null,   null,   null,   null,   null,   null,   null,   null    ],  //13
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //14
            [   null,   null,   null,   null,   null,   null,   null,   null,   (+4,S), null    ],  //15
            [   null,   null,   null,   null,   null,   null,   null,   (+5,S), Sgte0,  null    ],  //16
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //17
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //18
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //19
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //20
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //21
        ];

        private static ActionEnum H(GameState gamestate) => ActionEnum.H;
        private static ActionEnum Dh(GameState gamestate) => gamestate.CanDoubleDown() ? ActionEnum.D : ActionEnum.H;
        private static ActionEnum Ds(GameState gamestate) => gamestate.CanDoubleDown() ? ActionEnum.D : ActionEnum.S;
        private static ActionEnum S(GameState gamestate) => ActionEnum.S;
        private static ActionEnum Rh(GameState gamestate) => gamestate.CanSurrender() ? ActionEnum.R : ActionEnum.H;
        private static ActionEnum Rs(GameState gamestate) => gamestate.CanSurrender() ? ActionEnum.R : ActionEnum.S;
        private static ActionEnum P(GameState gamestate) => ActionEnum.P;
        private static ActionEnum Rp(GameState gamestate) => gamestate.CanSurrender() ? ActionEnum.R : ActionEnum.P;

        //It is unclear if (0, H) means hit at or above 0, or hit at or below 0, so for 0-based deviations, use methods like this
        private static (int, Func<GameState, ActionEnum>) Hlte0 => (0, (GameState gs) => gs.TrueCount <= 0 ? ActionEnum.H : ActionEnum.S);
        private static (int, Func<GameState, ActionEnum>) Sgte0 => (0, (GameState gs) => gs.TrueCount >= 0 ? ActionEnum.S : ActionEnum.H);
    }

    public enum ActionEnum
    {
        H, D, S, R, P, _
    }

    public class PlayingStrategy
    {
        public Func<GameState, ActionEnum>[][] HardHandStrategy { get; set; }
        public Func<GameState, ActionEnum>[][] SoftHandStrategy { get; set; }
        public Func<GameState, ActionEnum>[][] PairsStrategy { get; set; }
        public (int AtTrueCount, Func<GameState, ActionEnum> Deviation)?[][] HardHandDeviations { get; set; }

        public PlayingStrategy(Func<GameState, ActionEnum>[][] hardhand, Func<GameState, ActionEnum>[][] softhand, Func<GameState, ActionEnum>[][] pairs)
        {
            HardHandStrategy = hardhand;
            SoftHandStrategy = softhand;
            PairsStrategy = pairs;
        }

        public ActionEnum GetAction(GameState gamestate)
        {
            var dealerupcardindex = GetDealerUpcardIndex(gamestate.DealerUpcard);
            if (gamestate.CanSplit())
            {
                return PairsStrategy[GetSplitRowIndex(gamestate.PlayerHand)][dealerupcardindex](gamestate);
            }
            else if (gamestate.PlayerHand.IsSoft)
            {
                return SoftHandStrategy[GetSoftHandRowIndex(gamestate.PlayerHand)][dealerupcardindex](gamestate);
            }
            else
            {
                var rowindex = GetHardHandRowIndex(gamestate.PlayerHand);
                var deviation = HardHandDeviations?[rowindex][dealerupcardindex];

                if (deviation != null && UseDeviation(deviation.Value.AtTrueCount, gamestate.TrueCount))
                    return deviation.Value.Deviation(gamestate);
                else
                    return HardHandStrategy[rowindex][dealerupcardindex](gamestate);
            }
        }

        private static int GetDealerUpcardIndex(Card upcard) => upcard.Rank == "A" ? 9 : upcard.Value - 2;
        private static int GetSplitRowIndex(IHand hand) => hand.Cards[0].Rank == "A" ? 9 : hand.Cards[0].Value - 2;
        private static int GetSoftHandRowIndex(IHand hand) => hand.Value - 13;
        private static int GetHardHandRowIndex(IHand hand) => hand.Value - 4;
        private bool UseDeviation(int attruecount, int truecount) => attruecount == 0 || (attruecount < 0 && truecount <= attruecount) || (attruecount > 0 && truecount >= attruecount);
    }
}
