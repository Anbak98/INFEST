using Fusion;
using KINEMATION.FPSAnimationPack.Scripts.Player;
using KINEMATION.FPSAnimationPack.Scripts.Sounds;
using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using KINEMATION.KAnimationCore.Runtime.Core;
using KINEMATION.ProceduralRecoilAnimationSystem.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct IKTransforms
{
    public Transform tip;
    public Transform mid;
    public Transform root;
}

public class WeaponSpawner : NetworkBehaviour
{
    [SerializeField] private Player _player;
    public List<Weapon> Weapons;
    private List<Weapon> _removeWeapon = new List<Weapon>();
    public bool IsSwitching => _switchTimer.ExpiredOrNotRunning(Runner) == false;
    private TickTimer _switchTimer { get; set; }


    public void Init()
    {
        _animator = GetComponent<Animator>();
        _recoilAnimation = GetComponent<RecoilAnimation>();
        _playerSound = GetComponent<FPSPlayerSound>();

        _triggerDisciplineLayerIndex = _animator.GetLayerIndex("TriggerDiscipline");
        _rightHandLayerIndex = _animator.GetLayerIndex("RightHand");
        _tacSprintLayerIndex = _animator.GetLayerIndex("TacSprint");

        KTransform root = new KTransform(transform.root);
        var localCamera = root.GetRelativeTransform(new KTransform(cameraPoint), false);

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].key == _player.statHandler.info.data.StartAuxiliaryWeapon)
            {
                _player.inventory.auxiliaryWeapon[0] = Weapons[i];
                _player.inventory.auxiliaryWeapon[0].IsCollected = true;
            }

            if (Weapons[i].key == _player.statHandler.info.data.StartWeapon1)
            {
                _player.inventory.weapon[0] = Weapons[i];
                _player.inventory.weapon[0].IsCollected = true;
            }

            if (_player.inventory.auxiliaryWeapon[0] != null && _player.inventory.weapon[0] != null)
                break;
        }

        _player.inventory.equippedWeapon = _player.inventory.auxiliaryWeapon[0];

        foreach (var prefab in Weapons)
        {
            var prefabComponent = prefab;
            if (prefabComponent == null) continue;

            prefabComponent.Init();
            _prefabComponents.Add(prefabComponent);


            var weaponComponent = prefab;
            var component = prefab.FPSWeapon;
            component.Initialize(gameObject);

            KTransform weaponT = new KTransform(weaponBone);
            component.rightHandPose = new KTransform(rightHand.tip).GetRelativeTransform(weaponT, false);

            var localWeapon = root.GetRelativeTransform(weaponT, false);

            localWeapon.rotation *= ANIMATED_OFFSET;

            component.adsPose.position = localCamera.position - localWeapon.position;
            component.adsPose.rotation = Quaternion.Inverse(localWeapon.rotation);

            if (prefabComponent.IsCollected)
                _weapons.Add(weaponComponent);

            prefab.gameObject.SetActive(false);
        }

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (_weapons[i].key == _player.statHandler.info.data.StartAuxiliaryWeapon)
                _activeWeaponIndex = i;
        }

        GetActiveWeapon().gameObject.SetActive(true);

        GetActiveWeapon().OnEquipped();
    }
    public void OnMoveAnimation(Vector3 direction)
    {
        _moveInput = direction;
    }

    public void Fire(bool holdingPressed)
    {
        if (_weapons[_activeWeaponIndex].IsReloading || IsSwitching) return;
        if (throwChk) return;

        _weapons[_activeWeaponIndex].Fire(holdingPressed);
    }

    public void Reload()
    {
        if (_weapons[_activeWeaponIndex].IsReloading || IsSwitching) return;
        if (throwChk) return;


        _weapons[_activeWeaponIndex].Reload();

    }
    private float _value = 0;
    int _removeIndex = -1;
    bool _saleChk = false;
    public void Swap(float value, bool Sale = false)
    {
        if (_weapons[_activeWeaponIndex].IsReloading || IsSwitching) return;
        if (value == 0f) return;
        if (_weapons.Count == 1) return;
        if (throwChk) return;

        float delay = GetActiveWeapon().OnUnEquipped();
        //GetActiveWeapon().gameObject.SetActive(false);
        _value = value;
        _switchTimer = TickTimer.CreateFromSeconds(Runner, delay);
        _saleChk = Sale;

        if (HasStateAuthority)
            RPC_OnChangeWeapon(delay);

        //SetWeaponVisible();

        //GetActiveWeapon().gameObject.SetActive(true);
        //GetActiveWeapon().OnEquipped_Immediate();
        //GetActiveWeapon().RPC_OnEquipped();

    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_OnChangeWeapon(float delay)
    {
        if (_weapons.Count <= 1) return;
        //float delay = GetActiveWeapon().OnUnEquipped();
        Invoke(nameof(EquipWeapon_Incremental), delay);
    }
    private void EquipWeapon_Incremental()
    {
        GetActiveWeapon().gameObject.SetActive(false);

        _removeIndex = _activeWeaponIndex;
        _activeWeaponIndex += _value > 0f ? 1 : -1;

        if (_saleChk)
        {
            _weapons[_removeIndex].curBullet = _weapons[_removeIndex].instance.data.MaxBullet;
            _weapons[_removeIndex].curMagazineBullet = _weapons[_removeIndex].instance.data.MagazineBullet;
            _weapons[_removeIndex].IsCollected = false;
            //_weapons.Remove(_weapons[_removeIndex]);
            _weapons.RemoveAt(_removeIndex);


            if (_value > 0f)
                _activeWeaponIndex = _removeIndex % _weapons.Count;
            else
                _activeWeaponIndex = (_removeIndex - 1 + _weapons.Count) % _weapons.Count;
        }


        if (_activeWeaponIndex < 0) _activeWeaponIndex = _weapons.Count - 1;
        if (_activeWeaponIndex >= _weapons.Count) _activeWeaponIndex = 0;

        GetActiveWeapon().OnEquipped();

        _player.inventory.equippedWeapon = _weapons[_activeWeaponIndex];

        _switchTimer = TickTimer.CreateFromSeconds(Runner, 0.8f);
        if (_weapons[_activeWeaponIndex].key == Weapons[0].key)
            _switchTimer = TickTimer.CreateFromSeconds(Runner, 4f);

        Invoke(nameof(SetWeaponVisible), 0.1f);

        _saleChk = false;
    }


    public void Aiming(bool _isAiming)
    {
        if (_weapons[_activeWeaponIndex].IsReloading == true) return;
        if (throwChk) return;

        RPC_OnAim(_isAiming);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnAim(bool value)
    {
        bool wasAiming = _isAiming;
        _isAiming = value;
        _recoilAnimation.isAiming = _isAiming;

        _weapons[_activeWeaponIndex].Aiming();

        if (wasAiming != _isAiming)
        {
            _weapons[_activeWeaponIndex].StopAiming();

            _playerSound.PlayAimSound(_isAiming);
            PlayIkMotion(playerSettings.aimingMotion);
        }
    }

    bool throwChk = false;
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnThrowGrenade()
    {
        if (_weapons[_activeWeaponIndex].IsReloading || IsSwitching) return;
        if (throwChk) return;
        if (_player.inventory.consume[0]?.curNum <= 0) return;
        if (_player.inventory.consume[0] == null) return;

        throwChk = true;
        _animator.SetTrigger(THROW_GRENADE);
        Invoke(nameof(RPC_ThrowGrenade), GetActiveWeapon().UnEquipDelay);
        _switchTimer = TickTimer.CreateFromSeconds(Runner, 2.7f);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ThrowGrenade()
    {
        _player.Consumes.ActivateGrenade();
        GetActiveWeapon().gameObject.SetActive(false);
        Invoke(nameof(Throw), 1.5f);
        Invoke(nameof(SetWeaponVisible), 0.5f);
        Invoke(nameof(DeactivateGrenade), 2.5f);
    }

    private void Throw()
    {
        _player.Consumes.DeactivateGrenade();
        _player.Consumes.Throw();
    }

    private void DeactivateGrenade()
    {
        throwChk = false;
    }

    #region Model
    public float AdsWeight => _adsWeight;

    public FPSPlayerSettings playerSettings;

    [Header("Skeleton")]
    [SerializeField] private Transform skeletonRoot;
    [SerializeField] private Transform weaponBone;
    [SerializeField] private Transform weaponBoneAdditive;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private IKTransforms rightHand;
    [SerializeField] private IKTransforms leftHand;

    private KTwoBoneIkData _rightHandIk;
    private KTwoBoneIkData _leftHandIk;

    private RecoilAnimation _recoilAnimation;
    private float _adsWeight;
    public List<Weapon> _weapons = new List<Weapon>();
    private List<Weapon> _prefabComponents = new List<Weapon>();
    private int _activeWeaponIndex = 0;

    private Animator _animator;

    private static int RIGHT_HAND_WEIGHT = Animator.StringToHash("RightHandWeight");
    private static int TAC_SPRINT_WEIGHT = Animator.StringToHash("TacSprintWeight");
    private static int GRENADE_WEIGHT = Animator.StringToHash("GrenadeWeight");
    private static int THROW_GRENADE = Animator.StringToHash("ThrowGrenade");
    private static int GAIT = Animator.StringToHash("Gait");
    private static int IS_IN_AIR = Animator.StringToHash("IsInAir");

    private static Quaternion ANIMATED_OFFSET = Quaternion.Euler(90f, 0f, 0f);

    private int _tacSprintLayerIndex;
    private int _triggerDisciplineLayerIndex;
    private int _rightHandLayerIndex;

    [HideInInspector] public bool _isAiming;

    private Vector3 _moveInput;
    private float _smoothGait;

    private bool _bSprinting;
    private bool _bTacSprinting;

    private FPSPlayerSound _playerSound;

    private float _ikMotionPlayBack;
    private KTransform _ikMotion = KTransform.Identity;
    private KTransform _cachedIkMotion = KTransform.Identity;
    private IKMotion _activeMotion;

    public void ExpectedRemove(Weapon weapon)
    {
        _removeWeapon.Add(weapon);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        _weapons.Remove(weapon);
    }

    private void SetWeaponVisible()
    {
        GetActiveWeapon().gameObject.SetActive(true);
    }

    public FPSWeapon GetActiveWeapon()
    {
        return _weapons[_activeWeaponIndex].FPSWeapon;
    }

    public FPSWeapon GetActivePrefab()
    {
        return _prefabComponents[_activeWeaponIndex].FPSWeapon;
    }

    private float GetDesiredGait()
    {
        if (_bTacSprinting) return 3f;
        if (_bSprinting) return 2f;
        return _moveInput.magnitude;
    }

    private void Update()
    {

        _adsWeight = Mathf.Clamp01(_adsWeight + playerSettings.aimSpeed * Time.deltaTime * (_isAiming ? 1f : -1f));

        _smoothGait = Mathf.Lerp(_smoothGait, GetDesiredGait(),
            KMath.ExpDecayAlpha(playerSettings.gaitSmoothing, Time.deltaTime));

        _animator.SetFloat(GAIT, _smoothGait);
        _animator.SetLayerWeight(_tacSprintLayerIndex, Mathf.Clamp01(_smoothGait - 2f));

        bool triggerAllowed = GetActiveWeapon().weaponSettings.useSprintTriggerDiscipline;

        foreach (var weapon in _removeWeapon)
        {
            RemoveWeapon(weapon);
        }

        _removeWeapon.Clear();


        _animator.SetLayerWeight(_triggerDisciplineLayerIndex,
        triggerAllowed ? _animator.GetFloat(TAC_SPRINT_WEIGHT) : 0f);

        _animator.SetLayerWeight(_rightHandLayerIndex, _animator.GetFloat(RIGHT_HAND_WEIGHT));
    }

    private void SetupIkData(ref KTwoBoneIkData ikData, in KTransform target, in IKTransforms transforms,
            float weight = 1f)
    {
        ikData.target = target;

        ikData.tip = new KTransform(transforms.tip);
        ikData.mid = ikData.hint = new KTransform(transforms.mid);
        ikData.root = new KTransform(transforms.root);

        ikData.hintWeight = weight;
        ikData.posWeight = weight;
        ikData.rotWeight = weight;
    }

    private void ApplyIkData(in KTwoBoneIkData ikData, in IKTransforms transforms)
    {
        transforms.root.rotation = ikData.root.rotation;
        transforms.mid.rotation = ikData.mid.rotation;
        transforms.tip.rotation = ikData.tip.rotation;
    }

    private void ProcessOffsets(ref KTransform weaponT)
    {
        var root = transform.root;
        KTransform rootT = new KTransform(root);
        var weaponOffset = GetActiveWeapon().weaponSettings.ikOffset;

        float mask = 1f - _animator.GetFloat(TAC_SPRINT_WEIGHT);
        weaponT.position = KAnimationMath.MoveInSpace(rootT, weaponT, weaponOffset, mask);

        var settings = GetActiveWeapon().weaponSettings;
        KAnimationMath.MoveInSpace(root, rightHand.root, settings.rightClavicleOffset, mask);
        KAnimationMath.MoveInSpace(root, leftHand.root, settings.leftClavicleOffset, mask);
    }

    private void ProcessAdditives(ref KTransform weaponT)
    {
        KTransform rootT = new KTransform(skeletonRoot);
        KTransform additive = rootT.GetRelativeTransform(new KTransform(weaponBoneAdditive), false);

        float weight = Mathf.Lerp(1f, 0.3f, _adsWeight) * (1f - _animator.GetFloat(GRENADE_WEIGHT));

        weaponT.position = KAnimationMath.MoveInSpace(rootT, weaponT, additive.position, weight);
        weaponT.rotation = KAnimationMath.RotateInSpace(rootT, weaponT, additive.rotation, weight);
    }

    private void ProcessRecoil(ref KTransform weaponT)
    {
        KTransform recoil = new KTransform()
        {
            rotation = _recoilAnimation.OutRot,
            position = _recoilAnimation.OutLoc,
        };

        KTransform root = new KTransform(transform);
        weaponT.position = KAnimationMath.MoveInSpace(root, weaponT, recoil.position, 1f);
        weaponT.rotation = KAnimationMath.RotateInSpace(root, weaponT, recoil.rotation, 1f);
    }

    private void ProcessAds(ref KTransform weaponT)
    {
        var weapon = GetActiveWeapon();
        var weaponOffset = weapon.weaponSettings.ikOffset;
        var adsBlendWeight = weapon.weaponSettings.adsBlend;

        // 1. 카메라 기준으로 무기 위치와 방향 계산
        Vector3 targetPosition = cameraPoint.position;
        Quaternion targetRotation = cameraPoint.rotation;

        // 2. 무기 자체의 aimPoint 위치/회전 계산
        var weaponAimPoint = weapon.aimPoint;
        var aimOffsetPos = weaponAimPoint.localPosition + weapon.weaponSettings.aimPointOffset;
        var aimOffsetRot = weaponAimPoint.localRotation;

        // 3. 카메라 기준에서 무기 aimPoint가 정중앙으로 향하도록 이동 (보간 포함)
        Vector3 adsPosition = targetPosition - (targetRotation * aimOffsetPos);
        Quaternion adsRotation = targetRotation * Quaternion.Inverse(aimOffsetRot);

        // 4. 보간 적용 (카메라 기준 -> 무기 위치)
        adsPosition = Vector3.Lerp(cameraPoint.position, adsPosition, adsBlendWeight);
        adsRotation = Quaternion.Slerp(cameraPoint.rotation, adsRotation, adsBlendWeight);

        // 5. 부드러운 최종 보간 적용
        float weight = KCurves.EaseSine(0f, 1f, _adsWeight);
        weaponT.position = Vector3.Lerp(weaponT.position, adsPosition, weight);
        weaponT.rotation = Quaternion.Slerp(weaponT.rotation, adsRotation, weight);
    }

    private KTransform GetWeaponPose()
    {
        KTransform defaultWorldPose =
            new KTransform(rightHand.tip).GetWorldTransform(GetActiveWeapon().rightHandPose, false);
        float weight = _animator.GetFloat(RIGHT_HAND_WEIGHT);

        return KTransform.Lerp(new KTransform(weaponBone), defaultWorldPose, weight);
    }

    private void PlayIkMotion(IKMotion newMotion)
    {
        _ikMotionPlayBack = 0f;
        _cachedIkMotion = _ikMotion;
        _activeMotion = newMotion;
    }

    private void ProcessIkMotion(ref KTransform weaponT)
    {
        if (_activeMotion == null) return;

        _ikMotionPlayBack = Mathf.Clamp(_ikMotionPlayBack + _activeMotion.playRate * Time.deltaTime, 0f,
            _activeMotion.GetLength());

        Vector3 positionTarget = _activeMotion.translationCurves.GetValue(_ikMotionPlayBack);
        positionTarget.x *= _activeMotion.translationScale.x;
        positionTarget.y *= _activeMotion.translationScale.y;
        positionTarget.z *= _activeMotion.translationScale.z;

        Vector3 rotationTarget = _activeMotion.rotationCurves.GetValue(_ikMotionPlayBack);
        rotationTarget.x *= _activeMotion.rotationScale.x;
        rotationTarget.y *= _activeMotion.rotationScale.y;
        rotationTarget.z *= _activeMotion.rotationScale.z;

        _ikMotion.position = positionTarget;
        _ikMotion.rotation = Quaternion.Euler(rotationTarget);

        if (!Mathf.Approximately(_activeMotion.blendTime, 0f))
        {
            _ikMotion = KTransform.Lerp(_cachedIkMotion, _ikMotion,
                _ikMotionPlayBack / _activeMotion.blendTime);
        }

        var root = new KTransform(transform.root);
        weaponT.position = KAnimationMath.MoveInSpace(root, weaponT, _ikMotion.position, 1f);
        weaponT.rotation = KAnimationMath.RotateInSpace(root, weaponT, _ikMotion.rotation, 1f);
    }

    private void LateUpdate()
    {
        KAnimationMath.RotateInSpace(transform.root, rightHand.tip,
            GetActiveWeapon().weaponSettings.rightHandSprintOffset, _animator.GetFloat(TAC_SPRINT_WEIGHT));

        KTransform weaponTransform = GetWeaponPose();

        weaponTransform.rotation = KAnimationMath.RotateInSpace(weaponTransform, weaponTransform,
            ANIMATED_OFFSET, 1f);

        KTransform rightHandTarget = weaponTransform.GetRelativeTransform(new KTransform(rightHand.tip), false);
        KTransform leftHandTarget = weaponTransform.GetRelativeTransform(new KTransform(leftHand.tip), false);

        ProcessOffsets(ref weaponTransform);
        ProcessAds(ref weaponTransform);
        ProcessAdditives(ref weaponTransform);
        ProcessIkMotion(ref weaponTransform);
        ProcessRecoil(ref weaponTransform);

        weaponBone.position = weaponTransform.position;
        weaponBone.rotation = weaponTransform.rotation;

        rightHandTarget = weaponTransform.GetWorldTransform(rightHandTarget, false);
        leftHandTarget = weaponTransform.GetWorldTransform(leftHandTarget, false);

        SetupIkData(ref _rightHandIk, rightHandTarget, rightHand, playerSettings.ikWeight);
        SetupIkData(ref _leftHandIk, leftHandTarget, leftHand, playerSettings.ikWeight);

        KTwoBoneIK.Solve(ref _rightHandIk);
        KTwoBoneIK.Solve(ref _leftHandIk);

        ApplyIkData(_rightHandIk, rightHand);
        ApplyIkData(_leftHandIk, leftHand);
    }
    #endregion
}
