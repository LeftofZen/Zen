using System.ComponentModel;

namespace Zenith.Core
{
	public class BindingStack<T> : BindingList<T>
	{
		public void Push(T t) => Add(t);

		public T Peek() => this.Last();

		public T Pop()
		{
			var t = Peek();
			RemoveAt(Count - 1);
			return t;
		}
	}
}
