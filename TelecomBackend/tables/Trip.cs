namespace TelecomBackend;

public class Trip
{
    public IFormFile GPXFile {get; set;} = null!;
    public IFormFile CSVFile {get; set;} = null!;
}  