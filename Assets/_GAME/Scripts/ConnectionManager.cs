using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static ConnectionDictionary;
using System.Linq;
using Unity.VisualScripting;
//This determines which object can connect to which, the name resembles the part to connect not the part itself
public enum ConnectableType { Base, PinClip, WristPin, Rod, RodBearingRod, RodBearingCap, RodCap, RodBolt }

public class ConnectionManager : Singleton<ConnectionManager>
{
    [SerializeField] private Vector3 placement3DOffset;
    public bool ConnectionDone;

    [Header("References")]
    public Dictionary<Transform, bool> ConnectedObjects = new Dictionary<Transform, bool>();
    public List<ConnectionStatus> ConnectionInformations = new List<ConnectionStatus>();

    private bool CheckConnectionRequirements(ConnectableType sourceType, ConnectableType targetType)
    {
        switch (sourceType)
        {
            case ConnectableType.PinClip:
            case ConnectableType.WristPin:
                return targetType == ConnectableType.Base;

            case ConnectableType.Rod:
                return targetType == ConnectableType.WristPin;

            case ConnectableType.RodBearingRod:
            case ConnectableType.RodCap:
            case ConnectableType.RodBearingCap:
                return targetType == ConnectableType.Rod;

            case ConnectableType.RodBolt:
                return targetType == ConnectableType.RodCap;

            default:
                return false;
        }

    }

    public bool ConnectObjects(ConnectableObject sourceObj, ConnectableObject targetObj, Transform targetPlace)
    {
        //Check if connection can happen
        if (!CheckConnectionRequirements(sourceObj.ObjectType, targetObj.ObjectType))
        {
            //Check for a reverse connection availability
            if (CheckConnectionRequirements(targetObj.ObjectType, sourceObj.ObjectType))
                return ConnectObjects(targetObj, sourceObj, targetPlace);
            return false;
        }

        //Check if any object is placed here
        if(targetPlace.GetComponent<ConnectableObject>().IsPlaced) return false;

        sourceObj.ParentObject = targetObj.transform;
        sourceObj.tag = "Connected";

        sourceObj.transform.position = targetPlace.position;
        sourceObj.transform.SetParent(targetObj.transform);

        targetPlace.GetComponent<ConnectableObject>().ToggleConnectableSystem();

        //if (ConnectionInformations.ContainsKey(sourceObj.transform))
        //{
        //    ConnectionInformations[sourceObj.transform] = true;
        //    CheckAllConnections();
        //}
        var objFound = ConnectionInformations.FirstOrDefault(x => x.ObjTransform == sourceObj.transform).IsConnected = true;
        if (objFound) CheckAllConnections();

        //urceObj.MoveToTarget(targetPlace);
        //var newTarget = targetObj.ConnectableObjects[0];

        AudioManager.Instance.PlaySoundEffect(SoundEffects.Connect);
        return true;
    }

    private bool CheckAllConnections()
    {
        foreach (var item in ConnectionInformations)
        {
            //If at least 1 connection is not completed, return false therefore not end game
            if (!item.IsConnected) return false;
        }
        //All connections made, the game is over
        Debug.Log("You Win");
        return true;
    }

}
