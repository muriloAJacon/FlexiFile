using FlexiFile.Core.Interfaces.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.FileCommands.GetFiles {
	public class GetFilesCommand : IRequest<IResultCommand> {
		public List<Guid> IgnoreFileIds { get; set; } = null!;

		public GetFilesCommand(List<Guid> ignoreFileIds) {
			IgnoreFileIds = ignoreFileIds;
		}
	}
}
