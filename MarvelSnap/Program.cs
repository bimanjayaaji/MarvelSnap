using MarvelSnap;
using MarvelSnapEnum;
using MarvelSnapInterface;
using MarvelSnapTools;
namespace MainProgram;

class Program
{
	static void Main(string[] args)
	{
		// Players_Testing();
		// Tools.BigSpace();
		
		// InitGame_Testing();
		// Tools.BigSpace();
		
		GamePlay_Testing();
		
		// Serialization.Serialization.SerializeCards();
		// Serialization.Serialization.Deserialize();
		// Serialization.Serialization.SerializeLocs();
	}
	
	static void Players_Testing()
	{
		GameRunner gameRunner = new();
		
		// ADD PLAYERS
		
		IPlayer player1 = new HumanPlayer("Dora");
		IPlayer player2 = new HumanPlayer("Dora");
		
		Console.WriteLine(gameRunner.AddPlayer(player1));
		Console.WriteLine(gameRunner.AddPlayer(player2));
		
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
	
	static void InitGame_Testing()
	{
		GameRunner gameRunner = new();

		// CHECK ROUND
		
		Console.WriteLine("Current Round : " + gameRunner.CheckCurrentRound());
		Tools.SmallSpace();
		
		// GET ALL CARDS
		
		List<Card> allCards = gameRunner.GetAllCards();
		Console.WriteLine("All Generated Cards : ");
		foreach (Card card in allCards)
		{
			Console.WriteLine(card.GetName());
		}
		Tools.SmallSpace();
		
		// GET ALL LOCATIONS
		
		List<Location>? allLocations = gameRunner.GetAllLocations();
		Console.WriteLine("All Generated Locations : ");
		foreach (Location location in allLocations)
		{
			Console.WriteLine(location.GetName());
		}
		Tools.SmallSpace();
		
		// SET CARDS TO PLAYER
		
		IPlayer player1 = new HumanPlayer("Dora");
		IPlayer player2 = new HumanPlayer("Boots");
		gameRunner.AddPlayer(player1);
		gameRunner.AddPlayer(player2);
		
		// player1-round1
		gameRunner.SetCardsToPlayer(player1,gameRunner.CheckCurrentRound()+1);
		// player2-round1
		gameRunner.SetCardsToPlayer(player2,gameRunner.CheckCurrentRound()+1);
		// player1-round2
		gameRunner.SetCardsToPlayer(player1,gameRunner.CheckCurrentRound()+2);
		
		// GET CARDS
		
		// all players-all cards
		foreach (KeyValuePair<IPlayer, List<Card>> playerCards in gameRunner.GetPlayerCards())
		{
			Console.WriteLine(playerCards.Key.GetName() + "'s Cards : (using GetCards() with no params)");
			foreach (Card card in playerCards.Value)
			{
				Console.WriteLine(card.GetName());
			}
			Tools.SmallSpace();
		}
		
		// particular player's cards
		Console.WriteLine(player1.GetName() + "'s Cards : (using GetCards() with player param)");
		foreach(var x in gameRunner.GetPlayerCards(player1))
		{
			Console.WriteLine(x.GetName());
		}
		Tools.SmallSpace();
		
		// SET LOCATIONS
		
		gameRunner.SetLocations();
		
		// GET LOCATIONS
		
		Console.WriteLine("Chosen Random Locations : ");
		foreach (var location in gameRunner.GetLocations())
		{
			Console.WriteLine(location.GetName());	
		}
		Tools.SmallSpace();
		
		
		
	}

	static void GamePlay_Testing()
	{
		// DECLARATION
		GameRunner gameRunner = new();
		List<Location> locations = new();
		
		// INTRO
		Console.WriteLine("--- WELCOME TO MARVELSNAP! ---"); 
		Tools.SmallSpace();
		
		// ENTER PLAYER'S IDENTITY
		Console.Write("Input first player's name : ");
		IPlayer player1 = new HumanPlayer("Dora");
		gameRunner.AddPlayer(player1);
		Console.Write("Input second player's name : ");
		IPlayer player2 = new HumanPlayer("Boots");
		gameRunner.AddPlayer(player2);
		Console.WriteLine($"Welcome {player1.GetName()} and {player2.GetName()}");
		Tools.SmallSpace();
		
		// INITIALIZING - Locations
		gameRunner.SetLocations();
		
		// START ROUND
		Tools.Println("Let's Play!");
		Tools.BigSpace();
		gameRunner.GoNextRound();
		while (gameRunner.CheckGameStatus() == GameStatus.ONGOING)
		{
			// Set Cards to Player for each round
			gameRunner.SetCardsToPlayer(player1,gameRunner.CheckCurrentRound());
			gameRunner.SetCardsToPlayer(player2,gameRunner.CheckCurrentRound());
			
			// Display Locations and Players's cards
			// GameDisplay1_Testing(gameRunner);
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
							gameRunner.PlayerPlaceCard(player, cardIndex, locIndex);
							cardValid = true;
						}
						else
						{
							Tools.Println("Energy not enough!"); 
						}
					}
					
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
			
			// Go Next Round
			// gameRunner.ApplyOnRevealCards(); // !!! on progress
			// gameRunner.ApplyOnGoingCards(); // !!! on progress
			gameRunner.GoNextRound();
		}
		
		//determine winner
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
		Tools.Println($"Winner : {gameRunner.DetermineWinner()}");
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