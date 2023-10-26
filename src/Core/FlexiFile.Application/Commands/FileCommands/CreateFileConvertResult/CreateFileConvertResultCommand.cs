﻿using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.FileCommands.CreateFileConvertResult {
	public sealed record CreateFileConvertResultCommand : IRequest {
		public Guid FileId { get; set; }
		public Guid ConversionId { get; set; }
		public int Order { get; set; }
	}
}
