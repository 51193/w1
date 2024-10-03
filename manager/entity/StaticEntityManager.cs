using Godot;
using MyGame.Manager;
using MyGame.Entity;
using System.Threading.Tasks;

namespace MyGame.Manager
{
	public partial class StaticEntityManager : BaseEntityManager<BaseStaticEntity>
    {
        [Signal]
        public delegate void InitiateEntitiesOnMapEventHandler(string mapName);
        [Signal]
        public delegate void ExchangeEntityOnMapEventHandler(string currentMapName, string nextMapName);
        [Signal]
        public delegate void EntityTransitionCompleteEventHandler();

        public StaticEntityManager()
        {
            Init();
            _name = "StaticEntityManager";
        }

        private void Init()
        {
            _globalEntityPosition["Map00"] = new()
            {
                ["StaticEntity00"] = new()
            {
                new Vector2(50, -50),
                new Vector2(30, -30)
            }
            };
        }

        private void InitiateEntities(string mapName)
        {
            SpawnAllWaitingEntitiesFromMapRecord(mapName);
            AddAllLivingEntitiesToRenderingOrderGroup(mapName);
            SetAllLivingEntitiesPhysicsProcess(false);
            EmitSignal(SignalName.EntityTransitionComplete);
        }


        private void OnMapChanged(string currentMapName, string nextMapName)
        {
            if (currentMapName == null)
            {
                GD.PrintErr("Invalid current map, can't change map before initiate");
                return;
            }

            ClearAllLivinEntitiesRenderingOrderGroupName(currentMapName);

            ClearAllEntitiesFromMapRecord(currentMapName);
            RecordAllLivingEntitiesToMapRecord(currentMapName);
            ClearAllLivingEntities();
            SpawnAllWaitingEntitiesFromMapRecord(nextMapName);

            AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);

            SetAllLivingEntitiesPhysicsProcess(false);

            EmitSignal(SignalName.EntityTransitionComplete);
            GD.Print($"Static entities have swapped to {nextMapName}");
        }

        public async void AfterTransitionComplete()
        {
            await Task.Delay(1);
            SetAllLivingEntitiesPhysicsProcess(true);
            GD.Print($"Static entities transition complete");
        }

        public override void _EnterTree()
        {
            base._EnterTree();
            InitiateEntitiesOnMap += InitiateEntities;
            ExchangeEntityOnMap += OnMapChanged;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            InitiateEntitiesOnMap -= InitiateEntities;
            ExchangeEntityOnMap -= OnMapChanged;
        }
    }
}
