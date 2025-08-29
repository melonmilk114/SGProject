using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class BattleStageManager : GameElement
    , ITargetObjectReceiver<TableDataManager>
    {
        public class StageWaveData
        {
            public WaveTableDataItem WaveTableData = null;
            public long monsterSn = 0;
            public float interval = 0f;
        }
        
        public StageTableData stageTableData;
        public WaveTableData waveTableData;

        public StageTableDataItem currentStageTableData = null;
        private Queue<StageWaveData> _currentWaveQueue = new Queue<StageWaveData>();
        
        #region ITargetObjectReceiver<TableDataManager>
        TableDataManager ITargetObjectReceiver<TableDataManager>.GetTargetObject { get; set; }
        public void SetTargetObject(TableDataManager inObject)
        {
            ((ITargetObjectReceiver<TableDataManager>) this).GetTargetObject = inObject;
        }
        
        public TableDataManager GetTableDataManager(System.Action<TableDataManager> inOnNotNull = null)
        {
            var returnManager = ((ITargetObjectReceiver<TableDataManager>) this).GetTargetObject;
            if (inOnNotNull != null)
                inOnNotNull.Invoke(returnManager);

            return returnManager;
        }
        #endregion
     
        public void InitContent()
        {
            stageTableData = GetTableDataManager()?.stageTableData;
            waveTableData = GetTableDataManager()?.waveTableData;
        }

        public void InitStageData(long inStageSn)
        {
            _currentWaveQueue.Clear();
            currentStageTableData = stageTableData.FindStageData(inStageSn);
            
            float waveIntervalSum = 0f;
            if (currentStageTableData != null)
            {
                var waveList = waveTableData.FindWaveDataList(currentStageTableData.wave_group);
                for (int idx_1 = 0; idx_1 < waveList.Count; idx_1++)
                {
                    var tmpWaveData = waveList[idx_1];
                    for (int idx_2 = 0; idx_2 < tmpWaveData.monster_count; idx_2++)
                    {
                        StageWaveData tmpStageWaveData = new StageWaveData();
                        tmpStageWaveData.WaveTableData = tmpWaveData;
                        tmpStageWaveData.interval = waveIntervalSum;
                        tmpStageWaveData.monsterSn = tmpWaveData.monster_sn;
                        
                        _currentWaveQueue.Enqueue(tmpStageWaveData);
                        
                        waveIntervalSum += tmpWaveData.interval;
                    }
                }
            }
        }

        public void GameStart(long inStageSn)
        {
            InitStageData(inStageSn);
        }


        public long FindInstanceMonsterID(float inBattleStartElapsedTime)
        {
            // 웨이브 스택이 비어있지 않다면
            if (_currentWaveQueue.Count == 0)
                return 0;

            //굳이 코루틴을 돌리지 않았음
            StageWaveData tmpWaveData = null;
            if (_currentWaveQueue.TryPeek(out tmpWaveData))
            {
                if (tmpWaveData.interval <= inBattleStartElapsedTime)
                {
                    // 웨이브 시작
                    _currentWaveQueue.Dequeue();
                    // 적 생성
                    return tmpWaveData.monsterSn;
                }
            }
            
            return 0;
        }

        public bool IsNextWaveAvailable()
        {
            return _currentWaveQueue.Count > 0;
        }
    }
}