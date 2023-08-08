using System.ComponentModel;
using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

/// <summary>
/// A class that represents the details of a particular location. It contains the card thats been placed there
/// and the score for each player in that location. Each location is paired with a LocationConfig class.
/// </summary>
public class LocationConfig
{
	private Dictionary<IPlayer,int> _locScore;
	private Dictionary<IPlayer,List<Card>> _cardsOnLoc;
	
	public LocationConfig()
	{
		_locScore = new();
		_cardsOnLoc = new();
	}
	
	/// <summary>
	/// Method to initialize the player's card's and score attributes in particular location.
	/// </summary>
	/// <param name="players"></param>
	/// <returns></returns>
	public bool InitLocPlayer(List<IPlayer> players)
	{
		foreach (var player in players)
		{
			_locScore.Add(player, 0);
			_cardsOnLoc.Add(player, new List<Card>());	
		}
		
		return true;
	}
	
	/// <summary>
	/// Place a card thats been assigned by the player in particular location.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="card"></param>
	/// <returns></returns>
	public bool PlaceCard(IPlayer player, Card card)
	{
		_cardsOnLoc[player].Add(card);
		ComputeScore(player,card);
		return true;
	}
	
	/// <summary>
	/// Method to add the score of paricular player in particular locations. Mainly used when a card's skill or
	/// location's skill is applied.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="add"></param>
	/// <returns></returns>
	public bool AddScore(IPlayer player, int add)
	{
		_locScore[player] += add;
		return true;
	}
	
	/// <summary>
	/// Method to set the score manually and directly. Not used in the main game.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="score"></param>
	/// <returns></returns>
	public bool SetScore(IPlayer player, int score)
	{
		_locScore[player] = score;
		return true;
	}
	
	/// <summary>
	/// Method to compute a score when a player just placed a new card to the location. Called directly when
	/// a card has just been placed to a location.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="card"></param>
	/// <returns></returns>
	private bool ComputeScore(IPlayer player, Card card)
	{
		_locScore[player] += card.GetAttackingPower();
		return true;
	}
	
	/// <summary>
	/// Method to compute score in total. Taking all the card's attacking power and sum it all up.
	/// Not used in the main program's sequence.
	/// </summary>
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
	
	/// <summary>
	/// Returns all player's cards on the board/location
	/// </summary>
	/// <returns></returns>
	public Dictionary<IPlayer,List<Card>> GetLocInfo()
	{
		return _cardsOnLoc;
	}
	
	/// <summary>
	/// Returns particular player's cards on the board/location
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public List<Card> GetPlayerCardsOnLoc(IPlayer player)
	{
		return _cardsOnLoc[player];
	}
	
	public Dictionary<IPlayer,int> GetLocScore()
	{
		// ComputeScore();
		return _locScore;
	}
	
	/// <summary>
	/// Determines who is the winner in a particular location based on the highest score.
	/// </summary>
	/// <returns></returns>
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