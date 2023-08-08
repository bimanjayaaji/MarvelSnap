using MarvelSnapEnum;
using MarvelSnapInterface;

namespace MarvelSnap;

/// <summary>
/// CardSkill is a static class that is used or called when the game will
/// apply the skill of each card. 
/// </summary>
public static class CardSkill
{
	/// <summary>
	/// This method handles the implementation of the card's skill, typically a card that has
	/// OnReveal type. This method is used once only in each turn, meaning that it has no iteration whatsoever
	/// throughout the game. Instead, it uses switch-case to filter what kind of skill does the card have, and
	/// implement it directly when it is called. !!! If you want to add a new skill, you can add a new case in the
	/// swtich-case algorithm.
	/// </summary>
	/// <param name="gameRunner"></param>
	/// <param name="player"></param>
	/// <param name="card"></param>
	/// <param name="loc"></param>
	/// <param name="locIndex"></param>
	/// <returns></returns>
	public static bool ApplyOnRevealCards(GameRunner gameRunner, IPlayer player, Card card, Location loc, int locIndex)
	{
		if (card.GetApplyType() == CardApplyType.OnReveal)
		{
			switch (card.GetSkill())
			{
				case CardType.Normal:
					return true;

				case CardType.PlacedOn_Middle_IncreaseBy3:
					if (locIndex == 2)
					{
						// card.SetAttackingPower(card.GetAttackingPower() + 3);
						gameRunner.GetLocationInfo()[loc].AddScore(player,3);
					}
					return true;

				case CardType.Immortal_InDeck:
					gameRunner.SetCardsToPlayer(player, card);
					return true;

				case CardType.SameLocIncreaseBy2:
					List<Card> playerCards = gameRunner.GetLocationInfo()[loc].GetLocInfo()[player];
					gameRunner.GetLocationInfo()[loc].AddScore(player, (playerCards.Count-1) * 2); // mines 1!
					return true;
				
				case CardType.IncreaseAdjacentBy2:
					if (locIndex == 2)
					{
						gameRunner.GetLocationInfo()[gameRunner.LocFromIndex(1)].AddScore(player,2);
						gameRunner.GetLocationInfo()[gameRunner.LocFromIndex(3)].AddScore(player,2);
					}
					else
					{
						gameRunner.GetLocationInfo()[gameRunner.LocFromIndex(2)].AddScore(player,2);
					}
					return true;
			}
		}
		return false;
	}
	
	/// <summary>
	/// This method handles the implementation of the OnGoing Card. It differs with the OnReveal card
	/// in a sense that, this method will iterate continuously throughout the game to scan which card
	/// that meets the conditions for it to implements its skill. If you want to add a new skill, you can add
	/// the algorithm for the skill inside if (!card.IsPerformed()) logic sequence.
	/// </summary>
	/// <param name="gameRunner"></param>
	/// <returns></returns>
	public static bool ApplyOnGoingCards(GameRunner gameRunner)
	{
		foreach (Location loc in gameRunner.GetLocations())
		{
			foreach (IPlayer player in gameRunner.GetPlayers())
			{
				foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))
				{
					if (card.GetApplyType() == CardApplyType.OnGoing)
					{
						if (!card.IsPerformed()) // logic for ongoing cards here and beyond
						{
							if (card.GetSkill() == CardType.CombinedWith_3Cards_IncreaseBy3)
							{
								List<Card> playerCards = gameRunner.GetLocationInfo()[loc].GetLocInfo()[player];
								if(playerCards.Count == 4)
									{
										gameRunner.GetLocationInfo()[loc].AddScore(player, 3);
										card.SetIsPerformed(true);
									}
							}
						}
					}
				}
			}			
		}
		return true;
	}	
}