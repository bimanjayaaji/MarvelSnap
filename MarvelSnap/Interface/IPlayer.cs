namespace MarvelSnapInterface;

public interface IPlayer
{
	int GetId();
	string? GetName();
	bool SetId(int id);
	bool SetName(string name);
}