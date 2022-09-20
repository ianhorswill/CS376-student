namespace FakeUnity
{
    /// <summary>
    /// A placeholder to stand in for Unity's SpriteRenderer component.
    /// SpriteRenderers draws a sprite on the screen in the location of the GameObject that
    /// contains it.
    /// </summary>
    public class SpriteRenderer : Component
    {
        // The real version of SpriteRenderer takes a Sprite object,
        // but we'll keep things simple here and just pretend there's
        // a file name here.
        public string FileName;
    }
}
