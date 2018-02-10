
namespace CmdMessenger.Test
{
    public class TestHelper
    {
        public static Blocking Wait(int seconds) {
            return new Blocking(seconds);
        }
    }
}
