using System;
using System.Collections.Generic;
using Fusion;
using INFEST.Game;
using UnityEngine;

public class Store : NetworkBehaviour // ������ ����(���� ����, UI����ֱ� ��) �������ش�.
{
    public StoreController _storeController;
    public List<int> idList;
    public GameObject activatelighting;
    public SphereCollider col;


    #region  ���� �ݶ��̴� Ʈ���� �޼ҵ�

    #region ��ȣ�ۿ��
    /// <summary>
    /// ��ȣ�ۿ�� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestInteraction(PlayerRef _playerRef)
    {
        RPC_Interaction(_playerRef);

        if (_storeController.activeTime)
        {
            _storeController.AddTimer();
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� ����
    /// </summary>
    /// 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Interaction([RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        _storeController.uIShopView = Global.Instance.UIManager.Show<UIShopView>();
        _storeController.uIShopView.StoreInIt(this);
        _storeController.uIShopView.UpdateButtonState();
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
        Global.Instance.UIManager.Show<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIShopView>();

    }
    #endregion

    #region OnTriggerEnter ȣ��
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EnterShopZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        Global.Instance.UIManager.Show<UIInteractiveView>();
    }
    #endregion

    #region OnTriggerExit ȣ��
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
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIShopView>();
    }
    #endregion

    #endregion

    #region ���� ���� & �Ǹ� �޼ҵ�

    #region ����
    /// <summary>
    /// ���� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TryBuy(Player _player, int index)
    {
        if (idList[index] == 0) return;

        if (_player == null) return;


        if (idList[index] % 10000 < 700) // ����
        {
            Weapon _buyWeapon = null;

            for (int i = 0; i < _player.Weapons.Weapons.Count; i++)
            {
                if (_player.Weapons.Weapons[i].key == idList[index])
                {
                    _buyWeapon = _player.Weapons.Weapons[i];
                    break;
                }
            }
            _player.statHandler.CurGold -= _buyWeapon.instance.data.Price;
            _player.inventory.AddWeponItme(_buyWeapon);
            _buyWeapon.IsCollected = true;
            _player.Weapons._weapons.Add(_buyWeapon);
        }
        else if (idList[index] % 10000 < 1000) // ������
        {
            Consume _buyConsume = null;

            for (int i = 0; i < _player.Consumes.Consumes.Count; i++)
            {
                if (_player.Consumes.Consumes[i].key == idList[index])
                {
                    _buyConsume = _player.Consumes.Consumes[i];
                    break;
                }
            }

            _player.statHandler.CurGold -= _buyConsume.instance.data.Price;
            _player.inventory.AddConsumeItme(_buyConsume);
        }
        var inv = _player.inventory;
        int[] invKey = {inv.auxiliaryWeapon[0] != null? inv.auxiliaryWeapon[0].instance.data.key : 0,
                        inv.weapon[0] != null? inv.weapon[0].instance.data.key : 0,
                        inv.weapon[1] != null? inv.weapon[1].instance.data.key : 0,
                        inv.consume[0] != null? inv.consume[0].instance.data.key : 0,
                        inv.consume[1] != null? inv.consume[1].instance.data.key : 0,
                        inv.consume[2] != null? inv.consume[2].instance.data.key : 0};

        for (int i = 0; i < invKey.Length; i++)
        {
            if (invKey[i] == idList[index])
            {
                _storeController.uIShopView.SaleSet(i);
                if (i < 3)
                    _storeController.uIShopView.WeaponSet(i);
                else
                    _storeController.uIShopView.ItemSet(i - 3);
            }
        }

        _storeController.uIShopView.UpdateButtonState();
        _storeController.uIShopView.UpdateSaleButtonState();
    }


    #endregion

    #region �Ǹ�
    /// <summary>
    /// �Ǹ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TrySale(PlayerRef _playerRef, int index)
    {
        Player _player = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(_playerRef);

        if (_player == null) return;


        switch (index)
        {
            case 0: // ��������
                if (_player.inventory.auxiliaryWeapon[0] == null) return;
                _player.statHandler.CurGold += _player.inventory.auxiliaryWeapon[0].instance.data.Price / 2;

                if (_player.inventory.equippedWeapon == _player.inventory.auxiliaryWeapon[0])
                    _player.Weapons.Swap(-1, true);
                else
                {
                    _player.inventory.auxiliaryWeapon[0].curBullet = _player.inventory.auxiliaryWeapon[0].instance.data.MaxBullet;
                    _player.inventory.auxiliaryWeapon[0].curMagazineBullet = _player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                    _player.inventory.auxiliaryWeapon[0].IsCollected = false;
                    _player.Weapons._weapons.Remove(_player.inventory.auxiliaryWeapon[0]);
                }

                _player.inventory.RemoveWeaponItem(_player.inventory.auxiliaryWeapon[0], 0);

                _storeController.uIShopView.WeaponSet(0);
                break;
            case 1: // �ֹ��� 1
                if (_player.inventory.weapon[0] == null) return;
                _player.statHandler.CurGold += _player.inventory.weapon[0].instance.data.Price / 2;

                if (_player.inventory.equippedWeapon == _player.inventory.weapon[0])
                    _player.Weapons.Swap(-1, true);
                else
                {
                    _player.inventory.weapon[0].curBullet = _player.inventory.weapon[0].instance.data.MaxBullet;
                    _player.inventory.weapon[0].curMagazineBullet = _player.inventory.weapon[0].instance.data.MagazineBullet;
                    _player.inventory.weapon[0].IsCollected = false;
                    _player.Weapons._weapons.Remove(_player.inventory.weapon[0]);

                }
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[0], 0);

                _storeController.uIShopView.WeaponSet(1);
                break;
            case 2: // �ֹ��� 2
                if (_player.inventory.weapon[1] == null) return;
                _player.statHandler.CurGold += _player.inventory.weapon[1].instance.data.Price / 2;

                if (_player.inventory.equippedWeapon == _player.inventory.weapon[1])
                    _player.Weapons.Swap(-1, true);
                else
                {
                    _player.inventory.weapon[1].curBullet = _player.inventory.weapon[1].instance.data.MaxBullet;
                    _player.inventory.weapon[1].curMagazineBullet = _player.inventory.weapon[1].instance.data.MagazineBullet;
                    _player.inventory.weapon[1].IsCollected = false;
                    _player.Weapons._weapons.Remove(_player.inventory.weapon[1]);
                }
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[1], 1);

                _storeController.uIShopView.WeaponSet(2);
                break;
            case 3: // ������ 1
                if (_player.inventory.consume[0] == null) return;
                _player.statHandler.CurGold += _player.inventory.consume[0].instance.data.Price / 2;
                _player.inventory.RemoveConsumeItem(0);
                _storeController.uIShopView.ItemSet(0);
                break;
            case 4: // ������ 2
                if (_player.inventory.consume[1] == null) return;
                _player.statHandler.CurGold += _player.inventory.consume[1].instance.data.Price / 2;
                _player.inventory.RemoveConsumeItem(1);
                _storeController.uIShopView.ItemSet(1);
                break;
            case 5: // ������ 3
                if (_player.inventory.consume[2] == null) return;
                _player.statHandler.CurGold += _player.inventory.consume[2].instance.data.Price / 2;
                _player.inventory.RemoveConsumeItem(2);
                _storeController.uIShopView.ItemSet(2);
                break;
        }

        Debug.Log("�Ǹ� �� :" + _player.statHandler.CurGold + "\n�ֹ��� : " + _player.inventory.auxiliaryWeapon + "\n�������� : " + _player.inventory.weapon + "\n������ : " + _player.inventory.consume);

        _storeController.uIShopView.UpdateButtonState();
        _storeController.uIShopView.UpdateSaleButtonState();

    }
    #endregion
    #endregion

    #region ���� �޼ҵ�

    #region ���� ����
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TryAllSupplement(Player _player, PlayerRef _playerRef)
    {
        Weapon[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };
        Consume[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

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
            itemPrice += (itemInv[0].instance.data.MaxNum - itemInv[0].curNum) * itemInv[0].instance.data.Price;
        }

        if (itemInv[1] != null)
        {
            itemPrice += (itemInv[1].instance.data.MaxNum - itemInv[1].curNum) * itemInv[1].instance.data.Price;
        }

        if (itemInv[2] != null)
        {
            itemPrice += (itemInv[2].instance.data.MaxNum - itemInv[2].curNum) * itemInv[2].instance.data.Price;
        }
        #endregion

        if (_player.statHandler.CurDefGear >= 200)
        {
            totalprice += weaponPrice + itemPrice;
        }
        else
        {
            totalprice += 500 + weaponPrice + itemPrice;
        }

        if (totalprice > _player.statHandler.CurGold) return;

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
                while (itemInv[i].instance.data.MaxNum > itemInv[i].curNum)
                {
                    itemInv[i].AddNum();
                }
                _storeController.uIShopView.ItemSet(i);
            }

        }

        _player.statHandler.CurGold -= totalprice;
        _player.statHandler.CurDefGear += 200;
        _player.statHandler.CurDefGear = Mathf.Min(_player.statHandler.CurDefGear, 200);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #region �� ����
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TryDefSupplement(Player _player)
    {
        if (_player.statHandler.CurDefGear >= 200) return;
        if (_player.statHandler.CurGold < 500) return;


        _player.statHandler.CurGold -= 500;
        _player.statHandler.CurDefGear += 200;
        _player.statHandler.CurDefGear = Mathf.Min(_player.statHandler.CurDefGear, 200);
        _storeController.uIShopView.UpdateButtonState();
    }

    #endregion

    #region ź�� ����
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TryBulletSupplement(Player _player, int index)
    {
        Weapon[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };

        if (weaponInv[index] == null) return;
        if (weaponInv[index].curBullet >= weaponInv[index].instance.data.MaxBullet) return;

        if (weaponInv[index].curBullet + weaponInv[index].instance.data.MagazineBullet >= weaponInv[index].instance.data.MaxBullet)
            _player.statHandler.CurGold -= weaponInv[index].instance.data.BulletPrice * (weaponInv[index].instance.data.MaxBullet - weaponInv[index].curBullet);
        else
            _player.statHandler.CurGold -= weaponInv[index].instance.data.BulletPrice * (weaponInv[index].instance.data.MagazineBullet);

        weaponInv[index].SupplementBullet();
        _storeController.uIShopView.WeaponSet(index);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #region ������ ����
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TryItmeSupplement(Player _player, int index)
    {
        Consume[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

        if (itemInv[index] == null) return;
        if (itemInv[index].curNum >= itemInv[index].instance.data.MaxNum) return;

        _player.statHandler.CurGold -= itemInv[index].instance.data.Price;
        itemInv[index].AddNum();
        _storeController.uIShopView.ItemSet(index);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #endregion










}
