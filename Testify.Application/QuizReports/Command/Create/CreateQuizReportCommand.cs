using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.QuizReports.Command.Create;

public record CreateQuizReportCommand : IRequest<Guid>
{
    public Guid QuizId { get; set; }
    public string Reason { get; set; } = default!;
    public class Handler : IRequestHandler<CreateQuizReportCommand, Guid>
    {

        private readonly IQuizRepository quizRepository;
        private readonly ICurrentUserService currentUserService;
        private readonly IQuizReportRepository quizReportRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IQuizRepository quizRepository, ICurrentUserService currentUserService,
            IQuizReportRepository quizReportRepository, IUnitOfWork unitOfWork)
        {
            this.quizRepository = quizRepository;
            this.currentUserService = currentUserService;
            this.quizReportRepository = quizReportRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateQuizReportCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetById(request.QuizId) ?? throw new ArgumentException("Quiz not found", nameof(request.QuizId));
            var userId = currentUserService.UserId;

            var quizReport = QuizReport.Create(quiz.Id, userId, request.Reason);

            await quizReportRepository.Create(quizReport, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return quizReport.Id;
        }
    }
}
