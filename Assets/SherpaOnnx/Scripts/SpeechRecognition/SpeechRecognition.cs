using System;
using UnityEngine;

namespace SherpaOnnxUnity
{
    public class SpeechRecognition : MonoBehaviour
    {
        /// <summary>
        /// 在线识别的实时返回
        /// </summary>
        public Action<string> onResult;
        /// <summary>
        /// 在线识别的说话结束返回
        /// </summary>
        public Action<string> onResultEnd;

        public virtual void RecognizeOffline(float[] input, Action<string> onResult)
        {

        }

        public virtual void RecognizeOnline(int sampleRate, float[] input)
        {

        }
    }
}