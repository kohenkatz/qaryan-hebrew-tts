using System;
namespace Qaryan.Core
{
    public interface IPlatformSupported
    {
        bool PlatformSupported { get; }
    }

    public sealed class PlatformInstantiator<T> where T : class, IPlatformSupported
    {
        public static T Create(params Type[] types)
        {
            foreach (Type t in types)
            {
                if (typeof(T).IsAssignableFrom(t))
                {
                    T obj=t.GetConstructor(new Type[0]).Invoke(new object[0]) as T;
                    if (obj.PlatformSupported)
                        return obj;
                }
                else
                    throw new InvalidCastException(string.Format("Can't create {0} as {1}", t.FullName, typeof(T).FullName));
            }
            return null;
        }
    }
}
