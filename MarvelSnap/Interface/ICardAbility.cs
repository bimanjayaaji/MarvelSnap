namespace MarvelSnap;

public interface ICardAbility
{
	CardType GetType();
	string GetDesc();
	int GetEnergyCost();
	int GetAttackingPower();
	bool SetEnergyCost(int energy);
	bool SetAttackingPower(int power);
}