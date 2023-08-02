namespace MarvelSnapTools;

public class Tools
{
	public static void SmallSpace()
	{
		Console.WriteLine("");
	}
	
	public static void BigSpace()
	{
		Console.WriteLine("");
		Console.WriteLine("# # # # # # # # # #");
		Console.WriteLine("");
	}
	
	public static void Print(string x)
	{
		Console.Write(x);
	}
	
	public static void Println(string x)
	{
		Console.WriteLine(x);
	}
	
	public static string? Readln()
	{
		string? passed = Console.ReadLine();
		return passed;
	}
}