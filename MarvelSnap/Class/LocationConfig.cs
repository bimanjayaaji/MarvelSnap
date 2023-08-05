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
		ComputeScore(player,card);
		// KURANGIN KARTU DARI PLAYER
		return true;
		// kasih check kalo playernya ga ada
	}
	
	public bool AddScore(IPlayer player, int add)
	{
		_locScore[player] += add;
		return true;
	}
	
	public bool SetScore(IPlayer player, int score)
	{
		_locScore[player] = score;
		return true;
	}
	
	private bool ComputeScore(IPlayer player, Card card)
	{
		_locScore[player] += card.GetAttackingPower();
		return true;
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
		// ComputeScore();
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
				highest = kvp.Value;
				winner = kvp.Key;
			} 
			else if (kvp.Value == highest)
			{
				winner = null;
			}
		}
		return winner;
	}
}