using IPA.Config.Stores;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace StreamPartyCommand.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual bool IsBombEnable { get; set; } = true;
        public virtual float TextViewSec { get; set; } = 3.5f;
        public virtual float MissTextViewSec { get; set; } = 2.7f;
        public virtual bool IsSaberColorEnable { get; set; } = true;
        public virtual bool IsWallColorEnable { get; set; } = true;
        public virtual bool IsNoteColorEnable { get; set; } = true;
        public virtual bool IsPratformColorEnable { get; set; } = true;
        public virtual int NameObjectLayer { get; set; } = 0;
        public virtual bool ReloadeIfMissCut { get; set; } = true;

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}