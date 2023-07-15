using System.Collections;
using System.Text;

namespace Zenith.Debug
{
	public static class DebugSerialiser
	{
		public static string ObjectToString(object obj)
		{
			var sb = new StringBuilder();
			ObjectToString(obj, sb);
			return sb.ToString();
		}

		const string Separator = ", ";

		static void ObjectToString(object obj, StringBuilder sb)
		{
			if (obj == null)
			{
				_ = sb.Append("<null>");
				return;
			}

			var type = obj.GetType();

			// if object has overriden ToString(), lets use that instead
			var methods = type.GetMethods();
			var method = Array.Find(methods, m => m.Name.Contains("ToString"));
			if (method?.DeclaringType != typeof(object))
			{
				_ = sb.Append('\"');
				_ = sb.Append(obj.ToString());
				_ = sb.Append('\"');

				return;
			}

			// log IEnumerables
			if (type.GetInterfaces().Contains(typeof(IEnumerable)))
			{
				_ = sb.Append('[');
				foreach (var subObject in (IEnumerable)obj)
				{
					ObjectToString(subObject, sb);
				}

				_ = sb.Append(']');
				return;
			}

			_ = sb.Append('{');

			// log each public property of the object
			var props = type.GetProperties();
			var orderedProps = props.OrderBy(prop => prop.Name);
			foreach (var t in orderedProps)
			{
				_ = sb.Append(t.Name).Append('=');
				var tValue = t.GetValue(obj);

				if (tValue == null)
				{
					_ = sb.Append("<null>");
				}
				else
				{
					var tInterfaces = t.PropertyType.GetInterfaces();

					// shortcut for strings; string is class/IEnumerable, but we just want the string value
					if (t.PropertyType == typeof(string) || t.PropertyType.IsAssignableFrom(typeof(string)))
					{
						_ = sb.Append('\"');
						_ = sb.Append(tValue);
						_ = sb.Append('\"');
					}
					else if (t.PropertyType.IsClass || tInterfaces.Contains(typeof(IEnumerable))) // recurse into classes
					{
						ObjectToString(tValue, sb);
					}
					else // else just use built-in ToString()
					{
						_ = sb.Append(tValue);
					}
				}

				_ = sb.Append(Separator);
			}

			if (props.Length > 0)
			{
				_ = sb.Remove(sb.Length - 2, 2); // dirty hack to remove the trailing extra ", "
			}

			_ = sb.Append('}');

			return;
		}
	}
}