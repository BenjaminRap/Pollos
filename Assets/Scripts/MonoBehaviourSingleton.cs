using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour
	where T : MonoBehaviour {
	private static T _instance = null;

	protected virtual void Awake() {
		if (_instance != null) {
			Destroy(this);
			Debug.LogError($"An instance of {typeof(T).Name} already exists. Only one instance of this singleton is allowed. Destroying the script !");
			return;
		}
		_instance = this as T;
	}

	public static bool TryGetInstance(out T instance, bool logging) {
		instance = _instance;
		if (_instance == null) {
			if (logging)
				Debug.LogError($"The class {typeof(T).Name} has no instance !");
			return false;
		}
		return true;
	}
}
