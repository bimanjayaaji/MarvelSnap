using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

public class LocationConfig
{
	private Dictionary<IPlayer,int> _playersScore;
	private Dictionary<IPlayer,List<Card>> _cardsOnBoard;
	
	public LocationConfig()
	{
		_playersScore = new Dictionary<IPlayer,int>();
		_cardsOnBoard = new Dictionary<IPlayer,List<Card>>();
		// need to be analysed more
	}
	
	public bool UpdateLoc(IPlayer player, Card card)
	{
		return true;
		// need to be analysed more
	}
	
	public Dictionary<IPlayer,List<Card>> GetLocInfo()
	{
		return _cardsOnBoard;
		// need to be analysed more
	}
	
	public Dictionary<IPlayer,int> GetPlayersScore()
	{
		return _playersScore;
		// need to be analysed more
	}
}