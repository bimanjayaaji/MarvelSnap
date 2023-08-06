using MarvelSnapEnum;
using MarvelSnapInterface;

namespace MarvelSnap;

public static class CardSkill
{
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