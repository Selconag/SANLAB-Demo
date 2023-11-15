using UnityEngine;
using System.Collections.Generic;

public class ConnectionDictionary : MonoBehaviour
{
    [System.Serializable]
    public class ConnectionStatus
    {
        //public ConnectableType ObjectType;
        public Transform ObjTransform;
        public bool IsConnected;
    }
}
