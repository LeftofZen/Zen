﻿using System.Runtime.CompilerServices;

namespace Zenith.Core
{
	// Until this proposal is merged, we have to manually add them
	// https://github.com/dotnet/runtime/issues/20604
	public static class Verify
	{
		public static void AreEqual<T>(T left, T right, [CallerArgumentExpression(nameof(left))] string? leftName = null, [CallerArgumentExpression(nameof(right))] string? rightName = null, string? message = null)
			where T : IEquatable<T>
		{
			if (!left.Equals(right))
			{
				throw new ArgumentOutOfRangeException(leftName, $"{left} ({leftName}) was not equal to {right} ({rightName}). Message=\"{message}\"");
			}
		}

		public static void AreNotEqual<T>(T left, T right, [CallerArgumentExpression(nameof(left))] string? leftName = null, [CallerArgumentExpression(nameof(right))] string? rightName = null, string? message = null)
			where T : IEquatable<T>
		{
			if (left.Equals(right))
			{
				throw new ArgumentOutOfRangeException(leftName, $"{left} ({leftName}) was equal to {right} ({rightName}). Message=\"{message}\"");
			}
		}

		public static void LessThan<T>(T left, T right, [CallerArgumentExpression(nameof(left))] string? leftName = null, [CallerArgumentExpression(nameof(right))] string? rightName = null, string? message = null)
			where T : IComparable<T>
		{
			if (left.CompareTo(right) >= 0)
			{
				throw new ArgumentOutOfRangeException(leftName, $"{left} ({leftName}) was not less than {right} ({rightName}) .Message=\"{message}\"");
			}
		}

		public static void LessThanOrEqualTo<T>(T left, T right, [CallerArgumentExpression(nameof(left))] string? leftName = null, [CallerArgumentExpression(nameof(right))] string? rightName = null, string? message = null)
			where T : IComparable<T>
		{
			if (left.CompareTo(right) > 0)
			{
				throw new ArgumentOutOfRangeException(leftName, $"{left} ({leftName}) was not less than or equal to {right} ({rightName}). Message=\"{message}\"");
			}
		}

		public static void GreaterThan<T>(T left, T right, [CallerArgumentExpression(nameof(left))] string? leftName = null, [CallerArgumentExpression(nameof(right))] string? rightName = null, string? message = null)
			where T : IComparable<T>
		{
			if (left.CompareTo(right) <= 0)
			{
				throw new ArgumentOutOfRangeException(leftName, $"{left} ({leftName}) was not greater than {right} ({rightName}). Message=\"{message}\"");
			}
		}

		public static void GreaterThanOrEqualTo<T>(T left, T right, [CallerArgumentExpression(nameof(left))] string? leftName = null, [CallerArgumentExpression(nameof(right))] string? rightName = null, string? message = null)
			where T : IComparable<T>
		{
			if (left.CompareTo(right) < 0)
			{
				throw new ArgumentOutOfRangeException(leftName, $"{left} ({leftName}) was not greater than or equal to {right} ({rightName}). Message=\"{message}\"");
			}
		}

		public static void InRangeInclusive<T>(T value, T min, T max, [CallerArgumentExpression(nameof(value))] string? valueName = null, string? message = null)
			where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
			{
				throw new ArgumentOutOfRangeException(valueName, $"{value} ({valueName}) was not within the range [{min}, {max}]. Message=\"{message}\"");
			}
		}

		public static void NotNull<T>(T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null, string? message = null)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(paramName, $"{paramName} was null. Message=\"{message}\"");
			}
		}

		public static void NotNullOrEmpty(string argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null, string? message = null)
		{
			if (string.IsNullOrEmpty(argument))
			{
				throw new ArgumentNullException(paramName, $"{paramName} was null or empty. Message=\"{message}\"");
			}
		}

		public static void Empty<T>(IEnumerable<T> source, [CallerArgumentExpression(nameof(source))] string? paramName = null, string? message = null)
		{
			NotNull(source, paramName);

			if (source.Any())
			{
				throw new ArgumentOutOfRangeException(paramName, $"{paramName} was not empty. Message=\"{message}\"");
			}
		}

		public static void NotEmpty<T>(IEnumerable<T> source, [CallerArgumentExpression(nameof(source))] string? paramName = null, string? message = null)
		{
			NotNull(source, paramName);

			if (!source.Any())
			{
				throw new ArgumentOutOfRangeException(paramName, $"{paramName} was empty. Message=\"{message}\"");
			}
		}

		public static void Positive(decimal argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null, string? message = null)
		{
			if (argument <= 0m)
			{
				throw new ArgumentOutOfRangeException(paramName, $"{paramName} was not > 0. Actual={argument} Message=\"{message}\"");
			}
		}

		public static void Negative(decimal argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null, string? message = null)
		{
			if (argument >= 0m)
			{
				throw new ArgumentOutOfRangeException(paramName, $"{paramName} was not < 0. Actual={argument} Message=\"{message}\"");
			}
		}

		public static void MaxLength<T>(IEnumerable<T> source, int length, [CallerArgumentExpression(nameof(source))] string? paramName = null, string? message = null)
		{
			var sourceLength = source.Count();
			if (sourceLength > length)
			{
				throw new ArgumentOutOfRangeException(paramName, $"{sourceLength} exceeds maximum length of {length}. Message=\"{message}\"");
			}
		}

		// should be in Zenith.Maths, but then we have Core project depending on Maths, which I don't want
		public static bool IsEven<T>(T number) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
		{
			if (typeof(T) == typeof(char) || typeof(T) == typeof(bool))
				throw new ArgumentException("Unsupported type");

			dynamic value = Convert.ChangeType(number, typeof(long));
			return value % 2 == 0;
		}

		public static void Odd<T>(T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null, string? message = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
		{
			if (IsEven(argument))
			{
				throw new ArgumentOutOfRangeException(paramName, $"{paramName} was not odd. Actual={argument} Message=\"{message}\"");
			}
		}

		public static void Even<T>(T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null, string? message = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
		{
			if (!IsEven(argument))
			{
				throw new ArgumentOutOfRangeException(paramName, $"{paramName} was not even. Actual={argument} Message=\"{message}\"");
			}
		}
	}
}
