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
	
	public bool? AddPlayer(IPlayer player)
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
		Card quickSilver = new(1,"QuickSilver",CardType.Normal,1,2);
		Card antMan = new(2,"AntMan",CardType.CombinedWith_3Cards_IncreaseBy3,1,1);
		Card medusa = new(3,"Medusa",CardType.PlacedOn_Middle_IncreaseBy3,2,2);
		Card sentinel = new(4,"Sentinel",CardType.Immortal_InDeck,2,3);
		Card wolfsBane = new(5,"WolfsBane",CardType.SameLocIncreaseBy2,3,1);
		Card misterFantastic = new(6,"MisterFantastic",CardType.IncreaseAdjacentBy2,3,2);
		
		List<Card> allCards = new()
		{
			quickSilver,
			antMan,
			medusa,
			sentinel,
			wolfsBane,
			misterFantastic
		};
		
		_allCards = allCards;
		
		return true;
	}
	
	public List<Card> GetAllCards()
	{
		return _allCards;
	}
	
	private bool GenerateAllLocations()
	{
		Location ruins = new(1,"Ruins",LocationType.Normal);
		Location nidavellir = new(2,"Nidavellir", LocationType.CardsHere_IncreaseBy5);
		Location muirIsland = new(3,"Muir Island",LocationType.AfterEachTurn_IncreaseBy1);
		Location kyln = new(4,"Kyln",LocationType.Closed_OnTurn4);
		Location theBigHouse = new(5,"The Big House",LocationType.Cost456_CantPlay);
		Location atlantis = new(6,"Atlantis",LocationType.IfOnlyOne_IncreaseBy5);
		
		List<Location> allLocs = new()
		{
			ruins,
			nidavellir,
			muirIsland,
			kyln,
			theBigHouse,
			atlantis
		};
		
		_allLocations = allLocs;
		
		return true;
	}
	
	public List<Location>? GetAllLocations()
	{
		return _allLocations;
	}
	
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
}