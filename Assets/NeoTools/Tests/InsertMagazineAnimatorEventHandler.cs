public class InsertMagazineAnimatorEventHandler : AnimatorEventHandler
{
    protected override void Awake()
    {
        base.Awake();

        AddHandler("Reload.Exit", "OnReloadComplete");
    }
}