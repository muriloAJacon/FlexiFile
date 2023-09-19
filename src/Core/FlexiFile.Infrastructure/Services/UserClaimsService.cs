using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Infrastructure.Services {
	public class UserClaimsService : IUserClaimsService {
		private readonly IHttpContextAccessor _context;

		public UserClaimsService(IHttpContextAccessor context) {
			_context = context;
		}

		public Guid Id {
			get {
				string userGuid = _context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.NameIdentifier), "Cannot get user ID from JWT.");

				return Guid.Parse(userGuid);
			}
		}

		public string Name {
			get {
				return _context.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.Name), "Cannot get user Name from JWT.");
			}
		}

		public string Email {
			get {
				return _context.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.Email), "Cannot get user Email from JWT.");
			}
		}

		public AccessLevel AccessLevel {
			get {
				string level = _context.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.Role), "Cannot get user Role from JWT.");

				return Enum.Parse<AccessLevel>(level);
			}
		}

		public Guid? TryGetId() {
			string? userGuid = _context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userGuid is null) {
				return null;
			}

			return Guid.Parse(userGuid);
		}

		public string? TryGetName() {
			return _context.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
		}

		public string? TryGetEmail() {
			return _context.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
		}

		public AccessLevel? TryGetAccessLevel() {
			string? level = _context.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
			if (level is null) {
				return null;
			}

			return Enum.Parse<AccessLevel>(level);
		}
	}
}
