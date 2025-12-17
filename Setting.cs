using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;
using SimulationTools;

namespace SimulationTools
{
    [FileLocation(nameof(SimulationTools))]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";
        public const string kGroup = "Main";
        public static Setting Instance;

        public Setting(IMod mod) : base(mod)
        {
        }

        [SettingsUISection(kSection, kGroup)]
        [SettingsUIButton]
        [SettingsUIConfirmation]
        public bool DeleteHouseholds
        {
            set => HouseholdDeletionSystem.Instance?.DeleteEveryone();
        }
        
        [SettingsUISection(kSection, kGroup)]
        [SettingsUIButton]
        public bool CountOrphanedTrailers
        {
            set => OrphanedTrailerSystem.Instance.CountOrphanedTrailers();
        }
        
        [SettingsUISection(kSection, kGroup)]
        [SettingsUIButton]
        public bool DeleteOrphanedTrailers
        {
            set => OrphanedTrailerSystem.Instance.DeleteOrphanedTrailers();
        }

        
        public override void SetDefaults()
        {

        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Simulation Tools" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },
                

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeleteHouseholds)), "Delete Households" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.DeleteHouseholds)),
                    $"Delete all households. This will set your population to 0!"
                },
                {
                    m_Setting.GetOptionWarningLocaleID(nameof(Setting.DeleteHouseholds)),
                    "Are you sure you want to delete all households? Your population will be set to 0. This action cannot be undone!"
                },
                
                {m_Setting.GetOptionLabelLocaleID(nameof(Setting.CountOrphanedTrailers)), "Count Orphaned Trailers" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.CountOrphanedTrailers)),
                    $"Count all orphaned trailers in the game and log the count to the console."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeleteOrphanedTrailers)), "Delete Orphaned Trailers" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.DeleteOrphanedTrailers)),
                    $"Delete all orphaned trailers in the game."
                },
            };
        }

        public void Unload()
        {
        }
    }
}