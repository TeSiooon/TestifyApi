using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.ViewModels;
/// <summary>
/// Can add more properties in the future, like profile picture, bio, etc.
/// </summary>
/// <param name="Id"></param>
/// <param name="UserName"></param>
/// <param name="Email"></param>
public sealed record UserProfileVm(Guid Id, string UserName, string Email) : IViewModel<UserProfileVm, User>
{
    private static readonly Func<User, UserProfileVm> mapper = GetMapping().Compile();

    public static Expression<Func<User, UserProfileVm>> GetMapping()
    {
        return source => new UserProfileVm(
                source.Id,
                source.UserName,
                source.Email
            );
    }

    public static UserProfileVm From(User source)
    {
        return mapper(source);
    }

    public static UserProfileVm? FromNullable(User? source)
    {
        return source is null ? null : mapper(source);
    }
}
