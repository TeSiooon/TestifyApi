using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Application.ViewModels;
using Testify.Domain.Constants;

namespace Testify.Application.Quizzes.Queries.GetAllQuizzes;

public class GetAllQuizzesQuery : IRequest<PagedResult<QuizVm>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }

    public class Handler : IRequestHandler<GetAllQuizzesQuery, PagedResult<QuizVm>>
    {
        private readonly IQuizRepository quizRepository;

        public Handler(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }

        public async Task<PagedResult<QuizVm>> Handle(GetAllQuizzesQuery request, CancellationToken cancellationToken)
        {
            var (result, totalCount) = await quizRepository.GetAllMatchingAsync(request.SearchPhrase,
            request.PageSize, request.PageNumber, request.SortBy, request.SortDirection);

            var quizVms = result.Select(QuizVm.From).ToList();

            return new PagedResult<QuizVm>(quizVms, totalCount, request.PageSize, request.PageNumber);
        }
    }
}
