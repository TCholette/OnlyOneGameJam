using System.Collections;
using UnityEngine;

public class Pantheon : MonoBehaviour {
    private const float PANTHEON_BUFFER_S = 0.5f;
    [SerializeField] private GameObject _pantheonPoint;
    [SerializeField] private GameObject _pantheonSpawner;
    private bool _isInPantheon = false;
    public bool IsPopulated { get { return _isInPantheon; } set { _isInPantheon = value; } }
    private Player _ctx;
    public Player Context { set { _ctx = value; } }
    public IEnumerator SendToPantheon(EnemyType enemyType) {
        _ctx.Movement.CanMove = false;
        _pantheonSpawner.GetComponent<EnemySpawnPoint>().Init(enemyType);
        yield return new WaitForSeconds(PANTHEON_BUFFER_S);
        _ctx.ChangeCharges(1);
        _isInPantheon = true;
        ProxyFmodPlayer.SetParam<string>(StaticManager.gameMusic, new("Music", "Pantheon"));
        _ctx.gameObject.transform.position = _pantheonPoint.transform.position - new Vector3(2, 0);
        _ctx.Movement.CanMove = true;
        _ctx.Movement.TempCheckpoint = _pantheonPoint;
    }

}
