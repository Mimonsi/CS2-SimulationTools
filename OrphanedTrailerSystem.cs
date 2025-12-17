using Colossal.Entities;
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
    /// <summary>
    /// After removing obsolete vehicle, some trailers are left without owners. These cause traffic jams until manually removed.
    /// </summary>
    public partial class OrphanedTrailerSystem : GameSystemBase
    {
        private static ILog log;
        private EntityQuery _trailerEntityQuery;

        private PrefabSystem _prefabSystem;
        public static OrphanedTrailerSystem Instance { get; private set; }
        
        /// <summary>
        /// Creates entity queries and prepares the system.
        /// </summary>
        protected override void OnCreate()
        {
            base.OnCreate();
            Instance = this;
            log = Mod.log;
            Enabled = true;
            
            _trailerEntityQuery = SystemAPI
                .QueryBuilder()
                .WithAll<CarTrailerLane, Controller>()
                .Build();
            
            _prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            log.Info("OrphanedTrailerSystem created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
        }
        
        protected override void OnUpdate()
        {
            
        }

        public void CountOrphanedTrailers()
        {
            var entities = _trailerEntityQuery.ToEntityArray(Allocator.Temp);
            int deletedEntities = 0;
            foreach (var entity in entities)
            {
                if (EntityManager.TryGetComponent(entity, out Controller controller))
                {
                    if (controller.m_Controller == Entity.Null)
                    {
                        deletedEntities++;
                    }
                }
            }
            log.Info("Counted " + deletedEntities + " orphaned trailer entities.");
        }

        public void DeleteOrphanedTrailers()
        {
            var entities = _trailerEntityQuery.ToEntityArray(Allocator.Temp);
            int deletedEntities = 0;
            foreach (var entity in entities)
            {
                if (EntityManager.TryGetComponent(entity, out Controller controller))
                {
                    if (controller.m_Controller == Entity.Null)
                    {
                        deletedEntities++;
                        EntityManager.AddComponent<Deleted>(entity);
                    }
                }
            }
            log.Info("Deleted " + deletedEntities + " orphaned trailer entities.");
        }
    }
}