using System.ComponentModel;
using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

public class LocationConfig
{
	private Dictionary<IPlayer,int> _locScore;
	private Dictionary<IPlayer,List<Card>> _cardsOnLoc;
	
	public LocationConfig()
	{
		_locScore = new();
		_cardsOnLoc = new();
	}
	
	public bool InitLocPlayer(List<IPlayer> players)
	{
		foreach (var player in players)
		{
			_locScore.Add(player, 0);
			_cardsOnLoc.Add(player, new List<Card>());	
		}
		
		return true;
	}
	
	public bool PlaceCard(IPlayer player, Card card)
	{
		_cardsOnLoc[player].Add(card);
		return true;
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
			_locScore[kvp.Key] = score;
		}
	}
	
	public Dictionary<IPlayer,List<Card>> GetLocInfo()
	{
		return _cardsOnLoc;
	}
	
	public Dictionary<IPlayer,int> GetLocScore()
	{
		ComputeScore();
		return _locScore;
	}
	
	public IPlayer GetLocWinner()
	{
		int highest = 0;
		IPlayer winner = null;
		foreach (var kvp in _locScore)
		{
			if (kvp.Value > highest)
			{
				winner = kvp.Key;
			}
		}
		return winner;
	}
}