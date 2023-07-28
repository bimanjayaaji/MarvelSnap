namespace MarvelSnap;

public interface IPlayer
{
	int GetId();
	string GetName();
	bool SetId(int id);
	bool SetName(string name);
}