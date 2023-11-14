using UnityEngine;
public class Singleton<TType> : MonoBehaviour where TType : MonoBehaviour
{

    private static TType _instance;
    public static TType Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (TType)FindObjectOfType(typeof(TType));
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(TType).Name;
                    _instance = obj.AddComponent<TType>();
                }
            }
            return _instance;
        }
    }

}
