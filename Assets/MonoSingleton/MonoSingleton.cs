using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T instance { get; private set; }
    // Start is called before the first frame update
    //protected virtual void Awake()
    //{
    //    if (instance!=null)
    //    {
    //        throw new System.Exception(typeof(MonoSingleton<T>) + "already present at scene");
    //    }

    //    instance = (T)this;
    //}
    protected virtual void ArtificialAwake()
    {
        if (instance != null)
        {
            Debug.LogError("habia mas de un monosingleton en escena, me destruyo");
            Destroy(this);
            return;
        }

        instance = (T)this;
    }
}
