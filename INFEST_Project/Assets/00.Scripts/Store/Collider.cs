using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Collider : MonoBehaviour // ������ ������ �����߰� ��ȣ�ۿ��ϴ��� üũ���ش�.
{
    public List<Player> playersInShop = new();
    //public readonly HashSet<Player> _inside = new ();
    public Store _store;
    public StoreController _storeController;
    public Player[] usePlaeyr;
    private int _playerLayer = 6;
    bool _active = false;


    private void OnTriggerEnter(UnityEngine.Collider other) 
    {
        if (other.gameObject.layer == _playerLayer && other.TryGetComponent(out Player _player))
        {
            //if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            //var _player = other.GetComponent<NetworkObject>();
            //if (_player == null) return;

            if (_store == null) return;
            _player.store = _store;

            //if(_player.networkObject == null)
            //    _player.networkObject = _player.GetComponent<NetworkObject>();
            //if (!_inside.Contains(_player))
            //{
            playersInShop.Add(_player);
            Debug.Log("���� " + other.name);
            //_inside.Add(_player);
            _store.RPC_RequestEnterShopZone(_player, _player.networkObject.InputAuthority);
            _player.inStoreZoon = true;

            //}
            //else if (_inside.Contains(_player) && _store.isInteraction)
            //{
            //    Debug.Log("��ȣ�ۿ�");
            //    _store.RPC_RequestInteraction(_player, _player.networkObject.InputAuthority); 
            //}
            //else if (_store.isInteraction && Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Debug.Log("��ȣ�ۿ� ����");
            //    _inside.Remove(_player);
            //    _store.RPC_RequestLeaveShopZone(_player, _player.networkObject.InputAuthority);
            //}

            //if (_storeController.storeTimer.ExpiredOrNotRunning(_storeController.Runner))
            //{
            //    _inside.Remove(_player);
            //    Exit(_player, _store);
            //}

            //StopAllCoroutines();
            //StartCoroutine(ICorutine(_player, _store));
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.TryGetComponent(out Player _player))
        {
            //if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            //var _player = other.GetComponent<NetworkObject>();
            //if (_player == null) return;
            if (_store == null) return;
            _player.store = null;

            playersInShop.Remove(_player);
            //_inside.Remove(_player);
            _store.RPC_RequestLeaveShopZone(_player, _player.networkObject.InputAuthority);

            //Exit(_player, _store);
            _player.inStoreZoon = false;
        }
    }

    //IEnumerator ICorutine(Player _player, Store _store)
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    Exit(_player, _store);
    //}

    //public void Exit(Player _player, Store _store)
    //{
    //    Debug.Log("���� ����");
    //    //_inside.Remove(_player);
    //    _store.RPC_RequestLeaveShopZone(_player, _player.networkObject.InputAuthority);
    //}
}
