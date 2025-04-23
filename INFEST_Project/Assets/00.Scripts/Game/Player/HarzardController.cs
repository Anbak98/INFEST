using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HarzardController : NetworkBehaviour
{    
    // ���콺
    public const byte BUTTON_FIRE = 0;      // ���콺 ����
    public const byte BUTTON_ZOOM = 1;      // ���콺 ������
    public const byte BUTTON_SWAP = 2;      // ���콺 ��

    // Ű����
    public const byte BUTTON_JUMP = 3;      // space
    public const byte BUTTON_RELOAD = 4;    // R
    public const byte BUTTON_INTERACT = 5;  // F
    public const byte BUTTON_USEITEM = 6;   // E
    public const byte BUTTON_RUN = 7;       // lShift
    public const byte BUTTON_SIT = 8;       // lCtrl
    public const byte BUTTON_SCOREBOARD = 9;    // Tab

    // ���ο��� ����ϴ� ����
    public const byte BUTTON_FIREPRESSED = 10;    // ���콺 ����Ŭ�� ����
    public const byte BUTTON_ZOOMPRESSED = 11;    // ���콺 ������Ŭ�� ����

    [SerializeField] NetworkCharacterController _cc;
    [SerializeField] Weapons _weapons;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (GetInput(out NetworkInputData data))
        {        
            if ((data.buttons.Bits & (1 << BUTTON_FIRE)) != 0)
            {
                _weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));
            }

            if ((data.buttons.Bits & (1 << BUTTON_RELOAD)) != 0)
            {
                _weapons.Reload();
            }

            if ((data.buttons.Bits & (1 << BUTTON_JUMP)) != 0)
            {
                _cc.Jump();
            }

            if ((data.buttons.Bits & (1 << BUTTON_RUN)) != 0)
            {
                
            }

            if ((data.buttons.Bits & (1 << BUTTON_SIT)) != 0)
            {
                
            }

            if ((data.buttons.Bits & (1 << BUTTON_INTERACT)) != 0)
            {
                _weapons.Reload();
            }

            _cc.Move(5 * data.direction.magnitude * Runner.DeltaTime * transform.forward);
        }
    }
}
