using MarvelSnap;
using MarvelSnapInterface;
namespace MainProgram;

class Program
{
	static void Main(string[] args)
	{
		GameRunner gameRunner = new GameRunner();
		
		// ADD PLAYERS
		
		IPlayer player1 = new HumanPlayer();
		player1.SetId(1);
		player1.SetName("Dora");
		
		IPlayer player2 = new HumanPlayer();
		player2.SetId(2);
		player2.SetName("Boots");
		
		gameRunner.AddPlayer(player1);
		gameRunner.AddPlayer(player2);
		
		// GET PLAYERS
		
		List<IPlayer> players = gameRunner.GetPlayers();
		foreach (IPlayer p in players)
		{
			Console.WriteLine("Player ID : " + p.GetId());
			Console.WriteLine("Player Name : " + p.GetName());
			Console.WriteLine("---");
		}
		
		// CHECK ROUND
		
		Console.WriteLine("Current Round : " + gameRunner.CheckCurrentRound());
		Console.WriteLine("---");
	}
}
