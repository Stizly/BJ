namespace BJ.PlayingStrategies
{
	public static partial class PlayingStrategies
	{
		/// <summary>
		/// Strategy from https://wizardofodds.com/games/blackjack/strategy/calculator/
		/// 4 or more decks, Hit soft 17, DAS allowed, Surrender allowed, Dealer Peeks
		/// </summary>
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_HardHand_Shoe_H17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //4    4 is here in case I they cannot split a 4 for whatever reason.
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //5
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //6
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //7
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //8
            [   H,  Dh, Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //9
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],  //10
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh  ],  //11
            [   H,  H,  S,  S,  S,  H,  H,  H,  H,  H   ],  //12
            [   S,  S,  S,  S,  S,  H,  H,  H,  H,  H   ],  //13
            [   S,  S,  S,  S,  S,  H,  H,  H,  H,  H   ],  //14
            [   S,  S,  S,  S,  S,  H,  H,  H,  Rh, Rh  ],  //15
            [   S,  S,  S,  S,  S,  H,  H,  Rh, Rh, Rh  ],  //16
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  Rs  ],  //17
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //18
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21
        ];
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_SoftHand_Shoe_H17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  S,  S,  H,  H,  H,  H,  H   ],  //12 AA     ONLY IN CASE OF SPLIT ACES MULTIPLE TIMES TO LIMIT
            [   H,  H,  H,  Dh, Dh, H,  H,  H,  H,  H   ],  //13 A2
            [   H,  H,  H,  Dh, Dh, H,  H,  H,  H,  H   ],  //14 A3
            [   H,  H,  Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //15 A4
            [   H,  H,  Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //16 A5
            [   H,  Dh, Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //17 A6
            [   Ds, Ds, Ds, Ds, Ds, S,  S,  H,  H,  H   ],  //18 A7
            [   S,  S,  S,  S,  Ds, S,  S,  S,  S,  S   ],  //19 A8
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20 A9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21 A10
        ];
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_Pairs_Shoe_DAS_H17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //2,2
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //3,3
            [   H,  H,  H,  P,  P,  H,  H,  H,  H,  H   ],   //4,4
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],   //5,5
            [   P,  P,  P,  P,  P,  H,  H,  H,  H,  H   ],   //6,6
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //7,7
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  Rp  ],   //8,8
            [   P,  P,  P,  P,  P,  S,  P,  P,  S,  S   ],   //9,9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //10,10
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  P   ],   //A,A
        ];
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_Pairs_Shoe_NDAS =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  P,  P,  P,  P,  H,  H,  H,  H   ],   //2,2
            [   H,  H,  P,  P,  P,  P,  H,  H,  H,  H   ],   //3,3
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //4,4
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],   //5,5
            [   H,  P,  P,  P,  P,  H,  H,  H,  H,  H   ],   //6,6
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //7,7
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  Rp  ],   //8,8
            [   P,  P,  P,  P,  P,  S,  P,  P,  S,  S   ],   //9,9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //10,10
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  P   ],   //A,A
        ];

		/// <summary>
		/// Strategy from https://wizardofodds.com/games/blackjack/strategy/calculator/
		/// 4 or more decks, stay soft 17, DAS allowed, Surrender allowed, Dealer Peeks
		/// </summary>
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_HardHand_Shoe_S17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //4    4 is here in case I they cannot split a 4 for whatever reason.
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //5
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //6
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //7
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //8
            [   H,  Dh, Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //9
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],  //10
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh  ],  //11
            [   H,  H,  S,  S,  S,  H,  H,  H,  H,  H   ],  //12
            [   S,  S,  S,  S,  S,  H,  H,  H,  H,  H   ],  //13
            [   S,  S,  S,  S,  S,  H,  H,  H,  H,  H   ],  //14
            [   S,  S,  S,  S,  S,  H,  H,  H,  Rh, H   ],  //15
            [   S,  S,  S,  S,  S,  H,  H,  Rh, Rh, Rh  ],  //16
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //17
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //18
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21
        ];
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_SoftHand_Shoe_S17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  S,  S,  H,  H,  H,  H,  H   ],  //12 AA     ONLY IN CASE OF SPLIT ACES MULTIPLE TIMES TO LIMIT
            [   H,  H,  H,  Dh, Dh, H,  H,  H,  H,  H   ],  //13 A2
            [   H,  H,  H,  Dh, Dh, H,  H,  H,  H,  H   ],  //14 A3
            [   H,  H,  Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //15 A4
            [   H,  H,  Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //16 A5
            [   H,  Dh, Dh, Dh, Dh, H,  H,  H,  H,  H   ],  //17 A6
            [   S,  Ds, Ds, Ds, Ds, S,  S,  H,  H,  H   ],  //18 A7
            [   S,  S,  S,  S,  Ds, S,  S,  S,  S,  S   ],  //19 A8
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20 A9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21 A10
        ];
		public static readonly Func<GameState, ActionEnum>[][] BasicStrategy_Pairs_Shoe_DAS_S17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //2,2
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //3,3
            [   H,  H,  H,  P,  P,  H,  H,  H,  H,  H   ],   //4,4
            [   Dh, Dh, Dh, Dh, Dh, Dh, Dh, Dh, H,  H   ],   //5,5
            [   P,  P,  P,  P,  P,  H,  H,  H,  H,  H   ],   //6,6
            [   P,  P,  P,  P,  P,  P,  H,  H,  H,  H   ],   //7,7
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  P   ],   //8,8
            [   P,  P,  P,  P,  P,  S,  P,  P,  S,  S   ],   //9,9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //10,10
            [   P,  P,  P,  P,  P,  P,  P,  P,  P,  P   ],   //A,A
        ];

		public static readonly Func<GameState, ActionEnum>[][][] BasicStrategy_Shoe_H17_DAS = [BasicStrategy_HardHand_Shoe_H17, BasicStrategy_SoftHand_Shoe_H17, BasicStrategy_Pairs_Shoe_DAS_H17];
	}

	public class PlayingStrategyShoe : PlayingStrategy
	{
		public PlayingStrategyShoe() : base(PlayingStrategies.BasicStrategy_Shoe_H17_DAS) { }
		public override PlayingStrategy NDAS()
		{
			PairsStrategy = PlayingStrategies.BasicStrategy_Pairs_Shoe_NDAS;
			IsDAS = false;
			return this;
		}
		public override PlayingStrategy DAS()
		{
			PairsStrategy = IsH17 ? PlayingStrategies.BasicStrategy_Pairs_Shoe_DAS_H17 : PlayingStrategies.BasicStrategy_Pairs_Shoe_DAS_S17;
			IsDAS = true;
			return this;
		}
		public override PlayingStrategy S17()
		{
			HardHandStrategy = PlayingStrategies.BasicStrategy_HardHand_Shoe_S17;
			SoftHandStrategy = PlayingStrategies.BasicStrategy_SoftHand_Shoe_S17;
			PairsStrategy = IsDAS ? PlayingStrategies.BasicStrategy_Pairs_Shoe_DAS_S17 : PlayingStrategies.BasicStrategy_Pairs_Shoe_NDAS;
			IsH17 = false;
			return this;
		}
		public override PlayingStrategy H17()
		{
			HardHandStrategy = PlayingStrategies.BasicStrategy_HardHand_Shoe_H17;
			SoftHandStrategy = PlayingStrategies.BasicStrategy_SoftHand_Shoe_H17;
			PairsStrategy = IsDAS ? PlayingStrategies.BasicStrategy_Pairs_Shoe_DAS_H17 : PlayingStrategies.BasicStrategy_Pairs_Shoe_NDAS;
			IsH17 = true;
			return this;
		}
	}
}
