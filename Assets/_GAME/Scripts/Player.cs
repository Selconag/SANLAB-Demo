using UnityEngine;
using System;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : Singleton<Player>
{
    [Header("Variables")]
    [SerializeField] private Vector3 dragOffset;
    [SerializeField] private Vector2 rayCastAdditionalPosition = Vector2.zero;
    [SerializeField] private float dragSpeed = 5.0f;
    [SerializeField] private float minDragHeight, maxDragHeight;
    [SerializeField] private float minDragWidth, maxDragWidth;
    [SerializeField] private float holdingObjectMaxTime = 10f;

    private bool gameStarted;
    private Vector2 currentClickPosition;

    [SerializeField] private float holdingObjectTimer = 0f;
    private bool continueDrag = false;

    private bool midMergeEvent;
    private bool touchEvent = false;
    private Vector3 lastPosition;

    [Header("References")]
    [SerializeField] private Transform draggedObjTransform;
    [SerializeField] private ConnectableObject draggedObjRef;
    [SerializeField] private Camera activeCam;

    public static Action<ConnectableType, bool> OnObjectGrab;
    //public static Action<ConnectableType, bool> OnObjectRelease;

    private void Start()
    {
        if(activeCam == null) activeCam = Camera.main;
    }

    void Update()
    {
        //Actions will be here
        if (Input.GetMouseButton(0))
        {
            // Get the mouse click position in screen coordinates
            currentClickPosition = Input.mousePosition;
            touchEvent = true;
        }
        else
        {
            touchEvent = false;
        };

        //If End game GUI is open, prevent player from clicking objects
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        #region Raycast
        //Upload touch system to just touch here
        if (touchEvent && draggedObjTransform == null)
        {
            //Vector3 hitPos = new Vector3(currentTouchPosition.x, currentTouchPosition.y, 0);
            RaycastHit hit;
            Ray ray = activeCam.ScreenPointToRay(currentClickPosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("Ray Hit");
                if (hit.transform.tag == "Connectable" || hit.transform.tag == "Connected")
                {
                    //Start dragging it around
                    draggedObjTransform = hit.transform;
                    draggedObjRef = draggedObjTransform.GetComponent<ConnectableObject>();
                    draggedObjTransform.parent = null;
                    draggedObjRef.Collider.enabled = false;
                    continueDrag = true;
                    //Invoke onobjectgrab event
                    OnObjectGrab?.Invoke(draggedObjRef.ObjectType, true);
                }
            }
        }

        //Touch happened in first frame, now continuing to touch
        else if (touchEvent && continueDrag && draggedObjTransform != null)
        {
            holdingObjectTimer += Time.deltaTime;
            var worldPosWithFinger = activeCam.ScreenToWorldPoint(new Vector3(currentClickPosition.x + dragOffset.x, currentClickPosition.y + dragOffset.y, dragOffset.z)); // + MovementOffsetToCursor

            //Move object to active mouse position
            draggedObjTransform.position = Vector3.Lerp(draggedObjTransform.position, worldPosWithFinger, Time.deltaTime * dragSpeed);
            float newX = Mathf.Clamp(draggedObjTransform.position.x, minDragWidth, maxDragWidth);
            float newY = Mathf.Clamp(draggedObjTransform.position.y, minDragHeight, maxDragHeight);
            draggedObjTransform.position = new Vector3(newX, newY, draggedObjTransform.position.z);


            RaycastHit hit;
            Ray ray = activeCam.ScreenPointToRay(currentClickPosition);
            //Continue dragging
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //If object holded for too long, cut the process and return object to old place
                if (holdingObjectTimer > holdingObjectMaxTime)
                {
                    holdingObjectTimer = 0;
                    //Release object
                    draggedObjRef.Collider.enabled = true;
                    draggedObjTransform.parent = draggedObjRef.transform;
                    draggedObjTransform = null;
                    continueDrag = false;
                    AudioManager.Instance.PlaySoundEffect(SoundEffects.WrongConnect);
                    return;
                }
            }

        }


        //Touch stopped this frame, check for connection
        else if (!touchEvent && continueDrag && draggedObjTransform != null)
        {
            holdingObjectTimer = 0;
            RaycastHit hit;
            Ray ray = activeCam.ScreenPointToRay(currentClickPosition + rayCastAdditionalPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                try
                {
                    //if (ConnectionManager.Instance.ConnectObjects(draggedObjRef, hit.transform.GetComponent<ConnectableObject>().GetBaseConnectableObject(draggedObjRef.ObjectType), hit.transform))
                    if (!ConnectionManager.Instance.ConnectObjects(draggedObjRef, hit.transform.parent.GetComponent<ConnectableObject>(), hit.transform))
                    {
                        AudioManager.Instance.PlaySoundEffect(SoundEffects.WrongConnect);
                        draggedObjTransform.parent = draggedObjRef.transform;
                    }
                    continueDrag = false;
                    OnObjectGrab?.Invoke(draggedObjRef.ObjectType, false);

                    draggedObjRef.Collider.enabled = true;
                    //draggedObjTransform.parent = draggedObjRef.transform;

                    draggedObjTransform = null;
                    draggedObjRef = null;
                }
                catch (WrongObjectException)
                {
                    continueDrag = false;
                    OnObjectGrab?.Invoke(draggedObjRef.ObjectType,false);

                    draggedObjRef.Collider.enabled = true;
                    draggedObjTransform.parent = draggedObjRef.transform;

                    draggedObjTransform = null;
                    draggedObjRef = null;
                    throw;
                }
            }
            else
            {
                continueDrag = false;
                OnObjectGrab?.Invoke(draggedObjRef.ObjectType, false);

                draggedObjRef.Collider.enabled = true;

                draggedObjTransform = null;
                draggedObjRef = null;
            }
        }

        #endregion



    }

}