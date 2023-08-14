using MarvelSnap;
using MarvelSnapEnum;
using MarvelSnapInterface;
using MarvelSnapTools;
using NLog;
namespace MainProgram;

partial class Program
{
	private static Logger? logger;

	static void Main()
	{
		var nlogConfigPath = Path.Combine(Directory.GetCurrentDirectory(), ".\\Log\\nlog.config");
		// LogManager.Setup().LoadConfigurationFromFile(nlogConfigPath);
		LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(nlogConfigPath);
		logger = LogManager.GetCurrentClassLogger();
		
		logger.Info("asdasd");
		Console.ReadKey();
		
		GamePlay_Testing();
	}

	static void GamePlay_Testing()
	{
		// DECLARATION
		GameRunner gameRunner = new();
		List<Location> locations = new();

		// INTRO
		Console.Clear();
		Console.WriteLine("--- WELCOME TO MARVELSNAP! ---"); 
		Tools.SmallSpace();
		
		logger?.Info("asdasdasd");
		
		// ENTER PLAYER'S IDENTITY
		string? name;
		IPlayer player1, player2;
		do 
		{
			Console.Write("Input first player's name : ");	
			name = Console.ReadLine();
			player1 = new HumanPlayer(name);
		} while (name == "" || player1.GetName() == "");
		// IPlayer player1 = new HumanPlayer(name);
		gameRunner.AddPlayer(player1);
		
		do
		{
			Console.Write("Input second player's name : ");
			name= Console.ReadLine();
			player2 = new HumanPlayer(name);
		} while (name == "" || player2.GetName() == "");
		// IPlayer player2 = new HumanPlayer(name);
		gameRunner.AddPlayer(player2);
		
		Tools.SmallSpace();
		Console.WriteLine($"Welcome {player1.GetName()} and {player2.GetName()}");
		Tools.SmallSpace();
		
		// INITIALIZING - Locations
		gameRunner.SetLocations();
		
		// START ROUND
		Tools.Println("Let's Play! (Press any key to start the game)");
		Tools.BigSpace();
		Console.ReadKey();
		
		gameRunner.GoNextRound();
		while (gameRunner.CheckGameStatus() == GameStatus.ONGOING)
		{
			// Set Cards to Player for each round
			gameRunner.SetCardsToPlayer(player1,gameRunner.CheckCurrentRound());
			gameRunner.SetCardsToPlayer(player2,gameRunner.CheckCurrentRound());
			
			Console.Clear();
			
			// Players place card
			foreach (IPlayer player in gameRunner.GetPlayers())
			{
				bool placeAgain = true;
				while (placeAgain)
				{
					GameDisplay1_Testing(gameRunner); Tools.BigSpace();
					GameDisplay2_Testing(gameRunner, player); Tools.BigSpace();

					Tools.Println(player.GetName());

					bool cardValid = false;
					bool endTurn = false;
					while (!cardValid)
					{
						bool marker;
						int cardIndex;
						do
						{
							Tools.Print("Insert Card's index to place Card (0 to pass turn): ");
							marker = int.TryParse(Tools.Readln(), out cardIndex);
							if (!marker || cardIndex > gameRunner.GetPlayerCards(player).Count)
							{
								Tools.Println("Invalid");
								marker = false;
							}
						} while(!marker);

						if (cardIndex == 0)
						{
							endTurn = true;
							break;
						}

						if (gameRunner.CheckCardValid(player, cardIndex))
						{
							int locIndex;
							bool marker1;
							bool locCond;
							do
							{
								Tools.Print("Insert Location's index to place Card (0 to pass turn): ");
								marker1 = int.TryParse(Tools.Readln(), out locIndex);
								locCond = gameRunner.CheckLocFull(locIndex,player);
								if (!marker1 || !locCond || locIndex > gameRunner.GetLocations().Count)
								{
									if (locIndex == 0){break;}
									Tools.Println("Invalid or Locations is full (4 max)");
									marker1 = false;
								}	
							} while(!marker1);
							
							if (locIndex == 0)
							{
								endTurn = true;
								break;
							}

							gameRunner.PlayerPlaceCard(player, cardIndex, locIndex);
							cardValid = true;
						}
						else
						{
							Tools.Println("Energy not enough!"); 
						}
					}
					
					gameRunner.ApplyOnGoingLocs();
					
					if (endTurn)
					{
						Console.Clear();
						break;
					}
					
					Console.Clear();
					GameDisplay1_Testing(gameRunner); Tools.BigSpace();
					GameDisplay2_Testing(gameRunner, player); Tools.BigSpace();
					
					Tools.Print("Place Again? (y/n)");
					string? again = null;
					bool valid = false;
					do
					{
						again = Tools.Readln();
						if (again == "n")
						{
							placeAgain = false;
							break;
						}
						else if (again == "y")
						{
							break;
						}
						else
						{
							Tools.Println("Invalid");
						}
					}
					while(!valid);
					
					Console.Clear();
				} 
			}
			
			gameRunner.GoNextRound();
		}
		
		GameDisplay3_Testing(gameRunner);
	}
}