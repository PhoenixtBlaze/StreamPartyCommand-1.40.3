using AssetBundleLoadingTools.Utilities;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Util;
using SiraUtil.Zenject;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace StreamPartyCommand.Models
{
    public class FontAssetReader : MonoBehaviour, IAsyncInitializable
    {
        private static readonly string FontAssetPath = Path.Combine(Environment.CurrentDirectory, "UserData", "SPCFontAssets");
        private static readonly string MainFontPath = Path.Combine(FontAssetPath, "Main");

        // Instance-cached shader, resolved only after the main menu is ready
        private Shader _tmpNoGlowFontShader;
        private TMP_FontAsset _mainFont;
        public bool IsInitialized { get; private set; }

        public TMP_FontAsset MainFont => _mainFont;

        // Ensure BSML/BeatSaberUI is ready before accessing BeatSaberUI.
        private static Task _menuReadyTask;
        private static Task EnsureBsmlMenuReadyAsync()
        {
            if (_menuReadyTask == null) {
                _menuReadyTask = MainMenuAwaiter.WaitForMainMenuAsync();
            }
            return _menuReadyTask;
        }

        private async Task<Shader> GetTmpNoGlowFontShaderAsync()
        {
            if (_tmpNoGlowFontShader == null) {
                await EnsureBsmlMenuReadyAsync().ConfigureAwait(false);
                _tmpNoGlowFontShader = BeatSaberUI.MainTextFont?.material?.shader;
            }
            return _tmpNoGlowFontShader;
        }

        public async Task CreateChatFont()
        {
            IsInitialized = false;

            // Wait for BSML/BeatSaberUI to be usable, then resolve the shader once
            var shader = await GetTmpNoGlowFontShaderAsync().ConfigureAwait(false);

            if (_mainFont != null) {
                Destroy(_mainFont);
                _mainFont = null;
            }

            if (!Directory.Exists(MainFontPath))
                Directory.CreateDirectory(MainFontPath);

            AssetBundle bundle = null;
            foreach (var filename in Directory.EnumerateFiles(MainFontPath, "*.assets", SearchOption.TopDirectoryOnly)) {
                bundle = await AssetBundleExtensions.LoadFromFileAsync(filename).ConfigureAwait(false);
                if (bundle != null)
                    break;
            }

            if (bundle != null) {
                foreach (var bundleItem in bundle.GetAllAssetNames()) {
                    var asset = await AssetBundleExtensions.LoadAssetAsync<TMP_FontAsset>(bundle, bundleItem).ConfigureAwait(false);
                    if (asset != null) {
                        _mainFont = asset;

                        // Apply the resolved shader after BSML is ready
                        if (_mainFont.material != null && shader != null && _mainFont.material.shader != shader)
                            _mainFont.material.shader = shader;

                        bundle.Unload(false);
                        break;
                    }
                }
            }

            IsInitialized = true;
        }

        public async Task InitializeAsync(CancellationToken token)
        {
            // SiraKernel runs async-initializables during startup; delay BSML access until menu is ready
            await EnsureBsmlMenuReadyAsync().ConfigureAwait(false);
            await CreateChatFont().ConfigureAwait(false);
        }
    }
}
