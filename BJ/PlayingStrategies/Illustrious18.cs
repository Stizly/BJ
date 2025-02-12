namespace BJ.PlayingStrategies
{
	public static partial class PlayingStrategies
	{
		public static readonly (int, Func<GameState, ActionEnum>)?[][] Deviations_HardHand =
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

		public static readonly (int, Func<GameState, ActionEnum>)?[][] Deviations_Pairs =
		[
            //  2       3       4       5       6       7       8       9       10      A
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //2,2
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //3,3
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //4,4
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //5,5
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //6,6
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //7,7
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //8,8
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //9,9
            [   null,   null,   null,   (+5,P), (+4,P), null,   null,   null,   null,   null    ],  //10,10
            [   null,   null,   null,   null,   null,   null,   null,   null,   null,   null    ],  //A,A
        ];

		public static bool TakeInsuranceDeviation(GameState gamestate) => gamestate.TrueCount >= 3;
	}
}
