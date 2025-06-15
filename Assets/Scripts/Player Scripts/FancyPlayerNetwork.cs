using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FancyPlayerNetwork : NetworkBehaviour
{
    //                                                                 |         can remove this        | (I choose not to)
    private readonly NetworkVariable<PlayerNetworkData> netState = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);
    //private readonly NetworkVariable<SpriteRenderer> netColor = new NetworkVariable<SpriteRenderer>(writePerm: NetworkVariableWritePermission.Owner);

    public float interpolationTime = 0.1f;

    private Vector2 vel;

    public GameObject playerBody;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = playerBody.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsOwner)
        {
            netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Sprite = spriteRenderer
            };

            //netColor.Value = sprite;
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position, netState.Value.Position, ref vel, interpolationTime);

            spriteRenderer = netState.Value.Sprite;
            //sprite = netColor.Value;
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float x; 
        private float y;

        private SpriteRenderer playerSprite;


        internal Vector2 Position
        {
            get => new Vector2(x, y);

            set
            {
                x = value.x;
                y = value.y;
            }
        }

        internal SpriteRenderer Sprite
        {
            get => playerSprite;

            set
            {
                playerSprite.sprite = value.sprite;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref y);
        }
    }
}
