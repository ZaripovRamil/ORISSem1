using System.Text;

namespace SiteProject.Services;

public static class EncoderService
{
    public static byte[] ToByteArray(this string s) => Encoding.UTF8.GetBytes(s);
}