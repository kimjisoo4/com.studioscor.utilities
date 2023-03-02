namespace StudioScor.Utilities
{
    public class ChangeShaderFloat : ChangeShaderValue<float>
    {
        public override void SetValue(float value)
        {
            for (int i = Renderers.Count - 1; i >= 0; i--)
            {
                if (Renderers[i])
                {
                    foreach (var material in Renderers[i].materials)
                    {
                        Renderers[i].material.SetFloat(_PropertyID, value);
                    }
                }
            }
        }
    }
}
