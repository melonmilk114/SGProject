using System;
using System.Collections;
using System.Collections.Generic;
using LuckyDefense;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// 적유닛과의 충돌처리는 미사일이 하기
public class MissileObject : GameElement, IObjectPoolUnit
{
    public MissileView view;
    public MissileTableDataItem tableData = null;
    
    private MissileMoveHandler _moveHandler;
    private MissileDestroyHandler _destroyHandler;
    
    public MonsterObject monsterTarget;

    public bool isDestroyReady = false;

    private long missileDamage = 0;

    public Action<MissileObject, MonsterObject> onHitMonster = null;

    public void SetDamage(long inDamage)
    {
        missileDamage = inDamage;
    }
    public long GetDamage()
    {
        return missileDamage;
    }

    

    public void OnPoolDequeue()
    {
        missileDamage = 0;
    }

    public void OnPoolEnqueue()
    {
            
    }

    public void Init(MissileTableDataItem inData, MonsterObject inMonster)
    {
        tableData = inData;
  
        _destroyHandler = CommonUtils.AddComponent<MissileDestroyHandler>(gameObject, tableData.destroyHandlerType);
        _moveHandler = CommonUtils.AddComponent<MissileMoveHandler>(gameObject, tableData.moveHandlerType);
            
        if (_destroyHandler == null || _moveHandler == null)
        {
            Debug.LogError($"[MissileObject] 핸들러가 누락되었습니다. monsterSn={tableData.sn}");
            return;
        }

        var compList = GetComponents<MissileHandler>();
        for (var idx = 0; idx < compList.Length; idx++)
        {
            compList[idx].Setup(this);
        }
        
        monsterTarget = inMonster;
        isDestroyReady = false;
        
        view.Init(tableData);
    }
    
    public void UpdateObject(float inDeltaTime)
    {
        if (isDestroyReady)
            return;
        
        _moveHandler?.Move(inDeltaTime);

        ResolveMonsterHit();
    }

    public void ResolveMonsterHit()
    {
        float distanceToTarget = Vector3.Distance(transform.position, monsterTarget.transform.position);

        if (distanceToTarget <= 0.1f)
        {
            onHitMonster?.Invoke(this, monsterTarget);
        }
    }

    public void MissileDestroy(ActionResult inResult)
    {
        _destroyHandler.DestroyMissile(inResult);
    }

    
}
