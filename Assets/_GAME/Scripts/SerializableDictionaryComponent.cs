using UnityEngine;
using System.Collections.Generic;

public class SerializableDictionaryComponent : MonoBehaviour
{
    [System.Serializable]
    public class ConnectionPlaces
    {
        public ConnectableType ObjectType;
        public Transform ObjectLocation;
    }
    // This is the dictionary you want to serialize
    public List<ConnectionPlaces> ConnectionDictionary = new List<ConnectionPlaces>();

    // Rest of your component code...
}
