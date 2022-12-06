using System.Runtime.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public enum SortDirection
    {
        [EnumMember(Value = "asc")]
        ASC,
        [EnumMember(Value = "desc")]
        DESC
    }
}
