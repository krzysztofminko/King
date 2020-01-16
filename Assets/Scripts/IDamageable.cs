public interface IDamageable
{
	float Hp { get; }
	float HpMax { get; }

	bool Damage(float value);
}
