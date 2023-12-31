﻿using AutoMapper;
using FlexiFile.Application.ViewModels.FileConversionViewModels;
using FlexiFile.Application.ViewModels.FileTypeConversionViewModels;
using FlexiFile.Application.ViewModels.FileViewModels;
using FlexiFile.Application.ViewModels.UserViewModels;
using FlexiFile.Core.Entities.Postgres;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Application.ViewModels {
	public class MapProfile : Profile {
		public MapProfile() {
			CreateMap<File, FileViewModel>()
				.ForMember(x => x.TypeDescription, opt => opt.MapFrom(src => src.Type.Description));

			CreateMap<User, UserViewModel>();

			CreateMap<FileConversion, FileConversionViewModel>();

			CreateMap<FileTypeConversion, FileTypeConversionViewModel>();
		}
	}
}
