using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour // ������ ����(���� ����, UI����ֱ� ��) �������ش�.
{
    public StoreController _storeController;
    public Action<string> changeUI;
    public List<int> idList;
    public InputManager inputManager;

    #region  ���� �ݶ��̴� Ʈ���� �޼ҵ�

    #region ��ȣ�ۿ��
    /// <summary>
    /// ��ȣ�ۿ�� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestInteraction(Player _player, PlayerRef _playerRef)
    {
        RPC_Interaction(_player, _playerRef);

        if (_storeController.activeTime)
        {
            _storeController.RPC_Timer(_playerRef);
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� ����
    /// </summary>
    /// 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Interaction(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        // if (_playerRef != _player.Runner.LocalPlayer) return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _storeController.uIShopView.UpdateButtonState();

        //isInteraction = false;
        _storeController.uIShopView.bg.gameObject.SetActive(true);
        _storeController.uIShopView.profile.gameObject.SetActive(true);
        _storeController.uIShopView.interactionText.gameObject.SetActive(false); 
    }
    #endregion

    #region ��ȣ�ۿ�������
    /// <summary>
    /// ��ȣ�ۿ������� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestStopInteraction(PlayerRef _playerRef)
    {
        RPC_StopInteraction(_playerRef);

    }
    /// <summary>
    /// ��ȣ�ۿ� ��������
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_StopInteraction([RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        _storeController.uIShopView.interactionText.gameObject.SetActive(true);
        _storeController.uIShopView.bg.gameObject.SetActive(false);
        _storeController.uIShopView.profile.gameObject.SetActive(false);
    }
    #endregion

    #region OnTriggerEnter ȣ��
    /// <summary>
    /// ������ ������ ������ ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestEnterShopZone(Player _player, PlayerRef _playerRef)
    {
        RPC_EnterShopZone(_player, _playerRef);
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EnterShopZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        _storeController.uIShopView.bg.gameObject.SetActive(false);
        _storeController.uIShopView.profile.gameObject.SetActive(false);
        _storeController.uIShopView.interactionText.gameObject.SetActive(true);
    }
    #endregion

    #region OnTriggerExit ȣ��
    /// <summary>
    /// ������ �������� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestLeaveShopZone(Player _player, PlayerRef _playerRef)
    {
        RPC_LeaveShopZone(_player, _playerRef);
    }

    /// <summary>
    /// ������ ��������
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_LeaveShopZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _player.isInteraction = false;
        _storeController.uIShopView.bg.gameObject.SetActive(false);
        _storeController.uIShopView.profile.gameObject.SetActive(false);
        _storeController.uIShopView.interactionText.gameObject.SetActive(false);
    }
    #endregion

#endregion

#region ���� ���� & �Ǹ� �޼ҵ�

    #region ����
    /// <summary>
    /// ���� ��û �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryBuy(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryBuy(_player, _playerRef, index);
    }

    /// <summary>
    /// ���� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBuy(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        if (idList[index] == 0) return;

        if (_player == null) return;

        Weapon _buyWeapon = null;

        if (idList[index] % 10000 < 700) // ����
        {

            for (int i = 0; i< _player.Weapons.Weapons.Count; i++)
            {
                if (_player.Weapons.Weapons[i].key == idList[index])
                {
                    _buyWeapon = _player.Weapons.Weapons[i];
                    break;
                }
            }
            _player.characterInfoInstance.curGold -= _buyWeapon.instance.data.Price;
            _player.inventory.AddWeponItme(_buyWeapon);
            _buyWeapon.IsCollected = true;
            _player.Weapons._weapons.Add(_buyWeapon);
        }
        else if (idList[index] % 10000 < 1000) // ������
        {
            ConsumeInstance _consumeInstance = new(idList[index]);

            _player.characterInfoInstance.curGold -= _consumeInstance.data.Price;

            _player.inventory.AddConsumeItme(_consumeInstance);
        }
        var inv = _player.inventory;
        int[] invKey = {inv.auxiliaryWeapon[0] != null? inv.auxiliaryWeapon[0].instance.data.key : 0,
                        inv.weapon[0] != null? inv.weapon[0].instance.data.key : 0,
                        inv.weapon[1] != null? inv.weapon[1].instance.data.key : 0,
                        inv.consume[0] != null? inv.consume[0].data.key : 0,
                        inv.consume[1] != null? inv.consume[1].data.key : 0,
                        inv.consume[2] != null? inv.consume[2].data.key : 0};

        for (int i = 0; i < invKey.Length; i++)
        {
            if (invKey[i] == idList[index])
            {
                _storeController.uIShopView.SaleSet(i);
                if (i < 3)
                _storeController.uIShopView.WeaponSet(i);
                else
                _storeController.uIShopView.ItemSet(i-3);
            }
        }
        _storeController.uIShopView.UpdateButtonState();
        _storeController.uIShopView.UpdateSaleButtonState();



    }
    #endregion

    #region �Ǹ�
    /// <summary>
    /// �Ǹ� ��û �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTrySale(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TrySale(_player, _playerRef, index);
    }

    /// <summary>
    /// �Ǹ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TrySale(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        if (_player == null) return;
        

        switch (index)
        {
            case 0: // ��������
                if (_player.inventory.auxiliaryWeapon[0] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.auxiliaryWeapon[0].instance.data.Price / 2;


                    if (_player.inventory.equippedWeapon == _player.inventory.auxiliaryWeapon[0])
                    _player.Weapons.Swap(-1,true);

                //_player.Weapons.Swap(1);
                //    else if (_player.inventory.equippedWeapon == _player.Weapons._weapons[_player.Weapons._weapons.Count - 1])
                //        _player.Weapons.Swap(-1);
                //    else
                //        _player.Weapons.Swap(0);

                _player.inventory.RemoveWeaponItem(_player.inventory.auxiliaryWeapon[0], 0);

                //_player.Weapons._weapons.Remove(_player.inventory.auxiliaryWeapon[0]);
                _storeController.uIShopView.WeaponSet(0);
                break;
            case 1: // �ֹ��� 1
                if (_player.inventory.weapon[0] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.weapon[0].instance.data.Price / 2;

          
                    if (_player.inventory.equippedWeapon == _player.inventory.weapon[0])
                        _player.Weapons.Swap(-1, true);

                //_player.Weapons.Swap(1);
                //    else if (_player.inventory.equippedWeapon == _player.Weapons._weapons[_player.Weapons._weapons.Count - 1])
                //        _player.Weapons.Swap(-1);
                //    else
                //        _player.Weapons.Swap(0);


                //_player.inventory.weapon[0].curBullet = _player.inventory.weapon[0].instance.data.MaxBullet;
                //_player.inventory.weapon[0].curMagazineBullet = _player.inventory.weapon[0].instance.data.MagazineBullet;
                //_player.inventory.weapon[0].IsCollected = false;
                //_player.Weapons._weapons.Remove(_player.inventory.weapon[0]); 
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[0], 0);

                _storeController.uIShopView.WeaponSet(1);
                break;
            case 2: // �ֹ��� 2
                if (_player.inventory.weapon[1] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.weapon[1].instance.data.Price / 2;

                
                    if (_player.inventory.equippedWeapon == _player.inventory.weapon[1])
                    _player.Weapons.Swap(-1, true);

                //_player.Weapons.Swap(1);
                //    else if(_player.inventory.equippedWeapon == _player.Weapons._weapons[_player.Weapons._weapons.Count-1])
                //        _player.Weapons.Swap(-1);
                //    else
                //        _player.Weapons.Swap(0);
                //_player.inventory.weapon[1].curBullet = _player.inventory.weapon[1].instance.data.MaxBullet;
                //_player.inventory.weapon[1].curMagazineBullet = _player.inventory.weapon[1].instance.data.MagazineBullet;
                //_player.inventory.weapon[1].IsCollected = false;
                //_player.Weapons._weapons.Remove(_player.inventory.weapon[1]);
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[1], 1);

                _storeController.uIShopView.WeaponSet(2);
                break;
            case 3: // ������ 1
                if (_player.inventory.consume[0] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.consume[0].data.Price / 2;
                _player.inventory.RemoveConsumeItem(0);
                _storeController.uIShopView.ItemSet(0);
                break;
            case 4: // ������ 2
                if (_player.inventory.consume[1] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.consume[1].data.Price / 2;
                _player.inventory.RemoveConsumeItem(1);
                _storeController.uIShopView.ItemSet(1);
                break;
            case 5: // ������ 3
                if (_player.inventory.consume[2] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.consume[2].data.Price / 2;
                _player.inventory.RemoveConsumeItem(2);
                _storeController.uIShopView.ItemSet(2);
                break;
        }

        Debug.Log("�Ǹ� �� :" + _player.characterInfoInstance.curGold + "\n�ֹ��� : " + _player.inventory.auxiliaryWeapon + "\n�������� : " + _player.inventory.weapon + "\n������ : " + _player.inventory.consume);
        _storeController.uIShopView.UpdateButtonState();
        _storeController.uIShopView.UpdateSaleButtonState();

    }
    #endregion
#endregion

#region ���� �޼ҵ�

    #region ���� ����
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryAllSupplement(Player _player, PlayerRef _playerRef)
    {
        RPC_TryAllSupplement(_player, _playerRef);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryAllSupplement(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        Weapon[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };
        ConsumeInstance[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

        int weaponPrice = 0;
        int itemPrice = 0;
        int totalprice = 0;

        #region ����
        if (weaponInv[0] != null)
        {
            weaponPrice += (weaponInv[0].instance.data.MaxBullet - weaponInv[0].curBullet) * weaponInv[0].instance.data.BulletPrice;
        }

        if (weaponInv[1] != null)
        {
            weaponPrice += (weaponInv[1].instance.data.MaxBullet - weaponInv[1].curBullet) * weaponInv[1].instance.data.BulletPrice;
        }

        if (weaponInv[2] != null)
        {
            weaponPrice += (weaponInv[2].instance.data.MaxBullet - weaponInv[2].curBullet) * weaponInv[2].instance.data.BulletPrice;
        }
        #endregion
        #region ������
        if (itemInv[0] != null)
        {
            itemPrice += (itemInv[0].data.MaxNum - itemInv[0].curNum) * itemInv[0].data.Price;
        }

        if (itemInv[1] != null)
        {
            itemPrice += (itemInv[1].data.MaxNum - itemInv[1].curNum) * itemInv[1].data.Price;
        }

        if (itemInv[2] != null)
        {
            itemPrice += (itemInv[2].data.MaxNum - itemInv[2].curNum) * itemInv[2].data.Price;
        }
        #endregion

        if (_player.characterInfoInstance.curDefGear >= 200)
        {
            totalprice += weaponPrice + itemPrice;
        }
        else
        {
            totalprice += 500 + weaponPrice + itemPrice;
        }

        if (totalprice > _player.characterInfoInstance.curGold) return;

        for (int i = 0; i < 3; i++)
        {
            if (weaponInv[i] != null)
            {
                while (weaponInv[i].instance.data.MaxBullet > weaponInv[i].curBullet)
                {
                    weaponInv[i].SupplementBullet();
                }
                _storeController.uIShopView.WeaponSet(i);
            }

            if (itemInv[i] != null)
            {
                while (itemInv[i].data.MaxNum > itemInv[i].curNum)
                {
                    itemInv[i].AddNum();
                }
                _storeController.uIShopView.ItemSet(i);
            }

        }

        _player.characterInfoInstance.curGold -= totalprice;
        _player.characterInfoInstance.curDefGear += 200;
        _player.characterInfoInstance.curDefGear = Mathf.Min(_player.characterInfoInstance.curDefGear, 200);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #region �� ����
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryDefSupplement(Player _player, PlayerRef _playerRef)
    {
        RPC_TryDefSupplement(_player, _playerRef);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryDefSupplement(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        if (Player.local.characterInfoInstance.curDefGear >= 200) return;
        if (Player.local.characterInfoInstance.curGold < 500) return;


        Player.local.characterInfoInstance.curGold -= 500;
        Player.local.characterInfoInstance.curDefGear += 200;
        Player.local.characterInfoInstance.curDefGear = Mathf.Min(Player.local.characterInfoInstance.curDefGear, 200);
        _storeController.uIShopView.UpdateButtonState();
    }

    #endregion

    #region ź�� ����
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryBulletSupplement(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryBulletSupplement(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBulletSupplement(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        Weapon[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };

        if (weaponInv[index] == null) return;
        if (weaponInv[index].curBullet >= weaponInv[index].instance.data.MaxBullet) return;

        if (weaponInv[index].curBullet + weaponInv[index].instance.data.MagazineBullet >= weaponInv[index].instance.data.MaxBullet)
            _player.characterInfoInstance.curGold -= weaponInv[index].instance.data.BulletPrice * (weaponInv[index].instance.data.MaxBullet - weaponInv[index].curBullet);
        else
            _player.characterInfoInstance.curGold -= weaponInv[index].instance.data.BulletPrice * (weaponInv[index].instance.data.MagazineBullet);

        weaponInv[index].SupplementBullet();
        _storeController.uIShopView.WeaponSet(index);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #region ������ ����
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryItmeSupplement(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryItmeSupplement(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryItmeSupplement(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        ConsumeInstance[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

        if (itemInv[index] == null) return;
        if (itemInv[index].curNum >= itemInv[index].data.MaxNum) return;

        _player.characterInfoInstance.curGold -= itemInv[index].data.Price;
        itemInv[index].AddNum();
        _storeController.uIShopView.ItemSet(index);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #endregion










}
