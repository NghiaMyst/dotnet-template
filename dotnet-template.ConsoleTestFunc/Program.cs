using dotnet_boilderplate.SharedKernel.Utils;

var currentTime = DateTime.Now;

var currentUtcTime = DateTime.UtcNow;

//Epoch timestamp: 1767456494
//Timestamp in milliseconds: 1767456494000
//Date and time (GMT): Saturday, January 3, 2026 4:08:14 PM
//Date and time (your time zone): Saturday, January 3, 2026 11:08:14 PM GMT+07:00
var unixTimeExample = 1767456494;

var utc = DateTimeUtils.FromUnixSeconds(unixTimeExample);
var noUtc = DateTimeUtils.FromUnixSecondsNoUniversal(unixTimeExample);
var local = DateTimeUtils.FromUnixSecondsToLocalTime(unixTimeExample);

Console.WriteLine($"Converter - ToTimeZone: {DateTimeUtils.ToTimeZone(currentUtcTime, "Asia/Ho_Chi_Minh")}");


Log("Converter - UniversalTime", utc);
Log("Converter - NoUniversalTime", noUtc);
Log("Converter - LocalTime", local);

void Log(string label, DateTime dt)
{
    var dto = new DateTimeOffset(dt);

    Console.WriteLine($"[{label}]");
    Console.WriteLine($"  Value           : {dt:yyyy-MM-dd HH:mm:ss.fffffff}");
    Console.WriteLine($"  ISO (O)         : {dt:O}");
    Console.WriteLine($"  Kind            : {dt.Kind}");
    Console.WriteLine($"  Ticks           : {dt.Ticks}");
    Console.WriteLine($"  As UTC          : {dt.ToUniversalTime():yyyy-MM-dd HH:mm:ss.fffffff}");
    Console.WriteLine($"  As Local        : {dt.ToLocalTime():yyyy-MM-dd HH:mm:ss.fffffff}");
    Console.WriteLine($"  Offset          : {dto.Offset}");
    Console.WriteLine($"  With Offset ISO : {dto:O}");
    Console.WriteLine();
}