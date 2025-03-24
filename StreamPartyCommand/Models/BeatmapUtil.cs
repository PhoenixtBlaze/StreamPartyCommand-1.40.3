using IPA.Loader;
using System.Linq;
using Zenject;

namespace StreamPartyCommand.Models
{
    public class BeatmapUtil : IInitializable
    {
        public BeatmapLevel CurrentBeatmap { get; private set; }
        public BeatmapKey CurrentmapKey { get; private set; }
        public bool IsNoodle { get; private set; }
        public bool IsChroma { get; private set; }

        [Inject]
        public BeatmapUtil(GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
        {
            this.CurrentBeatmap = gameplayCoreSceneSetupData.beatmapLevel;
            this.CurrentmapKey = gameplayCoreSceneSetupData.beatmapKey;
        }

        public static bool IsNoodleMap(BeatmapKey key)
        {
            if (PluginManager.EnabledPlugins.Any(x => x.Name == "NoodleExtensions")) {
                var difficultyData = SongCore.Collections.GetCustomLevelSongDifficultyData(key);
                return difficultyData?.additionalDifficultyData?._requirements?.Any(x => x == "Noodle Extensions") == true;
            }
            return false;
        }
        public static bool IsChromaMap(BeatmapKey key)
        {
            if (PluginManager.EnabledPlugins.Any(x => x.Name == "Chroma")) {
                var difficultyData = SongCore.Collections.GetCustomLevelSongDifficultyData(key);
                bool isChroma = difficultyData?.additionalDifficultyData?._requirements?.Any(x => x == "Chroma") == true;
                isChroma |= difficultyData?.additionalDifficultyData?._suggestions?.Any(x => x == "Chroma") == true;
                return isChroma;
            }
            return false;
        }

        public void Initialize()
        {
            this.IsNoodle = IsNoodleMap(this.CurrentmapKey);
            this.IsChroma = IsChromaMap(this.CurrentmapKey);
            Plugin.Log.Debug($"Noodle?:{this.IsNoodle}");
            Plugin.Log.Debug($"Chroma?:{this.IsChroma}");
        }
    }
}
