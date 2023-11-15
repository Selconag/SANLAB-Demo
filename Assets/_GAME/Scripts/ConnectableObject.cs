using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
    using static ConnectionDictionary;

public class ConnectableObject : MonoBehaviour
{
    [Header("Variables")]
    public ConnectableType ObjectType;
    public bool IsPlaced;

    [Header("References")]
    public Collider Collider;
    private Transform parentObject;
    //public Transform MoveTarget = null;
    public AnimationCurve ShiftingMovementCurve;
    public Transform ParticleSystemPoint;

    [Header("Lists")]
    [SerializeField] List<ConnectableObject> hiddenObjects;
    public List<ConnectableObject> ConnectableObjectPoints;
    public List<ConnectionStatus> Connections;
    //public List<GameObject> OutlinedObjects;
    public Transform ParentObject
    {
        get { return parentObject; }
        set { parentObject = value; }
    }

    private void Awake()
    {
        Player.OnObjectGrab += ToggleHiddenObject;
        //Player.OnObjectRelease += ToggleHiddenObject;
    }

    private void OnDestroy()
    {
        Player.OnObjectGrab -= ToggleHiddenObject;
        //Player.OnObjectRelease -= ToggleHiddenObject;
    }

    public ConnectableObject GetBaseConnectableObject(ConnectableType type)
    {
        //if(parentObject != null) return parentObject.GetComponent<ConnectableObject>();
        //return this;
        //Find target object across connectable objects list
        foreach (ConnectableObject hiddenObject in hiddenObjects)
        {
            //If grabbed object is the same type as hidden object, toggle visibility of hidden object
            if (hiddenObject.ObjectType == type)
            {
                return hiddenObject;
            }
        }
        return this;
    }

    private void ToggleHiddenObject(ConnectableType type)
    {
        if (type == ObjectType) return;
        foreach (ConnectableObject hiddenObject in hiddenObjects)
        {
            //If grabbed object is the same type as hidden object, toggle visibility of hidden object
            if (hiddenObject.ObjectType == type && !hiddenObject.IsPlaced)
            {
                hiddenObject.gameObject.SetActive(!hiddenObject.gameObject.activeSelf);
            }
        }
    }

    public void ToggleConnectableSystem()
    {
        IsPlaced = !IsPlaced;
        if (!IsPlaced)
            Player.OnObjectGrab += ToggleHiddenObject;
        else
            Player.OnObjectGrab -= ToggleHiddenObject;
        GetComponent<Outline>().enabled = !IsPlaced;
        GetComponent<SphereCollider>().enabled = !IsPlaced;
    }


    public void MoveToTarget(Transform targetPlace)
    {
        //sourceObj.ParentObject = targetObj.transform;
    }

    public void CompletionSequence()
    {
        //Play sound
        AudioManager.Instance.PlaySoundEffect(SoundEffects.Success);
        //Play particle effect
        ParticleManager.Instance.PlayParticleEffect(ParticleEffects.Connect,transform.position);
    }

}
