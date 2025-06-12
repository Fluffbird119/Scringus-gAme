using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FancyPlayerNetwork : NetworkBehaviour
{
    //                                                                 |         can remove this        | (I choose not to)
    private readonly NetworkVariable<PlayerNetworkData> netState = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);

    public float interpolationTime = 0.1f;

    private Vector2 vel;

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

            netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Color = sprite.color
            };
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position, netState.Value.Position, ref vel, interpolationTime);

            sprite.color = netState.Value.Color;
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float x, y;

        private byte r, g, b, a;

        internal Vector2 Position
        {
            get => new Vector2(x, y);

            set
            {
                x = value.x;
                y = value.y;
            }
        }

        
        internal Color32 Color
        {
            get => new Color32(r, g, b, a);

            set
            {
                r = value.r;
                g = value.g;
                b = value.b;
                a = value.a;
            }
        }
        

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref y);
        }
    }
}
