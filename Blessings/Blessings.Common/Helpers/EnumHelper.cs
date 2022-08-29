using Blessings.Common.Models;

namespace Blessings.Common.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<EnumTypeViewModel<T>> GetEnumTypes<T>() where T : Enum
        {
            return GetEnums<T>()
                   .Select(type => new EnumTypeViewModel<T>
                   {
                       Id = type,
                       Name = type.ToString()
                   });
        }

        public static IEnumerable<T> GetEnums<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();
    }
}
