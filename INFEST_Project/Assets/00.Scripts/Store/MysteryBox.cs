using System.Collections.Generic;
using Fusion;
using INFEST.Game;
using UnityEngine;

public class MysteryInstance
{
    public readonly MysteryBoxTable data;


    public MysteryInstance(int key)
    {
        data = DataManager.Instance.GetByKey<MysteryBoxTable>(key);
    }
}


public class MysteryBox : NetworkBehaviour
{
    Dictionary<int, MysteryInstance> _instances = new();
    UIBoxopenView _uIBoxopenView;
    List<int> LevelList;
    public TutorialSiren tutorialSiren;

    [SerializeField] private List<int> _idList = new List<int>();
    private Dictionary<int, List<int>> _probabilityList = new Dictionary<int, List<int>>();
    private int[] modeSum = new int[4] { 0, 0, 0, 0 };
    public SphereCollider col;
    public Player player;

    #region ��ȣ�ۿ��
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestInteraction(PlayerRef _playerRef)
    {
        RPC_Interaction(_playerRef);

    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Interaction([RpcTarget] PlayerRef _playerRef)
    {
        init();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        Global.Instance.UIManager.Show<UIBoxopenView>().mysteryBox = this;
    }
    #endregion

    #region ��ȣ�ۿ�������
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestStopInteraction(PlayerRef _playerRef)
    {
        RPC_StopInteraction(_playerRef);

    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_StopInteraction([RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Global.Instance.UIManager.Show<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIBoxopenView>();

    }
    #endregion


    #region OnTriggerEnter ȣ��
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EnterMysteryBoxZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        player = _player;
        player.mysteryBox = this;
        Global.Instance.UIManager.Show<UIInteractiveView>();
    }
    #endregion

    #region OnTriggerExit ȣ��
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_LeaveMysteryBoxZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _player.isInteraction = false;
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIBoxopenView>();

    }
    #endregion

    public void OpenBox(int level = 1)
    {
        Debug.Log("���¹ڽ�");
        int appearedNum = Random.Range(1, modeSum[level] + 1);
        int appearedKey = 0;

        Debug.Log("������ : " + appearedNum);

        // 0~6�� �ݶ�� 100 7-4 =3

        for (int i = 0; i < _idList.Count; i++)
        {
            if (player.inventory.auxiliaryWeapon[0]?.key == _idList[i]) continue;
            if (player.inventory.weapon[0]?.key == _idList[i]) continue;
            if (player.inventory.weapon[1]?.key == _idList[i]) continue;

            if (appearedNum > _probabilityList[_idList[i]][level])
            {
                appearedNum -= _probabilityList[_idList[i]][level];
            }
            else
            {
                appearedKey = _idList[i];
                Debug.Log("���� Ű�� :" + appearedKey);

                SetWeapon(appearedKey, i);
                break;
            }
        }

        Debug.Log("��������");
        RPC_EndMysteryBoxZone();

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_EndMysteryBoxZone()
    {
        col.enabled = false;

#if UNITY_EDITOR
        col.enabled = true;
#endif 
        if (player == null) return;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.inMysteryBoxZoon = false;
        player.isInteraction = false;
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIBoxopenView>();

        tutorialSiren.gameObject.SetActive(true);
    }

    public void SetWeapon(int appearedKey, int index)
    {
        int weaponChk = appearedKey % 10000;
        bool auxiliaryWeapon = weaponChk < 200 && weaponChk > 0;
        bool mainWeapon = weaponChk < 700 && weaponChk > 200;

        if (auxiliaryWeapon)
        {
            if (player.inventory.auxiliaryWeapon[0] == null)
            {
                player.inventory.auxiliaryWeapon[0] = player.Weapons.Weapons[index];
                player.inventory.auxiliaryWeapon[0].IsCollected = true;
                player.Weapons._weapons.Add(player.inventory.auxiliaryWeapon[0]);

            }
            else // ������ ������
            {
                //���� �Ѿ� �ʱ�ȭ
                player.inventory.auxiliaryWeapon[0].FPSWeapon.activeAmmo = player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                player.inventory.auxiliaryWeapon[0].curMagazineBullet = player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                player.inventory.auxiliaryWeapon[0].curBullet = player.inventory.auxiliaryWeapon[0].instance.data.MaxBullet;

                // ���� ������, Ÿ�� ���Ѻ��� ��θ�
                if (player.inventory.auxiliaryWeapon[0].instance.data.Price > player.Weapons.Weapons[index].instance.data.Price)
                {
                    if (player.inventory.weapon[0] != null)
                    {
                        player.inventory.weapon[0].FPSWeapon.activeAmmo = player.inventory.weapon[0].instance.data.MagazineBullet;
                        player.inventory.weapon[0].curMagazineBullet = player.inventory.weapon[0].instance.data.MagazineBullet;
                        player.inventory.weapon[0].curBullet = player.inventory.weapon[0].instance.data.MaxBullet;
                    }
                    if (player.inventory.weapon[1] != null)
                    {
                        player.inventory.weapon[1].FPSWeapon.activeAmmo = player.inventory.weapon[1].instance.data.MagazineBullet;
                        player.inventory.weapon[1].curMagazineBullet = player.inventory.weapon[1].instance.data.MagazineBullet;
                        player.inventory.weapon[1].curBullet = player.inventory.weapon[1].instance.data.MaxBullet;
                    }
                }
                else // ���� ������, Ÿ�� ���Ѻ��� �θ�
                {
                    // �������� ���� ��������
                    if (player.inventory.equippedWeapon == player.inventory.auxiliaryWeapon[0])
                    {
                        player.Weapons.Swap(1, true);
                        player.inventory.auxiliaryWeapon[0] = player.Weapons.Weapons[index];
                        player.Weapons.Weapons[index].IsCollected = true;
                        player.Weapons._weapons.Add(player.Weapons.Weapons[index]);
                    }
                    else
                    {
                        player.inventory.auxiliaryWeapon[0].IsCollected = false;
                        player.Weapons._weapons.Remove(player.inventory.auxiliaryWeapon[0]);
                        player.inventory.auxiliaryWeapon[0] = player.Weapons.Weapons[index];
                        player.Weapons.Weapons[index].IsCollected = true;
                        player.Weapons._weapons.Add(player.Weapons.Weapons[index]);

                    }
                }
            }
        }
        else if (mainWeapon)
        {
            if (player.inventory.weapon[0] == null)
            {
                player.inventory.weapon[0] = player.Weapons.Weapons[index];
                player.inventory.weapon[0].IsCollected = true;
                player.Weapons._weapons.Add(player.inventory.weapon[0]);
            }
            else if (player.inventory.weapon[1] == null)
            {
                player.inventory.weapon[1] = player.Weapons.Weapons[index];
                player.inventory.weapon[1].IsCollected = true;
                player.Weapons._weapons.Add(player.inventory.weapon[1]);
            }
            else // ���Ⱑ �ΰ� �� ������
            {
                // 0�� ���Ⱑ �� ������ ����
                if (player.inventory.weapon[0].instance.data.Price <= player.inventory.weapon[1].instance.data.Price)
                {
                    player.inventory.weapon[0].FPSWeapon.activeAmmo = player.inventory.weapon[0].instance.data.MagazineBullet;
                    player.inventory.weapon[0].curMagazineBullet = player.inventory.weapon[0].instance.data.MagazineBullet;
                    player.inventory.weapon[0].curBullet = player.inventory.weapon[0].instance.data.MaxBullet;
                    if (player.inventory.weapon[0].instance.data.Price > player.Weapons.Weapons[index].instance.data.Price)
                    {
                        if (player.inventory.auxiliaryWeapon[0] != null)
                        {
                            player.inventory.auxiliaryWeapon[0].FPSWeapon.activeAmmo = player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                            player.inventory.auxiliaryWeapon[0].curMagazineBullet = player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                            player.inventory.auxiliaryWeapon[0].curBullet = player.inventory.auxiliaryWeapon[0].instance.data.MaxBullet;
                        }
                        if (player.inventory.weapon[1] != null)
                        {
                            player.inventory.weapon[1].FPSWeapon.activeAmmo = player.inventory.weapon[1].instance.data.MagazineBullet;
                            player.inventory.weapon[1].curMagazineBullet = player.inventory.weapon[1].instance.data.MagazineBullet;
                            player.inventory.weapon[1].curBullet = player.inventory.weapon[1].instance.data.MaxBullet;
                        }
                    }
                    else
                    {
                        if (player.inventory.equippedWeapon == player.inventory.weapon[0])
                        {
                            player.Weapons.Swap(1, true);
                            player.inventory.weapon[0] = player.Weapons.Weapons[index];
                            player.Weapons.Weapons[index].IsCollected = true;
                            player.Weapons._weapons.Add(player.Weapons.Weapons[index]);
                        }
                        else
                        {
                            player.inventory.weapon[0].IsCollected = false;
                            player.Weapons._weapons.Remove(player.inventory.weapon[0]);
                            player.inventory.weapon[0] = player.Weapons.Weapons[index];
                            player.Weapons.Weapons[index].IsCollected = true;
                            player.Weapons._weapons.Add(player.Weapons.Weapons[index]);
                        }
                    }
                }

                // 1�� ���Ⱑ �� ������ ����
                else
                {
                    player.inventory.weapon[1].FPSWeapon.activeAmmo = player.inventory.weapon[1].instance.data.MagazineBullet;
                    player.inventory.weapon[1].curMagazineBullet = player.inventory.weapon[1].instance.data.MagazineBullet;
                    player.inventory.weapon[1].curBullet = player.inventory.weapon[1].instance.data.MaxBullet;
                    if (player.inventory.weapon[1].instance.data.Price > player.Weapons.Weapons[index].instance.data.Price)
                    {
                        if (player.inventory.auxiliaryWeapon[0] != null)
                        {
                            player.inventory.auxiliaryWeapon[0].FPSWeapon.activeAmmo = player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                            player.inventory.auxiliaryWeapon[0].curMagazineBullet = player.inventory.auxiliaryWeapon[0].instance.data.MagazineBullet;
                            player.inventory.auxiliaryWeapon[0].curBullet = player.inventory.auxiliaryWeapon[0].instance.data.MaxBullet;
                        }
                        if (player.inventory.weapon[0] != null)
                        {
                            player.inventory.weapon[0].FPSWeapon.activeAmmo = player.inventory.weapon[0].instance.data.MagazineBullet;
                            player.inventory.weapon[0].curMagazineBullet = player.inventory.weapon[0].instance.data.MagazineBullet;
                            player.inventory.weapon[0].curBullet = player.inventory.weapon[0].instance.data.MaxBullet;
                        }
                    }
                    else
                    {
                        if (player.inventory.equippedWeapon == player.inventory.weapon[1])
                        {
                            player.Weapons.Swap(1, true);
                            player.inventory.weapon[1] = player.Weapons.Weapons[index];
                            player.Weapons.Weapons[index].IsCollected = true;
                            player.Weapons._weapons.Add(player.Weapons.Weapons[index]);
                        }
                        else
                        {
                            player.inventory.weapon[1].IsCollected = false;
                            player.Weapons._weapons.Remove(player.inventory.weapon[1]);
                            player.inventory.weapon[1] = player.Weapons.Weapons[index];
                            player.Weapons.Weapons[index].IsCollected = true;
                            player.Weapons._weapons.Add(player.Weapons.Weapons[index]);
                        }
                    }
                }
            }
        }

    }


public void init()
    {
        _probabilityList.Clear();
        modeSum[0] = 0;
        modeSum[1] = 0;
        modeSum[2] = 0;
        modeSum[3] = 0;


        for (int i = 0; i < _idList.Count; i++)
        {

            if (player.inventory.auxiliaryWeapon[0]?.key == _idList[i]) continue;
            if (player.inventory.weapon[0]?.key == _idList[i]) continue;
            if (player.inventory.weapon[1]?.key == _idList[i]) continue;

            if (!_instances.ContainsKey(_idList[i]))
                _instances[_idList[i]] = new MysteryInstance(_idList[i]);

            var instance = _instances[_idList[i]];
            List<int> LevelList = new List<int>
            {
                instance.data.EasyProbability,
                instance.data.NormalProbability,
                instance.data.HardProbability,
                instance.data.HellProbability
            };
            modeSum[0] += _instances[_idList[i]].data.EasyProbability;
            modeSum[1] += _instances[_idList[i]].data.NormalProbability;
            modeSum[2] += _instances[_idList[i]].data.HardProbability;
            modeSum[3] += _instances[_idList[i]].data.HellProbability;
            _probabilityList.Add(_idList[i], LevelList);
        }
    }
}
