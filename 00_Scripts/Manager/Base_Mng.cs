using Unity.VisualScripting;
using UnityEngine;

public class Base_Mng : MonoBehaviour
{
    public static Base_Mng instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Initalize();
    }

    private void Initalize()
    {
        Build = GetComponentInChildren<Building_Mng>();
        Game = GetComponentInChildren<Game_Mng>();
        Object = GetComponentInChildren<Object_Mng>();
        Wheather = GetComponentInChildren<Wheather_Mng>();
    }

    public static Building_Mng Build;
    public static Game_Mng Game;
    public static Object_Mng Object;
    public static Wheather_Mng Wheather;
}
