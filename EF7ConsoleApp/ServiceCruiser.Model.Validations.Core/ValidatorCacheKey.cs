using System;

namespace ServiceCruiser.Model.Validations.Core
{
    internal struct ValidatorCacheKey : IEquatable<ValidatorCacheKey>
    {
        public ValidatorCacheKey(Type sourceType, string ruleset, bool generic) : this()
        {
            SourceType = sourceType;
            Ruleset = ruleset;
            Generic = generic;
        }

        public bool Generic { get; private set; }

        public Type SourceType { get; private set; }
        
        public string Ruleset { get; private set; }

        public override int GetHashCode()
        {
            return SourceType.GetHashCode() ^ (Ruleset != null ? Ruleset.GetHashCode() : 0);
        }

        #region IEquatable<ValidatorCacheKey> Members

        bool IEquatable<ValidatorCacheKey>.Equals(ValidatorCacheKey other)
        {
            return (SourceType == other.SourceType)
                   && (Ruleset == null ? other.Ruleset == null : Ruleset.Equals(other.Ruleset))
                   && (Generic == other.Generic);
        }

        #endregion
    }
}
