public interface IDamageable
{
    public int Health { get; set; }
    void TakeDamage(int dmg);
}
