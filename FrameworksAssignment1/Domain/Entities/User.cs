namespace FrameworksAssignment1.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public bool Admin { get; set; }

    #region Constructors
    public User()
    {
        
    }
    
    public User(
        string username
        ) : this()
    {
        Username = username;
    }
    
    #endregion
}


