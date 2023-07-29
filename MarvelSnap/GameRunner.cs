using  MarvelSnapInterface;
using MarvelSnapEnum;
namespace MarvelSnap;

public class GameRunner
{
	private int _round;
	private GameStatus _gameStatus;
	private Dictionary<IPlayer,PlayerConfig> _playerInfo;
	private Dictionary<Location,LocationConfig> _locationInfo;
	
	public GameRunner()
	{
		_round = 0;
		_gameStatus = GameStatus.NOT_STARTED;
		_playerInfo = new Dictionary<IPlayer,PlayerConfig>();
		_locationInfo = new Dictionary<Location,LocationConfig>();
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
	
	public bool GenerateLocations()
	{
		return true; // not done yet
	}
	
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
}