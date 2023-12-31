using MarvelSnap;
using MarvelSnapEnum;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Serialization;

/// <summary>
/// Technically just a helper class to serialize and deserialize list of class and location to .json.
/// </summary>
public class Serialization
{
	public static readonly DataContractJsonSerializerSettings Settings = 
			new DataContractJsonSerializerSettings
			{ UseSimpleDictionaryFormat = true };
	
	public static void SerializeCards()
	{
		List<Card> allCards = new()
		{
			new (1,"QuickSilver",CardType.Normal,CardApplyType.Normal,1,2),
			new (2,"AntMan",CardType.CombinedWith_3Cards_IncreaseBy3,CardApplyType.OnGoing,1,1),
			new (3,"Medusa",CardType.PlacedOn_Middle_IncreaseBy3,CardApplyType.OnReveal,2,2),
			new (4,"Sentinel",CardType.Immortal_InDeck,CardApplyType.OnReveal,2,3),
			new (5,"WolfsBane",CardType.SameLocIncreaseBy2,CardApplyType.OnReveal,3,1),
			new (6,"MisterFantastic",CardType.IncreaseAdjacentBy2,CardApplyType.OnGoing,3,2)
		};
		FileStream stream = new FileStream("Cards.json", FileMode.Create);
		using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
		{
			var ser = new DataContractJsonSerializer(typeof(List<Card>),Settings);
			ser.WriteObject(writer, allCards);
			stream.Flush();
		}
	}
	
	public static void SerializeLocs()
	{
		List<Location> allLocs = new()
		{
			new(1,"Ruins",LocationType.Normal,LocApplyType.Normal),
			new(2,"Nidavellir", LocationType.CardsHere_IncreaseBy5,LocApplyType.OnGoing),
			new(3,"Cloning Vats",LocationType.PlayHere_AddACopy,LocApplyType.OnGoing),
			new(4,"Kyln",LocationType.Closed_OnTurn4,LocApplyType.OnPlacing),
			new(5,"The Big House",LocationType.Cost456_CantPlay,LocApplyType.OnPlacing),
			new(6,"Atlantis",LocationType.IfOnlyOne_IncreaseBy5,LocApplyType.OnGoing)
		};
		FileStream stream = new FileStream("Locations.json", FileMode.Create);
		using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
		{
			var ser = new DataContractJsonSerializer(typeof(List<Location>),Settings);
			ser.WriteObject(writer, allLocs);
			stream.Flush();
		}
	}
	
	public static void Deserialize()
	{
		var ser = new DataContractJsonSerializer(typeof(List<Card>));
		FileStream stream2 = new FileStream("Cards.json", FileMode.OpenOrCreate);
		List<Card> allCards = (List<Card>)ser?.ReadObject(stream2);
	}
}