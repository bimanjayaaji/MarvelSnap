using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

public class LocationConfig
{
	private Dictionary<IPlayer,int> _LocScore;
	private Dictionary<IPlayer,List<Card>> _cardsOnLoc;
	
	public LocationConfig()
	{
		_LocScore = new();
		_cardsOnLoc = new();
		// need to be analysed more
	}
	
	public bool PlaceCard(IPlayer player, Card card)
	{
		_cardsOnLoc[player].Add(card);
		return true;
		// need to be analysed more
		// kasih check kalo playernya ga ada
	}
	
	private void ComputeScore()
	{
		foreach (var kvp in _cardsOnLoc)
		{
			int score = 0;
			foreach(var card in kvp.Value)
			{
				score += card.GetAttackingPower();
			}
			_LocScore[kvp.Key] = score;
		}
	}
	
	public Dictionary<IPlayer,List<Card>> GetLocInfo()
	{
		return _cardsOnLoc;
	}
	
	public Dictionary<IPlayer,int> GetLocScore()
	{
		ComputeScore();
		return _LocScore;
	}
	
	public IPlayer GetLocWinner()
	{
		int highest = 0;
		IPlayer winner = null;
		foreach (var kvp in _LocScore)
		{
			if (kvp.Value > highest)
			{
				winner = kvp.Key;
			}
		}
		return winner;
	}
}