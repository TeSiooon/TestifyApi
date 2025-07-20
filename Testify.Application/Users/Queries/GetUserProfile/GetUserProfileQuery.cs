using MediatR;
using Microsoft.AspNetCore.Identity;
using Testify.Application.Common;
using Testify.Application.ViewModels;
using Testify.Domain.Entities;

namespace Testify.Application.Users.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<UserProfileVm>
{
    public class Handler : IRequestHandler<GetUserProfileQuery, UserProfileVm>
    {
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService currentUserService;
        public Handler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<UserProfileVm> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var user = await userManager.FindByIdAsync(userId.ToString());
            return UserProfileVm.FromNullable(user) ?? throw new KeyNotFoundException("User not found");
        }
    }
}
