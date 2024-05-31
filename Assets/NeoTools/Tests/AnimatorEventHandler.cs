using UnityEngine;

public class AnimatorEventHandler : AAnimatorEventHander
{
    //[SerializeField]
    protected ProjectileWeapon weapon;

    protected void AddHandler(string animEvtName, string stateEvtName)
    {
        AddHandler(animEvtName, (Animator animator, int layerIndex) =>
        {
            weapon.TriggerStateEvent(stateEvtName, layerIndex);
        });
    }

    protected override void Awake()
    {
        base.Awake();

        weapon = GetComponent<ProjectileWeapon>();

        AddHandler("Shoot.Exit", "OnUseComplete");
        AddHandler("Hide.Exit", "OnHideComplete");
        AddHandler("Raise.Exit", "OnRaiseComplete");
    }
}