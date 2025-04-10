using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : NetworkBehaviour
{

    [Networked] private TickTimer delay { get; set; }
    [Networked] public bool spawnedProjectile { get; set; }

    private Weapons weapons;

    private NetworkCharacterController _cc;
    private Vector3 _forward;

    private ChangeDetector _changeDetector;

    public Material _material;

    [Networked] private NetworkButtons _previousButtons { get; set; }
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _material = GetComponentInChildren<MeshRenderer>().material;
        weapons = GetComponent<Weapons>();

        _forward = transform.forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out TestNetworkInputData data))
        {

            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;

            if (data.reloadPressed)
            {
                weapons.Reload();
                data.reloadPressed = false;
            }

            if (data.buttons.IsSet(TestNetworkInputData.MOUSEBUTTON0))
            {
                bool _holdingPressed = data.isFiringHeld;
                weapons.Fire(_holdingPressed);
            }

            if (data.buttons.IsSet(TestNetworkInputData.MOUSEBUTTON1))
            {
                weapons.Aiming();
            }
            if (!data.buttons.IsSet(TestNetworkInputData.MOUSEBUTTON1))
            {
                weapons.StopAiming();
            }
            if (!data.buttons.IsSet(TestNetworkInputData.MOUSEBUTTON2))
            {
                weapons.Swap();
            }
        }
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    _material.color = Color.black;
                    break;
            }
        }
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);

    }
}