using System.Text.Json;
using CandleUp.Models;

namespace CandleUp.Services;

public class DbService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private List<Birthday> _birthdays = [];

    public DbService(string filePath)
    {
        _filePath = filePath;
        Load();
    }

    public IEnumerable<Birthday> GetAll()
    {
        return _birthdays;
    }

    public void Add(Birthday birthday)
    {
        birthday.Id = _birthdays.Any() ? _birthdays.Max(b => b.Id) + 1 : 1;
        _birthdays.Add(birthday);
        Save();
    }

    public void Update(Birthday birthday)
    {
        var existing = _birthdays.FirstOrDefault(b => b.Id == birthday.Id);
        if (existing == null) return;
        existing.Name = birthday.Name;
        existing.DateOfBirth = birthday.DateOfBirth;
        existing.Notes = birthday.Notes;
        Save();
    }

    public void Delete(int id)
    {
        _birthdays.RemoveAll(b => b.Id == id);
        Save();
    }

    private void Load()
    {
        if (!File.Exists(_filePath))
        {
            _birthdays = [];
            return;
        }

        var json = File.ReadAllText(_filePath);
        _birthdays = JsonSerializer.Deserialize<List<Birthday>>(json) ?? [];
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_birthdays, _options);
        File.WriteAllText(_filePath, json);
    }
}