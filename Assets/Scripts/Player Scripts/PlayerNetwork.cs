using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    //                                            |    can remove this   | (I choose not to)
    private readonly NetworkVariable<Vector2> netPos = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Color32> netColor = new NetworkVariable<Color32>(writePerm: NetworkVariableWritePermission.Owner);

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
            netColor.Value = sprite.color;
        }
        else
        {
            transform.position = netPos.Value;
            sprite.color = netColor.Value;
        }
    }
}
