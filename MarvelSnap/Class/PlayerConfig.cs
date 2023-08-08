namespace MarvelSnap;

/// <summary>
/// A class that represents the details of a particular player's situation in the game. It contains 
/// the total energy that the player has in each round, and the cards that the player holds in each round.
/// It also contains the total amount of score of the player.
/// </summary>
public class PlayerConfig
{
	/// <summary>
	/// Final score defines the score won by player in each location. Max is 3
	/// Total score defines the accumulated score from each location.
	/// </summary>
	private int _finalScore;
	private int _totalScore;
	private int _energyTotal;
	private List<Card> _cardDeck;
	
	public PlayerConfig()
	{
		_energyTotal = 0;
		_cardDeck = new List<Card>(); 
	}
	
	/// <summary>
	/// Returns the list of the card that the player has
	/// </summary>
	/// <returns></returns>
	public List<Card> GetCardDeck()
	{
		return _cardDeck;
	}
	
	/// <summary>
	/// Add a card to player's card deck. Typically is used early in each round and in some other condition.
	/// </summary>
	/// <param name="card"></param>
	/// <returns></returns>
	public bool AddCardDeck(Card card)
	{
		_cardDeck.Add(card);
		return true;
	}
	
	/// <summary>
	/// Remove a card from a card's deck. Typicaly is used after player's been placing a new card on the board
	/// </summary>
	/// <param name="card"></param>
	/// <returns></returns>
	public bool RemoveCard(Card card)
	{
		// card.SetIsPlayed(true);
		_cardDeck.Remove(card);
		return true;
	}
	
	public int GetEnergyTotal()
	{
		return _energyTotal;
	}
	
	public bool SetEnergyTotal(int energyTotal)
	{
		this._energyTotal = energyTotal;
		return true;
	}
	
	public bool SetFinalScore(int finalScore)
	{
		_finalScore = finalScore;
		return true;
	}
	
	public bool AddFinalScore()
	{
		_finalScore += 1;
		return true;
	}
	
	public int GetFinalScore()
	{
		return _finalScore;
	}
	
	public bool SetTotalScore(int totalScore)
	{
		_totalScore = totalScore;
		return true;
	}
	
	public bool AddTotalScore(int score)
	{
		_totalScore += score;
		return true;
	}
	
	public int GetTotalScore()
	{
		return _totalScore;
	}
}