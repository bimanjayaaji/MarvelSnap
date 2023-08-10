using MarvelSnap;
using MarvelSnapInterface;
using MarvelSnapTools;
namespace MainProgram;

partial class Program
{
	public static void GameDisplay1_Testing(GameRunner gameRunner) //board
	{
		List<Location> revealLoc = gameRunner.RevealLocation();
		
		// Printing
		Tools.Println($"Round {gameRunner.CheckCurrentRound()}");
		Tools.SmallSpace();
		foreach (Location loc in gameRunner.GetLocations())
		{
			if (revealLoc.Contains(loc))
			{
				Tools.Println($"	Location {revealLoc.IndexOf(loc) + 1} : {loc.GetName()}. {loc.GetDesc()}");
				DisplayLoc(gameRunner, loc);
			}
			else
			{
				Tools.Println($"	Location {revealLoc.IndexOf(loc)+1} :");
				DisplayLoc(gameRunner, loc);
			}
		}
	}

	private static void DisplayLoc(GameRunner gameRunner, Location loc) // display board's location
	{
		foreach (var player in gameRunner.GetLocationCards(loc))
		{
			Tools.Print($"		({gameRunner.GetLocationScore(loc, player.Key)}) {player.Key.GetName()}'s Cards : ");
			foreach (var card in player.Value)
			{
				Tools.Print($"{card.GetName()},");
			}
			Tools.SmallSpace();
		}
		Tools.SmallSpace();
	}

	public static void GameDisplay2_Testing(GameRunner gameRunner, IPlayer player) //player's deck
	{
		List<Card> playerCards = gameRunner.GetPlayerCards(player);
		Tools.Println($"{player.GetName()} ==> Energy : {gameRunner.GetPlayerEnergy(player)}");
		Tools.Println("Cards :");
		Tools.Println("| idx | NAME / COST / ATTACK / DESCRIPTION");
		foreach (var card in playerCards)
		{
			Tools.Println($"|  {playerCards.IndexOf(card)+1}  | {card.GetName()} / {card.GetEnergyCost()} / {card.GetAttackingPower()} / {card.GetDesc()}");
		}
	}

	public static void GameDisplay3_Testing(GameRunner gameRunner) //winner
	{
		Console.Clear();
		Tools.Println("----- GAME'S DONE -----");
		Tools.Println($"Winner : {gameRunner.GetWinner()}");
		Tools.BigSpace();
		
		foreach (var kvp in gameRunner.GetLocationWinner())
		{
			if (kvp.Value == null)
			{
				Tools.Print($"Location : {kvp.Key.GetName()} ---> Winner : DRAW ");		
				Tools.Println($"({gameRunner.GetLocationScore(kvp.Key, gameRunner.GetPlayers()[0])} / {gameRunner.GetLocationScore(kvp.Key, gameRunner.GetPlayers()[1])})");
			} 
			else
			{
				Tools.Print($"Location : {kvp.Key.GetName()} ---> Winner : {kvp.Value.GetName()} ");
				Tools.Println($"({gameRunner.GetLocationScore(kvp.Key, gameRunner.GetPlayers()[0])} / {gameRunner.GetLocationScore(kvp.Key, gameRunner.GetPlayers()[1])})");
			}
			
		}
		Console.ReadKey();
	}
}