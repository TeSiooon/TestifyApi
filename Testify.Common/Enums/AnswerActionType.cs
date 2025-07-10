using System.ComponentModel.DataAnnotations;

namespace Testify.Application.Common;

public enum AnswerActionType
{
    [Display(Name = "Add Answer")] Add,
    [Display(Name = "Update Answer")] Update,
    [Display(Name = "Delete Answer")] Delete
}
