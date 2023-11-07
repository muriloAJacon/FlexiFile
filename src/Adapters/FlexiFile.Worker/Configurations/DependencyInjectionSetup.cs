using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using FlexiFile.Infrastructure.Services;
using FlexiFile.Infrastructure.Services.ConvertServices;

namespace FlexiFile.Worker.Configurations {
	public static class DependencyInjectionSetup {
		public static void AddDependencyInjection(this IServiceCollection services) {
			services.AddTransient<IUserClaimsService, UserClaimsService>();
			services.AddTransient<IConvertImageService, MagickService>();
			services.AddTransient<IConvertVideoService, FFMpegService>();
			services.AddTransient<IConvertAudioService, FFMpegService>();
			services.AddTransient<ISplitDocumentService, ITextSplitDocumentService>();
			services.AddTransient<IMergeDocumentService, ITextMergeDocumentService>();
			services.AddTransient<IRearrangeDocumentService, ITextRearrangeDocumentService>();
		}
	}
}
