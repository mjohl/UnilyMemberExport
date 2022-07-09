namespace Unily.Member.Export.Models;

internal class UnilyUser
{
    public int Id { get; set; }

    public string UniqueId { get; set; }
    public string Email { get; set; }
    
    public string AccountName { get; set; }

    public bool Discoverable { get; set; }

    public string DisplayName { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    
}