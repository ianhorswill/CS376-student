namespace Assets.FakeUnity
{
    public class FakeSpriteRenderer : FakeComponent
    {
        // The real version of SpriteRenderer takes a Sprite object,
        // but we'll keep things simple here and just pretend there's
        // a file name here.
        public string FileName;
    }
}
