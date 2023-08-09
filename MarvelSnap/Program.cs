using MarvelSnap;
using MarvelSnapEnum;
using MarvelSnapInterface;
using MarvelSnapTools;
namespace MainProgram;

partial class Program
{
	static void Main(string[] args)
	{		
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
		
		// ENTER PLAYER'S IDENTITY
		string name;
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
						Tools.Print("Insert Card's index to place Card (0 to pass turn): ");
						int cardIndex = 0;
						bool marker = int.TryParse(Tools.Readln(), out cardIndex);
						while(!marker || cardIndex > gameRunner.GetPlayerCards(player).Count)
						{
							Tools.Println("Invalid");
							Tools.Print("Insert Card's index to place Card (0 to pass turn): ");
							marker = int.TryParse(Tools.Readln(), out cardIndex);
						};
						
						if (cardIndex == 0)
						{
							endTurn = true;
							break;
						}
						
						if (gameRunner.CheckCardValid(player, cardIndex))
						{
							Tools.Print("Insert Location's index to place Card : ");
							int locIndex = 0; 
							bool marker1 = int.TryParse(Tools.Readln(), out locIndex);
							bool locCond = gameRunner.CheckLocFull(locIndex,player);
							while(!marker1 || !locCond || locIndex > gameRunner.GetLocations().Count ||
									locIndex < 1)
							{
								Tools.Println("Invalid or Locations is full (4 max)");
								Tools.Print("Insert Location's index to place Card : ");
								marker1 = int.TryParse(Tools.Readln(), out locIndex);
								locCond = gameRunner.CheckLocFull(locIndex,player);
							};
							gameRunner.PlayerPlaceCard(player, cardIndex, locIndex); // delegate
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
	
	static void GameDisplay1_Testing(GameRunner gameRunner) //board
	{
		List<Location> revealLoc = gameRunner.RevealLocation();
		
		// Printing
		Tools.Println($"Round {gameRunner.CheckCurrentRound()}");
		Tools.SmallSpace();
		foreach (Location loc in gameRunner.GetLocations())
		{
			if (revealLoc.Contains(loc))
			{
				Tools.Println($"	Location {revealLoc.IndexOf(loc)+1} : {loc.GetName()}. {loc.GetDesc()}"); 
				foreach (var player in gameRunner.GetLocationCards(loc))
				{
					Tools.Print($"		({gameRunner.GetLocationScore(loc,player.Key)}) {player.Key.GetName()}'s Cards : ");
					foreach (var card in player.Value)
					{
						Tools.Print($"{card.GetName()},");
					}
					Tools.SmallSpace();
				}
				Tools.SmallSpace();				
			}
			else
			{
				Tools.Println($"	Location {revealLoc.IndexOf(loc)+1} :");
				foreach (var player in gameRunner.GetLocationCards(loc))
				{
					Tools.Print($"		({gameRunner.GetLocationScore(loc,player.Key)}) {player.Key.GetName()}'s Cards : ");
					foreach (var card in player.Value)
					{
						Tools.Print($"{card.GetName()},");
					}
					
					Tools.SmallSpace();	
				}
				Tools.SmallSpace();
			}
		}
	}
	
	static void GameDisplay2_Testing(GameRunner gameRunner, IPlayer player) //player's deck
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

	static void GameDisplay3_Testing(GameRunner gameRunner) //winner
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