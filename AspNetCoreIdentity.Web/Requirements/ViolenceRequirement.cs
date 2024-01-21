using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }

        public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
            {
                if (!context.User.HasClaim(x => x.Type == "Birthdate"))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                Claim birthDateClaim = context.User.FindFirst("Birthdate")!;
                var birthdate = Convert.ToDateTime(birthDateClaim.Value);
                var today = DateTime.Now;
                var age = today.Year - birthdate.Year;

                if (birthdate > today.AddYears(-age)) age--;

                if (requirement.ThresholdAge > age)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                context.Succeed(requirement);
                return Task.CompletedTask;

            }
        }
    }
}
