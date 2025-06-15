using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    //                                            |    can remove this   | (I choose not to)
    private readonly NetworkVariable<Vector2> netPos = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<SpriteRenderer> netColor = new NetworkVariable<SpriteRenderer>(writePerm: NetworkVariableWritePermission.Owner);

    public GameObject playerBody;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = playerBody.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsOwner)
        {
            netPos.Value = transform.position;
            netColor.Value = sprite;
        }
        else
        {
            transform.position = netPos.Value;
            sprite = netColor.Value;
        }
    }
}
