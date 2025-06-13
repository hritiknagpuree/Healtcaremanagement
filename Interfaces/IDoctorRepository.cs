public interface IDoctorRepository
{
    Task<bool> DoctorExists(string username);
    Task<Doctor> GetByUsername(string username);
    Task AddDoctor(Doctor doctor);
    Task<IEnumerable<Doctor>> GetAll();
    Task<Doctor> GetById(int id);
    Task UpdateDoctor(Doctor doctor);
    Task DeleteDoctor(int id);
}
