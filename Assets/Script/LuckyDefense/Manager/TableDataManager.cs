using Melon;

namespace LuckyDefense
{
    public class TableDataManager : Melon.TableDataManager
    {
        public TowerTableData towerTableData = new TowerTableData();
        public TowerLevelTableData towerLevelTableData = new TowerLevelTableData();
        public MissileTableData missileTableData = new MissileTableData();
        public MonsterTableData monsterTableData = new MonsterTableData();
        public TowerSpotData towerSpotData = new TowerSpotData();
        public StageTableData stageTableData = new StageTableData();
        public WaveTableData waveTableData = new WaveTableData();

        public override void InitManager()
        {
            base.InitManager();

            TargetObjectReceiver.DoTargetObjectInject(this, rootObj);
        }
        
        public override void InitData()
        {
            base.InitData();
            
            towerTableData.InitData();
            towerLevelTableData.InitData();
            missileTableData.InitData();
            monsterTableData.InitData();
            towerSpotData.InitData();
            stageTableData.InitData();
            waveTableData.InitData();
        }

        public override void ResetData()
        {
            base.ResetData();
            
            towerTableData.ResetData();
            towerLevelTableData.ResetData();
            missileTableData.ResetData();
            monsterTableData.ResetData();
            towerSpotData.ResetData();
            stageTableData.ResetData();
            waveTableData.ResetData();
        }
    }
}