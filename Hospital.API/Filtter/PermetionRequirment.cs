using Microsoft.AspNetCore.Authorization;

namespace Hospital.API.Filtter
{
    public class PermetionRequirment : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermetionRequirment(string permission)
        {
            Permission = permission;
        }
    }
}
