using System;

namespace ClassDemo
{
    public class Program
    {
        public static int Multiply(int left, int right)
        {
            return left * right;
        }

        public static void Swap(ref int left, ref int right)
        {
            int temp;
            temp = left;
            left = right;
            throw new InvalidCastException();
            right = temp;
        }
        public static void Swap(string left1, string right1)
        {
            string temp;
            temp = left1;
            left1 = right1;
            right1 = temp;
        }
        public static (string wasRight, string wasLeft) SwapTuple(string left1, string right1)
        {
            string temp;
            temp = left1;
            left1 = right1;
            right1 = temp;
            return (left1, right1);
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");
        }

        public void Login(string username, string password)
        {
            if (!TryLogin(username, password))
            {
                throw new InvalidOperationException("Invalid username or password");
            }
        }

        public bool TryLogin(string username, string password)
        {
            if (password == "goodpassword")
            {
                return true;
            }
            return false;
        }
    }
}
