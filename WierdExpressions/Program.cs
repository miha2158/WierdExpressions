using System;
using System.Linq;
using System.Text;

namespace WierdExpressions
{
    public class BinTreeNode
    {
        public BinTreeNode()
        {

        }

        public BinTreeNode(string value)
        {
            Info = value;
            Left = null;
            Right = null;
        }

        public string Info { get; set; }
        public BinTreeNode Left { get; set; }
        public BinTreeNode Right { get; set; }
    }
    
    public class TreeCreator
    {
        public TreeCreator(string input, out string put, out bool boolres)
        {
            expression = input
                .Replace(" ", "")
                .Replace("*", " * ")
                .Replace("/", " / ")
                .Replace("+", " + ")
                .Replace("-", " - ")
                .Replace("(", " ( ")
                .Replace(")", " ) ")
                .Replace("  ", " ")
                .Trim();

            exp = expression.Split(' ');
            Letters[0] = new object[0];
            Letters[1] = new object[0];
            
            boolres = Check(exp, out put);
            if (boolres)
            {
                MainTree = Create(exp);
                put = Calculate(MainTree).ToString();
            }
        }

        public bool Number(string str)
        {
            if (str == String.Empty)
                return false;

            foreach (char item in str)
                if (!char.IsDigit(item))
                    return false;

            return true;
        }

        public bool Word(string str)
        {
            if (str == String.Empty)
                return false;

            foreach (char item in str)
                if (!char.IsLetter(item))
                    return false;

            return true;
        }

        public bool Check(string[] things, out string message)
        {
            message = "правильно";
            int OPC = 0, CPC = 0;
            int i = 0;
            bool num = false;

            if (things.Length == 0 || things[0] == string.Empty)
            {
                message = "пустое выражение";
                return false;
            }

            if (things[i] == "-")
                i++;

            while (i < things.Length)
            {
                num = !num;

                if (!num)
                {
                    if (Signs.Contains(things[i]))
                    {
                        message = "не хватает числа";
                        i++;
                    }
                    else if (things[i] == ")")
                    {
                        message = "не совпадают скобки";
                        return false;
                    }
                    else
                    {
                        message = "не хавтает знака";
                        return false;
                    }
                }
                else if (Number(things[i]) || Word(things[i]))
                {
                    message = "правильно";
                    i++;
                }
                else if (things[i] == "(")
                {
                    OPC++;
                    int j = 0;
                    while (OPC > CPC)
                    {
                        j++;
                        if (i + j >= things.Length)
                        {
                            message = "не совпадают скобки";
                            return false;
                        }
                        if (things[i + j] == "(")
                            OPC++;
                        if (things[i + j] == ")")
                            CPC++;
                    }

                    j--;
                    string[] temp = new string[j];

                    for (int k = 0; k < temp.Length; k++)
                        temp[k] = things[i + 1 + k];

                    if (!Check(temp, out message))
                        return false;
                    i = i + j + 2;
                }
            }
            return num;
        }

        public BinTreeNode Create(string[] things)
        {
            BinTreeNode tree = new BinTreeNode();
            int i = 0;
            bool num = false;

            if (things.Length == 1)
                return new BinTreeNode(things[0]);
            while (i < things.Length)
            {
                num = !num;

                if (!num)
                {
                    tree.Info = things[i++];
                    string[] temp = new string[things.Length - i];
                    int k = 0;
                    for (; k < temp.Length; k++)
                        temp[k] = things[i + k];
                    tree.Right = Create(temp);
                    i += k;
                }

                else if (things[i] == "(")
                {
                    int OPC = 1, CPC = 0;
                    int j = 0;
                    while (OPC > CPC)
                    {
                        j++;
                        if (things[i + j] == "(")
                            OPC++;
                        else if (things[i + j] == ")")
                            CPC++;
                    }

                    j--;
                    string[] temp = new string[j];

                    for (int k = 0; k < temp.Length; k++)
                    {
                        temp[k] = things[i + 1 + k];
                    }
                    i = i + j + 2;

                    tree.Left = Create(temp);
                }
                else tree.Left = new BinTreeNode(things[i++]);

            }

            if (tree.Right == null && tree.Info == null)
                tree = tree.Left;

            return tree;
        }

        public int Calculate(BinTreeNode tree)
        {
            string p = tree.Info;
            int n = 0;

            if (p == "+")
                return Calculate(tree.Left) + Calculate(tree.Right);
            if (p == "-")
                return Calculate(tree.Left) - Calculate(tree.Right);
            if (p == "*")
                return Calculate(tree.Left)*Calculate(tree.Right);
            if (p == "/")
                return Calculate(tree.Left)/Calculate(tree.Right);

            if (!int.TryParse(p, out n) && !Letters[0].Contains(p))
            {
                var temp = new object[Letters.Length + 1];

                Letters[0].CopyTo(temp,0);
                Letters[0] = temp;
                Letters[0][Letters[0].Length - 1] = p;

                Console.WriteLine("\n\nВведите значение {0}", p);
                while(!int.TryParse(Console.ReadLine(), out n))
                    Console.WriteLine("Ожидалось число");

                temp = new object[Letters.Length + 1];
                Letters[1].CopyTo(temp,0);
                Letters[1] = temp;
                Letters[1][Letters[1].Length - 1] = n;
                return n;
            }

            if (!int.TryParse(p, out n) && Letters[0].Contains(p))
                return (int) Letters[1][Array.IndexOf(Letters[0], p)];

            if (int.TryParse(p, out n))
                return n;



            return 0;
        }

        public BinTreeNode MainTree = null;
        private readonly string[] Signs = {"+", "-", "*", "/"};

        private static object[][] Letters = new object[2][];
        private static string[] exp = null;
        private static string expression { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение");
            string expression = Console.ReadLine();
            string defaultRespose = "Результат";

            while (expression != "")
            {
                string response = String.Empty;
                bool valid = false;
                
                var boom = new TreeCreator(expression, out response, out valid);
                Console.WriteLine("\n{0} {1}", defaultRespose, response);

                Console.WriteLine("\n\nВведите выражение");
                expression = Console.ReadLine();
            }
        }
    }
}