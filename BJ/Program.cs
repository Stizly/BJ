using BJ;
using BJ.PlayingStrategies;

const decimal INITIALBANKROLL = 2000;
const int ROUNDS = 1000;
const int ROUNDSPERHOUR = 100;
const int CONCURRENTPLAYERS = 10000;
const int BETTINGUNIT = 1;
const int DECKPEN = 70;
const int SHOESIZE = 2;
const decimal BJPAYOUT = 1.5m;
const int MAXHANDS = 4;

Console.WriteLine("Barebones Blackjack Simulator!");


var rules = new BlackjackRules(SHOESIZE, DECKPEN)
{
	IsSurrenderAllowed = false,
	BlackjackPayout = BJPAYOUT,
	DAS = true,
	CanHitAcesAfterSplit = false,
	DealerHitsSoft17 = true,
	PairSplitLimit = MAXHANDS - 1,
	CanResplitAces = false
};

List<decimal> profits = [];
int bankruptcount = 0;
decimal maxprofit = 0;
Parallel.For(0, CONCURRENTPLAYERS, i =>
{
	var table = new Table(rules);
	var player = new Player(
		INITIALBANKROLL,
		//new BettingStrategy(BettingStrategies.BuildBetSpread([0, 0, 25], [25, 25, 50, 75, 125, 150, 200]), [1, 1, 2]),
		new BettingStrategy(BettingStrategies.BuildBetSpread([0, 0, 10], [10, 10, 20, 30, 40, 50]), [1, 2]),
		new PlayingStrategy_DD_H17().UseDeviations()
	);

	var bjplayer = new BJPlayer(table, player);

	var currentbankroll = PlayBlackjack(bjplayer, BETTINGUNIT, ROUNDS);
	var currentprofit = currentbankroll - INITIALBANKROLL;
	profits.Add(currentprofit);

	if (currentprofit < -INITIALBANKROLL)
		bankruptcount++;
	else if (currentprofit > maxprofit)
		maxprofit = currentprofit;

	Console.WriteLine($"Iteration {i} profits: ${currentprofit}.");
});

var averageprofit = profits.Average();
var stddev = Math.Sqrt(profits.Average(p => Math.Pow((double)(p - averageprofit), 2)));
Console.WriteLine($"Average profits: ${averageprofit:n2} over {ROUNDS * CONCURRENTPLAYERS:n0} rounds and {ROUNDS / ROUNDSPERHOUR} hours.");
Console.WriteLine($"\tStandard Deviation: {stddev:n2}");


Console.WriteLine($"Average profit/hour: ${averageprofit / ROUNDS * ROUNDSPERHOUR}");
Console.WriteLine($"Bankruptices: {bankruptcount} for a {bankruptcount * 100 / (decimal)CONCURRENTPLAYERS}% RoR.");
Console.WriteLine($"Max profit: ${maxprofit}");

Console.WriteLine("Press any key to end.");
Console.ReadKey();

static decimal PlayBlackjack(BJPlayer bjplayer, int bettingunit, int rounds)
{
	var player = bjplayer.Player;
	var bj = bjplayer.Table;

	for (int i = 0; i < rounds; i++)
	{
		if (player.BankRoll < 0)
			return player.BankRoll;

		var (bet, hands) = player.GetBetAmount((int)bjplayer.Table.GetTrueCount());
#if DEBUG
		Console.WriteLine($"Betting ${bet} on {hands} hands.");
#endif

		player.AddHands(hands);

		bj.Deal(player.Hands);
		bjplayer.Play(bet);
		bj.ClearHands(player);
		if (bj.DoReshuffleShoe())
		{
#if DEBUG
			Console.WriteLine($"Refilling and shuffling shoe!");
#endif
			bj.RefillShoe();
		}

#if DEBUG
		Console.WriteLine($"Player's new Bankroll is ${player.BankRoll}.");
		Console.WriteLine($"RC is {bj.RunningCount} and TC is {bj.GetTrueCount()}.\n\n");
#endif
	}

	return player.BankRoll;
}