using StreamPartyCommand.Configuration;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace StreamPartyCommand.Models
{
    public class FlyingBombNameEffect : FlyingObjectEffect
    {
        public void Initialize()
        {
            if (!this.gameObject.activeSelf) {
                this.gameObject.SetActive(true);
            }
            _ = this.StartCoroutine(this.SetParams());
        }

        public IEnumerator SetParams()
        {
            this._text = this.gameObject.GetComponent<TextMeshPro>();
            if (this._text == null) {
                this._text = this.gameObject.AddComponent<TextMeshPro>();
            }
            yield return new WaitWhile(() => !this._text);
            if (this._fontAssetReader.MainFont != null) {
                this._text.font = this._fontAssetReader.MainFont;
            }
            this._text.alignment = TextAlignmentOptions.Center;
            this._text.fontSize = 30;
            this.gameObject.layer = PluginConfig.Instance.NameObjectLayer;
            this.gameObject.SetActive(false);
        }

        public virtual void InitAndPresent(string text, float duration, Vector3 targetPos, Quaternion rotation, Color color, float fontSize, bool shake)
        {
            this._color = color;
            this._text.text = text;
            this._text.fontSize = fontSize;

            // Move the name effect further
            targetPos += new Vector3(0, 0, 3);

            base.InitAndPresent(duration, targetPos, rotation, shake);
        }

        public override void ManualUpdate(float t)
        {
            this._text.color = this._color.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t));
        }

        private TextMeshPro _text;
        private Color _color;
        private readonly AnimationCurve _fadeAnimationCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);
        private FontAssetReader _fontAssetReader;
        [Inject]
        public void Constractor(FontAssetReader fontAssetReader)
        {
            this._fontAssetReader = fontAssetReader;
        }

        public class Pool : MonoMemoryPool<FlyingBombNameEffect>
        {
            protected override void OnCreated(FlyingBombNameEffect item)
            {
                base.OnCreated(item);
                item._text = item.gameObject.GetComponent<TextMeshPro>();
                if (item._text == null) {
                    item._text = item.gameObject.AddComponent<TextMeshPro>();
                }
                item.Initialize();
            }

            //private FontAssetReader _fontAssetReader;
            //[Inject]
            //public void Constractor(FontAssetReader fontAssetReader)
            //{
            //    this._fontAssetReader = fontAssetReader;
            //}
            //protected override void OnCreated(FlyingBombNameEffect item)
            //{
            //    base.OnCreated(item);
            //    item._fontAssetReader = this._fontAssetReader;
            //}
        }
    }
}
