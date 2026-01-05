namespace vansur.script;

public interface DamagingObject
{
    public float Hp { get; set; }
    public void TakeDamage(float damage);
}