using MarvelSnap;
using MarvelSnapInterface;
namespace MainProgram;

class Program
{
	static void Main(string[] args)
	{
		Players_Test();
		AddSpace();
		
		InitGame_Test();
		AddSpace();
	}
	
	static void Players_Test() // scratching
	{
		GameRunner gameRunner = new();
		
		// ADD PLAYERS
		
		IPlayer player1 = new HumanPlayer("Dora");
		IPlayer player2 = new HumanPlayer("Boots");
		
		gameRunner.AddPlayer(player1);
		gameRunner.AddPlayer(player2);
		
		// GET-SET NAME-ID
		
		// Console.WriteLine(player1.SetName("Dora")); // false. still Dora
		// Console.WriteLine(player1.SetName("Boots")); // false. still Dora
		// Console.WriteLine(player1.SetId(1)); // false. still 1
		// Console.WriteLine(player1.SetId(2)); // false. still 1 
		// Console.WriteLine(player1.SetId(3)); Console.WriteLine(player2.SetId(1)); // true, true. 1 already been deleted 
		
		// GET PLAYERS
		
		List<IPlayer> players = gameRunner.GetPlayers();
		foreach (IPlayer p in players)
		{
			Console.WriteLine("ID : " + p.GetId() + " | Name : " + p.GetName());
		}
	}
	
	static void InitGame_Test() // scratching
	{
		GameRunner gameRunner = new();

		// CHECK ROUND
		
		Console.WriteLine("Current Round : " + gameRunner.CheckCurrentRound());
		
		// GET ALL CARDS
		
		List<Card> allCards = gameRunner.GetAllCards();
		
		// GET ALL LOCATIONS
		
		List<Location>? allLocations = gameRunner.GetAllLocations();
	}
	
	static void AddSpace()
	{
		Console.WriteLine("");
		Console.WriteLine("# # # # # # # # # #");
		Console.WriteLine("");
	}
}
