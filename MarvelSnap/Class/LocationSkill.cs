using MarvelSnapEnum;
using MarvelSnapInterface;

namespace MarvelSnap;

public static class LocationSkill
{	
	public static bool ApplyOnGoingLocs(GameRunner gameRunner)
	{
		foreach (var loc in gameRunner.GetLocations())
		{
			if (loc.GetApplyType() == LocApplyType.OnGoing)
			{
				if (loc.IsRevealed())
				{
					switch (loc.GetSkill())
					{
						case LocationType.Normal:
							return true;

						case LocationType.CardsHere_IncreaseBy5:
							foreach (var player in gameRunner.GetPlayers())
							{
								foreach(Card card in gameRunner.GetPlayerCardsOnLoc(loc,player))
								{
									if (!card.IsLocEffect())
									{
										gameRunner.GetLocationInfo()[loc].AddScore(player,5);
										card.SetIsLocEffect(true);
									}
								}
							}
							return true;
							
						case LocationType.PlayHere_AddACopy:
							foreach (var player in gameRunner.GetPlayers())
							{
								foreach(Card card in gameRunner.GetPlayerCardsOnLoc(loc,player))
								{
									if (!card.IsLocEffect())
									{
										gameRunner.SetCardsToPlayer(player, card);
										card.SetIsLocEffect(true);
									}
								}
							}
							return true;
							
						case LocationType.IfOnlyOne_IncreaseBy5:
							foreach (var player in gameRunner.GetPlayers())
							{
								List<Card> cards = gameRunner.GetPlayerCardsOnLoc(loc,player);
								if (cards.Count == 1)
								{
									if (!cards[0].IsLocEffect())
									{
										gameRunner.GetLocationInfo()[loc].AddScore(player,5);
										cards[0].SetIsLocEffect(true);
									}									
								}
								else if (cards.Count > 1)
								{
									foreach (var card in cards)
									{
										if (card.IsLocEffect())
										{
											gameRunner.GetLocationInfo()[loc].AddScore(player,-5);
											cards[0].SetIsLocEffect(false);
										}
									}
								}
							}
							return true;
					}
				}
			}
		}
		return false;
	}
	
	public static bool ApplyPlacingCards(GameRunner gameRunner)
	{
		throw new NotImplementedException();
	}
}