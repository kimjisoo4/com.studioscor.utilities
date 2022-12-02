namespace StudioScor.StatSystem
{

    [System.Serializable]
	public class StatModifier
	{
		public float Value;
		public EStatModType Type;
		public int Order;
		public object Source;

		public StatModifier(StatModifier statModifier)
        {
			Value = statModifier.Value;
			Type = statModifier.Type;
			Order = statModifier.Order;
			Source = statModifier.Source;
		}

		public StatModifier(float value, EStatModType type, int order, object source)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
		}

		public StatModifier(float value, EStatModType type) : this(value, type, (int)type, null) { }

		public StatModifier(float value, EStatModType type, int order) : this(value, type, order, null) { }

		public StatModifier(float value, EStatModType type, object source) : this(value, type, (int)type, source) { }
	}
}
