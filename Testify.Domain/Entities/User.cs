using Microsoft.AspNetCore.Identity;

namespace Testify.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public User() : base()
    {
        // konstruktor bezparametrowy wymagany przez Identity i EF
    }

    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    public ICollection<Quiz> CreatedQuizzes { get; private set; } = new List<Quiz>();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    public ICollection<UserQuizAttempt> QuizAttempts { get; private set; } = new List<UserQuizAttempt>();

    public static User Create(string userName, string email)
    {
        return new User(userName, email);
    }

    public void Update(string userName, string email)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}
