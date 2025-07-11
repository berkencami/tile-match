using UnityEngine;

namespace TileMatch.Utility 
{
    
    public class Singleton<T> : UnityEngine.MonoBehaviour where T : Component {
        private static object Lock = new object();
        private static T instance;
        
        public static T Instance {
            get {
                lock (Lock) {
                    if (instance == null) {
                        instance = (T)FindObjectOfType(typeof(T));
                        
                        if (instance == null) {
                            var singletonObject = new GameObject();
                            instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        }
                    }

                    return instance;
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init() {
            instance = null;
        }

        public static void SetInstance(T newInstance) => instance = newInstance;

    }
    
}