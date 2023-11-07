using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FlexiFile.API.Filters {
	public class AccessLevelAttribute : ActionFilterAttribute {
		public readonly AccessLevel _minimumLevel;

		public AccessLevelAttribute(AccessLevel minimumLevel) {
			_minimumLevel = minimumLevel;
		}

		public override void OnActionExecuting(ActionExecutingContext context) {
			string level = context.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.Role), "Cannot get user Role from JWT.");

			AccessLevel accessLevel = Enum.Parse<AccessLevel>(level);

			if (accessLevel < _minimumLevel) {
				context.Result = new ObjectResult(new MessageViewModel("You do not have access to perform this action.", "permissionRequired")) {
					StatusCode = (int)HttpStatusCode.Forbidden
				};
			}

			base.OnActionExecuting(context);
		}
	}
}
