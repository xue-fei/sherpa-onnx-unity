using UnityEngine;

namespace SherpaOnnxUnity
{
    /// <summary>
    /// 语音合成
    /// </summary>
    public class SpeechSynthesis : MonoBehaviour
    {
        /// <summary>
        /// 生成语音
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="speed">速度</param>
        /// <param name="speakerId">说话人id</param>
        public virtual void Generate(string text, float speed, int speakerId)
        {

        }
    }
}