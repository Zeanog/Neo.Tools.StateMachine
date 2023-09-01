using UnityEngine;

public interface IProjectileLauncher {
    bool Launch(float spread, Transform startTransform);
}