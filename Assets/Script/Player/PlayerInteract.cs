using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private float distance = 3f;

    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;

    private Inputmanager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<Inputmanager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        //create a ray at center of a camera, shooting outwards.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable to store our collision infomation.
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interacable>() != null)
            {
                Interacable interacable = hitInfo.collider.GetComponent<Interacable>();
                playerUI.UpdateText(interacable.promptMessage);
                if (inputManager.onFoot.Interact.triggered)
                {
                    interacable.BaseInteract();
                }
            }
        };

    }
}
