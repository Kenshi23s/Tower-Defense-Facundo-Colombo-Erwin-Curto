using UnityEngine;


public enum ProjectileType
{
    Bullet,
    Misile
}
public class ProjectileManager : MonoBehaviour
{
    BulletPool pool = new BulletPool();
   public static ProjectileManager instance;
    [SerializeField] ProjectileData[] projectiles;
    
  
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < projectiles.Length; i++)
        {
            pool.CreateProjectileTypePool(projectiles[i]);
        }
    }

    public Projectile GetProjectile(ProjectileType type) => pool.AskForProjectile(type);
   
}
