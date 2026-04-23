using UnityEngine;

namespace MonsterModifiers.Modifiers;

// 시너지: FireInfused + FastAttackSpeed → 이동 시 화염 흔적 파티클
public class FireTrail : MonoBehaviour
{
    private Character _character;
    private Vector3 _lastPosition;
    private float _spawnInterval = 0.4f;
    private float _minMoveDistance = 0.5f;
    private float _timer;

    private GameObject _fireVfxPrefab;

    public void Initialize(Character character)
    {
        _character = character;
        _lastPosition = character.transform.position;

        // 게임 내 불꽃 파티클 프리팹 사용
        _fireVfxPrefab = ZNetScene.instance?.GetPrefab("vfx_fire_emitter");
        if (_fireVfxPrefab == null)
            _fireVfxPrefab = ZNetScene.instance?.GetPrefab("fx_fireskeleton_nova");
    }

    private void Update()
    {
        if (_character == null || _fireVfxPrefab == null) return;

        _timer += Time.deltaTime;
        if (_timer < _spawnInterval) return;
        _timer = 0f;

        float moved = Vector3.Distance(_character.transform.position, _lastPosition);
        if (moved < _minMoveDistance) return;

        _lastPosition = _character.transform.position;
        Vector3 spawnPos = _character.transform.position + Vector3.down * 0.3f;
        GameObject trail = Instantiate(_fireVfxPrefab, spawnPos, Quaternion.identity);

        // 1.5초 후 자동 제거
        Destroy(trail, 1.5f);
    }
}
