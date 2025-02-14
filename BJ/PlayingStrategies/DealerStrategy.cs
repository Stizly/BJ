namespace BJ.PlayingStrategies
{
	public static partial class PlayingStrategies
	{
		public static readonly Func<GameState, ActionEnum>[][] DealerHardHand =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //4
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //5
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //6
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //7
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //8
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //9
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //10
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //11
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //12
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //13
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //14
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //15
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //16
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //17
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //18
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21
        ];
		public static readonly Func<GameState, ActionEnum>[][] DealerSoftHand_H17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //12 AA     
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //13 A2
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //14 A3
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //15 A4
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //16 A5
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //17 A6
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //18 A7
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19 A8
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20 A9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21 A10
        ];
		public static readonly Func<GameState, ActionEnum>[][] DealerSoftHand_S17 =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //12 AA     
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //13 A2
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //14 A3
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //15 A4
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],  //16 A5
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //17 A6
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //18 A7
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //19 A8
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],  //20 A9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ]   //21 A10
        ];
		public static readonly Func<GameState, ActionEnum>[][] DealerPairs =
		[
            //  2   3   4   5   6   7   8   9   10  A
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //2,2
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //3,3
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //4,4
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //5,5
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //6,6
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //7,7
            [   H,  H,  H,  H,  H,  H,  H,  H,  H,  H   ],   //8,8
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //9,9
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //10,10
            [   S,  S,  S,  S,  S,  S,  S,  S,  S,  S   ],   //A,A
        ];
		public static readonly Func<GameState, ActionEnum>[][][] DealerStrategy_H17 = [DealerHardHand, DealerSoftHand_H17, DealerPairs];
		public static readonly Func<GameState, ActionEnum>[][][] DealerStrategy_S17 = [DealerHardHand, DealerSoftHand_S17, DealerPairs];
	}

	public class DealerStrategy_H17 : PlayingStrategy
	{
		public DealerStrategy_H17() : base(PlayingStrategies.DealerStrategy_H17) { }
	}
	public class DealerStrategy_S17 : PlayingStrategy
	{
		public DealerStrategy_S17() : base(PlayingStrategies.DealerStrategy_S17) { }
	}
}
