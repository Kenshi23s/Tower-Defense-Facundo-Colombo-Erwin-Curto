using System;
using UnityEngine;

public class Bullet : ProjectileUpdate
{
    // lo hardcodeo pq es en el momento que quiero que vuelva a la pool en caso de que tarde mucho en impactar
    float timeAlive;
  

    public void SetParameters(Transform canon)
    {
        transform.position = canon.position;
        transform.right  = canon.right;
        timeAlive = 20f;
       
    }
    public override void OnShoot()
    {
        //no se usa en este caso
        return;
    }

    public override void EveryTick()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        timeAlive-= Time.deltaTime;
        if (timeAlive<=0)
        {
            Destroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemies>();

        if (enemy!=null)
        {
            enemy.TakeDamage(damage);
            Destroy();
        }
    }

   

    // se me habia ocurrido un enemigo q absorba las balas y estas vayan hacia el pero esa idea quedo descartada D:
    //public void NewDir(Vector3 newDir) => dir = newDir.normalized;
}
