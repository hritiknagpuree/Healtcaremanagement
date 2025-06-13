using Microsoft.EntityFrameworkCore;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;
    public DoctorRepository(AppDbContext context) => _context = context;

    public async Task<bool> DoctorExists(string username) =>
        await _context.Doctors.AnyAsync(d => d.Username == username);

    public async Task<Doctor> GetByUsername(string username) =>
        await _context.Doctors.FirstOrDefaultAsync(d => d.Username == username);

    public async Task AddDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Doctor>> GetAll() => await _context.Doctors.ToListAsync();

    public async Task<Doctor> GetById(int id) => await _context.Doctors.FindAsync(id);

    public async Task UpdateDoctor(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDoctor(int id)
    {
        var doc = await _context.Doctors.FindAsync(id);
        if (doc != null)
        {
            _context.Doctors.Remove(doc);
            await _context.SaveChangesAsync();
        }
    }
}
