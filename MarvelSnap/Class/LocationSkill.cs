using MarvelSnapEnum;
using MarvelSnapInterface;

namespace MarvelSnap;

/// <summary>
/// LocationSkill is a static class that is used or called when the game will
/// apply the skill of a location. It differs with the CardSkill in a way that it has no
/// OnReveal type of location. So it only has OnGoing type and OnPlacing type.
/// </summary>
public static class LocationSkill
{
	/// <summary>
	/// Method to implement the effect of an OnGoing typed location. Throughout the game, it will scan the entire
	/// location and decide which location should implement its effect based on the conditions. The conditions varies,
	/// dependant on factors like rounds and how many cards are placed there. If you want to add a new skillset of
	/// location, just add the skill algorithm inside the switch-case.
	/// </summary>
	/// <param name="gameRunner"></param>
	/// <returns></returns>
	public static bool ApplyOnGoingLocs(GameRunner gameRunner)
	{	
		var locations = gameRunner.GetLocations();
		foreach (var loc in locations)
		{
			if (loc.GetApplyType() == LocApplyType.OnGoing && loc.IsRevealed())
			{
				switch (loc.GetSkill())
				{
					case LocationType.Normal:
						return true;

					case LocationType.CardsHere_IncreaseBy5:
						foreach (var player in gameRunner.GetPlayers())
						{
							foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))
							{
								if (!card.IsLocEffect())
								{
									gameRunner.GetLocationInfo()[loc].AddScore(player, 5);
									card.SetIsLocEffect(true);
								}
							}
						}
						return true;

					case LocationType.PlayHere_AddACopy:
						foreach (var player in gameRunner.GetPlayers())
						{
							foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))
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
							List<Card> cards = gameRunner.GetPlayerCardsOnLoc(loc, player);
							if (cards.Count == 1)
							{
								if (!cards[0].IsLocEffect())
								{
									gameRunner.GetLocationInfo()[loc].AddScore(player, 5);
									cards[0].SetIsLocEffect(true);
								}
							}
							else if (cards.Count > 1)
							{
								foreach (var card in cards)
								{
									if (card.IsLocEffect())
									{
										gameRunner.GetLocationInfo()[loc].AddScore(player, -5);
										cards[0].SetIsLocEffect(false);
									}
								}
							}
						}
						return true;
				}

			}
		}
		return false;
	}

	/// <summary>
	/// Method to implement a location that has OnPlacing type. This type of location has its own constraint
	/// regarding placement. Not implemented yet.
	/// </summary>
	/// <param name="gameRunner"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static bool ApplyPlacingCards(GameRunner gameRunner)
	{
		throw new NotImplementedException();
	}
}