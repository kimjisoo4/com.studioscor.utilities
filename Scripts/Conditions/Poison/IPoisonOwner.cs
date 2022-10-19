using System.Collections.Generic;

namespace KimScor.Utilities
{
    public interface IPoisonOwner
    {
        public void TakePoisonDamage(IPoison poison);
        public IReadOnlyList<IPoison> Poisons { get; }
    }
}