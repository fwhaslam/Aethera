// Copyright (c) 2023 Frederick William Haslam born 1962 in the USA.

using UnityEngine;

namespace Global {

abstract public class SingletonMonoBehaviour<T> : MonoBehaviour 
        where T: SingletonMonoBehaviour<T> {

	public static T Instance { get; private set; }

	public void Awake() {
        FixSingleton();
    }

    public void FixSingleton() {
//print("FixSingleton  Instance="+Instance+"   IsThis="+(Instance==this) );
        if (Instance != null && Instance != this) { 
            Destroy(this.gameObject); 
        } 
        else { 
            Instance = (T)this;
            Initialize();
        } 
	}

	abstract protected void Initialize();
}

}