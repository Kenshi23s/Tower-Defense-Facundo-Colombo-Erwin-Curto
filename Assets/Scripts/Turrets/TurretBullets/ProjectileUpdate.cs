
public abstract class ProjectileUpdate : Projectile
{
    private void Update() => EveryTick();
    public abstract void EveryTick();
}
