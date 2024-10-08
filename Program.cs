using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

[DllImport("user32.dll")]
static extern bool OpenClipboard(IntPtr hWndNewOwner);

[DllImport("user32.dll")]
static extern bool CloseClipboard();

[DllImport("user32.dll")]
static extern bool SetClipboardData(uint uFormat, IntPtr data);

string publicKeyXML = "<DSAKeyValue><P>3N35IcqKMJLdrg5HmSYa6duURBVDNZgj7BCnwcz/ufmuTgBqQSf3cxqHNTX31BTKJWBBdfF2LxA+uLRmTXZGzw==</P><Q>hEN4EgQsu/HHDcZh9Qwxg43wkL8=</Q><G>gYixcJeFwqXYS9td15uvi1o5Ontd6U00qjtvo7aPo4ccNB7jt5SGLEBM9RPsYPKnmC8PRBze5gm2MBgZIm4YsQ==</G><Y>RSijtNxu4sTZc50YrQLR19KX3PEsIGSrvcRYdAUKb1nWGJNY0aAdt/E5HtbMbSqIFPI3mLcOYdgxu9WyzGkRNw==</Y><J>AAAAAat+p7co8MRenHZUQ+BsY94gSFBLvhftoBGgwzmSBZZc+PRlV6daw3iAVN6y</J><Seed>J0VzBGcedEyNe+AWgq/mC/Wf9RU=</Seed><PgenCounter>pg==</PgenCounter><X>EaOoelpYoydZvGFMZHUobAPlfSY=</X></DSAKeyValue>";

start:
Console.Clear();
Console.WriteLine("########## Macro Recorder 5.9.0 KeyGen ##########");
Console.WriteLine("                   BY TH3C0D3R                   ");
Console.WriteLine("");
Console.Write("Enter Username: ");
string? Username = Console.ReadLine();
if (Username == null || string.IsNullOrWhiteSpace(Username)) goto start;
Console.WriteLine("");
Console.WriteLine($"Generating SerialKey for '{Username}'...");
Console.WriteLine("");
Console.WriteLine("");

int num2 = 240000;

byte highByte = (byte)(num2 >> 8);  // Extract the high byte
byte lowByte = (byte)(num2 & 0xFF); // Extract the low byte (mask with 0xFF)
byte[] invertedBytes = [highByte, lowByte];

byte lastbyte = invertedBytes[1];
byte secondLastbyte = invertedBytes[0];

string serial = generateSerial(new UTF8Encoding().GetBytes(Username), [secondLastbyte, lastbyte]);

OpenClipboard(IntPtr.Zero); 
nint ptr = Marshal.StringToHGlobalUni(serial);
SetClipboardData(13, ptr);
CloseClipboard();
Marshal.FreeHGlobal(ptr);

Console.WriteLine($"Serial for Username '{Username}' generated:");

Console.WriteLine($"{serial}");
Console.WriteLine("Serial copied to Clipboard");
Console.WriteLine("");
Console.WriteLine("Press any key to exit");
Console.ReadKey();


string generateSerial(byte[] bytesData, byte[] expirationTime)
{
    byte[] serial = [];
    try
    {
        using DSACryptoServiceProvider dsa = new(512);
        dsa.FromXmlString(publicKeyXML);
        serial = dsa.SignData(bytesData);
        dsa.PersistKeyInCsp = false;
    }
    catch
    {

    }
    return Convert.ToBase64String(serial.Concat(expirationTime).ToArray());
}