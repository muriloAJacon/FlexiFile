using AutoMapper;
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
				.ForMember(x => x.TypeDescription, opt => opt.MapFrom(src => src.Type.Description))
				.ForMember(x => x.MimeType, opt => opt.MapFrom(src => src.Type.MimeType))
				.ForMember(x => x.Conversions, opt => opt.MapFrom(src => src.FileConversionOrigins.Select(x => x.FileConversion)));

			CreateMap<User, UserViewModel>();

			CreateMap<FileConversion, FileConversionViewModel>()
				.ForMember(x => x.FileResults, opt => opt.MapFrom(src => src.FileConversionResults));

			CreateMap<FileTypeConversion, FileTypeConversionViewModel>()
				.ForMember(x => x.ToTypeDescription, opt => opt.MapFrom(src => src.ToType == null ? null : src.ToType.Description));

			CreateMap<FileConversionResult, FileConversionResultViewModel>();
		}
	}
}
