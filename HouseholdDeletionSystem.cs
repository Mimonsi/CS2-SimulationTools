using Colossal.Logging;
using Game;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Purpose = Colossal.Serialization.Entities.Purpose;

namespace SimulationTools
{
    public partial class HouseholdDeletionSystem : GameSystemBase
    {
        private static ILog log;
        private EntityQuery _citizenPrefabQuery;

        private PrefabSystem _prefabSystem;
        public static HouseholdDeletionSystem Instance { get; private set; }
        
        /// <summary>
        /// Creates entity queries and prepares the system.
        /// </summary>
        protected override void OnCreate()
        {
            base.OnCreate();
            Instance = this;
            log = Mod.log;
            Enabled = true;
            
            _citizenPrefabQuery = SystemAPI
                .QueryBuilder()
                .WithAny<Household>()
                .Build();
            
            _prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            log.Info("HouseholdDeletionSystem created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
        }

        public void DeleteEveryone()
        {
            var prefabs = _citizenPrefabQuery.ToEntityArray(Allocator.Temp);
            int prefabsUpdated = 0;
            foreach (var entity in prefabs)
            {
                EntityManager.AddComponent<Deleted>(entity);
                prefabsUpdated++;
            }
            log.Info("Deleted " + prefabsUpdated + " households.");
        }

        protected override void OnUpdate()
        {
            
        }
    }
}