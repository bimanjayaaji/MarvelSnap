using MarvelSnapEnum;
namespace MarvelSnapInterface;

public interface ICardAbility
{
	CardType GetSkill();
	CardApplyType GetApplyType();
	string? GetDesc();
	int GetEnergyCost();
	int GetAttackingPower();
	bool SetEnergyCost(int energy);
	bool SetAttackingPower(int power);
}