namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";

        // public abstract Vector2 Axis { get; }

        // public abstract bool IsAttackButtonUp();

        // protected static Vector2 SimpleInputAxis() =>
        //     new(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}