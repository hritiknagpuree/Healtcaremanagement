using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Doctor")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorController(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctorRepository.GetAll();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var doctor = await _doctorRepository.GetById(id);
        if (doctor == null) return NotFound();
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
    {
        await _doctorRepository.AddDoctor(doctor);
        return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
    {
        var existing = await _doctorRepository.GetById(id);
        if (existing == null) return NotFound();

        existing.Name = doctor.Name;
        existing.Email = doctor.Email;
        existing.Username = doctor.Username;

        await _doctorRepository.UpdateDoctor(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var doctor = await _doctorRepository.GetById(id);
        if (doctor == null) return NotFound();

        await _doctorRepository.DeleteDoctor(id);
        return NoContent();
    }
}
