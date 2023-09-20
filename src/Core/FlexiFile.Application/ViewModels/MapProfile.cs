using AutoMapper;
using FlexiFile.Application.ViewModels.FileViewModels;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Application.ViewModels {
	public class MapProfile : Profile {
		public MapProfile() {
			CreateMap<File, FileViewModel>()
				.ForMember(x => x.TypeDescription, opt => opt.MapFrom(src => src.Type.Description));
		}
	}
}
