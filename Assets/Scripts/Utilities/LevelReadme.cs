using UnityEngine;

namespace Utilities
{
    public class LevelReadme : MonoBehaviour
    {
        [Header("Level Information")]
        public string levelName;
        
        [TextArea(10, 20)]
        public string description;

        [Header("Controls")]
        [TextArea(5, 10)]
        public string controls;
    }
}
