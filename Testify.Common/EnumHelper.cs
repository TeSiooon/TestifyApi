using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Testify.Application.Common;
using Testify.Domain.Constants;

namespace Testify.Common;

public class EnumHelper
{
    private readonly Dictionary<string, Type> enums = new()
    {
        { "AnswerActionType", typeof(AnswerActionType) },
        { "QuizCategoryType", typeof(QuizCategoryType) },
        { "SortDirection", typeof(SortDirection) }
    };

    public Dictionary<string, List<Dictionary<string, object>>> GetAllEnums()
    {
        var result = new Dictionary<string, List<Dictionary<string, object>>>();

        foreach (var (key, type) in enums)
        {
            result[key] = GetEnumValues(type);
        }

        return result;
    }

    private List<Dictionary<string, object>> GetEnumValues(Type enumType)
    {
        return Enum.GetValues(enumType)
            .Cast<Enum>()
            .Select(e => new Dictionary<string, object>
            {
                ["Value"] = Convert.ToInt32(e),
                ["Name"] = GetDisplayName(e)
            })
            .ToList();
    }

    private string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DisplayAttribute>();
        return attr?.Name ?? value.ToString();
    }
}
