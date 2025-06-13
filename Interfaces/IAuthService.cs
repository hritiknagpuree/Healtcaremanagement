using HealthcareApi.ViewModels;

public interface IAuthService
{
    Task<string> RegisterDoctor(DoctorRegisterDto dto);
    Task<string> LoginDoctor(DoctorLoginDto dto);
}
