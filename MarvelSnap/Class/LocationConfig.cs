using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

public class LocationConfig
{
	private Dictionary<IPlayer,int> _playersScore;
	private Dictionary<IPlayer,List<Card>> _cardsOnBoard;
	
	public LocationConfig()
	{
		_playersScore = new();
		_cardsOnBoard = new();
		// need to be analysed more
	}
	
	public bool PlaceCard(IPlayer player, Card card)
	{
		_cardsOnBoard[player].Add(card);
		return true;
		// need to be analysed more
		// kasih check kalo playernya ga ada
	}
	
	private void ComputeScore()
	{
		// ngitung total score di location
		foreach (var kvp in _cardsOnBoard)
		{
			int score = 0;
			foreach(var card in kvp.Value)
			{
				score += card.GetAttackingPower();
			}
			_playersScore[kvp.Key] = score;
		}
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
	
	// GetLocWinner()
}