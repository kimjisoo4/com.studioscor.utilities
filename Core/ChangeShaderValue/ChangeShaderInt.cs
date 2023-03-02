namespace StudioScor.Utilities
{
    public class ChangeShaderInt : ChangeShaderValue<int>
    {
        public override void SetValue(int value)
        {
            for (int i = Renderers.Count - 1; i >= 0; i--)
            {
                if (Renderers[i])
                {
                    foreach (var material in Renderers[i].materials)
                    {
                        Renderers[i].material.SetInt(_PropertyID, value);
                    }
                }
            }
        }
    }
}
