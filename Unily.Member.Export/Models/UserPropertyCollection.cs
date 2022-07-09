namespace Unily.Member.Export.Models;

public class UserPropertyCollection : Dictionary<string, string>
{
    public string SyncSourceImmutableId => this.GetValueOrDefault(UserProperties.SyncSourceImmutableId);

    public string ExternalAuthenticationObjectId => this.GetValueOrDefault(UserProperties.ExternalAuthenticationObjectId);

    public string IdentityIssuer => this.GetValueOrDefault(UserProperties.IdentityIssuer);

    public string IdentityProvider => this.GetValueOrDefault(UserProperties.IdentityProvider);
        
    public string IsApproved => this.GetValueOrDefault(UserProperties.IsApproved);
    public string DisplayName => this.GetValueOrDefault(UserProperties.DisplayName);
        
    public string FirstName => this.GetValueOrDefault(UserProperties.FirstName);
        
    public string LastName => this.GetValueOrDefault(UserProperties.LastName);


}