
using System.Collections.Generic;

public static class ScreenManager 
{
    static List<IPausable> pausables =  new List<IPausable>();

   public static float time = 1;
   public static bool isPaused;

    public static void AddPausable(IPausable item)
    {
        if (!pausables.Contains(item))
        {
            pausables.Add(item);
        }
    }

    public static void RemovePausable(IPausable item)
    {
        if (!pausables.Contains(item))
        {
            pausables.Add(item);
        }
    }


    public static void PauseGame()
    {
        time = 0 ;
        isPaused=true ;
        foreach (IPausable item in pausables)
        {
            item.Pause();
        }
    }

    public static void ResumeGame()
    {
        time = 1 ;
        isPaused = false;
        foreach (IPausable item in pausables)
        {
            item.Resume();
        }
    }
}

public interface IPausable
{
    void Pause();

    void Resume();
}

