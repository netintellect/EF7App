﻿using System;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	internal static class ValidatorArgumentsValidatorHelper
	{
		internal static void ValidateContainsCharacterValidator(string characterSet)
		{
			if (characterSet == null)
			{
				throw new ArgumentNullException("characterSet");
			}
		}

		internal static void ValidateDomainValidator(object domain)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
		}

		internal static void ValidateEnumConversionValidator(Type enumType)
		{
			if (null == enumType)
			{
				throw new ArgumentNullException("enumType");
			}
		}

		internal static void ValidateRangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundaryType, IComparable upperBound, RangeBoundaryType upperBoundaryType)
		{
			if (lowerBoundaryType != RangeBoundaryType.Ignore && null == lowerBound)
			{
				throw new ArgumentNullException("lowerBound");
			}
			if (upperBoundaryType != RangeBoundaryType.Ignore && null == upperBound)
			{
				throw new ArgumentNullException("upperBound");
			}
			if (lowerBoundaryType == RangeBoundaryType.Ignore && upperBoundaryType == RangeBoundaryType.Ignore)
			{
				throw new ArgumentException(Translations.ExceptionCannotIgnoreBothBoundariesInRange, "lowerBound");
			}

			if (lowerBound != null && upperBound != null && lowerBound.GetType() != upperBound.GetType())
			{
				throw new ArgumentException(Translations.ExceptionTypeOfBoundsMustMatch, "upperBound");
			}
		}

		internal static void ValidateRegexValidator(string pattern, string patternResourceName, Type patternResourceType)
		{
			if (null == pattern && (patternResourceName == null || patternResourceType == null))
			{
				throw new ArgumentNullException("pattern");
			}
			if (pattern == null && patternResourceName == null)
			{
				throw new ArgumentNullException("patternResourceName");
			}
			if (pattern == null && patternResourceType == null)
			{
				throw new ArgumentNullException("patternResourceType");
			}
		}

		internal static void ValidateRelativeDatimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
			int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
		{
			if ((lowerBound != 0 && lowerUnit == DateTimeUnit.None && lowerBoundType != RangeBoundaryType.Ignore) ||
				(upperBound != 0 && upperUnit == DateTimeUnit.None && upperBoundType != RangeBoundaryType.Ignore))
			{
				throw new ArgumentException(Translations.RelativeDateTimeValidatorNotValidDateTimeUnit);
			}
		}

		internal static void ValidateTypeConversionValidator(Type targetType)
		{
			if (null == targetType)
			{
				throw new ArgumentNullException("targetType");
			}
		}
	}
}