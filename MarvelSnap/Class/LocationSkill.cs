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
			// Console.WriteLine("Entering foreach (var loc in locations)");
			// Console.ReadKey();
			
			if (loc.GetApplyType() == LocApplyType.OnGoing && loc.IsRevealed())
			{
				// Console.WriteLine("Entering if (loc.GetApplyType() == LocApplyType.OnGoing && loc.IsRevealed())");
				// Console.ReadKey();
				
				switch (loc.GetSkill())
				{					
					case LocationType.Normal:
						// Console.WriteLine("Entering case LocationType.Normal:");
						// Console.WriteLine("Not doing anything");
						// Console.ReadKey();	
						break;

					case LocationType.CardsHere_IncreaseBy5:
						// Console.WriteLine("Entering case LocationType.CardsHere_IncreaseBy5:");
						// Console.ReadKey();
						foreach (var player in gameRunner.GetPlayers())
						{
							// Console.WriteLine("Entering foreach (var player in gameRunner.GetPlayers())");
							// Console.ReadKey();
							foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))
							{
								// Console.WriteLine("Entering foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))");
								// Console.ReadKey();
								if (!card.IsLocEffect())
								{
									// Console.WriteLine("Entering if (!card.IsLocEffect())");
									// Console.ReadKey();
									gameRunner.GetLocationInfo()[loc].AddScore(player, 5);
									card.SetIsLocEffect(true);
									// Console.WriteLine("Successfully applied LocationType.CardsHere_IncreaseBy5 effect");
									// Console.ReadKey();
								}
							}
						}
						break;

					case LocationType.PlayHere_AddACopy:
						// Console.WriteLine("Entering case LocationType.PlayHere_AddACopy:");
						// Console.ReadKey();
						foreach (var player in gameRunner.GetPlayers())
						{
							// Console.WriteLine("Entering foreach (var player in gameRunner.GetPlayers())");
							// Console.ReadKey();
							foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))
							{
								// Console.WriteLine("Entering foreach (Card card in gameRunner.GetPlayerCardsOnLoc(loc, player))");
								// Console.ReadKey();
								if (!card.IsLocEffect())
								{
									// Console.WriteLine("Entering if (!card.IsLocEffect())");
									// Console.ReadKey();
									gameRunner.SetCardsToPlayer(player, card);
									card.SetIsLocEffect(true);
									// Console.WriteLine("Successfully applied LocationType.PlayHere_AddACopy: effect");
									// Console.ReadKey();
								}
							}
						}
						break;

					case LocationType.IfOnlyOne_IncreaseBy5:
						// Console.WriteLine("Entering case LocationType.IfOnlyOne_IncreaseBy5:");
						// Console.ReadKey();
						foreach (var player in gameRunner.GetPlayers())
						{
							// Console.WriteLine("Entering foreach (var player in gameRunner.GetPlayers())");
							// Console.ReadKey();
							List<Card> cards = gameRunner.GetPlayerCardsOnLoc(loc, player);
							if (cards.Count == 1)
							{
								// Console.WriteLine("Entering if (cards.Count == 1)");
								// Console.ReadKey();
								if (!cards[0].IsLocEffect())
								{
									// Console.WriteLine("Entering if (!cards[0].IsLocEffect())");
									// Console.ReadKey();
									gameRunner.GetLocationInfo()[loc].AddScore(player, 5);
									cards[0].SetIsLocEffect(true);
									// Console.WriteLine("Successfully applied LocationType.IfOnlyOne_IncreaseBy5 (only 1, add 5)");
									// Console.ReadKey();
								}
							}
							else if (cards.Count > 1)
							{
								// Console.WriteLine("Entering else if (cards.Count > 1)");
								// Console.ReadKey();
								foreach (var card in cards)
								{
									// Console.WriteLine("Entering foreach (var card in cards)");
									// Console.ReadKey();
									if (card.IsLocEffect())
									{
										// Console.WriteLine("Entering if (card.IsLocEffect())");
										// Console.ReadKey();
										gameRunner.GetLocationInfo()[loc].AddScore(player, -5);
										cards[0].SetIsLocEffect(false);
										// Console.WriteLine("Successfully applied LocationType.IfOnlyOne_IncreaseBy5 (more than 1, decrease 5)");
										// Console.ReadKey();
									}
								}
							}
						}
						break;
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