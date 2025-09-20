// Copyright (c) 2023 Frederick William Haslam born 1962 in the USA.

using UnityEngine;

namespace Global {

    /// <summary>
    /// Lesson on SingletonPersistent and LoadScene():
    /// 
    /// If you load an object multiple times ( eg. load the scene containing it ),
    /// then the object will STILL call OnDisable() and OnDestroy().
    /// 
    /// For true SingletonPersistence, be sure to isolate into a scene that is 
    /// only loaded ONCE.
    /// 
    /// I the case of Starland, we check to see if PlayStateGlobal.Instance is null 
    /// before deciding to load our SharedGlobalsScene which contains all the singletons.
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract public class SingletonPersistentMonoBehaviour<T> : MonoBehaviour 
            where T: SingletonPersistentMonoBehaviour<T> {

	    public static T Instance { get; private set; }

	    public void Awake() {
            FixSingleton();
        }

        public void FixSingleton() {
    //Debug.Log("FixSingleton ("+this.GetType()+")");
            if (Instance != null && Instance != this) { 
    Debug.Log("FixSingleton.Destroy! ("+this.GetType()+")");
                Destroy(this.gameObject); 
            } 
            else { 
    Debug.Log("FixSingleton.create! ("+this.GetType()+")");
                DontDestroyOnLoad( this.gameObject );
                Instance = (T)this;
                Initialize();
            } 
	    }

	    abstract protected void Initialize();
    }

}