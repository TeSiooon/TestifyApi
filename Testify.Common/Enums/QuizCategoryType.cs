using System.ComponentModel.DataAnnotations;

namespace Testify.Domain.Constants;

public enum QuizCategoryType
{
    [Display(Name = "GeneralKnowledge")] GeneralKnowledge,
    [Display(Name = "Science")] Science,
    [Display(Name = "History")] History,
    [Display(Name = "Sports")] Sports,
    [Display(Name = "Entertainment")] Entertainment,
    [Display(Name = "Technology")] Technology
}
