using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NumMethods3
{
    //Постфиксная нотация
    class PostfixNotation
    {
        //Здесь хранится сама нотация в виде массива подстрок (операндов/операторов)
        private string[] separated_notation;

        //Операторы
        private List<string> standart_operators =
            new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^", "!" });
        //Функции
        private List<string> functions =
            new List<string>(new string[] { "exp", "ln", "sin", "cos" });
 

        public PostfixNotation(string input)
        {
            separated_notation = ConvertToPostfixNotation(input);
        }

        //Разбор входной строки на подстроки
        private string[] Separate(string input)
        {
            //Сначала убираем из строки все пробелы
            List<string> result = new List<string>();
            input = input.Replace(" ", string.Empty);

            //Далее, пока не будет разобрана вся строка,
            int pos = 0;
            while (pos < input.Length)
            {
                //Считываем очередной символ
                string s = string.Empty + input[pos];

                if (!standart_operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.'); i++)
                            s += input[i];
                    else if (Char.IsLetter(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsLetter(input[i]) || Char.IsDigit(input[i])); i++)
                            s += input[i];
                }
                result.Add(s);
                pos += s.Length;
            }
            return result.ToArray();
        }

        //Вычисление приоритетов операций
        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "!":
                    return 2;
                case "^":
                    return 3;
                case "exp":
                case "ln":
                case "sin":
                case "cos":
                    return 4;
                default:
                    return 5;
            }
        }
 
        //Преобразование в постфиксную нотацию
        private string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            string[] inputSeparated = Separate(input);
            string c;

            for (int i = 0; i < inputSeparated.Length; i++)
            {
                c = inputSeparated[i];

                if (c.Equals("-") && (i == 0 || inputSeparated[i-1].Equals("(")))
                {
                    c = c.Replace("-", "!");
                }


                if (standart_operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }

                            if (stack.Count > 0 && functions.Contains(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else if (functions.Contains(c))
                {
                    stack.Push(c);
                }
                else
                    outputSeparated.Add(c);

            }

            if (stack.Count > 0)
                foreach (string s in stack)
                    outputSeparated.Add(s);
 
            return outputSeparated.ToArray();
        }

        public double Calculate(double x)
        {
            return Calculate(x, 0);
        }

        public double Calculate(double x, double y)
        {
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(Signify(x, y));
            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!standart_operators.Contains(str) && !functions.Contains(str))
                {
                    stack.Push(str);
                }
                else
                {
                    double summ = 0;
                    switch (str)
                    {
 
                        case "+":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                summ = a + b;
                                break;
                            }
                        case "-":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                summ=b-a;
                                break;
                            }
                        case "*":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                summ = b * a;
                                break;
                            }
                        case "/":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                summ = b / a;
                                break;
                            }
                        case "^":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                summ = Math.Pow(b, a);
                                break;
                            }
                        case "!":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                summ = -a;
                                break;
                            }
                        case "exp":
                            {
                                double a = a = Convert.ToDouble(stack.Pop());
                                summ = Math.Exp(a);
                                break;
                            }
                        case "ln":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                summ = Math.Log(a);
                                break;
                            }
                        case "sin":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                summ = Math.Sin(a);
                                break;
                            }
                        case "cos":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                summ = Math.Cos(a);
                                break;
                            }
                    }

                    if (Double.IsNaN(summ) || Double.IsInfinity(summ))
                        throw new Exception("Недопустимая операция");

                    stack.Push(summ.ToString());
                }

                if (queue.Count > 0)
                    str = queue.Dequeue();
                else
                    break;
 
            }

            //Если в конце выполнения в стеке остался только один элемент,
            if (stack.Count == 1)
                //возвращаем полученный результат,
                return Convert.ToDouble(stack.Pop());
            //иначе
            else
                //бросаем исключение. Такое может быть, если входное выражение некорректно
                throw new Exception("Входная строка имеет некорректный формат");

        }

        //Означивание нотации функции (замена символа "x" на значение)
        private string[] Signify(double x, double y)
        {
            string[] result = new string[separated_notation.Length];

            //Проходим циклом по массиву нотации
            for (int i = 0; i < separated_notation.Length; i++)
            {
                //Если нашли x - 
                if (separated_notation[i].Equals("x"))
                    //заменяем его значением
                    result[i] = x.ToString();
                //Если нашли y - 
                else if (separated_notation[i].Equals("y"))
                    //заменяем его значением
                    result[i] = y.ToString();
                else
                    //копируем подстроку без изменений
                    result[i] = separated_notation[i];
            }

            return result;
        }

        public override string ToString()
        {
            return String.Join(" ", separated_notation);
        }

    }
}
