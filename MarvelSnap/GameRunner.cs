using  MarvelSnapInterface;
using MarvelSnapEnum;
namespace MarvelSnap;

public class GameRunner
{
	private int _round;
	private GameStatus _gameStatus;
	private List<Card> _allCards;
	private List<Location>? _allLocations;
	private Dictionary<IPlayer,PlayerConfig> _playerInfo;
	private Dictionary<Location,LocationConfig> _locationInfo;
	
	public GameRunner()
	{
		_round = 0;
		_gameStatus = GameStatus.NOT_STARTED;
		_allCards = new();
		_allLocations = new();
		_playerInfo = new(); 
		_locationInfo = new();
		GenerateAllCards();
		GenerateAllLocations();
	}
	
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
	
	public List<IPlayer> GetPlayers()
	{
		List<IPlayer> players = new();
		foreach (var key in _playerInfo.Keys)
		{
			players.Add(key);
		}
		return players;
	}
	
	public int CheckCurrentRound()
	{
		return _round;
	}
	
	private bool GenerateAllCards() // called when generating new instance of GR
	{		
		List<Card> allCards = new()
		{
			new (1,"QuickSilver",CardType.Normal,1,2),
			new (2,"AntMan",CardType.CombinedWith_3Cards_IncreaseBy3,1,1),
			new (3,"Medusa",CardType.PlacedOn_Middle_IncreaseBy3,2,2),
			new (4,"Sentinel",CardType.Immortal_InDeck,2,3),
			new (5,"WolfsBane",CardType.SameLocIncreaseBy2,3,1),
			new (6,"MisterFantastic",CardType.IncreaseAdjacentBy2,3,2)
		};
		
		_allCards = allCards;
		
		return true;
	}
	
	public List<Card> GetAllCards()
	{
		return _allCards;
	}
	
	private bool GenerateAllLocations() // called when generating new instance of GR
	{
		List<Location> allLocs = new()
		{
			new(1,"Ruins",LocationType.Normal),
			new(2,"Nidavellir", LocationType.CardsHere_IncreaseBy5),
			new(3,"Muir Island",LocationType.AfterEachTurn_IncreaseBy1),
			new(4,"Kyln",LocationType.Closed_OnTurn4),
			new(5,"The Big House",LocationType.Cost456_CantPlay),
			new(6,"Atlantis",LocationType.IfOnlyOne_IncreaseBy5)
		};
		
		_allLocations = allLocs;
		
		return true;
	}
	
	public List<Location>? GetAllLocations()
	{
		return _allLocations;
	}
	
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
			playerConfig.AddCardDeck(_allCards[ind]);	
		}
		
		return true;
	}
	
	public bool SetCardsToPlayer(IPlayer player, Card card)
	{
		PlayerConfig playerConfig = _playerInfo[player];
		playerConfig.AddCardDeck(card);
		
		return true;
	}
	
	public Dictionary<IPlayer, List<Card>> GetPlayerCards()
	{
		Dictionary<IPlayer, List<Card>> playerCards = new();
		
		foreach (KeyValuePair<IPlayer, PlayerConfig> player in _playerInfo)
		{
			playerCards.Add(player.Key, player.Value.GetCardDeck());
		}
		
		return playerCards;
	}
	
	public List<Card> GetPlayerCards(IPlayer player)
	{
		return _playerInfo[player].GetCardDeck();
	}
	
	public Dictionary<IPlayer,int> GetPlayerEnergy()
	{
		Dictionary<IPlayer,int> energy = new();
		
		foreach (var player in _playerInfo)
		{
			energy.Add(player.Key,player.Value.GetEnergyTotal());
		}
		
		return energy;
	}
	
	public int GetPlayerEnergy(IPlayer player)
	{
		return _playerInfo[player].GetEnergyTotal();
	}
	
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
	
	public bool SetLocations(List<Location> locList)
	{
		foreach (var loc in locList)
		{
			LocationConfig config = new();
			_locationInfo.Add(loc, config);
		}
		return true;
	}
	
	public List<Location> GetLocations()
	{
		return _locationInfo.Keys.ToList();
	}
	
	public Dictionary<Location,Dictionary<IPlayer,List<Card>>> GetLocationCards()
	{
		Dictionary<Location,Dictionary<IPlayer,List<Card>>> locCards = new();
		foreach (KeyValuePair<Location,LocationConfig> locInfo in _locationInfo)
		{
			locCards.Add(locInfo.Key, locInfo.Value.GetLocInfo());
		}
		
		return locCards;
		// add exception/warning if _locationInfo is still Empty
		// biar ga terlalu kompleks. method GetLocInfo di LocationConfig dioverload, kasi parameter IPlayer
		// Dictionary<IPlayer,List<Card> isa dibikin KVP
	}
	
	public Dictionary<IPlayer,List<Card>> GetLocationCards(Location loc)
	{
		return _locationInfo[loc].GetLocInfo();
		// add exception/warning if the loc argument doesn't exist in _locationInfo	
	}
	
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
	
	public Dictionary<IPlayer,int> GetLocationScore(Location loc)
	{
		return _locationInfo[loc].GetLocScore();
		// add exception/warning if the loc argument doesn't exist in _locationInfo
	}
	
	public int GetLocationScore(Location loc, IPlayer player)
	{
		return _locationInfo[loc].GetLocScore()[player];
	}
	
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
	
	public Dictionary<Location,IPlayer> GetLocationWinner()
	{
		Dictionary<Location,IPlayer> winners = new();
		foreach (Location loc in _locationInfo.Keys)
		{
			winners.Add(loc,GetLocationWinner(loc));
		}		
		return winners;
	}
	
	public IPlayer GetLocationWinner(Location loc)
	{
		return _locationInfo[loc].GetLocWinner();	
	}
	
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
	
	public bool PlayerPlaceCard(IPlayer player, Card card, Location loc)
	{
		LocationConfig config = _locationInfo[loc];
		config.PlaceCard(player, card);
		
		_playerInfo[player].RemoveCard(card);
		
		int energy = _playerInfo[player].GetEnergyTotal();
		_playerInfo[player].SetEnergyTotal(energy - card.GetEnergyCost());
		return true;
		// KURANGIN KARTU DARI PLAYER VV
		// KURANGIN ENERGY DARI PLAYER
	}

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
		LocationConfig config = _locationInfo[desiredLoc];
		
		config.PlaceCard(player, desiredCard);
		
		_playerInfo[player].RemoveCard(desiredCard);
		
		int energy = _playerInfo[player].GetEnergyTotal();
		_playerInfo[player].SetEnergyTotal(energy - desiredCard.GetEnergyCost());
		
		return true;
		// KURANGIN KARTU DARI PLAYER VV
		// KURANGIN ENERGY DARI PLAYER
	}

	public bool CheckCardValid(IPlayer player,int cardIndex)
	{
		return PlayerCardOptions(player).Contains(GetPlayerCards(player)[cardIndex-1]);
	}

	public virtual List<Location> RevealLocation()
	{
		List<Location> revealLoc = new();
		int num = 0;
		
		foreach (Location loc in _locationInfo.Keys)
		{
			if (num < CheckCurrentRound())
			{
				revealLoc.Add(loc);
				num += 1;
			} 
			else
			{
				break;	
			}
		}
		return revealLoc;
	}
	
	// ExecuteCard
	
	// Execute Location
	
	// PlayerRetreat
	
	public IPlayer DetermineWinner()
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
		
		List<int> playersScore = new();

		if (_playerInfo[GetPlayers()[0]].GetFinalScore() > _playerInfo[GetPlayers()[0]].GetFinalScore())
		{
			return GetPlayers()[0];
		}
		else
		{
			return GetPlayers()[1];
		}
	}
	
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
	
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
}