using System.Threading.Tasks;

namespace Jar
{
    public static class WebBinding
    {
        public delegate Task ExecuteJavascriptDelegate(string Code);
    }
}
