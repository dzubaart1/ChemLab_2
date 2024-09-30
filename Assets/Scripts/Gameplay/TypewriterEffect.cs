using System.Collections;
using TMPro;
using UnityEngine;

namespace BioEngineerLab.Gameplay
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypewriterEffect : MonoBehaviour
    {
        [SerializeField] private float delayBeforeStart = 1f;
        [SerializeField] private float timeBtwChars = 0.1f;
        [SerializeField] private string leadingChar = "|";
        [SerializeField] private bool leadingCharBeforeDelay;

        private TMP_Text _textObject;

        private string _startText;

        private void Awake()
        {
            _textObject = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            StartTyping();
        }

        private void StartTyping()
        {
            _startText = _textObject.text;
            _textObject.text = leadingCharBeforeDelay ? leadingChar : "";

            StartCoroutine(nameof(TMPTypeWriter));
        }

        public void SkipTyping()
        {
            StopCoroutine(nameof(TMPTypeWriter));
            _textObject.text = _startText;
        }

        private IEnumerator TMPTypeWriter()
        {
            yield return new WaitForSeconds(delayBeforeStart);

            foreach (var c in _startText)
            {
                if (_textObject.text.Length > 0)
                {
                    _textObject.text = _textObject.text[..^leadingChar.Length];
                }

                _textObject.text += c;
                _textObject.text += leadingChar;

                yield return new WaitForSeconds(timeBtwChars);
            }

            if (leadingChar != "")
            {
                _textObject.text = _textObject.text[..^leadingChar.Length];
            }
        }
    }
}