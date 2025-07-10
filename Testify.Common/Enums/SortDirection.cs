using System.ComponentModel.DataAnnotations;

namespace Testify.Domain.Constants;

public enum SortDirection
{
    [Display(Name = "Ascending")] Ascending,
    [Display(Name = "Descending")] Descending
}

