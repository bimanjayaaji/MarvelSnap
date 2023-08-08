using  MarvelSnapInterface;
using MarvelSnapEnum;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace MarvelSnap;

public delegate bool WinningDelegate();

public class GameRunner
{
	private int _round;
	// private bool _isNewRound;
	private string? _winner;
	private GameStatus _gameStatus;
	private List<Card> _allCards;
	private List<Location>? _allLocations;
	private Dictionary<IPlayer,PlayerConfig> _playerInfo;
	private Dictionary<Location,LocationConfig> _locationInfo;
	private WinningDelegate? _winningEvent;
	
	public GameRunner()
	{
		_round = 0;
		// _isNewRound = false;
		_gameStatus = GameStatus.NOT_STARTED;
		_allCards = new();
		_allLocations = new();
		_playerInfo = new(); 
		_locationInfo = new();
		GenerateAllCards();
		GenerateAllLocations();
		_winningEvent += DetermineWinner;
	}
	
	/// <summary>
	/// Method to add new player in the game. Returns true when successfully add new player.
	/// You should make a new instance of a player first, then pass that object to this method
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public bool AddPlayer(IPlayer player)
	{
		if (_playerInfo.Count < 2)
		{
			if (!_playerInfo.ContainsKey(player))
			{
				foreach (IPlayer assigned in _playerInfo.Keys)
				{
					if (assigned.GetId() == player.GetId()) // checking if id's already assigned
					{
						return false;			
					}
				}
				PlayerConfig config = new();
				_playerInfo.Add(player, config);
				return true;
			}
		}
		return false;
	}
	
	/// <summary>
	/// Returns list of IPlayer objects thats been assigned
	/// </summary>
	/// <returns></returns>
	public List<IPlayer> GetPlayers()
	{
		List<IPlayer> players = new();
		foreach (var key in _playerInfo.Keys)
		{
			players.Add(key);
		}
		return players;
	}
	
	/// <summary>
	/// Returns current round
	/// </summary>
	/// <returns></returns>
	public int CheckCurrentRound()
	{
		return _round;
	}
	
	/// <summary>
	/// Generate all cards from .json file. Called only once in GR constructor
	/// </summary>
	/// <returns></returns>
	private bool GenerateAllCards() // called when generating new instance of GR
	{		
		var ser = new DataContractJsonSerializer(typeof(List<Card>));
		string currentFolder = Directory.GetCurrentDirectory();
		string fullPath = Path.Combine(currentFolder,@"Database\Cards.json");
		FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate);
		_allCards = (List<Card>)ser?.ReadObject(stream);
		
		return true;
	}
	
	/// <summary>
	/// Returns all cards that were generated from .json file
	/// </summary>
	/// <returns></returns>
	public List<Card> GetAllCards()
	{
		return _allCards;
	}
	
	/// <summary>
	/// Generate all locations from .json file. Called only once in GR constructor
	/// </summary>
	/// <returns></returns>
	private bool GenerateAllLocations() // called when generating new instance of GR
	{
		var ser = new DataContractJsonSerializer(typeof(List<Location>));
		string currentFolder = Directory.GetCurrentDirectory();
		string fullPath = Path.Combine(currentFolder,@"Database\Locations.json");
		FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate);
		_allLocations = (List<Location>)ser?.ReadObject(stream);
		return true;
	}
	
	/// <summary>
	/// Returns all locations generated from .json file
	/// </summary>
	/// <returns></returns>
	public List<Location>? GetAllLocations()
	{
		return _allLocations;
	}
	
	/// <summary>
	/// Gives particular player random cards according to the round. In round 1, players are given 4 cards.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="round"></param>
	/// <returns></returns>
	public bool SetCardsToPlayer(IPlayer player, int round)
	{
		PlayerConfig playerConfig = _playerInfo[player];
		Random random = new();
		List<int> randomList = new();

		if (round == 1) // when round 1, assign 4 lower cards
		{
			while (randomList.Count < 4)
			{
				int num = random.Next(0, 6);
				// should be (0,_allCards.Count), but set to 5 considering initial energyCost
				 
				if (!randomList.Contains(num))
				{
					randomList.Add(num);
				}
			}
		} 
		else // other than round 1, assign 1 card
		{
			int num = random.Next(0, _allCards.Count);
			randomList.Add(num);	
		}	
		
		foreach (var ind in randomList)
		{
			Card chosenCard = JsonConvert.DeserializeObject<Card>(JsonConvert.SerializeObject(_allCards[ind]));
			playerConfig.AddCardDeck(chosenCard);	
			// playerConfig.AddCardDeck(_allCards[ind]);	
		}
		
		return true;
	}
	
	/// <summary>
	/// Gives particular player a particular card
	/// </summary>
	/// <param name="player"></param>
	/// <param name="card"></param>
	/// <returns></returns>
	public bool SetCardsToPlayer(IPlayer player, Card card)
	{
		PlayerConfig playerConfig = _playerInfo[player];
		Card chosenCard = JsonConvert.DeserializeObject<Card>(JsonConvert.SerializeObject(card));
		playerConfig.AddCardDeck(chosenCard);
		
		return true;
	}
	
	/// <summary>
	/// Returns all cards that are owned by all players in Dictionary
	/// </summary>
	/// <returns></returns>
	public Dictionary<IPlayer, List<Card>> GetPlayerCards()
	{
		Dictionary<IPlayer, List<Card>> playerCards = new();
		
		foreach (KeyValuePair<IPlayer, PlayerConfig> player in _playerInfo)
		{
			playerCards.Add(player.Key, player.Value.GetCardDeck());
		}
		
		return playerCards;
	}
	
	/// <summary>
	/// Returns all cards that are owned by particular player
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public List<Card> GetPlayerCards(IPlayer player)
	{
		return _playerInfo[player].GetCardDeck();
	}
	
	/// <summary>
	/// Returns player's energy in dictionary form
	/// </summary>
	/// <returns></returns>
	public Dictionary<IPlayer,int> GetPlayerEnergy()
	{
		Dictionary<IPlayer,int> energy = new();
		
		foreach (var player in _playerInfo)
		{
			energy.Add(player.Key,player.Value.GetEnergyTotal());
		}
		
		return energy;
	}
	
	/// <summary>
	/// Returns particular player's energy
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public int GetPlayerEnergy(IPlayer player)
	{
		return _playerInfo[player].GetEnergyTotal();
	}
	
	/// <summary>
	/// Returns true after successfully assign 3 random locations to be played on in the game
	/// </summary>
	/// <returns></returns>
	public bool SetLocations()
	{
		Random random = new();
		List<Location> randomLoc = new();
		
		while (randomLoc.Count < 3)
		{
			int num = random.Next(0, _allLocations.Count);
				
			if (!randomLoc.Contains(_allLocations[num]))
			{
				randomLoc.Add(_allLocations[num]);
			}
		}
		
		foreach (Location location in randomLoc)
		{
			LocationConfig config = new();
			config.InitLocPlayer(GetPlayers());
			_locationInfo.Add(location, config);
		}
		
		return true;
	}
	
	/// <summary>
	/// Returns true after successfully set customized locations from the user to be played on in the game
	/// </summary>
	/// <param name="locList"></param>
	/// <returns></returns>
	public bool SetLocations(List<Location> locList)
	{
		foreach (var loc in locList)
		{
			LocationConfig config = new();
			_locationInfo.Add(loc, config);
		}
		return true;
	}
	
	/// <summary>
	/// Returns list of all locations that were chosen and stored in _locationInfo variable
	/// </summary>
	/// <returns></returns>
	public List<Location> GetLocations()
	{
		return _locationInfo.Keys.ToList();
	}
	
	/// <summary>
	/// Returns dictionary of location and its locationConfig
	/// </summary>
	/// <returns></returns>
	public Dictionary<Location,LocationConfig> GetLocationInfo()
	{
		return _locationInfo;
	}
	
	/// <summary>
	/// Returns cards that are already placed on all locations by all players in dictionary form
	/// </summary>
	/// <param name="loc"></param>
	/// <returns></returns>
	public Dictionary<IPlayer,List<Card>> GetLocationCards(Location loc)
	{
		return _locationInfo[loc].GetLocInfo();
		// add exception/warning if the loc argument doesn't exist in _locationInfo	
	}
	
	/// <summary>
	/// Returns list of cards that's been placed by a particular player in particular location
	/// </summary>
	/// <param name="loc"></param>
	/// <param name="player"></param>
	/// <returns></returns>
	public List<Card> GetPlayerCardsOnLoc(Location loc, IPlayer player)
	{
		return _locationInfo[loc].GetPlayerCardsOnLoc(player);
	}
	
	/// <summary>
	/// Returns all scores of all players from all locations in Dictionary form. Not recommended to be used.
	/// Use the overload method instead.
	/// </summary>
	/// <returns></returns>
	public Dictionary<Location,Dictionary<IPlayer,int>> GetLocationScore()
	{
		Dictionary<Location,Dictionary<IPlayer,int>> locScore = new();
		foreach (KeyValuePair<Location,LocationConfig> locInfo in _locationInfo)
		{
			locScore.Add(locInfo.Key, locInfo.Value.GetLocScore());
		}
		
		return locScore;
		// add exception/warning if _locationInfo is still Empty
		// TINGGAL MANGGIL GETLOCATIONSCORE(...) TO NJAY! ^-^
 	}
	
	/// <summary>
	/// Returns players scores in particular location
	/// </summary>
	/// <param name="loc"></param>
	/// <returns></returns>
	public Dictionary<IPlayer,int> GetLocationScore(Location loc)
	{
		return _locationInfo[loc].GetLocScore();
		// add exception/warning if the loc argument doesn't exist in _locationInfo
	}
	
	/// <summary>
	/// Return the score of particular player in particular location
	/// </summary>
	/// <param name="loc"></param>
	/// <param name="player"></param>
	/// <returns></returns>
	public int GetLocationScore(Location loc, IPlayer player)
	{
		return _locationInfo[loc].GetLocScore()[player];
	}
	
	/// <summary>
	/// Returns players's scores in particular location. This method differs from its brothers in a way that
	/// it takes location index that was chosen by user/player as an argument
	/// </summary>
	/// <param name="locIndex"></param>
	/// <returns></returns>
	public Dictionary<IPlayer,int> GetLocationScore(int locIndex)
	{
		Location? desiredLoc = null;
		int counter = 1;
		foreach (Location loc in _locationInfo.Keys)
		{
			if (locIndex == counter)
			{
				desiredLoc = loc;
				break;
			}
			counter++;
		}
		LocationConfig config = _locationInfo[desiredLoc];
		
		return config.GetLocScore();
	}
	
	/// <summary>
	/// Returns the winner in each loaction
	/// </summary>
	/// <returns></returns>
	public Dictionary<Location,IPlayer> GetLocationWinner()
	{
		Dictionary<Location,IPlayer> winners = new();
		foreach (Location loc in _locationInfo.Keys)
		{
			winners.Add(loc,GetLocationWinner(loc));
		}		
		return winners;
	}
	
	/// <summary>
	/// Returns the winner from particular location
	/// </summary>
	/// <param name="loc"></param>
	/// <returns></returns>
	public IPlayer GetLocationWinner(Location loc)
	{
		return _locationInfo[loc].GetLocWinner();	
	}
	
	/// <summary>
	/// Returns list of cards that are possible to be chosen by particulat player. This method consider
	/// the amount of energy that a player has.
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public List<Card> PlayerCardOptions(IPlayer player)
	{
		List<Card> options = new();
		PlayerConfig config = _playerInfo[player];
		int energy = config.GetEnergyTotal();
		
		foreach (Card card in config.GetCardDeck())
		{
			if (card.GetEnergyCost() <= energy)
			{
				options.Add(card);
			}
		}
		return options;
	}
	
	/// <summary>
	/// Placing a card that has been chosen by a player to a particular location. Takes player, card, and location
	/// as passing arguments 
	/// </summary>
	/// <param name="player"></param>
	/// <param name="card"></param>
	/// <param name="loc"></param>
	/// <returns></returns>
	public bool PlayerPlaceCard(IPlayer player, Card card, Location loc)
	{
		LocationConfig config = _locationInfo[loc];
		config.PlaceCard(player, card);
		
		_playerInfo[player].RemoveCard(card); // removing the placed card from player's deck. set card's isperformed to true
		if (card.GetApplyType() != CardApplyType.OnGoing)
		{
			card.SetIsPerformed(true);
		}
		// card.SetIsPlayed(true);	
		
		int energy = _playerInfo[player].GetEnergyTotal();
		_playerInfo[player].SetEnergyTotal(energy - card.GetEnergyCost()); // reducing player's energy
		return true;
	}

	/// <summary>
	/// Method to determines the actual location that was chosen by the player. Takes an integer of location
	/// index, passed by user when choosing the location. Returns location object.
	/// </summary>
	/// <param name="locIndex"></param>
	/// <returns></returns>
	public Location LocFromIndex(int locIndex)
	{
		Location? desiredLoc = null;
		int counter = 1;
		foreach (Location loc in _locationInfo.Keys)
		{
			if (locIndex == counter)
			{
				desiredLoc = loc;
				break;
			}
			counter++;
		}
		return desiredLoc;
	}

	/// <summary>
	/// Checking whether or not the location has been filled with 4 cards. Returns false is the location is full
	/// </summary>
	/// <param name="locIndex"></param>
	/// <param name="player"></param>
	/// <returns></returns>
	public bool CheckLocFull(int locIndex, IPlayer player)
	{
		if (locIndex < 1 || locIndex > GetLocations().Count)
		{
			return false;
		}
		
		if (_locationInfo[LocFromIndex(locIndex)].GetLocInfo()[player].Count > 3)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	/// <summary>
	/// Just like its brother, just differs in the arguments. This method takes the index of the passed
	/// card index and location index from user and determines by itself what are the actual card and location
	/// that are desired by user.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="cardIndex"></param>
	/// <param name="locIndex"></param>
	/// <returns></returns>
	public bool PlayerPlaceCard(IPlayer player, int cardIndex, int locIndex)
	{
		Card desiredCard = GetPlayerCards(player)[cardIndex-1];
		Location? desiredLoc = null;
		int counter = 1;
		foreach (Location loc in _locationInfo.Keys)
		{
			if (locIndex == counter)
			{
				desiredLoc = loc;
				break;
			}
			counter++;
		}
		
		PlayerPlaceCard(player, desiredCard, desiredLoc);
		
		CardSkill.ApplyOnRevealCards(this, player, desiredCard, desiredLoc, locIndex);
		CardSkill.ApplyOnGoingCards(this);
		// LocationSkill.ApplyOnGoingLocs(this);
		
		desiredCard.SetIsPlayed(true);
		
		return true;
	}
	
	/// <summary>
	/// Method for calling the static method "ApplyOnGoingLocs" from LocationSkill class.
	/// </summary>
	/// <returns></returns>
	public bool ApplyOnGoingLocs()
	{
		LocationSkill.ApplyOnGoingLocs(this);
		return true;
	}

	/// <summary>
	/// Method to check whether or not the card thats been chosen by user is valid. Considering
	/// the amount of energy that the user has.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="cardIndex"></param>
	/// <returns></returns>
	public bool CheckCardValid(IPlayer player,int cardIndex)
	{
		return PlayerCardOptions(player).Contains(GetPlayerCards(player)[cardIndex-1]);
	}

	/// <summary>
	/// Returns list of location that will be revealed in each round.
	/// </summary>
	/// <returns></returns>
	public List<Location> RevealLocation()
	{
		int num = 0;
		List<Location> revealLoc = new();
		
		foreach (Location loc in _locationInfo.Keys)
		{
			if (num < CheckCurrentRound())
			{
				revealLoc.Add(loc);
				loc.SetIsRevealed(true);
				num += 1;
			} 
			else
			{
				break;	
			}
		}
		return revealLoc;
	}
	
	/// <summary>
	/// Return total/summation score thats been collected by players from all locations
	/// </summary>
	/// <returns></returns>
	public Dictionary<IPlayer,int> GetTotalScore()
	{
		Dictionary<IPlayer,int> totalScore = new();
		foreach (var kvpPlayer in _playerInfo)
		{
			int playerScore = 0;
			foreach (var kvpLoc in _locationInfo)
			{
				playerScore += kvpLoc.Value.GetLocScore()[kvpPlayer.Key];
			}
			totalScore.Add(kvpPlayer.Key, playerScore);	
		}
		return totalScore;
	}
	
	/// <summary>
	/// MEthod to determine who is the winner, or the state of the game because there is a "draw" possibility
	/// </summary>
	/// <returns></returns>
	public bool DetermineWinner()
	{	
		foreach (var kvp in GetLocationWinner())
		{
			foreach (IPlayer player in GetPlayers())
			{
				if (kvp.Value == player)
				{
					_playerInfo[player].AddFinalScore();
				}
			}
		}
		
		// List<int> playersScore = new();
		int player1Score = _playerInfo[GetPlayers()[0]].GetFinalScore();
		int player2Score = _playerInfo[GetPlayers()[1]].GetFinalScore();
		if (player1Score > player2Score)
		{
			_winner = GetPlayers()[0].GetName();
		}
		else if (player1Score < player2Score)
		{
			_winner = GetPlayers()[1].GetName();
		} 
		else
		{
			int player1Total = GetTotalScore()[GetPlayers()[0]];
			int player2Total = GetTotalScore()[GetPlayers()[1]];
			if (player1Total > player2Total)
			{
				_winner = GetPlayers()[0].GetName();
			}
			else if (player1Total < player2Total)
			{
				_winner = GetPlayers()[1].GetName();
			}
			else
			{
				_winner = "DRAW";
			}
		}
		return true;
	}
	
	/// <summary>
	/// Returns string of the winner of the game. Could be "DRAW"
	/// </summary>
	/// <returns></returns>
	public string GetWinner()
	{
		return _winner;
	}
	
	/// <summary>
	/// Set the game to the next round. Set the GameStatus and add more energy.
	/// </summary>
	/// <returns></returns>
	public bool GoNextRound()
	{	
		if (_gameStatus == GameStatus.NOT_STARTED)
		{
			_gameStatus = GameStatus.ONGOING;
			_round += 1;
		} 
		else if (_gameStatus == GameStatus.ONGOING)
		{
			if (_round == 6)
			{
				_gameStatus = GameStatus.ENDED;	
				_winningEvent?.Invoke();
			} 
			else
			{
				_round += 1;	
			}
		}
		
		foreach (PlayerConfig config in _playerInfo.Values)
		{
			config.SetEnergyTotal(_round);
		}
		
		return true;
	}
	
	// public bool CheckNewRound()
	// {
	// 	return _isNewRound;
	// }
	
	/// <summary>
	/// Returns the status of the game
	/// </summary>
	/// <returns></returns>
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
}