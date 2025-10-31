using System;
using System.IO;
using System.Security.Cryptography;

public class BCryptHashAlgorithm : HashAlgorithm
{
    private MemoryStream _buffer;
    public string Salt { get; set; }
    public int WorkFactor { get; set; } = 10;

    public BCryptHashAlgorithm(string salt = null, int workFactor = 10)
    {
        _buffer = new MemoryStream();
        Salt = salt ?? BCrypt.Net.BCrypt.GenerateSalt(workFactor);
        WorkFactor = workFactor;
        HashSizeValue = 192; // bcrypt hashes are 192 bits (24 bytes)
    }

    public override void Initialize()
    {
        _buffer.SetLength(0);
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        _buffer.Write(array, ibStart, cbSize);
    }

    protected override byte[] HashFinal()
    {
        var data = _buffer.ToArray();
        var plainText = System.Text.Encoding.UTF8.GetString(data);
        var hash = BCrypt.Net.BCrypt.HashPassword(plainText, Salt);
        return System.Text.Encoding.UTF8.GetBytes(hash);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _buffer?.Dispose();
        }
        base.Dispose(disposing);
    }
}
