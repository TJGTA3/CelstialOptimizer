using Celstial.Functions;
using Celstial.Main;
using UnityEngine;

namespace Celstial
{
    public class Loader : MonoBehaviour
    {
        static GameObject go;
        public static void Init()
        {
            go = new GameObject("Celstial Optimizer");
            go.AddComponent<Menu>();
            go.AddComponent<Entity>();
            go.AddComponent<Misc>();
            DontDestroyOnLoad(go);
        }
    }
}